import { PlayerColor } from '../enums/player-color.enum';
import { CheckerPosition } from './checker-position.model';

export interface BoardStateDto {
  points: Record<number, CheckerPosition>;
  barWhite: number;
  barBlack: number;
  offWhite: number;
  offBlack: number;
  currentPlayer: number;
}
