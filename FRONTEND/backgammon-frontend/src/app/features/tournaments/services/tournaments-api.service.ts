import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { CreateTournamentRequest } from '../models/api/requests/create-tournament.request';
import { Observable } from 'rxjs';
import { TournamentBaseResponse } from '../models/api/responses/tournament-base.response';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class TournamentsApiService {
  private readonly tournamentsBaseUrl = `${environment.apiBaseUrl}/tournaments`;

  constructor(private http: HttpClient) {}

  createTournament(
    request: CreateTournamentRequest,
  ): Observable<TournamentBaseResponse> {
    return this.http.post<TournamentBaseResponse>(
      this.tournamentsBaseUrl,
      request,
    );
  }

  getAllTournaments(): Observable<TournamentBaseResponse[]> {
    return this.http.get<TournamentBaseResponse[]>(this.tournamentsBaseUrl);
  }
}
