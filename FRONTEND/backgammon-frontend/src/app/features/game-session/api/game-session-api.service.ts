import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../environments/environment';
import { Observable } from 'rxjs';
import { CreateGameSessionRequest } from './models/create-game-session.request';

@Injectable({
  providedIn: 'root',
})
export class GameSessionApiService {
  private readonly baseUrl = `${environment.apiBaseUrl}/api/v1/game-sessions`;

  constructor(private http: HttpClient) {}

  createSession(request: CreateGameSessionRequest): Observable<string> {
    return this.http.post<string>(this.baseUrl, request);
  }
}
