import { PlayerColor } from '../enums/player-color.enum';

export interface UiPoint {
  index: number;
  color: PlayerColor | null;
  count: number;
}
