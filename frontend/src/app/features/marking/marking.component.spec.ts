import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { MarkingComponent } from './marking.component';
import { SkillsService } from '../../core/services/skills.service';
import { MarkingService } from '../../core/services/marking.service';
import { of } from 'rxjs';

describe('MarkingComponent', () => {
  let component: MarkingComponent;
  let fixture: ComponentFixture<MarkingComponent>;
  let skillsServiceSpy: jasmine.SpyObj<SkillsService>;
  let markingServiceSpy: jasmine.SpyObj<MarkingService>;

  beforeEach(async () => {
    skillsServiceSpy = jasmine.createSpyObj('SkillsService', ['getSkills']);
    markingServiceSpy = jasmine.createSpyObj('MarkingService', ['markEssay']);

    skillsServiceSpy.getSkills.and.returnValue(of([
      { id: '1', name: 'English', description: 'English Lit', subject: 'English' }
    ]));

    await TestBed.configureTestingModule({
      imports: [MarkingComponent, HttpClientTestingModule],
      providers: [
        { provide: SkillsService, useValue: skillsServiceSpy },
        { provide: MarkingService, useValue: markingServiceSpy }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(MarkingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load skills on init', () => {
    expect(skillsServiceSpy.getSkills).toHaveBeenCalled();
    expect(component.skills.length).toBe(1);
  });

  it('should start in form state', () => {
    expect(component.state).toBe('form');
  });

  it('should not allow submit without all fields', () => {
    expect(component.canSubmit).toBe(false);
  });

  it('should allow submit when all fields are filled', () => {
    component.selectedSkillId = '1';
    component.question = 'What is the theme?';
    component.pdfBase64 = 'base64data';

    expect(component.canSubmit).toBe(true);
  });

  it('should update state on skill selection', () => {
    component.onSkillSelected('1');
    expect(component.selectedSkillId).toBe('1');
  });

  it('should update state on question change', () => {
    component.onQuestionChanged('My question');
    expect(component.question).toBe('My question');
  });

  it('should update state on pdf selection', () => {
    component.onPdfSelected({ base64: 'data', name: 'test.pdf' });
    expect(component.pdfBase64).toBe('data');
  });

  it('should clear pdf on pdfCleared', () => {
    component.pdfBase64 = 'data';
    component.onPdfCleared();
    expect(component.pdfBase64).toBe('');
  });

  it('should reset to form state', () => {
    component.state = 'results';
    component.selectedSkillId = '1';
    component.question = 'test';
    component.pdfBase64 = 'data';

    component.reset();

    expect(component.state).toBe('form');
    expect(component.selectedSkillId).toBe('');
    expect(component.question).toBe('');
    expect(component.pdfBase64).toBe('');
  });

  it('should submit for marking', () => {
    markingServiceSpy.markEssay.and.returnValue(of({
      raw: 'Feedback',
      gradeBand: 'Band 4',
      timestamp: new Date()
    }));

    component.selectedSkillId = '1';
    component.question = 'Question';
    component.pdfBase64 = 'data';

    component.submitForMarking();

    expect(component.state).toBe('results');
    expect(component.result).toBeTruthy();
  });
});
