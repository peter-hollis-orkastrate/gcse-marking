import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

export interface AdminUser {
  id: number;
  email: string;
  name: string | null;
  enabled: boolean;
  isAdmin: boolean;
  createdAt: string;
  lastLoginAt: string | null;
  usageCount: number;
}

export interface UsageSummary {
  totalMarks: number;
  uniqueUsers: number;
  periodDays: number;
}

export interface SkillUsage {
  skillId: string;
  count: number;
}

export interface TopUser {
  email: string;
  count: number;
}

export interface UsageStats {
  summary: UsageSummary;
  bySkill: SkillUsage[];
  topUsers: TopUser[];
}

@Injectable({ providedIn: 'root' })
export class AdminService {
  constructor(private http: HttpClient) {}

  getUsers(): Observable<{ users: AdminUser[] }> {
    return this.http.get<{ users: AdminUser[] }>(`${environment.apiUrl}/admin/users`);
  }

  addUser(email: string, name?: string): Observable<{ success: boolean; user: { id: number; email: string; name: string | null } }> {
    return this.http.post<{ success: boolean; user: { id: number; email: string; name: string | null } }>(
      `${environment.apiUrl}/admin/users`,
      { email, name }
    );
  }

  updateUser(id: number, updates: { enabled?: boolean; isAdmin?: boolean }): Observable<{ success: boolean }> {
    return this.http.patch<{ success: boolean }>(`${environment.apiUrl}/admin/users/${id}`, updates);
  }

  deleteUser(id: number): Observable<{ success: boolean }> {
    return this.http.delete<{ success: boolean }>(`${environment.apiUrl}/admin/users/${id}`);
  }

  getUsage(days: number = 30): Observable<UsageStats> {
    return this.http.get<UsageStats>(`${environment.apiUrl}/admin/usage?days=${days}`);
  }
}
