import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { AppComponent } from './app.component';
import { AuthService } from './core/services/auth.service';
import { of } from 'rxjs';

describe('AppComponent', () => {
  let mockAuthService: jasmine.SpyObj<AuthService>;

  beforeEach(async () => {
    mockAuthService = jasmine.createSpyObj('AuthService', ['checkAuth']);
    mockAuthService.checkAuth.and.returnValue(of(null));

    await TestBed.configureTestingModule({
      imports: [AppComponent, HttpClientTestingModule, RouterTestingModule],
      providers: [
        { provide: AuthService, useValue: mockAuthService }
      ]
    }).compileComponents();
  });

  it('should create the app', () => {
    const fixture = TestBed.createComponent(AppComponent);
    const app = fixture.componentInstance;
    expect(app).toBeTruthy();
  });

  it('should check auth on init', () => {
    const fixture = TestBed.createComponent(AppComponent);
    fixture.detectChanges();
    expect(mockAuthService.checkAuth).toHaveBeenCalled();
  });
});
