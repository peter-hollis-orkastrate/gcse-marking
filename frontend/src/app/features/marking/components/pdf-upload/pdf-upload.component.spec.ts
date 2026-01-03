import { ComponentFixture, TestBed } from '@angular/core/testing';
import { PdfUploadComponent } from './pdf-upload.component';

describe('PdfUploadComponent', () => {
  let component: PdfUploadComponent;
  let fixture: ComponentFixture<PdfUploadComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PdfUploadComponent]
    }).compileComponents();

    fixture = TestBed.createComponent(PdfUploadComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should not have fileName initially', () => {
    expect(component.fileName).toBeNull();
  });

  it('should emit pdfCleared when cleared', () => {
    const spy = spyOn(component.pdfCleared, 'emit');
    component.fileName = 'test.pdf';

    component.onClear();

    expect(component.fileName).toBeNull();
    expect(spy).toHaveBeenCalled();
  });

  it('should handle drag over', () => {
    const event = new DragEvent('dragover');
    spyOn(event, 'preventDefault');
    spyOn(event, 'stopPropagation');

    component.onDragOver(event);

    expect(component.isDragOver).toBe(true);
  });

  it('should handle drag leave', () => {
    component.isDragOver = true;
    const event = new DragEvent('dragleave');
    spyOn(event, 'preventDefault');
    spyOn(event, 'stopPropagation');

    component.onDragLeave(event);

    expect(component.isDragOver).toBe(false);
  });
});
