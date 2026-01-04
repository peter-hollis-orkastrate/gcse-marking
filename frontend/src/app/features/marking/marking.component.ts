import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SkillSelectorComponent } from './components/skill-selector/skill-selector.component';
import { QuestionInputComponent } from './components/question-input/question-input.component';
import { PdfUploadComponent } from './components/pdf-upload/pdf-upload.component';
import { ProgressIndicatorComponent } from './components/progress-indicator/progress-indicator.component';
import { ResultsDisplayComponent } from './components/results-display/results-display.component';
import { HeaderComponent } from '../../shared/components/header/header.component';
import { SkillsService } from '../../core/services/skills.service';
import { MarkingService } from '../../core/services/marking.service';
import { Skill } from '../../core/models/skill.model';
import { MarkingResult } from '../../core/models/marking-result.model';

type AppState = 'form' | 'loading' | 'results' | 'error';

@Component({
  selector: 'app-marking',
  standalone: true,
  imports: [
    CommonModule,
    HeaderComponent,
    SkillSelectorComponent,
    QuestionInputComponent,
    PdfUploadComponent,
    ProgressIndicatorComponent,
    ResultsDisplayComponent
  ],
  template: `
    <div class="min-h-screen bg-gray-50">
      <app-header />

      <main class="max-w-2xl mx-auto px-4 py-8">
        @switch (state) {
          @case ('form') {
            <div class="bg-white rounded-lg shadow-sm border border-gray-200 p-6 space-y-6">
              <h2 class="text-xl font-semibold text-gray-800">Mark an Essay</h2>

              <app-skill-selector
                [skills]="skills"
                (skillSelected)="onSkillSelected($event)" />

              <app-question-input
                (questionChanged)="onQuestionChanged($event)" />

              <app-pdf-upload
                (pdfSelected)="onPdfSelected($event)"
                (pdfCleared)="onPdfCleared()" />

              <button
                (click)="submitForMarking()"
                [disabled]="!canSubmit"
                class="w-full py-3 px-4 bg-blue-600 text-white font-medium rounded-md hover:bg-blue-700 disabled:bg-gray-300 disabled:cursor-not-allowed transition">
                Mark Essay
              </button>
            </div>
          }
          @case ('loading') {
            <app-progress-indicator />
          }
          @case ('results') {
            <app-results-display
              [result]="result!"
              (onReset)="reset()" />
          }
          @case ('error') {
            <div class="bg-red-50 border border-red-200 rounded-lg p-6 text-center">
              <p class="text-red-700 mb-4">{{ errorMessage }}</p>
              <button (click)="reset()" class="px-4 py-2 bg-red-600 text-white rounded-md hover:bg-red-700">
                Try Again
              </button>
            </div>
          }
        }
      </main>
    </div>
  `
})
export class MarkingComponent implements OnInit {
  private skillsService = inject(SkillsService);
  private markingService = inject(MarkingService);

  skills: Skill[] = [];
  state: AppState = 'form';
  result: MarkingResult | null = null;
  errorMessage = '';

  selectedSkillId = '';
  question = '';
  pdfBase64 = '';

  ngOnInit(): void {
    this.skillsService.getSkills().subscribe(skills => {
      this.skills = skills;
    });
  }

  get canSubmit(): boolean {
    return !!this.selectedSkillId && !!this.question.trim() && !!this.pdfBase64;
  }

  onSkillSelected(skillId: string): void {
    this.selectedSkillId = skillId;
  }

  onQuestionChanged(question: string): void {
    this.question = question;
  }

  onPdfSelected(data: { base64: string; name: string }): void {
    this.pdfBase64 = data.base64;
  }

  onPdfCleared(): void {
    this.pdfBase64 = '';
  }

  submitForMarking(): void {
    this.state = 'loading';
    const selectedSkill = this.skills.find(s => s.id === this.selectedSkillId);
    const skillName = selectedSkill?.name || this.selectedSkillId;
    this.markingService.markEssay(this.selectedSkillId, skillName, this.question, this.pdfBase64)
      .subscribe({
        next: (result) => {
          this.result = result;
          this.state = 'results';
        },
        error: (err) => {
          this.errorMessage = err.message || 'Failed to mark essay. Please try again.';
          this.state = 'error';
        }
      });
  }

  reset(): void {
    this.state = 'form';
    this.result = null;
    this.errorMessage = '';
    this.selectedSkillId = '';
    this.question = '';
    this.pdfBase64 = '';
  }
}
