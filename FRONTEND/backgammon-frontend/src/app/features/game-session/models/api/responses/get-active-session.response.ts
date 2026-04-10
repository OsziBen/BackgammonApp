import { GameSessionSettings } from '../../game-session-settings.model';

export interface GetActiveSessionResponse {
  sessionId: string;
  version: number;
  sessionCode: string;
  settings: GameSessionSettings;
  createdAt: string;
}
