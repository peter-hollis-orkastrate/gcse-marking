import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-question-input',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div>
      <label class="block text-sm font-medium text-gray-700 mb-2">Essay Question</label>
      <textarea
        [(ngModel)]="question"
        (ngModelChange)="onQuestionChange($event)"
        [placeholder]="placeholder"
        rows="3"
        class="w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500 resize-none">
      </textarea>
    </div>
  `
})
export class QuestionInputComponent {
  @Input() placeholder = 'Enter the essay question the student was answering...';
  @Output() questionChanged = new EventEmitter<string>();

  question = '';

  onQuestionChange(value: string): void {
    this.questionChanged.emit(value);
  }
}
