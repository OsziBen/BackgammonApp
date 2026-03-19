import { computed, Injectable } from '@angular/core';
import { TurnStateService } from '../state/turn-state.service';
import { MoveGeneratorService } from './move.generator.service';
import { MoveSequence } from '../models/turn/move-sequence.model';

@Injectable({
  providedIn: 'root',
})
export class TurnAnalysisService {
  constructor(
    private turnState: TurnStateService,
    private moveGenerator: MoveGeneratorService,
  ) {}
  // TODO: mi az a signal/computed?
  readonly board = computed(() => this.turnState.board());
  readonly dice = computed(() => this.turnState.remainingDice());
  readonly selectedPoint = computed(() => this.turnState.selectedPoint());

  readonly sequences = computed<MoveSequence[]>(() => {
    const board = this.board();
    const dice = this.dice();

    if (!board || dice.length === 0) {
      return [];
    }

    return this.moveGenerator.generateAllMoves(board, dice);
  });

  readonly clickablePoints = computed<number[]>(() => {
    const sequences = this.sequences();
    const points = new Set<number>();

    for (const seq of sequences) {
      if (seq.moves.length > 0) {
        points.add(seq.moves[0].from);
      }
    }

    return [...points];
  });

  readonly targetPoints = computed<number[]>(() => {
    const selected = this.selectedPoint();

    if (!selected) {
      return [];
    }

    const sequences = this.sequences();

    const targets = new Set<number>();

    for (const seq of sequences) {
      const firstMove = seq.moves[0];

      if (firstMove && firstMove.from === selected) {
        targets.add(firstMove.to);
      }
    }

    return [...targets];
  });

  isClickable(pointIndex: number): boolean {
    return this.clickablePoints().includes(pointIndex);
  }

  isTarget(pointIndex: number): boolean {
    return this.targetPoints().includes(pointIndex);
  }
}
