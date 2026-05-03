import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CreateGroupRequest } from '../models/api/requests/create-group.request';
import { BaseGroupResponse } from '../models/api/responses/base-group.response';
import { UserGroupJoinRequestResponse } from '../models/api/responses/user-group-join-request.response';

@Injectable({
  providedIn: 'root',
})
export class GroupsApiService {
  private readonly groupsBaseUrl = `${environment.apiBaseUrl}/groups`;
  private readonly userGroupsBaseUrl = `${environment.apiBaseUrl}/users/me/groups`;
  private readonly userJoinRequestsBaseUrl = `${environment.apiBaseUrl}/users/me/group-join-requests`;

  constructor(private http: HttpClient) {}

  createGroup(request: CreateGroupRequest): Observable<BaseGroupResponse> {
    return this.http.post<BaseGroupResponse>(this.groupsBaseUrl, request);
  }

  getAllGroups(): Observable<BaseGroupResponse[]> {
    return this.http.get<BaseGroupResponse[]>(this.groupsBaseUrl);
  }

  getMyGroups(): Observable<BaseGroupResponse[]> {
    return this.http.get<BaseGroupResponse[]>(this.userGroupsBaseUrl);
  }

  getMyJoinRequests(): Observable<UserGroupJoinRequestResponse[]> {
    return this.http.get<UserGroupJoinRequestResponse[]>(
      this.userJoinRequestsBaseUrl,
    );
  }

  joinGroup(groupId: string): Observable<void> {
    return this.http.post<void>(`${this.groupsBaseUrl}/${groupId}/join`, {});
  }
}
