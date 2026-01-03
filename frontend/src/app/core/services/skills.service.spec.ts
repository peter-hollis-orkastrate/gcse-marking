import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { SkillsService } from './skills.service';
import { environment } from '../../../environments/environment';

describe('SkillsService', () => {
  let service: SkillsService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [SkillsService]
    });
    service = TestBed.inject(SkillsService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should fetch skills', () => {
    const mockSkills = [
      { id: '1', name: 'English Literature', description: 'GCSE English Lit', subject: 'English' },
      { id: '2', name: 'History', description: 'GCSE History', subject: 'History' }
    ];

    service.getSkills().subscribe(skills => {
      expect(skills).toEqual(mockSkills);
      expect(skills.length).toBe(2);
    });

    const req = httpMock.expectOne(`${environment.apiUrl}/skills`);
    expect(req.request.method).toBe('GET');
    req.flush({ skills: mockSkills });
  });
});
