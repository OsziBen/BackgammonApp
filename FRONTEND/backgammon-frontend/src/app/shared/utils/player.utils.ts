import { PlayerColor } from '../../features/game-session/models/enums/player-color.enum';

export function getOpponent(player: PlayerColor): PlayerColor {
  return player === PlayerColor.White ? PlayerColor.Black : PlayerColor.White;
}
