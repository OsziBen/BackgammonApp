import { Injectable, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { ConnectionState } from '../models/enums/connection-state.enum';
import { SignalRService } from '../../../core/services/signalr.service';
import * as signalR from '@microsoft/signalr';
import { SessionUpdatedMessage } from '../models/api/session-updated-message.model';
import { GameSessionStore } from '../state/game-session.store';
import { HUB_METHODS } from '../../../shared/utils/constants/hub.constants';

@Injectable({
  providedIn: 'root',
})
export class GameSessionFacade implements OnDestroy {
  private connectionSubscription?: Subscription;

  private hubEventRegistered = false;
  private hasJoinedSession = false;

  constructor(
    private readonly signalRService: SignalRService,
    private readonly store: GameSessionStore,
  ) {
    this.listenToConnectionState();
  }

  // PUBLIC API
  public async joinSession(sessionCode: string, token: string): Promise<void> {
    this.store.setConnectionState(ConnectionState.Connecting);

    try {
      await this.signalRService.startConnection(token);

      this.registerHubEvents();

      this.hasJoinedSession = true;
      await this.signalRService.invoke(HUB_METHODS.JoinSession, sessionCode);
    } catch (error) {
      console.error('Join session failed', error);

      this.store.setConnectionState(ConnectionState.Disconnected);
    }
  }

  public async leaveSession(): Promise<void> {
    await this.signalRService.stopConnection();

    this.hasJoinedSession = false;
    this.hubEventRegistered = false;

    this.store.reset();
  }

  // SIGNALR HUB EVENTS
  private registerHubEvents(): void {
    this.signalRService.onSessionUpdated((message) =>
      this.handleSessionUpdated(message),
    );

    this.hubEventRegistered = true;
  }

  private handleSessionUpdated(message: SessionUpdatedMessage): void {
    this.store.setSnapshot(message.snapshot);
  }

  private tryAutoRejoin(): void {
    if (!this.hasJoinedSession) {
      return;
    }

    const snapshot = this.store.snapshot();

    if (!snapshot) {
      return;
    }

    const sessionCode = snapshot.sessionCode;

    if (!sessionCode) {
      return;
    }

    console.log('Auto rejoining session', sessionCode);

    this.signalRService
      .invoke(HUB_METHODS.JoinSession, sessionCode)
      .catch((err) => console.error('Auto rejoin failed', err));
  }

  // CONNECTION STATE
  private listenToConnectionState(): void {
    this.connectionSubscription =
      this.signalRService.connectionStates$.subscribe((state) => {
        switch (state) {
          case signalR.HubConnectionState.Connected:
            this.store.setConnectionState(ConnectionState.Connected);

            this.tryAutoRejoin();
            break;

          case signalR.HubConnectionState.Connecting:
            this.store.setConnectionState(ConnectionState.Connecting);
            break;

          case signalR.HubConnectionState.Reconnecting:
            this.store.setConnectionState(ConnectionState.Reconnecting);
            break;

          default:
            this.store.setConnectionState(ConnectionState.Disconnected);
            break;
        }
      });
  }

  // CLEANUP
  ngOnDestroy(): void {
    this.connectionSubscription?.unsubscribe();
  }
}
