import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { MarkingResult } from '../models/marking-result.model';
import { environment } from '../../../environments/environment';

interface MarkResponse {
  success: boolean;
  feedback?: MarkingResult;
  error?: string;
}

@Injectable({ providedIn: 'root' })
export class MarkingService {
  constructor(private http: HttpClient) {}

  markEssay(skillId: string, question: string, pdfBase64: string): Observable<MarkingResult> {
    return this.http.post<MarkResponse>(
      `${environment.apiUrl}/mark`,
      { skillId, question, pdfBase64 }
    ).pipe(
      map(response => {
        if (!response.success || !response.feedback) {
          throw new Error(response.error || 'Failed to mark essay');
        }
        return { ...response.feedback, question };
      })
    );
  }
}
