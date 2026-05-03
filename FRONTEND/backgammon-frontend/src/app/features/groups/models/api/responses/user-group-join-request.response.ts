import { JoinRequestStatus } from '../../enums/join-request-status.type';

export interface UserGroupJoinRequestResponse {
  id: string;
  groupName: string;
  status: JoinRequestStatus;
  createdAt: string;
}
