import { GameSessionSettings } from '../../game-session-settings.model';

export interface CreateGameSessionRequest {
  settings: GameSessionSettings;
}
