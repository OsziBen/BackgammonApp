import { computed, Injectable, signal } from '@angular/core';
import { GameSessionStore } from './game-session.store';
import { Move } from '../models/turn/move.model';
import { BoardSelectorsService } from '../selectors/board-selectors.service';
import { applyMove } from '../engine/board-logic.utils';
import { TurnState } from '../models/turn/turn-state.model';

@Injectable({
  providedIn: 'root',
})
export class TurnStateService {
  constructor(
    private boardSelectors: BoardSelectorsService,
    private session: GameSessionStore,
  ) {}

  private readonly _state = signal<TurnState>({
    board: null,
    selectedPoint: null,
    remainingDice: [],
    moves: [],
  });

  readonly board = computed(() => this._state().board);
  readonly selectedPoint = computed(() => this._state().selectedPoint);
  readonly remainingDice = computed(() => this._state().remainingDice);
  readonly moves = computed(() => this._state().moves);

  readonly isTurnFinished = computed(() => {
    return this.remainingDice().length === 0;
  });

  readonly canUndo = computed(() => this._state().moves.length > 0);

  initializeTurn() {
    const board = this.boardSelectors.board();
    const dice = this.session.snapshot()?.lastDiceRoll ?? [];

    if (!board) {
      return;
    }

    const remainingDice =
      dice.length === 2 && dice[0] === dice[1]
        ? [dice[0], dice[0], dice[0], dice[0]]
        : [...dice];

    this._state.set({
      board: structuredClone(board),
      selectedPoint: null,
      remainingDice,
      moves: [],
    });
  }

  selectPoint(point: number) {
    this._state.update((s) => ({
      ...s,
      selectedPoint: point,
    }));
  }

  clearSelection() {
    this._state.update((s) => ({
      ...s,
      selectedPoint: null,
    }));
  }

  applyMove(move: Move) {
    const state = this._state();

    if (!state.board) {
      return;
    }

    const diceIndex = state.remainingDice.indexOf(move.die);

    if (diceIndex === -1) {
      return;
    }

    const board = structuredClone(state.board);

    applyMove(board, move);

    const newDice = [...state.remainingDice];
    newDice.splice(diceIndex, 1);

    this._state.update((s) => ({
      ...s,
      board,
      remainingDice: newDice,
      moves: [...s.moves, move],
      selectedPoint: null,
    }));
  }

  undoAll() {
    const originalBoard = this.boardSelectors.board();
    const dice = this.session.snapshot()?.lastDiceRoll ?? [];

    if (!originalBoard) {
      return;
    }

    const remainingDice =
      dice.length === 2 && dice[0] === dice[1]
        ? [dice[0], dice[0], dice[0], dice[0]]
        : [...dice];

    this._state.update((s) => ({
      ...s,
      board: structuredClone(originalBoard),
      remainingDice,
      moves: [],
      selectedPoint: null,
    }));
  }

  undo() {
    const state = this._state();

    if (state.moves.length === 0 || !state.board) {
      return;
    }

    const moves = state.moves.slice(0, -1);
    const originalBoard = this.boardSelectors.board();

    if (!originalBoard) {
      return;
    }

    let board = structuredClone(originalBoard);

    for (const move of moves) {
      applyMove(board, move);
    }

    const dice = this.session.snapshot()?.lastDiceRoll ?? [];

    const remainingDice =
      dice.length === 2 && dice[0] === dice[1]
        ? [dice[0], dice[0], dice[0], dice[0]]
        : [...dice];

    for (const move of moves) {
      const index = remainingDice.indexOf(move.die);

      if (index !== -1) {
        remainingDice.splice(index, 1);
      }
    }

    this._state.update((s) => ({
      ...s,
      board,
      remainingDice,
      moves,
      selectedPoint: null,
    }));
  }
}
