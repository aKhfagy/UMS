import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Subject } from '../models/Subject';
import { ConfigService } from './ConfigService';
import { AuthService } from './AuthService';
import { Result } from '../models/Result';
import { IsPagedOutputObj } from '../models/IsPagedOutputObj';

@Injectable({
  providedIn: 'root'
})
export class SubjectService {
  private apiUrl: string;

  constructor(
    private http: HttpClient,
    private configService: ConfigService,
    private authService: AuthService) {
    this.apiUrl = this.configService.getApiUrl();
  }

  getAll(): Observable<Result<Subject[]>> {
    return this.http.get<Result<Subject[]>>(`${this.apiUrl}/Subject/GetAll`);
  }
  isPaged(page: number, isDeleted: boolean): Observable<Result<IsPagedOutputObj<Subject>>> {
    return this.http.put<Result<IsPagedOutputObj<Subject>>>(`${this.apiUrl}/Subject/IsPaged`,
      {
        pageIndex: page,
        pageSize: 5,
        isDeleted: isDeleted
      });
  }

  delete(id: number): Observable<Result<boolean>> {
    let body = {
      objId: id,
    };
    return this.http.post<Result<boolean>>(
      `${this.apiUrl}/Subject/SoftDelete?id=${id}`, "");
  }
  restore(id: number): Observable<Result<boolean>> {
    return this.http.post<Result<boolean>>(
      `${this.apiUrl}/Subject/Restore?id=${id}`, "");
  }

  add(subject: Subject): Observable<Result<Subject>> {
    return this.http.post<Result<Subject>>(
      `${this.apiUrl}/Subject/Add`, subject);
  }

  edit(subject: Subject): Observable<Result<Subject>> {
    return this.http.put<Result<Subject>>(
      `${this.apiUrl}/Subject/Edit`, subject);
  }
}
