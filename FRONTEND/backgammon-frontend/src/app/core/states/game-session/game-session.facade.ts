import { Injectable } from '@angular/core';
import { BehaviorSubject, map, Observable, tap } from 'rxjs';
import { GameSessionApiService } from '../../services/game-session-api.service';
import { GameSessionSettings } from '../../models/game-session/game-session-settings.model';
import { GetActiveSessionResponse } from '../../../features/game-session/models/api/responses/get-active-session.response';

@Injectable({
  providedIn: 'root',
})
export class GameSessionFacade {
  private readonly activeSessionSubject =
    new BehaviorSubject<GetActiveSessionResponse | null>(null);

  private readonly loadingSubject = new BehaviorSubject<boolean>(false);
  private readonly errorSubject = new BehaviorSubject<string | null>(null);

  readonly activeSession$ = this.activeSessionSubject.asObservable();
  readonly loading$ = this.loadingSubject.asObservable();
  readonly error$ = this.errorSubject.asObservable();

  constructor(private readonly api: GameSessionApiService) {}

  loadActiveSession(userId: string): void {
    this.loadingSubject.next(true);
    this.errorSubject.next(null);

    this.api.getActiveSession(userId).subscribe({
      next: (session) => {
        this.activeSessionSubject.next(session);
        this.loadingSubject.next(false);
      },
      error: () => {
        this.activeSessionSubject.next(null);
        this.loadingSubject.next(false);
        this.errorSubject.next('Failed to load active session');
      },
    });
  }

  createSession(
    userId: string,
    settings: GameSessionSettings,
  ): Observable<void> {
    this.loadingSubject.next(true);
    this.errorSubject.next(null);

    return this.api
      .createSession({
        hostPlayerId: userId,
        settings,
      })
      .pipe(
        tap(() => this.loadActiveSession(userId)),
        tap({
          next: () => this.loadingSubject.next(false),
          error: () => {
            this.loadingSubject.next(false);
            this.errorSubject.next('Failed to create session');
          },
        }),
        map(() => void 0),
      );
  }

  deleteSession(sessionId: string): Observable<void> {
    this.loadingSubject.next(true);
    this.errorSubject.next(null);

    return this.api.softDeleteSession(sessionId).pipe(
      tap({
        next: () => {
          this.activeSessionSubject.next(null);
          this.loadingSubject.next(false);
        },
        error: () => {
          this.loadingSubject.next(false);
          this.errorSubject.next('Failed to delete session');
        },
      }),
    );
  }
}
