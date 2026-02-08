export interface SessionState {
  hasActiveSession: boolean;

  sessionId: string | null;
  sessionCode: string | null;

  localPlayerId: string | null;
  opponentPlayerId: string | null;

  isRejoin: boolean;

  connectionState: 'connected' | 'reconnecting' | 'disconnected';
  reconnectDeadline: Date | null;
}

export const initialSessionState: SessionState = {
  hasActiveSession: false,

  sessionId: null,
  sessionCode: null,

  localPlayerId: null,
  opponentPlayerId: null,

  isRejoin: false,

  connectionState: 'disconnected',
  reconnectDeadline: null,
};
