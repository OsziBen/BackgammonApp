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
  backgammonRules: 'backgammon-rules',
  backgammonHistory: 'backgammon-history',

  groups: 'groups',
  groupsAll: 'all',
  groupsMy: 'my',
  groupsJoinRequests: 'join-requests',
  groupsCreate: 'create',
  groupsDetails: ':groupId',
  groupsOverview: 'overview',
  groupsMembers: 'members',
  groupsRequests: 'requests',

  tournaments: 'tournaments',
  tournamentsAll: 'all',
  tournamentsMy: 'my',
  tournamentsJoinRequests: 'join-requests',
  tournamentsCreate: 'create',
  tournamentsDetails: ':tournamentId',
  tournamentsOverview: 'overview',
  tournamentsParticipants: 'participants',
  tournamentsRequests: 'requests',
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
        path: AppRoutes.backgammonRules,
        loadComponent: () =>
          import('./features/static/pages/backgammon-rules/backgammon-rules.component').then(
            (m) => m.BackgammonRulesComponent,
          ),
      },
      {
        path: AppRoutes.backgammonHistory,
        loadComponent: () =>
          import('./features/static/pages/backgammon-history/backgammon-history.component').then(
            (m) => m.BackgammonHistoryComponent,
          ),
      },
      {
        path: AppRoutes.profile,
        component: UserComponent, // Profile Component
        canActivate: [authGuard],
      },
      {
        path: AppRoutes.groups,
        canActivate: [authGuard],
        loadComponent: () =>
          import('./features/groups/pages/groups-page/groups-page.component').then(
            (m) => m.GroupsPageComponent,
          ),
        children: [
          {
            path: '',
            redirectTo: AppRoutes.groupsAll,
            pathMatch: 'full',
          },
          {
            path: AppRoutes.groupsAll,
            loadComponent: () =>
              import('./features/groups/pages/groups-all/groups-all.component').then(
                (m) => m.GroupsAllComponent,
              ),
          },
          {
            path: AppRoutes.groupsMy,
            loadComponent: () =>
              import('./features/groups/pages/groups-my/groups-my.component').then(
                (m) => m.GroupsMyComponent,
              ),
          },
          {
            path: AppRoutes.groupsJoinRequests,
            loadComponent: () =>
              import('./features/groups/pages/groups-join-requests/groups-join-requests.component').then(
                (m) => m.GroupsJoinRequestsComponent,
              ),
          },
          {
            path: AppRoutes.groupsCreate,
            loadComponent: () =>
              import('./features/groups/pages/groups-create/groups-create.component').then(
                (m) => m.GroupsCreateComponent,
              ),
          },
          {
            path: AppRoutes.groupsDetails,
            loadComponent: () =>
              import('./features/groups/pages/group-details/group-details.component').then(
                (m) => m.GroupDetailsComponent,
              ),
            children: [
              {
                path: '',
                redirectTo: AppRoutes.groupsOverview,
                pathMatch: 'full',
              },
              {
                path: AppRoutes.groupsOverview,
                loadComponent: () =>
                  import('./features/groups/pages/group-details/overview/group-overview.component').then(
                    (m) => m.GroupOverviewComponent,
                  ),
              },
              {
                path: AppRoutes.groupsMembers,
                loadComponent: () =>
                  import('./features/groups/pages/group-details/members/group-members.component').then(
                    (m) => m.GroupMembersComponent,
                  ),
              },
              {
                path: AppRoutes.groupsRequests,
                loadComponent: () =>
                  import('./features/groups/pages/group-details/requests/group-requests.component').then(
                    (m) => m.GroupRequestsComponent,
                  ),
              },
            ],
          },
        ],
      },
      {
        path: AppRoutes.tournaments,
        canActivate: [authGuard],
        loadComponent: () =>
          import('./features/tournaments/pages/tournaments-page/tournaments-page.component').then(
            (m) => m.TournamentsPageComponent,
          ),
        children: [
          {
            path: '',
            redirectTo: AppRoutes.tournamentsAll,
            pathMatch: 'full',
          },
          {
            path: AppRoutes.tournamentsAll,
            loadComponent: () =>
              import('./features/tournaments/pages/tournaments-all/tournaments-all.component').then(
                (m) => m.TournamentsAllComponent,
              ),
          },
          {
            path: AppRoutes.tournamentsMy,
            loadComponent: () =>
              import('./features/tournaments/pages/tournaments-my/tournaments-my.component').then(
                (m) => m.TournamentsMyComponent,
              ),
          },
          {
            path: AppRoutes.tournamentsJoinRequests,
            loadComponent: () =>
              import('./features/tournaments/pages/tournaments-join-requests/tournaments-join-requests.component').then(
                (m) => m.TournamentsJoinRequestsComponent,
              ),
          },
          {
            path: AppRoutes.tournamentsCreate,
            loadComponent: () =>
              import('./features/tournaments/pages/tournaments-create/tournaments-create.component').then(
                (m) => m.TournamentsCreateComponent,
              ),
          },
          {
            path: AppRoutes.tournamentsDetails,
            loadComponent: () =>
              import('./features/tournaments/pages/tournament-details/tournament-details.component').then(
                (m) => m.TournamentDetailsComponent,
              ),
            children: [
              {
                path: '',
                redirectTo: AppRoutes.tournamentsOverview,
                pathMatch: 'full',
              },
              {
                path: AppRoutes.tournamentsOverview,
                loadComponent: () =>
                  import('./features/tournaments/pages/tournament-details/overview/tournament-overview.component').then(
                    (m) => m.TournamentOverviewComponent,
                  ),
              },
              {
                path: AppRoutes.tournamentsParticipants,
                loadComponent: () =>
                  import('./features/tournaments/pages/tournament-details/participants/tournament-participants.component').then(
                    (m) => m.TournamentParticipantsComponent,
                  ),
              },
              {
                path: AppRoutes.tournamentsRequests,
                loadComponent: () =>
                  import('./features/tournaments/pages/tournament-details/requests/tournament-requests.component').then(
                    (m) => m.TournamentRequestsComponent,
                  ),
              },
            ],
          },
        ],
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
