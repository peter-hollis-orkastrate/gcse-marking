import { ComponentFixture, TestBed } from '@angular/core/testing';
import { QuestionInputComponent } from './question-input.component';
import { FormsModule } from '@angular/forms';

describe('QuestionInputComponent', () => {
  let component: QuestionInputComponent;
  let fixture: ComponentFixture<QuestionInputComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [QuestionInputComponent, FormsModule]
    }).compileComponents();

    fixture = TestBed.createComponent(QuestionInputComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should emit question change', () => {
    const spy = spyOn(component.questionChanged, 'emit');

    component.onQuestionChange('What is the theme?');

    expect(spy).toHaveBeenCalledWith('What is the theme?');
  });

  it('should use default placeholder', () => {
    expect(component.placeholder).toContain('Enter the essay question');
  });
});
