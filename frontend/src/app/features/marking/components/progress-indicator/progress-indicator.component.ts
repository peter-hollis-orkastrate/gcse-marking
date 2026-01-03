import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Subscription, interval } from 'rxjs';

@Component({
  selector: 'app-progress-indicator',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="space-y-4 p-6 bg-white rounded-lg border border-gray-200">
      <div class="w-full bg-gray-200 rounded-full h-2 overflow-hidden">
        <div
          class="bg-blue-600 h-2 rounded-full transition-all duration-1000 ease-out"
          [style.width.%]="progressPercentage">
        </div>
      </div>

      <div class="flex items-center justify-center gap-3">
        <svg class="animate-spin h-5 w-5 text-blue-600" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24">
          <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4"></circle>
          <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
        </svg>
        <span class="text-sm text-gray-600">{{ currentMessage }}</span>
      </div>

      <p class="text-center text-xs text-gray-400">
        Elapsed: {{ formattedTime }} - This typically takes 90-120 seconds
      </p>
    </div>
  `
})
export class ProgressIndicatorComponent implements OnInit, OnDestroy {
  private readonly primaryMessages = [
    'Reading the essay...',
    'Analysing handwriting...',
    'Checking against marking scheme...',
    'Evaluating argument structure...',
    'Assessing use of quotations...',
    'Reviewing technical accuracy...',
    'Generating feedback...',
    'Almost done...',
  ];

  private readonly extendedMessages = [
    'Still working on detailed feedback...',
    'Taking extra care with this one...',
    'Nearly there, just polishing the feedback...',
    'Doing a thorough analysis...',
  ];

  currentMessage = this.primaryMessages[0];
  elapsedSeconds = 0;
  progressPercentage = 5;

  private messageIndex = 0;
  private timerSubscription?: Subscription;
  private messageSubscription?: Subscription;

  ngOnInit(): void {
    this.timerSubscription = interval(1000).subscribe(() => {
      this.elapsedSeconds++;
    });

    this.messageSubscription = interval(12000).subscribe(() => {
      this.messageIndex++;
      if (this.messageIndex < this.primaryMessages.length) {
        this.currentMessage = this.primaryMessages[this.messageIndex];
        this.progressPercentage = Math.min(((this.messageIndex + 1) / this.primaryMessages.length) * 95, 95);
      } else {
        const extendedIndex = (this.messageIndex - this.primaryMessages.length) % this.extendedMessages.length;
        this.currentMessage = this.extendedMessages[extendedIndex];
        this.progressPercentage = 95;
      }
    });
  }

  ngOnDestroy(): void {
    this.timerSubscription?.unsubscribe();
    this.messageSubscription?.unsubscribe();
  }

  get formattedTime(): string {
    const mins = Math.floor(this.elapsedSeconds / 60);
    const secs = this.elapsedSeconds % 60;
    return mins > 0 ? `${mins}m ${secs}s` : `${secs}s`;
  }
}
