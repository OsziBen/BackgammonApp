import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { CreateGameSessionRequest } from '../../features/game-session/models/api/requests/create-game-session.request';
import { Observable } from 'rxjs';
import { CreateGameSessionResponse } from '../../features/game-session/models/api/responses/create-game-session.response';
import { GetActiveSessionResponse } from '../../features/game-session/models/api/responses/get-active-session.response';

@Injectable({
  providedIn: 'root',
})
export class GameSessionApiService {
  private readonly baseUrl = `${environment.apiBaseUrl}/game-sessions`;

  constructor(private http: HttpClient) {}

  createSession(
    request: CreateGameSessionRequest,
  ): Observable<CreateGameSessionResponse> {
    return this.http.post<CreateGameSessionResponse>(this.baseUrl, request);
  }

  getActiveSession(): Observable<GetActiveSessionResponse | null> {
    return this.http.get<GetActiveSessionResponse | null>(
      `${this.baseUrl}/active`,
    );
  }

  softDeleteSession(sessionId: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${sessionId}`);
  }
}
