import { UserBaseResponse } from '../../../../user/models/api/responses/user-base.response';

export interface GroupMembersResponse {
  members: UserBaseResponse[];

  maxModeratorNumber?: number | null;

  currentModeratorNumber: number;
}
