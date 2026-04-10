import { CommonModule } from '@angular/common';
import { Component, effect } from '@angular/core';
import { GameSessionStore } from '../../state/game-session.store';
import { BoardComponent } from '../../components/board/board.component';
import { DiceComponent } from '../../components/dice/dice.component';
import { TurnAnalysisService } from '../../engine/turn-analysis.service';
import { TurnStateService } from '../../state/turn-state.service';
import { GameUiStateService } from '../../state/game-ui-state.service';
import { GameSessionFacade } from '../../facade/game-session.facade';
import { Move } from '../../models/turn/move.model';
import { TurnActionModalComponent } from '../../components/turn-action-modal/turn-action-modal.component';
import { DoublingCubeComponent } from '../../components/doubling-cube.component/doubling-cube.component';
import { PlayerColor } from '../../models/enums/player-color.enum';
import { GameFinishedModalComponent } from '../../components/game-finished-modal/game-finished-modal.component';

@Component({
  selector: 'app-game-board',
  standalone: true,
  imports: [
    CommonModule,
    BoardComponent,
    DiceComponent,
    TurnActionModalComponent,
    DoublingCubeComponent,
    GameFinishedModalComponent,
  ],
  templateUrl: './game-board.component.html',
  styleUrls: ['./game-board.component.css'],
})
export class GameBoardComponent {
  private lastVersion: number | null = null;

  constructor(
    public readonly store: GameSessionStore,
    public readonly analysis: TurnAnalysisService,
    public readonly turnState: TurnStateService,
    public readonly ui: GameUiStateService,
    private readonly facade: GameSessionFacade,
  ) {
    // turn init
    effect(() => {
      const snapshot = this.store.snapshot();
      console.log('INIT TURN', snapshot);
      if (!snapshot) {
        return;
      }

      if (snapshot.version !== this.lastVersion) {
        this.lastVersion = snapshot.version;
        this.turnState.initializeTurn();
      }
    });
  }

  board() {
    return this.turnState.board();
  }

  dice() {
    return this.turnState.remainingDice();
  }

  selectedPoint() {
    return this.turnState.selectedPoint();
  }

  canInteract(): boolean {
    return this.ui.canInteract();
  }

  clickablePoints(): number[] {
    if (!this.canInteract()) {
      return [];
    }

    return Object.keys(this.analysis.moveAnalysis().clickablePoints).map(
      Number,
    );
  }

  targetPoints(): number[] {
    const selected = this.selectedPoint();
    if (selected === null) {
      return [];
    }

    return (
      this.analysis.moveAnalysis().clickablePoints[selected]?.targets || []
    );
  }

  onPointClick(point: number) {
    if (!this.canInteract()) {
      return;
    }

    const selected = this.selectedPoint();
    const analysis = this.analysis.moveAnalysis();

    // SELECT
    if (selected === null) {
      if (analysis.clickablePoints[point]) {
        this.turnState.selectPoint(point);
        console.log(`[DEBUG] Selecting point ${point}`);
      }
      return;
    }

    // DESELECT
    if (point === selected) {
      this.turnState.clearSelection();
      console.log(`[DEBUG] Deselecting point ${point}`);
      return;
    }

    const targets = analysis.clickablePoints[selected]?.targets || [];

    // MOVE
    if (targets.includes(point)) {
      const sequences = analysis.clickablePoints[selected].sequences;

      // Debug: minden sequence
      console.log(`[DEBUG] All sequences for point ${selected}:`, sequences);

      // Keressük a sequence-t, ami illik a selected -> point lépéshez
      let chosenMove = null;
      for (const seq of sequences) {
        for (const m of seq.moves) {
          if (m.from === selected && m.to === point) {
            chosenMove = m;
            console.log(
              `[DEBUG] Chosen move from sequence: from=${m.from}, to=${m.to}, die=${m.die}`,
            );
            break;
          }
        }
        if (chosenMove) break;
      }

      if (!chosenMove) {
        console.warn(
          `[WARN] No valid move found for selected ${selected} -> point ${point}`,
        );
        return;
      }

      this.turnState.applyMove(chosenMove);
      this.turnState.clearSelection();

      console.log(`[DEBUG] Moving from ${selected} to target ${point}`);

      return;
    }

    // ignore invalid click
    console.log(
      `[DEBUG] Clicked point ${point} is neither target nor clickable → keeping selection ${selected}`,
    );
  }

