import { Component, Injector, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../shared/services/AuthService';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  templateUrl: './login.component.html',
  styleUrl: './login.component.css',
})
export class LoginComponent implements OnInit {
  submitting = false;
  public hide = true;
  constructor(
    private router: Router,
    private _authService: AuthService,
    private _snackbar: MatSnackBar) {
  }
  ngOnInit(): void {

  }


  public login(username: string, password: string) {
    this._authService.login(username, password).subscribe({
      next: () => {
        this.router.navigate(['/app']);
        this._snackbar.open('Login Successful', 'Ok');
      },
      error: (error) => {
        console.log(error);
        this._snackbar.open('Username or password is incorrect', 'Ok');
      }
    })
  }


  registerBtn() {
    this.router.navigate(['/account/register']);
  }
}
