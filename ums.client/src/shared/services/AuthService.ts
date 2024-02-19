import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { ConfigService } from './ConfigService';
import { Result } from '../models/Result';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private currentUserSubject: BehaviorSubject<any>;
  public currentUser: Observable<any>;
  public baseUrl: string;

  constructor(private http: HttpClient, private configService: ConfigService) {
    this.currentUserSubject = new BehaviorSubject<any>(JSON.parse(localStorage.getItem('currentUser') || '{}'));
    this.currentUser = this.currentUserSubject.asObservable();
    this.baseUrl = this.configService.getApiUrl();
  }

  public get currentUserValue(): any {
    return this.currentUserSubject.value;
  }

  login(username: string, password: string) {
    return this.http.post<any>(`${this.baseUrl}/Identity/Login`, { username, password })
      .pipe(map(user => {
        localStorage.setItem('currentUser', JSON.stringify(user));
        this.currentUserSubject.next(user);
        return user;
      })
    );
  }

  refreshToken() {
    return this.http.post<any>(`${this.baseUrl}/Identity/RefreshToken`, this.currentUserSubject.value)
      .pipe(map(user => {
        localStorage.setItem('currentUser', JSON.stringify(user));
        this.currentUserSubject.next(user);
        return user;
      })
    );
  }

  revoke() {

  }

  isAdmin(): Observable<boolean> {
    return this.http.get<Result<boolean>>(`${this.baseUrl}/Identity/IsAdmin`).pipe(
      map((result: Result<boolean>) => result.data ?? false)
    );
  }

  logout() {
    this.http.post<any>(`${this.baseUrl}/Identity/Revoke`, "").subscribe()
    this.removeLocalStorage();
  }

  isAuthenticated(): boolean {
    return !!this.currentUserSubject.value &&
      !!this.currentUserSubject.value.accessToken &&
      !!this.currentUserSubject.value.refreshToken;
  }

  public getToken(): any {
    return this.currentUserSubject.value ? this.currentUserSubject.value.accessToken : null;
  }

  public getRefreshToken(): any {
    return this.currentUserSubject.value ? this.currentUserSubject.value.refreshToken : null;
  }

  public removeLocalStorage() {
    localStorage.removeItem('currentUser');
    this.currentUserSubject.next(null);
  }
}
