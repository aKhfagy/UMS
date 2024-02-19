import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Student } from '../models/Student';
import { ConfigService } from './ConfigService';
import { AuthService } from './AuthService';
import { Result } from '../models/Result';
import { StudentSubject } from '../models/StudentSubject';

@Injectable({
  providedIn: 'root'
})
export class ReportService {
  private apiUrl: string;

  constructor(
    private http: HttpClient,
    private configService: ConfigService) {
    this.apiUrl = this.configService.getApiUrl();
  }

  getSummary(): Observable<Blob> {
    return this.http.get(`${this.apiUrl}/Report/Summary`, { responseType: 'blob' });
  }
  getTables(): Observable<Blob> {
    return this.http.get(`${this.apiUrl}/Report/Tables`, { responseType: 'blob' });
  }
}
