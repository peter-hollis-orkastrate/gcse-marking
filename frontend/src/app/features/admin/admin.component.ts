import { Component, OnInit, signal } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { AdminService, AdminUser, UsageStats } from '../../core/services/admin.service';

@Component({
  selector: 'app-admin',
  standalone: true,
  imports: [CommonModule, FormsModule, DatePipe, RouterLink],
  template: `
    <div class="min-h-screen bg-gray-50">
      <header class="bg-white shadow-sm border-b">
        <div class="max-w-6xl mx-auto px-4 py-4 flex justify-between items-center">
          <h1 class="text-xl font-semibold text-gray-800">Admin Dashboard</h1>
          <a routerLink="/" class="text-sm text-blue-600 hover:text-blue-800">Back to Marker</a>
        </div>
      </header>

      <main class="max-w-6xl mx-auto px-4 py-8">
        <!-- Usage Stats Section -->
        <section class="mb-8">
          <div class="flex justify-between items-center mb-4">
            <h2 class="text-lg font-semibold text-gray-800">Usage Statistics</h2>
            <select [(ngModel)]="selectedDays" (change)="loadUsage()"
                    class="border rounded px-3 py-1 text-sm">
              <option [value]="7">Last 7 days</option>
              <option [value]="30">Last 30 days</option>
              <option [value]="90">Last 90 days</option>
            </select>
          </div>

          @if (usage()) {
            <div class="grid grid-cols-1 md:grid-cols-3 gap-4 mb-6">
              <div class="bg-white rounded-lg shadow p-4">
                <p class="text-sm text-gray-500">Total Essays Marked</p>
                <p class="text-2xl font-bold text-blue-600">{{ usage()!.summary.totalMarks }}</p>
              </div>
              <div class="bg-white rounded-lg shadow p-4">
                <p class="text-sm text-gray-500">Unique Users</p>
                <p class="text-2xl font-bold text-green-600">{{ usage()!.summary.uniqueUsers }}</p>
              </div>
              <div class="bg-white rounded-lg shadow p-4">
                <p class="text-sm text-gray-500">Period</p>
                <p class="text-2xl font-bold text-gray-700">{{ usage()!.summary.periodDays }} days</p>
              </div>
            </div>

            <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
              <!-- By Skill -->
              <div class="bg-white rounded-lg shadow p-4">
                <h3 class="font-medium text-gray-700 mb-3">Usage by Skill</h3>
                @if (usage()!.bySkill.length === 0) {
                  <p class="text-sm text-gray-500">No usage data yet</p>
                } @else {
                  <ul class="space-y-2">
                    @for (skill of usage()!.bySkill; track skill.skillId) {
                      <li class="flex justify-between text-sm">
                        <span class="text-gray-600">{{ skill.skillId }}</span>
                        <span class="font-medium">{{ skill.count }}</span>
                      </li>
                    }
                  </ul>
                }
              </div>

              <!-- Top Users -->
              <div class="bg-white rounded-lg shadow p-4">
                <h3 class="font-medium text-gray-700 mb-3">Top Users</h3>
                @if (usage()!.topUsers.length === 0) {
                  <p class="text-sm text-gray-500">No usage data yet</p>
                } @else {
                  <ul class="space-y-2">
                    @for (user of usage()!.topUsers; track user.email) {
                      <li class="flex justify-between text-sm">
                        <span class="text-gray-600 truncate max-w-[200px]">{{ user.email }}</span>
                        <span class="font-medium">{{ user.count }}</span>
                      </li>
                    }
                  </ul>
                }
              </div>
            </div>
          }
        </section>

        <!-- User Management Section -->
        <section>
          <div class="flex justify-between items-center mb-4">
            <h2 class="text-lg font-semibold text-gray-800">User Management</h2>
            <button (click)="showAddForm = true"
                    class="px-4 py-2 bg-blue-600 text-white text-sm font-medium rounded hover:bg-blue-700">
              Add User
            </button>
          </div>

          <!-- Add User Form -->
          @if (showAddForm) {
            <div class="bg-white rounded-lg shadow p-4 mb-4">
              <h3 class="font-medium text-gray-700 mb-3">Add New User</h3>
              <div class="flex gap-3">
                <input type="email" [(ngModel)]="newUserEmail" placeholder="Email address"
                       class="flex-1 border rounded px-3 py-2 text-sm" />
                <input type="text" [(ngModel)]="newUserName" placeholder="Name (optional)"
                       class="flex-1 border rounded px-3 py-2 text-sm" />
                <button (click)="addUser()" [disabled]="!newUserEmail"
                        class="px-4 py-2 bg-green-600 text-white text-sm font-medium rounded hover:bg-green-700 disabled:opacity-50">
                  Add
                </button>
                <button (click)="showAddForm = false; newUserEmail = ''; newUserName = ''"
                        class="px-4 py-2 border text-gray-700 text-sm font-medium rounded hover:bg-gray-50">
                  Cancel
                </button>
              </div>
              @if (addError) {
                <p class="text-red-600 text-sm mt-2">{{ addError }}</p>
              }
            </div>
          }

          <!-- Users Table -->
          <div class="bg-white rounded-lg shadow overflow-hidden">
            <table class="min-w-full divide-y divide-gray-200">
              <thead class="bg-gray-50">
                <tr>
                  <th class="px-4 py-3 text-left text-xs font-medium text-gray-500 uppercase">Email</th>
                  <th class="px-4 py-3 text-left text-xs font-medium text-gray-500 uppercase">Name</th>
                  <th class="px-4 py-3 text-left text-xs font-medium text-gray-500 uppercase">Created</th>
                  <th class="px-4 py-3 text-left text-xs font-medium text-gray-500 uppercase">Last Login</th>
                  <th class="px-4 py-3 text-left text-xs font-medium text-gray-500 uppercase">Usage</th>
                  <th class="px-4 py-3 text-left text-xs font-medium text-gray-500 uppercase">Status</th>
                  <th class="px-4 py-3 text-left text-xs font-medium text-gray-500 uppercase">Admin</th>
                  <th class="px-4 py-3 text-left text-xs font-medium text-gray-500 uppercase">Actions</th>
                </tr>
              </thead>
              <tbody class="divide-y divide-gray-200">
                @for (user of users(); track user.id) {
                  <tr>
                    <td class="px-4 py-3 text-sm text-gray-900">{{ user.email }}</td>
                    <td class="px-4 py-3 text-sm text-gray-500">{{ user.name || '-' }}</td>
                    <td class="px-4 py-3 text-sm text-gray-500">{{ user.createdAt | date:'shortDate' }}</td>
                    <td class="px-4 py-3 text-sm text-gray-500">{{ user.lastLoginAt ? (user.lastLoginAt | date:'short') : 'Never' }}</td>
                    <td class="px-4 py-3 text-sm text-gray-500">{{ user.usageCount }}</td>
                    <td class="px-4 py-3">
                      <button (click)="toggleEnabled(user)"
                              [class]="user.enabled ? 'bg-green-100 text-green-800' : 'bg-red-100 text-red-800'"
                              class="px-2 py-1 rounded text-xs font-medium">
                        {{ user.enabled ? 'Enabled' : 'Disabled' }}
                      </button>
                    </td>
                    <td class="px-4 py-3">
                      <button (click)="toggleAdmin(user)"
                              [class]="user.isAdmin ? 'bg-purple-100 text-purple-800' : 'bg-gray-100 text-gray-600'"
                              class="px-2 py-1 rounded text-xs font-medium">
                        {{ user.isAdmin ? 'Yes' : 'No' }}
                      </button>
                    </td>
                    <td class="px-4 py-3">
                      <button (click)="deleteUser(user)"
                              class="text-red-600 hover:text-red-800 text-sm">
                        Delete
                      </button>
                    </td>
                  </tr>
                }
              </tbody>
            </table>
          </div>
        </section>
      </main>
    </div>
  `
})
export class AdminComponent implements OnInit {
  users = signal<AdminUser[]>([]);
  usage = signal<UsageStats | null>(null);
  selectedDays = 30;
  showAddForm = false;
  newUserEmail = '';
  newUserName = '';
  addError = '';

