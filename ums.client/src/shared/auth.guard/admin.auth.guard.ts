import { Injectable } from '@angular/core';
import { CanActivate, Route, Router } from '@angular/router';
import { AuthService } from '../services/AuthService';
import { Observable, catchError, map, of } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AdminGuard implements CanActivate {

  constructor(
    private router: Router,
    private authService: AuthService) { }

  canActivate(): Observable<boolean> | boolean {
    if (this.authService.isAuthenticated()) {
      return this.authService.isAdmin().pipe(
        map((isAdmin: boolean) => {
          if (!isAdmin) {
            this.router.navigate(['viewer-app']);
          }
          return isAdmin;
        }),
        catchError((error: any) => {
          console.log(error);
          this.router.navigate(['account']);
          return of(false) as Observable<boolean>;
        })
      );
    }

    this.router.navigate(['account'])
    return false;
  }
}
