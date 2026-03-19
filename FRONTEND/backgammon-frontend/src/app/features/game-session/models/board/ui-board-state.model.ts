import { PlayerColor } from '../enums/player-color.enum';
import { BarState, OffState } from './board-types';
import { UiPoint } from './ui-point.model';

export interface UiBoardState {
  points: UiPoint[];
  pointsMap: Record<number, UiPoint>;
  bar: BarState;
  off: OffState;
  currentPlayer: PlayerColor;
}
