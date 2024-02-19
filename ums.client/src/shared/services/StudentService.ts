import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Student } from '../models/Student';
import { ConfigService } from './ConfigService';
import { AuthService } from './AuthService';
import { Result } from '../models/Result';
import { StudentSubject } from '../models/StudentSubject';
import { IsPagedOutputObj } from '../models/IsPagedOutputObj';

@Injectable({
  providedIn: 'root'
})
export class StudentService {
  private apiUrl: string;

  constructor(
    private http: HttpClient,
    private configService: ConfigService,
    private authService: AuthService) {
    this.apiUrl = this.configService.getApiUrl();
  }

  getAll(): Observable<Result<Student[]>> {
    return this.http.get<Result<Student[]>>(`${this.apiUrl}/Student/GetAll`);
  }
  isPaged(page: number, isDeleted: boolean): Observable<Result<IsPagedOutputObj<Student>>> {
    return this.http.put<Result<IsPagedOutputObj<Student>>>(`${this.apiUrl}/Student/IsPaged`, 
      {
        pageIndex: page,
        pageSize: 5,
        isDeleted: isDeleted
      });
  }

  getSubjects(id: number): Observable<Result<StudentSubject[]>> {
    return this.http.get<Result<StudentSubject[]>>(`${this.apiUrl}/Student/GetSubjects?id=${id}`);
  }

  enroll(studentId: number, subjectId: number): Observable<Result<boolean>> {
    return this.http.post<Result<boolean>>(
      `${this.apiUrl}/Student/Enroll?studentId=${studentId}&subjectId=${subjectId}`,"");
  }

  delete(id: number): Observable<Result<boolean>> {
    return this.http.post<Result<boolean>>(
      `${this.apiUrl}/Student/SoftDelete?id=${id}`, "");
  }

  restore(id: number): Observable<Result<boolean>> {
    return this.http.post<Result<boolean>>(
      `${this.apiUrl}/Student/Restore?id=${id}`, "");
  }

  add(student: Student): Observable<Result<Student>> {
    return this.http.post<Result<Student>>(
      `${this.apiUrl}/Student/Add`,  student );
  }

  edit(student: Student): Observable<Result<Student>> {
    return this.http.put<Result<Student>>(
      `${this.apiUrl}/Student/Edit`, student );
  }
}
