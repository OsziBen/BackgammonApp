import { GameSessionSettings } from '../../../../../core/models/game-session/game-session-settings.model';

export interface CreateGameSessionRequest {
  hostPlayerId: string;
  settings: GameSessionSettings;
}
