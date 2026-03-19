import { GameFinishReason } from '../enums/game-finish-reason.enum';
import { GamePhase } from '../enums/game-phase.enum';
import { GameResultType } from '../enums/game-result-type.enum';
import { PlayerSnapshot } from './player-snapshot.model';

export interface GameSessionSnapshotResponse {
  version: number;
  sessionId: string;
  sessionCode: string;
  createdByUserId: string;

  players: PlayerSnapshot[];

  currentPhase: GamePhase;
  currentPlayerId: string | null;

  lastDiceRoll: number[] | null;
  boardStateJson: string | null;

  doublingCubeValue: number | null;
  doublingCubeOwnerPlayerId: string | null;
  crawfordRuleApplies: boolean;

  isFinished: boolean;
  finishReason: GameFinishReason | null;
  winnerPlayerId: string | null;
  resultType: GameResultType | null;
  awardedPoints: number | null;

  localPlayerId: string | null;
  isRejoin: boolean;
}
