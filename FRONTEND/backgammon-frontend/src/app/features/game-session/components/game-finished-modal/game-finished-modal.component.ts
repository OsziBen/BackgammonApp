import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';
import { GameResultType } from '../../models/enums/game-result-type.enum';
import { GameFinishReason } from '../../models/enums/game-finish-reason.enum';

@Component({
  selector: 'app-game-finished-modal',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './game-finished-modal.component.html',
  styleUrls: ['./game-finished-modal.component.css'],
})
export class GameFinishedModalComponent {
  @Input() winnerPlayerId: string | null = null;
  @Input() localPlayerId: string | null = null;

  @Input() resultType: GameResultType | null = null;
  @Input() finishReason: GameFinishReason | null = null;
  @Input() awardedPoints: number | null = null;

  constructor(private router: Router) {}

  isWinner(): boolean {
    return this.winnerPlayerId === this.localPlayerId;
  }

  title(): string {
    return this.isWinner() ? '🎉 You won!' : '❌ You lost';
  }

  goToSessions() {
    this.router.navigate(['/sessions']);
  }

  resultTypeLabel(): string {
    switch (this.resultType) {
      case GameResultType.SimpleVictory:
        return 'Simple Victory';
      case GameResultType.GammonVictory:
        return 'Gammon Victory';
      case GameResultType.BackgammonVictory:
        return 'Backgammon Victory';
      default:
        return '-';
    }
  }

  finishReasonLabel(): string {
    switch (this.finishReason) {
      case GameFinishReason.Victory:
        return 'Normal finish';
      case GameFinishReason.CubeDeclined:
        return 'Double declined';
      case GameFinishReason.Forfeit:
        return 'Forfeit';
      case GameFinishReason.Timeout:
        return 'Timeout';
      default:
        return '-';
    }
  }
}
