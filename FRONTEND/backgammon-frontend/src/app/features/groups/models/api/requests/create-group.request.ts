import { GroupSizePreset } from '../../enums/group-size-preset.type';
import { GroupVisibility } from '../../enums/group-visibility.type';

export interface CreateGroupRequest {
  name: string;
  description: string;
  visibility: GroupVisibility;
  sizePreset: GroupSizePreset;
}
