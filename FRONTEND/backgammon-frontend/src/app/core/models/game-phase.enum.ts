export enum GamePhase {
  WaitingForPlayers = 'WaitingForPlayers',
  DeterminingStartingPlayer = 'DeterminingStartingPlayer',

  TurnStart = 'TurnStart',
  RollDice = 'RollDice',
  MoveCheckers = 'MoveCheckers',

  CubeOffered = 'CubeOffered',

  GameAbandoned = 'GameAbandoned',
  GameFinished = 'GameFinished',
}
