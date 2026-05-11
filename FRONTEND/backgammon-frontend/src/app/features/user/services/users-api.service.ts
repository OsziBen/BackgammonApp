import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { USER_API_ROUTES } from '../constants/user-api-routes.constants';
import { UserProfileResponse } from '../models/api/responses/user-profile.response';
import { BaseGroupResponse } from '../../groups/models/api/responses/base-group.response';
import { UserGroupJoinRequestResponse } from '../../groups/models/api/responses/user-group-join-request.response';
import { TournamentBaseResponse } from '../../tournaments/models/api/responses/tournament-base.response';
import { UserTournamentJoinRequestResponse } from '../../tournaments/models/api/responses/user-tournament-join-request.response';
@Injectable({
  providedIn: 'root',
})
export class UsersApiService {
  private readonly usersBaseUrl = `${environment.apiBaseUrl}/${USER_API_ROUTES.BASE}`;

  constructor(private http: HttpClient) {}

  // GET CURRENT USER
  getMe(): Observable<UserProfileResponse> {
    return this.http.get<UserProfileResponse>(
      `${this.usersBaseUrl}/${USER_API_ROUTES.ME}`,
    );
  }

  // GET MY GROUPS
  getMyGroups(): Observable<BaseGroupResponse[]> {
    return this.http.get<BaseGroupResponse[]>(
      `${this.usersBaseUrl}/${USER_API_ROUTES.GROUPS}`,
    );
  }

  // GET MY GROUP JOIN REQUESTS
  getMyGroupJoinRequests(): Observable<UserGroupJoinRequestResponse[]> {
    return this.http.get<UserGroupJoinRequestResponse[]>(
      `${this.usersBaseUrl}/${USER_API_ROUTES.GROUP_JOIN_REQUESTS}`,
    );
  }

  // GET MY TOURNAMENTS
  getMyTournaments(): Observable<TournamentBaseResponse[]> {
    return this.http.get<TournamentBaseResponse[]>(
      `${this.usersBaseUrl}/${USER_API_ROUTES.TOURNAMENTS}`,
    );
  }

  // GET MY TOURNAMENT JOIN REQUESTS
  getMyTournamentJoinRequests(): Observable<
    UserTournamentJoinRequestResponse[]
  > {
    return this.http.get<UserTournamentJoinRequestResponse[]>(
      `${this.usersBaseUrl}/${USER_API_ROUTES.TOURNAMENT_JOIN_REQUESTS}`,
    );
  }
}
