import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { ViewerAppComponent } from './viewer.app.component';
import { RouterModule } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButton, MatButtonModule } from '@angular/material/button';

@NgModule({
  imports: [
    CommonModule,
    RouterModule.forChild(
      [
        { path: '', component: ViewerAppComponent },
        { path: '**', component: ViewerAppComponent },
      ],),
    MatCardModule,
    MatButton,
    MatButtonModule,
  ],
  declarations: [
    ViewerAppComponent
  ]
})
export class ViewerAppModule {

}