  undoMove() {
    if (!this.ui.canUndo()) {
      return;
    }
    console.log('Undo move triggered.');

    this.turnState.undoAll();
  }

  async onTurnFinished() {
    if (this.ui.isSubmitting()) {
      return;
    }

    const snapshot = this.store.snapshot();
    const moves = this.turnState.moves();
    const sequences = this.analysis.sequences();

    if (!snapshot) {
      return;
    }

    console.log('[SUBMIT DEBUG]', {
      moves,
      movesLength: moves.length,
      possibleSequences: sequences.length,
    });

    // 1. NINCS sem move, sem sequence -> pass
    if (sequences.length === 0 && moves.length === 0) {
      console.log('[SUBMIT] No moves at all → sending empty turn');
      await this.submitMoves(snapshot.sessionId, []);
      return;
    }

    // 2. VANNAK move-jaid, de nincs több sequence -> KÉSZ -> submit
    if (sequences.length === 0 && moves.length > 0) {
      console.log('[SUBMIT] Turn finished → sending moves');
      await this.submitMoves(snapshot.sessionId, moves);
      return;
    }

    // 3. VAN lehetőség lépni, de nem léptél -> TILT
    if (moves.length === 0) {
      console.warn(
        '[SUBMIT BLOCKED] Moves available but none made → blocking submit',
      );
      return;
    }

    const isExact = this.analysis.isExactMatch(moves);

    if (!isExact) {
      console.warn('[SUBMIT BLOCKED] Invalid FULL move sequence');
      return;
    }

    await this.submitMoves(snapshot.sessionId, moves);
  }

  private async submitMoves(sessionId: string, moves: Move[]) {
    this.ui.setSubmitting(true);
    this.ui.setError(null);

    try {
      console.log('Sending moves:', moves);

      await this.facade.moveCheckers(sessionId, moves);
    } catch (error: any) {
      console.error('Move submission failed', error);

      if (error?.error?.code === 'InvalidMove') {
        this.ui.setError('Invalid move! Please try again.');
      } else {
        this.ui.setError('Something went wrong.');
      }

      this.turnState.initializeTurn();
    } finally {
      this.ui.setSubmitting(false);
    }
  }

  rollDice() {
    if (!this.ui.canRoll()) {
      return;
    }

    this.facade.rollDice(this.store.snapshot()?.sessionId!);
  }

  offerDouble() {
    if (!this.ui.canOfferDouble()) {
      return;
    }

    this.facade.offerDoublingCube(this.store.snapshot()?.sessionId!);
  }

  acceptDouble() {
    if (!this.ui.canRespondToDouble()) {
      return;
    }

    this.facade.acceptDoublingCube(this.store.snapshot()?.sessionId!);
  }

  declineDouble() {
    if (!this.ui.canRespondToDouble()) {
      return;
    }

    this.facade.declineDoublingCube(this.store.snapshot()?.sessionId!);
  }

  doublingCubeValue(): number {
    return this.store.snapshot()?.doublingCubeValue ?? 2;
  }

  doublingCubeOwner(): PlayerColor | null {
    const snapshot = this.store.snapshot();

    if (!snapshot || !snapshot.doublingCubeOwnerPlayerId) {
      return null;
    }

    const ownerId = snapshot.doublingCubeOwnerPlayerId;
    const player = snapshot.players.find((p) => p.playerId === ownerId);

    if (!player) {
      return null;
    }

    return player.color as PlayerColor;
  }

  doublingCubeEnabled(): boolean {
    return this.ui.canOfferDouble() || this.ui.canRespondToDouble();
  }

  onBarClick(player: PlayerColor) {
    if (!this.canInteract()) return;

    const analysis = this.analysis.moveAnalysis();
    const selected = this.selectedPoint();

    // csak ha nincs kiválasztott point
    if (selected === null) {
      if (player === PlayerColor.Black && analysis.clickablePoints[-1]) {
        this.turnState.selectPoint(-1); // bar fekete
      } else if (player === PlayerColor.White && analysis.clickablePoints[-2]) {
        this.turnState.selectPoint(-2); // bar fehér
      }
      return;
    }
  }
}
