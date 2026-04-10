import { Routes } from '@angular/router';
import { authGuard } from './shared/utils/auth.guard';
import { ForbiddenComponent } from './features/forbidden/components/forbidden.component';
import { DashboardLayoutComponent } from './features/dashboard/pages/dashboard-layout/components/dashboard-layout.component';
import { DashboardHomeComponent } from './features/dashboard/pages/dashboard-home/components/dashboard-home.component';
import { UserComponent } from './features/user/components/user.component';

// TODO: kiszervezni külön fájlba
export const AppRoutes = {
  home: '',
  signup: 'signup',
  signin: 'signin',
  forbidden: 'forbidden',
  profile: 'profile',
  sessions: 'sessions',
  sessionRoom: 'sessions/:code',
  game: 'sessions/:code/game',
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
        children: [
          {
            path: '',
            loadComponent: () =>
              import('./features/game-session/pages/game-session-management/game-session-management-page.component').then(
                (m) => m.GameSessionManagementPageComponent,
              ),
          },
          {
            path: ':code',
            loadComponent: () =>
              import('./features/game-session/pages/session-room/session-room.page.component').then(
                (m) => m.SessionRoomPageComponent,
              ),
          },
          {
            path: ':code/game',
            loadComponent: () =>
              import('./features/game-session/pages/game-board/game-board.component').then(
                (m) => m.GameBoardComponent,
              ),
          },
        ],
      },
    ],
  },

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
];
