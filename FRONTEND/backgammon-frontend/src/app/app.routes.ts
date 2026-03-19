import { Routes } from '@angular/router';
import { RegistrationComponent } from './features/auth/registration/components/registration.component';
import { LoginComponent } from './features/auth/login/components/login.component';
import { authGuard } from './shared/utils/auth.guard';
import { ForbiddenComponent } from './features/forbidden/components/forbidden.component';
import { DashboardLayoutComponent } from './features/dashboard/pages/dashboard-layout/components/dashboard-layout.component';
import { DashboardHomeComponent } from './features/dashboard/pages/dashboard-home/components/dashboard-home.component';
import { UserComponent } from './features/user/components/user.component';

export const AppRoutes = {
  home: '',
  signup: 'signup',
  signin: 'signin',
  forbidden: 'forbidden',
  profile: 'profile',
  sessions: 'sessions',
} as const;

export const routes: Routes = [
  // DASHBOARD LAYOUT
  {
    path: '',
    component: DashboardLayoutComponent,
    children: [
      {
        path: '',
        component: DashboardHomeComponent,
      },
      {
        path: AppRoutes.profile,
        component: UserComponent, // Profile Component
        canActivate: [authGuard],
      },
      {
        path: AppRoutes.sessions,
        canActivate: [authGuard],
        loadComponent: () =>
          import('./features/game-session/pages/game-session-management/game-session-management-page.component').then(
            (m) => m.GameSessionManagementPageComponent,
          ),
      },
    ],
  },

  // // AUTH PAGES
  // {
  //   path: AppRoutes.signup,
  //   component: RegistrationComponent,
  //   outlet: 'modal',
  // },
  // {
  //   path: AppRoutes.signin,
  //   component: LoginComponent,
  //   outlet: 'modal',
  // },

  // FORBIDDEN
  {
    path: AppRoutes.forbidden,
    component: ForbiddenComponent,
  },

  // FALLBACK
  {
    path: '**',
    redirectTo: '',
  },

  // {
  //   path: AppRoutes.profile,
  //   canActivate: [authGuard],
  //   loadComponent: () =>
  //     import('./features/user/components/user.component').then(
  //       (m) => m.UserComponent,
  //     ),
  // },
  // {
  //   path: '',
  //   component: MainLayoutComponent,
  //   canActivate: [authGuard],
  //   canActivateChild: [authGuard],
  //   children: [
  //     { path: AppRoutes.dashboard, component: DashboardComponent },
  //     { path: 'forbidden', component: ForbiddenComponent },
  //     // {
  //     //   path: 'backgammon',
  //     //   loadComponent: () =>
  //     //     import('./features/game-session/pages/game-session-management/game-session-management-page.component').then(
  //     //       (m) => m.GameSessionManagementPageComponent,
  //     //     ),
  //     // },
  //   ],
  // },
];
