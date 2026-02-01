import { GameSessionSettings } from './game-session-settings.model';

export interface CreateGameSessionRequest {
  hostPlayerId: string;
  settings: GameSessionSettings;
}
