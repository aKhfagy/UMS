import {
  Component,
  OnInit
} from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../shared/services/AuthService'
@Component({
  templateUrl: './viewer.app.component.html',
  styleUrl: './viewer.app.component.css'
})
export class ViewerAppComponent implements OnInit {
  constructor(
    private router: Router,
    private _authService: AuthService) {

  }

  ngOnInit(): void {

  }

  logout() {
    this._authService.logout();
    this.router.navigate(['/account']);
  }
}
