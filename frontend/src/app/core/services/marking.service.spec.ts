import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { MarkingService } from './marking.service';
import { environment } from '../../../environments/environment';

describe('MarkingService', () => {
  let service: MarkingService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [MarkingService]
    });
    service = TestBed.inject(MarkingService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should mark essay successfully', () => {
    const mockResponse = {
      success: true,
      feedback: {
        raw: '## Feedback\nGood essay.',
        gradeBand: 'Band 4',
        timestamp: new Date()
      }
    };

    service.markEssay('skill-1', 'What is the theme?', 'base64pdf').subscribe(result => {
      expect(result.gradeBand).toBe('Band 4');
      expect(result.raw).toContain('Feedback');
      expect(result.question).toBe('What is the theme?');
    });

    const req = httpMock.expectOne(`${environment.apiUrl}/mark`);
    expect(req.request.method).toBe('POST');
    expect(req.request.body).toEqual({
      skillId: 'skill-1',
      question: 'What is the theme?',
      pdfBase64: 'base64pdf'
    });
    req.flush(mockResponse);
  });

  it('should handle marking error', () => {
    const mockResponse = {
      success: false,
      error: 'Failed to process PDF'
    };

    service.markEssay('skill-1', 'Question', 'pdf').subscribe({
      error: (err) => {
        expect(err.message).toBe('Failed to process PDF');
      }
    });

    const req = httpMock.expectOne(`${environment.apiUrl}/mark`);
    req.flush(mockResponse);
  });
});
