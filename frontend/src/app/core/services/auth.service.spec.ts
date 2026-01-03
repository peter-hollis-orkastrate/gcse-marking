import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { AuthService } from './auth.service';
import { environment } from '../../../environments/environment';

describe('AuthService', () => {
  let service: AuthService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [AuthService]
    });
    service = TestBed.inject(AuthService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should check auth and set user', () => {
    const mockUser = { id: 1, email: 'test@test.com', name: 'Test', isAdmin: false };

    service.checkAuth().subscribe(user => {
      expect(user).toEqual(mockUser);
      expect(service.isAuthenticated()).toBe(true);
    });

    const req = httpMock.expectOne(`${environment.apiUrl}/auth/me`);
    expect(req.request.method).toBe('GET');
    req.flush(mockUser);
  });

  it('should handle auth check failure', () => {
    service.checkAuth().subscribe(user => {
      expect(user).toBeNull();
      expect(service.isAuthenticated()).toBe(false);
    });

    const req = httpMock.expectOne(`${environment.apiUrl}/auth/me`);
    req.error(new ErrorEvent('Network error'));
  });

  it('should logout and clear user', () => {
    // First set a user
    const mockUser = { id: 1, email: 'test@test.com', name: 'Test', isAdmin: false };
    service.checkAuth().subscribe();
    httpMock.expectOne(`${environment.apiUrl}/auth/me`).flush(mockUser);

    // Then logout
    service.logout().subscribe(() => {
      expect(service.isAuthenticated()).toBe(false);
      expect(service.user()).toBeNull();
    });

    const req = httpMock.expectOne(`${environment.apiUrl}/auth/logout`);
    expect(req.request.method).toBe('POST');
    req.flush({});
  });
});
