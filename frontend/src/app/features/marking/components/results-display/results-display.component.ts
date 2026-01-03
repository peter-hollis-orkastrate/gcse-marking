import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { MarkingResult } from '../../../../core/models/marking-result.model';

@Component({
  selector: 'app-results-display',
  standalone: true,
  imports: [CommonModule, DatePipe],
  template: `
    <div class="max-w-3xl mx-auto">
      <div class="mb-4 flex flex-wrap gap-3 justify-center">
        <button (click)="onReset.emit()"
                class="px-5 py-2 bg-blue-600 text-white font-medium rounded-md hover:bg-blue-700 transition">
          Mark Another Essay
        </button>
        <button (click)="print()"
                class="px-5 py-2 border border-gray-300 text-gray-700 font-medium rounded-md hover:bg-gray-50 transition">
          Print
        </button>
        <button (click)="downloadPdf()"
                class="px-5 py-2 border border-gray-300 text-gray-700 font-medium rounded-md hover:bg-gray-50 transition">
          Download PDF
        </button>
      </div>

      <div class="bg-blue-50 border border-blue-200 rounded-lg p-6 mb-4 text-center">
        <p class="text-sm text-blue-600 font-medium">Overall Grade Band</p>
        <p class="text-4xl font-bold text-blue-700 mt-2">{{ result.gradeBand }}</p>
        <p class="text-xs text-gray-500 mt-2">Marked on {{ result.timestamp | date:'medium' }}</p>
      </div>

      @if (result.question) {
        <div class="bg-amber-50 border border-amber-200 rounded-lg p-4 mb-6">
          <p class="text-sm font-medium text-amber-800 mb-1">Essay Question</p>
          <p class="text-gray-800 italic">"{{ result.question }}"</p>
        </div>
      }

      <div id="feedback-content" class="bg-white border border-gray-200 rounded-lg p-6 shadow-sm prose prose-sm max-w-none"
           [innerHTML]="renderedFeedback">
      </div>
    </div>
  `
})
export class ResultsDisplayComponent {
  @Input() result!: MarkingResult;
  @Output() onReset = new EventEmitter<void>();

  get renderedFeedback(): string {
    return this.renderMarkdown(this.result.raw);
  }

  private renderMarkdown(text: string): string {
    return text
      .replace(/^---$/gm, '<hr class="my-6 border-t border-gray-300" />')
      .replace(/^# (.*$)/gm, '<h1 class="text-2xl font-bold mt-8 mb-4 text-gray-900">$1</h1>')
      .replace(/^### (.*$)/gm, '<h3 class="text-lg font-semibold mt-6 mb-2 text-gray-800">$1</h3>')
      .replace(/^## (.*$)/gm, '<h2 class="text-xl font-bold mt-8 mb-3 text-gray-900">$1</h2>')
      .replace(/\*\*(.*?)\*\*/g, '<strong class="font-semibold">$1</strong>')
      .replace(/\*(.*?)\*/g, '<em>$1</em>')
      .replace(/^- (.*$)/gm, '<li class="ml-4 text-gray-700">$1</li>')
      .replace(/\n\n/g, '</p><p class="mb-4 text-gray-700">')
      .replace(/\n/g, '<br />');
  }

  print(): void {
    window.print();
  }

  async downloadPdf(): Promise<void> {
    const { jsPDF } = await import('jspdf');
    const html2canvas = (await import('html2canvas')).default;

    const element = document.getElementById('feedback-content');
    if (!element) return;

    // Use lower scale and JPEG for smaller file size
    const canvas = await html2canvas(element, {
      scale: 1.5,
      useCORS: true,
      logging: false,
      backgroundColor: '#ffffff'
    });

    // JPEG with 85% quality is much smaller than PNG
    const imgData = canvas.toDataURL('image/jpeg', 0.85);

    const pdf = new jsPDF({
      orientation: 'portrait',
      unit: 'mm',
      format: 'a4'
    });

    const pdfWidth = pdf.internal.pageSize.getWidth();
    const pdfHeight = pdf.internal.pageSize.getHeight();
    const imgWidth = canvas.width;
    const imgHeight = canvas.height;
    const ratio = Math.min(pdfWidth / imgWidth, pdfHeight / imgHeight);
    const imgX = (pdfWidth - imgWidth * ratio) / 2;
    const scaledHeight = imgHeight * ratio;

    // Handle multi-page PDFs
    let heightLeft = scaledHeight;
    let position = 0;

    pdf.addImage(imgData, 'JPEG', imgX, position, imgWidth * ratio, scaledHeight);
    heightLeft -= pdfHeight;

    while (heightLeft > 0) {
      position = heightLeft - scaledHeight;
      pdf.addPage();
      pdf.addImage(imgData, 'JPEG', imgX, position, imgWidth * ratio, scaledHeight);
      heightLeft -= pdfHeight;
    }

    pdf.save(`essay-feedback-${this.result.gradeBand}.pdf`);
  }

}
