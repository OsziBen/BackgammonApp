export interface RulesTemplateResponse {
  id: string;

  name: string;
  description?: string | null;
  authorName: string;

  targetScore: number;

  useClock: boolean;

  matchTimePerPlayerInSeconds?: number | null;
  startOfTurnDelayPerPlayerInSeconds?: number | null;

  crawfordRuleEnabled: boolean;

  createdAt: string;
}
