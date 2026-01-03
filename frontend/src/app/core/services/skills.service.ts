import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { Skill } from '../models/skill.model';
import { environment } from '../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class SkillsService {
  constructor(private http: HttpClient) {}

  getSkills(): Observable<Skill[]> {
    return this.http.get<{ skills: Skill[] }>(`${environment.apiUrl}/skills`).pipe(
      map(response => response.skills)
    );
  }
}
