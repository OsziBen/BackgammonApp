import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { GetActiveSessionResponse } from '../../../features/game-session/models/api/responses/get-active-session.response';

@Injectable({
  providedIn: 'root',
})
export class GameSessionState {
  private readonly activeSessionSubject =
    new BehaviorSubject<GetActiveSessionResponse | null>(null);

  private readonly loadingSubject = new BehaviorSubject<boolean>(false);
  private readonly errorSubject = new BehaviorSubject<string | null>(null);

  readonly activeSession$ = this.activeSessionSubject.asObservable();
  readonly loading$ = this.loadingSubject.asObservable();
  readonly error$ = this.errorSubject.asObservable();

  setActiveSession(session: GetActiveSessionResponse | null): void {
    this.activeSessionSubject.next(session);
  }

  setLoading(isLoading: boolean): void {
    this.loadingSubject.next(isLoading);
  }

  setError(error: string | null): void {
    this.errorSubject.next(error);
  }

  reset(): void {
    this.activeSessionSubject.next(null);
    this.loadingSubject.next(false);
    this.errorSubject.next(null);
  }
}
