export interface GameOutcome {
  winnerPlayerId: string;
  reason: 'Normal' | 'Resign' | 'Timeout' | 'Abandoned';
  outcome: 'Single' | 'Gammon' | 'Backgammon';
}
