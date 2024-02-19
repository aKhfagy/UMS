import { Component, Injector, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../shared/services/AuthService';
import { MatSnackBar } from '@angular/material/snack-bar';
import { AbstractControl, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { UserProxy } from '../../shared/services/UserService';

@Component({
  templateUrl: './register.component.html',
  styleUrl: './register.component.css',
})
export class RegisterComponent implements OnInit {
  submitting = false;
  public hide = true;
  public form: FormGroup;
  constructor(
    private router: Router,
    private fb: FormBuilder,
    private _userProxy: UserProxy,
    private _snackbar: MatSnackBar) {
    this.form = this.fb.group({
      username: ['', [Validators.required, Validators.minLength(6)]],
      email: ['', [Validators.required,  Validators.email]],
      password: ['', [Validators.required, Validators.pattern(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[#$^+=!*()@%&]).{8,}$/gm)]],
      rePassword: ['', [Validators.required, Validators.pattern(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[#$^+=!*()@%&]).{8,}$/gm)]]
    }, {
      Validators: this.passwordMatchValidator
    })
  }
  ngOnInit(): void {

  }

  passwordMatchValidator(control: AbstractControl): { [key: string]: boolean } | null {
    const password = control.get('password');
    const rePassword = control.get('rePassword');

    if (password && rePassword && password.value !== rePassword.value) {
      return { 'passwordMismatch': true };
    }

    return null;
  }

  backBtn() {
    this.router.navigate(['/account']);
  }

  get username() {
    return this.form.get('username');
  }
  get email() {
    return this.form.get('email');
  }
  get password() {
    return this.form.get('password');
  }

  register() {
    this._userProxy.register(
      this.username?.value,
      this.password?.value,
      this.email?.value).subscribe({
      error: (err) => console.log(err),
      complete: () => {
        this._snackbar.open("Successful Operation", "OK");
        this.backBtn();
      }
    })
  }
}
