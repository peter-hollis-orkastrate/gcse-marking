import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [CommonModule, RouterLink],
  template: `
    <header class="bg-white shadow-sm border-b">
      <div class="max-w-4xl mx-auto px-4 py-4 flex justify-between items-center">
        <h1 class="text-xl font-semibold text-gray-800">GCSE Essay Marker</h1>
        @if (authService.isAuthenticated()) {
          <div class="flex items-center gap-4">
            @if (authService.user()?.isAdmin) {
              <a routerLink="/admin" class="text-sm text-purple-600 hover:text-purple-800 font-medium">Admin</a>
            }
            <span class="text-sm text-gray-600">{{ authService.user()?.email }}</span>
            <button (click)="logout()" class="text-sm text-blue-600 hover:text-blue-800">Logout</button>
          </div>
        }
      </div>
    </header>
  `
})
export class HeaderComponent {
  authService = inject(AuthService);

  logout(): void {
    this.authService.logout().subscribe(() => {
      window.location.href = '/login';
    });
  }
}
