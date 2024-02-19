import { HttpClient } from '@angular/common/http';
import { Injectable, Inject, Optional, InjectionToken } from '@angular/core';
import { ConfigService } from './ConfigService';
import { Result } from '../models/Result';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserProxy {
  constructor(private http: HttpClient, private configService: ConfigService) {}

  register(username: string, password: string, email: string): Observable<any> {
    return this.http.post<any>(`${this.configService.getApiUrl()}/Identity/RegisterViewer`, {
      username: username,
      password: password,
      email: email,
    });
  }
}
