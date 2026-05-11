export interface UserTournamentJoinRequestResponse {
  id: string;

  tournamentName: string;

  startDate: string;
  endDate: string;
  deadline: string;

  organizerUserName: string;

  status: string;

  createdAt: string;
}
