import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { AuthService } from '../services/AuthService';

@Injectable({
  providedIn: 'root'
})
export class LoginGuard implements CanActivate {

  constructor(
    private router: Router,
    private authService: AuthService) { }

  canActivate(): boolean {
    if (this.authService.isAuthenticated())
      this.router.navigate(['app'])
    return !this.authService.isAuthenticated();
  }
}
