import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { HeaderComponent } from './header.component';
import { AuthService } from '../../../core/services/auth.service';
import { of } from 'rxjs';

describe('HeaderComponent', () => {
  let component: HeaderComponent;
  let fixture: ComponentFixture<HeaderComponent>;
  let mockAuthService: any;

  beforeEach(async () => {
    mockAuthService = {
      isAuthenticated: jasmine.createSpy('isAuthenticated').and.returnValue(true),
      user: jasmine.createSpy('user').and.returnValue({ email: 'test@test.com', name: 'Test' }),
      logout: jasmine.createSpy('logout').and.returnValue(of(undefined))
    };

    await TestBed.configureTestingModule({
      imports: [HeaderComponent, HttpClientTestingModule],
      providers: [
        { provide: AuthService, useValue: mockAuthService }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(HeaderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
