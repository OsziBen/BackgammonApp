export interface GameSessionSettings {
  targetPoints: number;

  doublingCubeEnabled: boolean;

  clockEnabled: boolean;
  matchTimePerPlayerInSeconds?: number | null;
  startOfTurnDelayPerPlayerInSeconds?: number | null;

  crawfordRuleEnabled: boolean;
}
