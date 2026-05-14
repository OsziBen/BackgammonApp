import { PlayerColor } from '../enums/player-color.enum';

export interface PlayerSnapshot {
  playerId: string;
  userId: string;
  userName: string;
  isHost: boolean;
  color: PlayerColor;
  isConnected: boolean;
  startingRoll: number | null;
}
