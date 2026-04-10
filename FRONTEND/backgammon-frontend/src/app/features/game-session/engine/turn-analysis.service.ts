import { computed, Injectable } from '@angular/core';
import { TurnStateService } from '../state/turn-state.service';
import { MoveGeneratorService } from './move.generator.service';
import { MoveSequence } from '../models/turn/move-sequence.model';
import { Move } from '../models/turn/move.model';

interface TargetInfo {
  targets: number[];
  sequences: MoveSequence[];
}

interface MoveAnalysis {
  clickablePoints: Record<number, TargetInfo>;
  maxMovesPerTurn: number;
}

@Injectable({
  providedIn: 'root',
})
export class TurnAnalysisService {
  constructor(
    private turnState: TurnStateService,
    private moveGenerator: MoveGeneratorService,
  ) {}

  readonly board = computed(() => this.turnState.board());
  readonly dice = computed(() => this.turnState.remainingDice());
  readonly moves = computed(() => this.turnState.moves());

  // teljes sequence
  readonly sequences = computed<MoveSequence[]>(() => {
    const board = this.board();
    const dice = this.dice();

    if (!board || dice.length === 0) {
      return [];
    }

    return this.moveGenerator.generateAllMoves(board, dice);
  });

  // összegyűjti a clickable pontokat és a célpontokat
  readonly moveAnalysis = computed<MoveAnalysis>(() => {
    const sequences = this.sequences();

    const analysis: MoveAnalysis = { clickablePoints: {}, maxMovesPerTurn: 0 };

    let maxMoves = 0;

    for (const seq of sequences) {
      if (seq.moves.length === 0) {
        continue;
      }

      // max lépésszám
      if (seq.moves.length > maxMoves) {
        maxMoves = seq.moves.length;
      }

      const move = seq.moves[0];

      if (!analysis.clickablePoints[move.from]) {
        analysis.clickablePoints[move.from] = {
          targets: [],
          sequences: [],
        };
      }

      const info = analysis.clickablePoints[move.from];

      if (!info.targets.includes(move.to)) {
        info.targets.push(move.to);
      }

      info.sequences.push(seq);
    }

    analysis.maxMovesPerTurn = maxMoves;

    // Debug log
    console.log('==== MoveAnalysis DEBUG ====');
    console.log('Clickable Points and Targets:', analysis.clickablePoints);
    console.log('Max moves per turn:', analysis.maxMovesPerTurn);
    console.log('============================');

    return analysis;
  });

  hasAnyValidMove(): boolean {
    return this.sequences().length > 0;
  }

  isExactMatch(moves: Move[]): boolean {
    return this.sequences().some((seq) => {
      if (seq.moves.length !== moves.length) {
        return false;
      }

      return seq.moves.every((m, i) => {
        const mm = moves[i];
        return m.from === mm.from && m.to === mm.to && m.die === mm.die;
      });
    });
  }

  isValidPrefix(moves: Move[]): boolean {
    return this.sequences().some((seq) =>
      moves.every((m, i) => {
        const seqMove = seq.moves[i];
        return (
          seqMove &&
          seqMove.from === m.from &&
          seqMove.to === m.to &&
          seqMove.die === m.die
        );
      }),
    );
  }

  getValidSequencesForPrefix(prefix: Move[]): MoveSequence[] {
    return this.sequences().filter((seq) =>
      prefix.every((m, i) => {
        const seqMove = seq.moves[i];
        return (
          seqMove &&
          seqMove.from === m.from &&
          seqMove.to === m.to &&
          seqMove.die === m.die
        );
      }),
    );
  }
}
