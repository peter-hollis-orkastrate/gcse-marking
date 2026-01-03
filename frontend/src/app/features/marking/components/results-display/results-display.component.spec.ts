import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ResultsDisplayComponent } from './results-display.component';

describe('ResultsDisplayComponent', () => {
  let component: ResultsDisplayComponent;
  let fixture: ComponentFixture<ResultsDisplayComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ResultsDisplayComponent]
    }).compileComponents();

    fixture = TestBed.createComponent(ResultsDisplayComponent);
    component = fixture.componentInstance;
    component.result = {
      raw: '## Feedback\n**Good work!**',
      gradeBand: 'Band 4',
      timestamp: new Date(),
      question: 'What is the theme?'
    };
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should render markdown', () => {
    const rendered = component.renderedFeedback;
    expect(rendered).toContain('<h2');
    expect(rendered).toContain('<strong');
  });

  it('should emit reset event', () => {
    const spy = spyOn(component.onReset, 'emit');

    component.onReset.emit();

    expect(spy).toHaveBeenCalled();
  });

  it('should call window.print on print', () => {
    spyOn(window, 'print');

    component.print();

    expect(window.print).toHaveBeenCalled();
  });
});
