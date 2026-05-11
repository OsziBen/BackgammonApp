export interface UpdateTournamentRequest {
  name: string;
  description: string;
  type: string;
  visibility: string;
  status: string;
  maxParticipants: number;

  startDate: string;
  endDate: string;
  deadline: string;

  rulesTemplateId: string;
}
