import { Component, Output, EventEmitter, ViewChild, ElementRef } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-pdf-upload',
  standalone: true,
  imports: [CommonModule],
  template: `
    @if (!fileName) {
      <div>
        <label class="block text-sm font-medium text-gray-700 mb-2">Upload Essay (PDF)</label>
        <div
          class="flex flex-col items-center justify-center w-full h-32 border-2 border-dashed rounded-lg bg-gray-50 cursor-pointer transition-colors"
          [class.border-blue-500]="isDragOver"
          [class.bg-blue-50]="isDragOver"
          [class.border-gray-300]="!isDragOver"
          (dragover)="onDragOver($event)"
          (dragleave)="onDragLeave($event)"
          (drop)="onDrop($event)"
          (click)="fileInput.click()">
          <svg class="mx-auto h-8 w-8 text-gray-400 mb-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                  d="M7 21h10a2 2 0 002-2V9.414a1 1 0 00-.293-.707l-5.414-5.414A1 1 0 0012.586 3H7a2 2 0 00-2 2v14a2 2 0 002 2z" />
          </svg>
          <p class="text-sm text-gray-600">Click or drag PDF to upload</p>
          <p class="text-xs text-gray-400 mt-1">PDF files only (max 10MB)</p>
          <input #fileInput type="file" accept="application/pdf" class="hidden" (change)="onFileSelected($event)" />
        </div>
      </div>
    } @else {
      <div>
        <label class="block text-sm font-medium text-gray-700 mb-2">Uploaded Essay</label>
        <div class="flex items-center gap-3 p-3 bg-gray-50 rounded-lg border border-gray-200">
          <div class="flex-shrink-0 w-12 h-12 bg-red-100 rounded flex items-center justify-center">
            <svg class="w-8 h-8 text-red-600" fill="currentColor" viewBox="0 0 24 24">
              <path d="M14 2H6a2 2 0 00-2 2v16a2 2 0 002 2h12a2 2 0 002-2V8l-6-6zm-1 2l5 5h-5V4zM6 20V4h6v6h6v10H6z"/>
            </svg>
          </div>
          <div class="flex-grow min-w-0">
            <p class="text-sm font-medium text-gray-900 truncate">{{ fileName }}</p>
            <p class="text-xs text-gray-500">PDF Document</p>
          </div>
          <button type="button" (click)="onClear()" class="text-gray-400 hover:text-red-500 transition-colors">
            <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" />
            </svg>
          </button>
        </div>
      </div>
    }
  `
})
export class PdfUploadComponent {
  @ViewChild('fileInput') fileInput!: ElementRef<HTMLInputElement>;
  @Output() pdfSelected = new EventEmitter<{ base64: string; name: string }>();
  @Output() pdfCleared = new EventEmitter<void>();

  fileName: string | null = null;
  isDragOver = false;

  onDragOver(event: DragEvent): void {
    event.preventDefault();
    event.stopPropagation();
    this.isDragOver = true;
  }

  onDragLeave(event: DragEvent): void {
    event.preventDefault();
    event.stopPropagation();
    this.isDragOver = false;
  }

  onDrop(event: DragEvent): void {
    event.preventDefault();
    event.stopPropagation();
    this.isDragOver = false;
    const file = event.dataTransfer?.files[0];
    if (file) this.processFile(file);
  }

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    const file = input.files?.[0];
    if (file) this.processFile(file);
  }

  private processFile(file: File): void {
    if (file.type !== 'application/pdf') {
      alert('Please select a PDF file');
      return;
    }

    if (file.size > 10 * 1024 * 1024) {
      alert('File is too large. Maximum size is 10MB.');
      return;
    }

    const reader = new FileReader();
    reader.onload = () => {
      const base64 = (reader.result as string).split(',')[1];
      this.fileName = file.name;
      this.pdfSelected.emit({ base64, name: file.name });
    };
    reader.readAsDataURL(file);
  }

  onClear(): void {
    this.fileName = null;
    if (this.fileInput) {
      this.fileInput.nativeElement.value = '';
    }
    this.pdfCleared.emit();
  }
}
