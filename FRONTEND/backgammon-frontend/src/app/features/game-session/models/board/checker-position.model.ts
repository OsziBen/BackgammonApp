import { PlayerColor } from '../enums/player-color.enum';

export interface CheckerPosition {
  owner: PlayerColor | null;
  count: number;
}
