import { GameSessionSettings } from '../../../../../core/models/game-session/game-session-settings.model';

export interface GetActiveSessionResponse {
  sessionId: string;
  sessionCode: string;
  settings: GameSessionSettings;
  createdAt: string;
}
