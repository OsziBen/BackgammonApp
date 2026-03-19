import { UiPoint } from '../../features/game-session/models/board/ui-point.model';
import { PlayerColor } from '../../features/game-session/models/enums/player-color.enum';
import { HOME_BOARD } from './constants/board.constants';

export function isHomeBoard(point: number, player: PlayerColor): boolean {
  const { start, end } = HOME_BOARD[player];

  return point >= start && point <= end;
}

export function buildPointsMap(points: UiPoint[]): Record<number, UiPoint> {
  const map: Record<number, UiPoint> = {};

  for (const p of points) {
    map[p.index] = p;
  }

  return map;
}
