import { UiBoardState } from '../board/ui-board-state.model';
import { Move } from './move.model';

export interface TurnState {
  board: UiBoardState | null;
  selectedPoint: number | null;
  remainingDice: number[];
  moves: Move[];
}
