import { ConnectionState } from './enums/connection-state.enum';
import { GameSessionSnapshotResponse } from './api/game-session-snapshot-response.model';

export interface GameSessionState {
  snapshot: GameSessionSnapshotResponse | null;
  connectionState: ConnectionState;
}
