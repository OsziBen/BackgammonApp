import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { GROUP_API_ROUTES } from '../constants/group-api-routes.constants';
import { CreateGroupRequest } from '../models/api/requests/create-group.request';
import { EditGroupRequest } from '../models/api/requests/edit-group.request';
import { BaseGroupResponse } from '../models/api/responses/base-group.response';
import { GroupJoinRequestResponse } from '../models/api/responses/group-join-request.response';
import { GroupMembersResponse } from '../models/api/responses/group-members.response';

@Injectable({
  providedIn: 'root',
})
export class GroupsApiService {
  private readonly groupsBaseUrl = `${environment.apiBaseUrl}/${GROUP_API_ROUTES.BASE}`;

  constructor(private http: HttpClient) {}

  // CREATE GROUP
  createGroup(request: CreateGroupRequest): Observable<BaseGroupResponse> {
    return this.http.post<BaseGroupResponse>(this.groupsBaseUrl, request);
  }

  // GET ALL GROUPS
  getAllGroups(): Observable<BaseGroupResponse[]> {
    return this.http.get<BaseGroupResponse[]>(this.groupsBaseUrl);
  }

  // JOIN GROUP
  joinGroup(groupId: string): Observable<void> {
    return this.http.post<void>(
      `${this.groupsBaseUrl}/${GROUP_API_ROUTES.JOIN(groupId)}`,
      {},
    );
  }

  // GET GROUP BY ID
  getGroupById(groupId: string): Observable<BaseGroupResponse> {
    return this.http.get<BaseGroupResponse>(
      `${this.groupsBaseUrl}/${GROUP_API_ROUTES.BY_ID(groupId)}`,
    );
  }

  // EDIT GROUP
  editGroup(
    groupId: string,
    request: EditGroupRequest,
  ): Observable<BaseGroupResponse> {
    return this.http.patch<BaseGroupResponse>(
      `${this.groupsBaseUrl}/${GROUP_API_ROUTES.BY_ID(groupId)}`,
      request,
    );
  }

  // DELETE GROUP
  deleteGroup(groupId: string): Observable<void> {
    return this.http.delete<void>(
      `${this.groupsBaseUrl}/${GROUP_API_ROUTES.BY_ID(groupId)}`,
    );
  }

  // ADD GROUP MEMBER
  addGroupMember(groupId: string, userName: string): Observable<void> {
    return this.http.post<void>(
      `${this.groupsBaseUrl}/${groupId}/members/${userName}`,
      {},
    );
  }

  // GET ALL GROUP JOIN REQUESTS
  getGroupJoinRequests(
    groupId: string,
  ): Observable<GroupJoinRequestResponse[]> {
    return this.http.get<GroupJoinRequestResponse[]>(
      `${this.groupsBaseUrl}/${GROUP_API_ROUTES.REQUESTS(groupId)}`,
    );
  }

  // APPROVE GROUP JOIN REQUEST
  approveJoinRequest(groupId: string, requestId: string): Observable<void> {
    return this.http.post<void>(
      `${this.groupsBaseUrl}/${GROUP_API_ROUTES.APPROVE_JOIN_REQUEST(
        groupId,
        requestId,
      )}`,
      {},
    );
  }

  // REJECT GROUP JOIN REQUEST
  rejectJoinRequest(groupId: string, requestId: string): Observable<void> {
    return this.http.post<void>(
      `${this.groupsBaseUrl}/${GROUP_API_ROUTES.REJECT_JOIN_REQUEST(
        groupId,
        requestId,
      )}`,
      {},
    );
  }

  // GET ALL GROUP MEMBERS
  getGroupMembers(groupId: string): Observable<GroupMembersResponse> {
    return this.http.get<GroupMembersResponse>(
      `${this.groupsBaseUrl}/${GROUP_API_ROUTES.MEMBERS(groupId)}`,
    );
  }

  // LEAVE GROUP
  leaveGroup(groupId: string): Observable<void> {
    return this.http.post<void>(
      `${this.groupsBaseUrl}/${GROUP_API_ROUTES.LEAVE(groupId)}`,
      {},
    );
  }

  // REMOVE GROUP MEMBER
  removeGroupMember(groupId: string, userId: string): Observable<void> {
    return this.http.delete<void>(
      `${this.groupsBaseUrl}/${GROUP_API_ROUTES.MEMBER(groupId, userId)}`,
    );
  }

  // PROMOTE GROUP MEMBER TO MODERATOR
  promoteToModerator(groupId: string, userId: string): Observable<void> {
    return this.http.post<void>(
      `${this.groupsBaseUrl}/${GROUP_API_ROUTES.MODERATOR(groupId, userId)}`,
      {},
    );
  }

  // DEMOTE GROUP MODERATOR
  demoteModerator(groupId: string, userId: string): Observable<void> {
    return this.http.delete<void>(
      `${this.groupsBaseUrl}/${GROUP_API_ROUTES.MODERATOR(groupId, userId)}`,
    );
  }
}
