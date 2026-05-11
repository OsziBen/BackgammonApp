export interface TournamentParticipantBaseResponse {
  tournamentId: string;
  userId: string;

  status: string;

  displayName: string;
  email?: string | null;
  notes?: string | null;

  createdAt: string;
}
