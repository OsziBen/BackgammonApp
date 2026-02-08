import { GameState } from './game.state';
import { SessionState } from './session.state';

export interface AppRealtimeState {
  session: SessionState;
  game: GameState;
}