  constructor(private adminService: AdminService) {}

  ngOnInit(): void {
    this.loadUsers();
    this.loadUsage();
  }

  loadUsers(): void {
    this.adminService.getUsers().subscribe({
      next: (response) => this.users.set(response.users),
      error: (err) => console.error('Failed to load users:', err)
    });
  }

  loadUsage(): void {
    this.adminService.getUsage(this.selectedDays).subscribe({
      next: (stats) => this.usage.set(stats),
      error: (err) => console.error('Failed to load usage:', err)
    });
  }

  addUser(): void {
    if (!this.newUserEmail) return;
    this.addError = '';

    this.adminService.addUser(this.newUserEmail, this.newUserName || undefined).subscribe({
      next: () => {
        this.showAddForm = false;
        this.newUserEmail = '';
        this.newUserName = '';
        this.loadUsers();
      },
      error: (err) => {
        this.addError = err.error?.error || 'Failed to add user';
      }
    });
  }

  toggleEnabled(user: AdminUser): void {
    this.adminService.updateUser(user.id, { enabled: !user.enabled }).subscribe({
      next: () => this.loadUsers(),
      error: (err) => console.error('Failed to update user:', err)
    });
  }

  toggleAdmin(user: AdminUser): void {
    this.adminService.updateUser(user.id, { isAdmin: !user.isAdmin }).subscribe({
      next: () => this.loadUsers(),
      error: (err) => console.error('Failed to update user:', err)
    });
  }

  deleteUser(user: AdminUser): void {
    if (!confirm(`Are you sure you want to delete ${user.email}?`)) return;

    this.adminService.deleteUser(user.id).subscribe({
      next: () => this.loadUsers(),
      error: (err) => {
        alert(err.error?.error || 'Failed to delete user');
      }
    });
  }
}
