import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { TOURNAMENT_API_ROUTES } from '../constants/tournament-api-routes.constants';
import { CreateTournamentRequest } from '../models/api/requests/create-tournament.request';
import { UpdateTournamentRequest } from '../models/api/requests/update-tournament.request';
import { TournamentBaseResponse } from '../models/api/responses/tournament-base.response';
import { TournamentJoinRequestResponse } from '../models/api/responses/tournament-join-request.response';
import { TournamentParticipantsResponse } from '../models/api/responses/tournament-participants.response';

@Injectable({
  providedIn: 'root',
})
export class TournamentsApiService {
  private readonly tournamentsBaseUrl = `${environment.apiBaseUrl}/${TOURNAMENT_API_ROUTES.BASE}`;

  constructor(private http: HttpClient) {}

  // CREATE TOURNAMENT
  createTournament(
    request: CreateTournamentRequest,
  ): Observable<TournamentBaseResponse> {
    return this.http.post<TournamentBaseResponse>(
      this.tournamentsBaseUrl,
      request,
    );
  }

  // GET TOURNAMENT BY ID
  getTournamentById(tournamentId: string): Observable<TournamentBaseResponse> {
    return this.http.get<TournamentBaseResponse>(
      `${this.tournamentsBaseUrl}/${TOURNAMENT_API_ROUTES.BY_ID(tournamentId)}`,
    );
  }

  // UPDATE TOURNAMENT
  updateTournament(
    tournamentId: string,
    request: UpdateTournamentRequest,
  ): Observable<TournamentBaseResponse> {
    return this.http.patch<TournamentBaseResponse>(
      `${this.tournamentsBaseUrl}/${TOURNAMENT_API_ROUTES.BY_ID(tournamentId)}`,
      request,
    );
  }

  // GET ALL TOURNAMENTS
  getAllTournaments(): Observable<TournamentBaseResponse[]> {
    return this.http.get<TournamentBaseResponse[]>(this.tournamentsBaseUrl);
  }

  // DELETE TOURNAMENT
  deleteTournament(tournamentId: string): Observable<void> {
    return this.http.delete<void>(
      `${this.tournamentsBaseUrl}/${TOURNAMENT_API_ROUTES.BY_ID(tournamentId)}`,
    );
  }

  // JOIN TOURNAMENT
  joinTournament(tournamentId: string): Observable<void> {
    return this.http.post<void>(
      `${this.tournamentsBaseUrl}/${TOURNAMENT_API_ROUTES.JOIN(tournamentId)}`,
      {},
    );
  }

  // APPROVE TOURNAMENT JOIN REQUEST
  approveJoinRequest(
    tournamentId: string,
    requestId: string,
  ): Observable<void> {
    return this.http.post<void>(
      `${this.tournamentsBaseUrl}/${TOURNAMENT_API_ROUTES.APPROVE_JOIN_REQUEST(
        tournamentId,
        requestId,
      )}`,
      {},
    );
  }

  // REJECT TOURNAMENT JOIN REQUEST
  rejectJoinRequest(tournamentId: string, requestId: string): Observable<void> {
    return this.http.post<void>(
      `${this.tournamentsBaseUrl}/${TOURNAMENT_API_ROUTES.REJECT_JOIN_REQUEST(
        tournamentId,
        requestId,
      )}`,
      {},
    );
  }

  // GET TOURNAMENT JOIN REQUESTS
  getTournamentJoinRequests(
    tournamentId: string,
  ): Observable<TournamentJoinRequestResponse[]> {
    return this.http.get<TournamentJoinRequestResponse[]>(
      `${this.tournamentsBaseUrl}/${TOURNAMENT_API_ROUTES.REQUESTS(
        tournamentId,
      )}`,
    );
  }

  // GET TOURNAMENT PARTICIPANTS
  getTournamentParticipants(
    tournamentId: string,
  ): Observable<TournamentParticipantsResponse> {
    return this.http.get<TournamentParticipantsResponse>(
      `${this.tournamentsBaseUrl}/${TOURNAMENT_API_ROUTES.PARTICIPANTS(
        tournamentId,
      )}`,
    );
  }

  // ADD TOURNAMENT PARTICIPANT
  addTournamentParticipant(
    tournamentId: string,
    userName: string,
  ): Observable<void> {
    return this.http.post<void>(
      `${this.tournamentsBaseUrl}/${TOURNAMENT_API_ROUTES.PARTICIPANTS(
        tournamentId,
      )}`,
      {
        userName,
      },
    );
  }

  // REMOVE TOURNAMENT PARTICIPANT
  removeTournamentParticipant(
    tournamentId: string,
    userId: string,
  ): Observable<void> {
    return this.http.delete<void>(
      `${this.tournamentsBaseUrl}/${TOURNAMENT_API_ROUTES.PARTICIPANT(
        tournamentId,
        userId,
      )}`,
    );
  }

  // WITHDRAW TOURNAMENT PARTICIPATION
  withdrawTournamentParticipation(tournamentId: string): Observable<void> {
    return this.http.post<void>(
      `${this.tournamentsBaseUrl}/${TOURNAMENT_API_ROUTES.WITHDRAW(
        tournamentId,
      )}`,
      {},
    );
  }
}
