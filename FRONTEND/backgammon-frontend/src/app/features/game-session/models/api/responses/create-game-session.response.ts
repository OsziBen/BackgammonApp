import { GameSessionSettings } from '../../game-session-settings.model';

export interface CreateGameSessionResponse {
  sessionId: string;
  sessionCode: string;
  settings: GameSessionSettings;
  createdAt: string;
  currentPhase: number;
  hostUserId: string;
  playersCount: number;
}
