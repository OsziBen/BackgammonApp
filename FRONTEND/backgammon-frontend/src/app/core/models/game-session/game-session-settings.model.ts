export interface GameSessionSettings {
  targetPoints: number;
  doublingCubeEnabled: boolean;

  clockEnabled: boolean;
  clockSettings?: ClockSettings;

  crawfordRuleEnabled: boolean;
}

export interface ClockSettings {
  matchTimePerPlayerInSeconds?: number | null;
  startOfTurnDelayPerPlayerInSeconds?: number | null;
}
