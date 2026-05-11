import { TournamentParticipantBaseResponse } from './tournament-participant-base.response';

export interface TournamentParticipantsResponse {
  participants: TournamentParticipantBaseResponse[];

  maxParticipantsNumber?: number | null;

  currentParticipantsNumber: number;
}
