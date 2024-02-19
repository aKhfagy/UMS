import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AdminGuard } from './shared/auth.guard/admin.auth.guard';
import { ViewerGuard } from './shared/auth.guard/viewer.auth.guard';
import { LoginGuard } from './shared/auth.guard/login.auth.guard';

const routes: Routes = [
  { path: '', redirectTo: '/account', pathMatch: 'full' },
  {
    path: 'account',
    loadChildren: () => import('./account/account.module').then(m => m.AccountModule),
    data: { preload: true },
    canActivate: [LoginGuard]
  },
  {
    path: 'app',
    loadChildren: () => import('./app/app.module').then(m => m.AppModule),
    data: { preload: true },
    canActivate: [AdminGuard]
  },
  {
    path: 'viewer-app',
    loadChildren: () => import('./viewer.app/viewer.app.module').then(m => m.ViewerAppModule),
    data: { preload: true },
    canActivate: [ViewerGuard]
  },
  { path: '**', redirectTo: '/account', pathMatch: 'full' },
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule],
    providers: []
})
export class RootRoutingModule { }
