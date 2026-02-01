import { Routes } from '@angular/router';
import { MainLayoutComponent } from './core/layout/main-layout/main-layout.component';
import { DashboardComponent } from './features/dashboard/pages/dashboard.component';
import { BackgammonPageComponent } from './features/backgammon/pages/backgammon-page/backgammon-page.component';

export const routes: Routes = [
  {
    path: '',
    component: MainLayoutComponent,
    children: [
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
      { path: 'dashboard', component: DashboardComponent },
      { path: 'backgammon', component: BackgammonPageComponent },
    ],
  },
  {
    path: '**',
    redirectTo: 'dashboard',
  },
];
