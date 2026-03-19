import { PlayerColor } from '../../../features/game-session/models/enums/player-color.enum';

export const BOARD_POINTS = 24;

export const BAR = 0;
export const OFF = -1;

export const HOME_BOARD = {
  [PlayerColor.White]: { start: 19, end: 24 },
  [PlayerColor.Black]: { start: 1, end: 6 },
};

export const WHITE_BEAR_OFF_TARGET = 25;
export const BLACK_BEAR_OFF_TARGET = 0;

export const MOVE_DIRECTION = {
  [PlayerColor.White]: 1,
  [PlayerColor.Black]: -1,
};
