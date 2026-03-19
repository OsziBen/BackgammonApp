import { GameSessionSettings } from '../../game-session-settings.model';

export interface GetActiveSessionResponse {
  sessionId: string;
  sessionCode: string;
  settings: GameSessionSettings;
  createdAt: string;
}
