import { computed, Injectable, signal } from '@angular/core';
import { GameSessionStore } from './game-session.store';
import { GamePhase } from '../models/enums/game-phase.enum';
import { TurnStateService } from './turn-state.service';
import { TurnAnalysisService } from '../engine/turn-analysis.service';

@Injectable({
  providedIn: 'root',
})
export class GameUiStateService {
  constructor(
    private store: GameSessionStore,
    private stateService: TurnStateService,
    private analysisService: TurnAnalysisService,
  ) {}
  private readonly _isSubmitting = signal(false);
  private readonly _error = signal<string | null>(null);

  setSubmitting(value: boolean) {
    this._isSubmitting.set(value);
  }

  setError(message: string | null) {
    this._error.set(message);
  }

  readonly isSubmitting = computed(() => this._isSubmitting());
  readonly error = computed(() => this._error());

  // snapshot shortcut
  readonly snapshot = computed(() => this.store.snapshot());

  readonly localPlayerId = computed(() => this.store.localPlayerId());
  readonly currentPlayerId = computed(() => this.store.currentPlayerId());

  // ki van soron
  readonly isMyTurn = computed(() => {
    const local = this.localPlayerId();
    const current = this.currentPlayerId();

    console.log('TURN CHECK', {
      localPlayerId: local,
      currentPlayerId: current,
      result: local !== null && local === current,
    });

    return local !== null && local === current;
  });

  // game phase
  readonly phase = computed(() => this.snapshot()?.currentPhase);

  // van-e dobás
  readonly hasDice = computed(() => {
    const dice = this.snapshot()?.lastDiceRoll;

    return Array.isArray(dice) && dice.length > 0;
  });

  // akciók
  readonly canRoll = computed(() => {
    return (
      this.isMyTurn() &&
      (this.phase() === GamePhase.RollDice ||
        this.phase() === GamePhase.TurnStart) &&
      !this.hasDice()
    );
  });

  readonly canMove = computed(() => {
    return (
      this.isMyTurn() &&
      this.phase() === GamePhase.MoveCheckers &&
      this.hasDice()
    );
  });

  readonly canSubmitTurn = computed(() => {
    const myTurn = this.isMyTurn();
    const submitting = this.isSubmitting();

    const analysis = this.analysisService.moveAnalysis();
    const maxMoves = analysis?.maxMovesPerTurn ?? 0;

    const sequences = this.analysisService.sequences();
    const movesMade = this.stateService.moves().length;

    console.log('[DEBUG] canSubmitTurn check', {
      myTurn,
      submitting,
      maxMoves,
      movesMade,
      disabledBecauseNotMaxed: maxMoves > 0 && movesMade < maxMoves,
    });

    if (!myTurn || submitting) {
      return false;
    }

    // van lehetőség lépni, de nem léptél
    if (sequences.length > 0 && movesMade === 0) {
      return false;
    }

    // TODO: refactor later + rename to remaining
    if (maxMoves !== 0) {
      return false;
    }

    return true;
  });

  readonly canUndo = computed(() => {
    return (
      this.isMyTurn() && this.stateService.canUndo() && !this.isSubmitting()
    );
  });

  readonly canOfferDouble = computed(() => {
    return (
      this.isDoublingEnabled() &&
      this.isMyTurn() &&
      this.phase() === GamePhase.TurnStart
    );
  });

  readonly canRespondToDouble = computed(() => {
    return (
      this.isDoublingEnabled() &&
      this.isMyTurn() &&
      this.phase() === GamePhase.CubeOffered
    );
  });

  readonly canInteract = computed(() => {
    return this.canMove() && !this.isSubmitting();
  });

  readonly isWaiting = computed(() => {
    return !this.isMyTurn();
  });

  readonly isGameFinished = computed(() => {
    return this.snapshot()?.isFinished === true;
  });

  readonly showActionModal = computed(() => {
    const phase = this.phase();

    if (phase === GamePhase.CubeOffered) {
      return this.isDoublingEnabled();
    }

    return (
      this.isMyTurn() &&
      (phase === GamePhase.TurnStart || phase === GamePhase.RollDice)
    );
  });

  readonly isDoublingEnabled = computed(() => {
    const snapshot = this.snapshot();

    if (!snapshot) {
      return false;
    }

    if (snapshot.doublingCubeValue === null || snapshot.crawfordRuleApplies) {
      return false;
    }

    return true;
  });
}
