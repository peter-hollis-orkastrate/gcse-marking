import { ComponentFixture, TestBed } from '@angular/core/testing';
import { SkillSelectorComponent } from './skill-selector.component';
import { FormsModule } from '@angular/forms';

describe('SkillSelectorComponent', () => {
  let component: SkillSelectorComponent;
  let fixture: ComponentFixture<SkillSelectorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SkillSelectorComponent, FormsModule]
    }).compileComponents();

    fixture = TestBed.createComponent(SkillSelectorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should emit skill when selected', () => {
    const spy = spyOn(component.skillSelected, 'emit');
    component.skills = [
      { id: '1', name: 'English', description: 'English Lit', subject: 'English' }
    ];

    component.onSkillChange('1');

    expect(spy).toHaveBeenCalledWith('1');
  });
});
