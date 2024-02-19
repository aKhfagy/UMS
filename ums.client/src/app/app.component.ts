import {
  Component,
  OnInit,
  ViewEncapsulation,
  Injector,
} from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../shared/services/AuthService';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ReportService } from '../shared/services/ReportService';
import { saveAs } from 'file-saver';

@Component({
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
  encapsulation: ViewEncapsulation.None
})
export class AppComponent implements OnInit {
  constructor(
    private router: Router,
    private authService: AuthService,
    private reportService: ReportService,
    private _snackbar: MatSnackBar) {

  }

  ngOnInit(): void {

  }
  public logout() {
    this.authService.logout();
    this.router.navigate(['/account']);
    this._snackbar.open('Logout Succeful', 'Ok')
  }

  public getSummary() {
    this.reportService.getSummary().subscribe({
      next: (data: Blob) => {
        saveAs(data, 'Output.pdf');

        this._snackbar.open('Download Starting soon...', 'Ok')
      },
      error: (err) => {
        console.log(err);
      },
      complete: () => {

      }
    })
  }
  public getTables() {
    this.reportService.getTables().subscribe({
      next: (data: Blob) => {
        saveAs(data, 'Output.doc');

        this._snackbar.open('Download Starting soon...', 'Ok')
      },
      error: (err) => {
        console.log(err);
      },
      complete: () => {

      }
    })
  }
}
