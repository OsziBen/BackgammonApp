import { DiceRoll } from '../models/dice.model';
import { DoublingCubeState } from '../models/doubling-cube.model';
import { GameBoard } from '../models/game-board.model';
import { GameOutcome } from '../models/game-outcome.model';
import { GamePhase } from '../models/game-phase.enum';
import { Player } from '../models/player.model';

export interface GameState {
  phase: GamePhase;

  players: Player[];

  currentPlayerId: string | null;
  isLocalPlayersTurn: boolean;

  dice: DiceRoll | null;

  board: GameBoard | null;

  doublingCube: DoublingCubeState;

  outcome: GameOutcome | null;
}

export const initialGameState: GameState = {
  phase: GamePhase.WaitingForPlayers,

  players: [],

  currentPlayerId: null,
  isLocalPlayersTurn: false,

  dice: null,

  board: null,

  doublingCube: {
    value: 1,
    ownerPlayerId: null,
    isOffered: false,
  },

  outcome: null,
};
