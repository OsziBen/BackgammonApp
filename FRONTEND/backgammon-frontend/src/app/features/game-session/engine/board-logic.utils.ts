import { BAR, OFF } from '../../../shared/utils/constants/board.constants';
import { getOpponent } from '../../../shared/utils/player.utils';
import { UiBoardState } from '../models/board/ui-board-state.model';
import { MoveSnapshot } from '../models/turn/move-snapshot.model';
import { Move } from '../models/turn/move.model';

export function applyMove(board: UiBoardState, move: Move): MoveSnapshot {
  const player = board.currentPlayer;
  const opponent = getOpponent(player);

  const snapshot: MoveSnapshot = {
    hit: false,
  };

  // FROM
  if (move.from === BAR) {
    board.bar[player]--;
  } else {
    const from = board.pointsMap[move.from];
    from.count--;

    if (from.count === 0) {
      from.color = null;
    }
  }

  // TO OFF
  if (move.to === OFF) {
    board.off[player]++;
    return snapshot;
  }

  const target = board.pointsMap[move.to];

  // HIT
  if (target.color === opponent && target.count === 1) {
    snapshot.hit = true;

    target.color = player;
    target.count = 1;

    board.bar[opponent]++;
    return snapshot;
  }

  // NORMAL
  target.color = player;
  target.count++;

  return snapshot;
}

export function undoMove(
  board: UiBoardState,
  move: Move,
  snapshot: MoveSnapshot,
) {
  const player = board.currentPlayer;
  const opponent = getOpponent(player);

  // TO
  if (move.to === OFF) {
    board.off[player]--;
  } else {
    const target = board.pointsMap[move.to];

    if (snapshot.hit) {
      // restore opponent checker
      target.color = opponent;
      target.count = 1;

      board.bar[opponent]--;
    } else {
      target.count--;

      if (target.count === 0) {
        target.color = null;
      }
    }
  }

  // FROM
  if (move.from === BAR) {
    board.bar[player]++;
  } else {
    const from = board.pointsMap[move.from];

    if (from.color === null) {
      from.color = player;
    }

    from.count++;
  }
}
