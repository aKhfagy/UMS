import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Teacher } from '../models/Teacher';
import { Subject } from '../models/Subject';
import { ConfigService } from './ConfigService';
import { AuthService } from './AuthService';
import { Result } from '../models/Result';
import { IsPagedOutputObj } from '../models/IsPagedOutputObj';

@Injectable({
  providedIn: 'root'
})
export class TeacherService {
  private apiUrl: string;

  constructor(private http: HttpClient,
    private configService: ConfigService,
    private authService: AuthService) {
    this.apiUrl = this.configService.getApiUrl();
  }

  getAll(): Observable<Result<Teacher[]>> {
    return this.http.get<Result<Teacher[]>>(`${this.apiUrl}/Teacher/GetAll`);
  }
  isPaged(page: number, isDeleted: boolean): Observable<Result<IsPagedOutputObj<Teacher>>> {
    return this.http.put<Result<IsPagedOutputObj<Teacher>>>(`${this.apiUrl}/Teacher/IsPaged`,
      {
        pageIndex: page,
        pageSize: 5,
        isDeleted: isDeleted
      });
  }
  getSubjects(id: number): Observable<Result<Subject[]>> {
    return this.http.get<Result<Subject[]>>(`${this.apiUrl}/Teacher/GetSubjects?id=${id}`);
  }

  assign(teacherId: number, subjectId: number): Observable<Result<boolean>> {
    return this.http.post<Result<boolean>>(
      `${this.apiUrl}/Teacher/Assign?teacherId=${teacherId}&subjectId=${subjectId}`, '');
  }

  delete(id: number): Observable<Result<boolean>> {
    let body = {
      objId: id,
    };
    return this.http.post<Result<boolean>>(
      `${this.apiUrl}/Teacher/SoftDelete?id=${id}`, "");
  }
  restore(id: number): Observable<Result<boolean>> {
    return this.http.post<Result<boolean>>(
      `${this.apiUrl}/Teacher/Restore?id=${id}`, "");
  }

  add(teacher: Teacher): Observable<Result<Teacher>> {
    return this.http.post<Result<Teacher>>(
      `${this.apiUrl}/Teacher/Add`, teacher);
  }

  edit(teacher: Teacher): Observable<Result<Teacher>> {
    return this.http.put<Result<Teacher>>(
      `${this.apiUrl}/Teacher/Edit`, teacher);
  }
}
