import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { RootComponent } from './root.component';
import { RootRoutingModule } from './root-routing.module';
import { AuthService } from './shared/services/AuthService';
import { JwtInterceptor } from './shared/services/JwtInterceptor';

@NgModule({
  imports: [
    BrowserModule,
    RouterModule,
    BrowserAnimationsModule,
    HttpClientModule,
    RootRoutingModule,
  ],
  declarations: [RootComponent],
  bootstrap: [RootComponent],
  providers: [
    AuthService,
    {
      provide: HTTP_INTERCEPTORS,
      useFactory: (authService: AuthService) => new JwtInterceptor(authService),
      deps: [AuthService],
      multi: true
    }
  ],
})
export class RootModule {}
