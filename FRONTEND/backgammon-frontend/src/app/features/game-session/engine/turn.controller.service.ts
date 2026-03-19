import { Injectable } from '@angular/core';
import { TurnStateService } from '../state/turn-state.service';
import { TurnAnalysisService } from './turn-analysis.service';

@Injectable({
  providedIn: 'root',
})
export class TurnControllerService {
  constructor(
    private turnState: TurnStateService,
    private analysis: TurnAnalysisService,
  ) {}

  onPointClicked(point: number) {
    const selected = this.turnState.selectedPoint();

    if (!selected) {
      const clickable = this.analysis.clickablePoints();

      if (clickable.includes(point)) {
        this.turnState.selectPoint(point);
      }

      return;
    }

    const targets = this.analysis.targetPoints();

    if (targets.includes(point)) {
      this.applyMove(selected, point);

      return;
    }

    this.turnState.clearSelection();
  }

  private applyMove(from: number, to: number) {
    const sequences = this.analysis.sequences();

    const move = sequences
      .map((s) => s.moves[0])
      .find((m) => m.from === from && m.to === to);

    if (!move) {
      return;
    }

    this.turnState.applyMove(move);
  }
}
