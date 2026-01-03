import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Skill } from '../../../../core/models/skill.model';

@Component({
  selector: 'app-skill-selector',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div>
      <label class="block text-sm font-medium text-gray-700 mb-2">Select Subject</label>
      <select
        [(ngModel)]="selectedSkillId"
        (ngModelChange)="onSkillChange($event)"
        class="w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500">
        <option value="">Select a subject...</option>
        @for (skill of skills; track skill.id) {
          <option [value]="skill.id">{{ skill.name }}</option>
        }
      </select>
    </div>
  `
})
export class SkillSelectorComponent {
  @Input() skills: Skill[] = [];
  @Output() skillSelected = new EventEmitter<string>();

  selectedSkillId = '';

  onSkillChange(skillId: string): void {
    this.skillSelected.emit(skillId);
  }
}
