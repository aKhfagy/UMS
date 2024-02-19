import {
  Component,
  OnInit,
  ViewEncapsulation,
  Injector,
} from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../shared/services/AuthService';

@Component({
  templateUrl: './account.component.html',
  encapsulation: ViewEncapsulation.None
})
export class AccountComponent implements OnInit {
  constructor(private router: Router, private authService: AuthService) {

  }

  ngOnInit(): void {

  }
}
