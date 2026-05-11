import { GroupVisibility } from '../../enums/group-visibility.type';
import { GroupJoinPolicy } from '../../enums/group-join-policy.type';
import { GroupSizePreset } from '../../enums/group-size-preset.type';
import { GroupUserState } from '../../enums/group-user-state.type';

export interface BaseGroupResponse {
  id: string;
  creatorName: string;
  name: string;
  description: string;

  visibility: GroupVisibility;
  joinPolicy: GroupJoinPolicy;
  sizePreset: GroupSizePreset;

  maxMembers: number;
  maxModerators: number;

  groupUserState: GroupUserState | null;

  createdAt: string;
}
