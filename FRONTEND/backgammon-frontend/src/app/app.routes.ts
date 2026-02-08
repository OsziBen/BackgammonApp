import { Routes } from '@angular/router';
import { MainLayoutComponent } from './core/layouts/main-layout/main-layout.component';
import { DashboardComponent } from './features/dashboard/pages/dashboard.component';

export const routes: Routes = [
  {
    path: '',
    component: MainLayoutComponent,
    children: [
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
      { path: 'dashboard', component: DashboardComponent },
      {
        path: 'backgammon',
        loadComponent: () =>
          import('./features/game-session/pages/game-session-management/game-session-management-page.component').then(
            (m) => m.GameSessionManagementPageComponent,
          ),
      },
    ],
  },
  { path: '**', redirectTo: 'dashboard' },
];
