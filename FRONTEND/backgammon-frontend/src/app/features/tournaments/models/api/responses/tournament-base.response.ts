import { TournamentUserState } from '../../enums/tournament-user-state.type';
import { RulesTemplateResponse } from '../../../../rules-template/models/api/responses/rules-template.response';

export interface TournamentBaseResponse {
  id: string;

  name: string;
  description?: string;

  type: string;
  visibility: string;
  status: string;

  maxParticipants: number;

  startDate: string;
  endDate: string;
  deadline: string;

  organizerUserName: string;

  rulesTemplate: RulesTemplateResponse;

  tournamentUserState: TournamentUserState | null;
}
