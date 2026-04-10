import { BoardStateDto } from '../../features/game-session/models/board/board-state.model';
import { CheckerPosition } from '../../features/game-session/models/board/checker-position.model';
import { UiBoardState } from '../../features/game-session/models/board/ui-board-state.model';
import { UiPoint } from '../../features/game-session/models/board/ui-point.model';
import { PlayerColor } from '../../features/game-session/models/enums/player-color.enum';
import { buildPointsMap } from './board.utils';
import { BOARD_POINTS } from './constants/board.constants';

export function mapBoardState(dto: BoardStateDto): UiBoardState {
  const points: UiPoint[] = [];

  if (!dto || !dto.points) {
    console.error('INVALID DTO', dto);

    return {
      points: [],
      pointsMap: {},
      bar: { [PlayerColor.White]: 0, [PlayerColor.Black]: 0 },
      off: { [PlayerColor.White]: 0, [PlayerColor.Black]: 0 },
      currentPlayer: PlayerColor.White,
    };
  }

  console.log('DTO POINTS RAW:', dto.points);

  for (let i = 1; i <= BOARD_POINTS; i++) {
    const position: CheckerPosition = dto.points[i] ?? {
      owner: null,
      count: 0,
    };

    points.push({
      index: i,
      color:
        position?.owner === 0
          ? PlayerColor.White
          : position?.owner === 1
            ? PlayerColor.Black
            : null,
      count: position?.count ?? 0,
    });
  }

  console.log('UI POINTS:', points);

  return {
    points,
    pointsMap: buildPointsMap(points),
    bar: {
      [PlayerColor.White]: dto.barWhite ?? 0,
      [PlayerColor.Black]: dto.barBlack ?? 0,
    },
    off: {
      [PlayerColor.White]: dto.offWhite ?? 0,
      [PlayerColor.Black]: dto.offBlack ?? 0,
    },
    currentPlayer:
      dto.currentPlayer === 0
        ? PlayerColor.White
        : dto.currentPlayer === 1
          ? PlayerColor.Black
          : PlayerColor.White,
  };
}
