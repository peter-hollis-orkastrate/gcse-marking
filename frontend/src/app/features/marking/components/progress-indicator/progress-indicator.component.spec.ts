import { ComponentFixture, TestBed, fakeAsync, tick, discardPeriodicTasks } from '@angular/core/testing';
import { ProgressIndicatorComponent } from './progress-indicator.component';

describe('ProgressIndicatorComponent', () => {
  let component: ProgressIndicatorComponent;
  let fixture: ComponentFixture<ProgressIndicatorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ProgressIndicatorComponent]
    }).compileComponents();
  });

  afterEach(() => {
    if (component) {
      component.ngOnDestroy();
    }
  });

  it('should create', () => {
    fixture = TestBed.createComponent(ProgressIndicatorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
    expect(component).toBeTruthy();
  });

  it('should have initial message', () => {
    fixture = TestBed.createComponent(ProgressIndicatorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
    expect(component.currentMessage).toBe('Reading the essay...');
  });

  it('should have initial progress', () => {
    fixture = TestBed.createComponent(ProgressIndicatorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
    expect(component.progressPercentage).toBe(5);
  });

  it('should format time correctly for seconds only', () => {
    fixture = TestBed.createComponent(ProgressIndicatorComponent);
    component = fixture.componentInstance;
    component.elapsedSeconds = 45;
    expect(component.formattedTime).toBe('45s');
  });

  it('should format time correctly for minutes and seconds', () => {
    fixture = TestBed.createComponent(ProgressIndicatorComponent);
    component = fixture.componentInstance;
    component.elapsedSeconds = 90;
    expect(component.formattedTime).toBe('1m 30s');
  });

  it('should increment elapsed seconds', fakeAsync(() => {
    fixture = TestBed.createComponent(ProgressIndicatorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();

    expect(component.elapsedSeconds).toBe(0);

    tick(1000);
    expect(component.elapsedSeconds).toBe(1);

    tick(2000);
    expect(component.elapsedSeconds).toBe(3);

    // Clean up pending timers
    discardPeriodicTasks();
  }));
});
