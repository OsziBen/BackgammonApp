import { Injectable } from '@angular/core';
import { UiBoardState } from '../models/board/ui-board-state.model';
import { MoveSequence } from '../models/turn/move-sequence.model';
import { Move } from '../models/turn/move.model';
import { PlayerColor } from '../models/enums/player-color.enum';
import { applyMove, undoMove } from './board-logic.utils';
import { MoveSnapshot } from '../models/turn/move-snapshot.model';
import { MoveSequenceRule } from './rules/move-sequence-rule.model';
import { MustUseMaxDiceRule } from './rules/must-use-max-dice.rule';
import { PreferHigherDieRule } from './rules/prefer-higher-die.rule';
import {
  BAR,
  BLACK_BEAR_OFF_TARGET,
  BOARD_POINTS,
  HOME_BOARD,
  MOVE_DIRECTION,
  OFF,
  WHITE_BEAR_OFF_TARGET,
} from '../../../shared/utils/constants/board.constants';
import { getOpponent } from '../../../shared/utils/player.utils';

@Injectable({
  providedIn: 'root',
})
export class MoveGeneratorService {
  private rules: MoveSequenceRule[] = [
    new MustUseMaxDiceRule(),
    new PreferHigherDieRule(),
  ];

  generateAllMoves(board: UiBoardState, dice: number[]): MoveSequence[] {
    const allSequences: MoveSequence[] = [];

    const permutations = this.getPermutations(dice);

    for (const perm of permutations) {
      this.search(board, perm, [], allSequences);
    }

    const unique = this.deduplicate(allSequences);
    const finalSequences = this.applyRules(unique, dice);

    // DEBUG LOG
    console.log('==== MoveGenerator DEBUG ====');
    console.log('Current player:', board.currentPlayer);
    console.log('Dice:', dice);
    console.log('Generated sequences:');
    finalSequences.forEach((seq, idx) => {
      console.log(
        `Sequence ${idx + 1}:`,
        seq.moves.map((m) => `${m.from}->${m.to}(${m.die})`).join(', '),
      );
    });
    console.log('============================');

    return finalSequences;
  }

  private getPermutations(arr: number[]): number[][] {
    if (arr.length <= 1) {
      return [arr];
    }

    const result: number[][] = [];

    arr.forEach((num, i) => {
      const rest = [...arr.slice(0, i), ...arr.slice(i + 1)];
      const perms = this.getPermutations(rest);

      perms.forEach((p) => {
        result.push([num, ...p]);
      });
    });

    return result;
  }

  private deduplicate(sequences: MoveSequence[]): MoveSequence[] {
    const map = new Map<string, MoveSequence>();

    for (const seq of sequences) {
      const key = seq.moves.map((m) => `${m.from}-${m.to}-${m.die}`).join('|');

      if (!map.has(key)) {
        map.set(key, seq);
      }
    }

    return Array.from(map.values());
  }

  private applyRules(
    sequences: MoveSequence[],
    dice: number[],
  ): MoveSequence[] {
    return this.rules.reduce((acc, rule) => rule.apply(acc, dice), sequences);
  }

  private search(
    board: UiBoardState,
    dice: number[],
    currentMoves: Move[],
    result: MoveSequence[],
  ) {
    let anyMove = false;

    for (let i = 0; i < dice.length; i++) {
      const die = dice[i];

      const remainingDice = [...dice];
      remainingDice.splice(i, 1);

      const moves = this.generateSingleDieMoves(board, die);

      if (moves.length === 0) {
        continue;
      }

      for (const move of moves) {
        anyMove = true;

        const snapshot: MoveSnapshot = applyMove(board, move);

        this.search(board, remainingDice, [...currentMoves, move], result);

        undoMove(board, move, snapshot);
      }
    }

    if (!anyMove && currentMoves.length > 0) {
      result.push({
        moves: currentMoves,
      });
    }
  }

  private generateSingleDieMoves(board: UiBoardState, die: number): Move[] {
    const player = board.currentPlayer;
    const opponent = getOpponent(player);

    const moves: Move[] = [];

    const hasBar = board.bar[player] > 0;

    // BAR PRIORITY
    if (hasBar) {
      const target =
        player === PlayerColor.White ? die : BOARD_POINTS + 1 - die;

      const point = board.pointsMap[target];

      if (!point) {
        return [];
      }

      // blocked
      if (point.color === opponent && point.count >= 2) {
        return [];
      }

      moves.push({
        from: BAR,
        to: target,
        die,
      });

      return moves;
    }

    for (const point of board.points) {
      if (point.color !== player || point.count === 0) {
        continue;
      }

      const direction = MOVE_DIRECTION[player];
      const target = point.index + direction * die;

      // NORMAL MOVE
      if (target >= 1 && target <= BOARD_POINTS) {
        const targetPoint = board.pointsMap[target];

        if (
          !targetPoint ||
          (targetPoint.color === opponent && targetPoint.count >= 2)
        ) {
          continue;
        }

        moves.push({
          from: point.index,
          to: target,
          die,
        });
      }

      // BEAR OFF
      if (this.canBearOff(board, player)) {
        if (this.canBearOffFromPoint(board, point.index, die, player)) {
          moves.push({
            from: point.index,
            to: OFF,
            die,
          });
        }
      }
    }

    return moves;
  }

  private canBearOff(board: UiBoardState, player: PlayerColor): boolean {
    for (const p of board.points) {
      if (p.color !== player || p.count === 0) {
        continue;
      }

      const home = HOME_BOARD[player];

      if (player === PlayerColor.White && p.index < home.start) {
        return false;
      }
      if (player === PlayerColor.Black && p.index > home.end) {
        return false;
      }
    }

    return true;
  }

  private canBearOffFromPoint(
    board: UiBoardState,
    from: number,
    die: number,
    player: PlayerColor,
  ): boolean {
    const direction = MOVE_DIRECTION[player];
    const target = from + direction * die;

    // exact
    if (
      (player === PlayerColor.White && target === WHITE_BEAR_OFF_TARGET) ||
      (player === PlayerColor.Black && target === BLACK_BEAR_OFF_TARGET)
    ) {
      return true;
    }

    const home = HOME_BOARD[player];

    // overshoot
    if (
      (player === PlayerColor.White && target > home.end) ||
      (player === PlayerColor.Black && target < home.start)
    ) {
      return !this.hasCheckerFurther(board, player, from);
    }

    return false;
  }

  private hasCheckerFurther(
    board: UiBoardState,
    player: PlayerColor,
    from: number,
  ): boolean {
    return board.points.some((p) => {
      if (p.color !== player || p.count === 0) {
        return false;
      }

      return player === PlayerColor.White ? p.index > from : p.index < from;
    });
  }
}
