import { BoardStateDto } from '../../features/game-session/models/board/board-state.model';
import { UiBoardState } from '../../features/game-session/models/board/ui-board-state.model';
import { UiPoint } from '../../features/game-session/models/board/ui-point.model';
import { PlayerColor } from '../../features/game-session/models/enums/player-color.enum';
import { buildPointsMap } from './board.utils';
import { BOARD_POINTS } from './constants/board.constants';

export function mapBoardState(dto: BoardStateDto): UiBoardState {
  const points: UiPoint[] = [];

  for (let i = 1; i <= BOARD_POINTS; i++) {
    const position = dto.points[i];

    points.push({
      index: i,
      color: position?.color ?? null,
      count: position?.count ?? 0,
    });
  }

  return {
    points,
    pointsMap: buildPointsMap(points),
    bar: {
      [PlayerColor.White]: dto.barWhite,
      [PlayerColor.Black]: dto.barBlack,
    },
    off: {
      [PlayerColor.White]: dto.offWhite,
      [PlayerColor.Black]: dto.offBlack,
    },
    currentPlayer: dto.currentPlayer,
  };
}
