import { SessionEventType } from '../enums/session-event-type.enum';
import { GameSessionSnapshotResponse } from './game-session-snapshot-response.model';

export interface SessionUpdatedMessage {
  eventType: SessionEventType;
  snapshot: GameSessionSnapshotResponse;
}
