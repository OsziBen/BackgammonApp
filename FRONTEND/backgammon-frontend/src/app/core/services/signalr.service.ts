import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { BehaviorSubject, Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { SessionUpdatedMessage } from '../../features/game-session/models/api/session-updated-message.model';
import { HUB_EVENTS } from '../../shared/utils/constants/hub.constants';

@Injectable({
  providedIn: 'root',
})
export class SignalRService {
  private hubConnection: signalR.HubConnection | null = null;

  private connectionStateSubject =
    new BehaviorSubject<signalR.HubConnectionState>(
      signalR.HubConnectionState.Disconnected,
    );

  public connectionStates$: Observable<signalR.HubConnectionState> =
    this.connectionStateSubject.asObservable();

  constructor() {}

  // Kapcsolat indítása + JWT token
  public async startConnection(token: string): Promise<void> {
    if (this.hubConnection) {
      if (
        this.hubConnection.state === signalR.HubConnectionState.Disconnected
      ) {
        await this.hubConnection.start();
      }

      return;
    }

    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`${environment.apiBaseUrl}/hubs/game-session`, {
        accessTokenFactory: () => token,
      })
      .withAutomaticReconnect({
        nextRetryDelayInMilliseconds: (retryContext) => {
          if (retryContext.elapsedMilliseconds > 30000) {
            return null;
          }

          return Math.min(
            1000 * Math.pow(2, retryContext.previousRetryCount),
            30000,
          );
        },
      })
      .build();

    this.registerConnectionLifecycleEvents();

    try {
      await this.hubConnection.start();
      this.connectionStateSubject.next(this.hubConnection.state);
      console.log('SignalR connected');
    } catch (error) {
      console.error('SignalR connection error:', error);
      throw error;
    }
  }

  //  Kapcsolat leállítása
  public async stopConnection(): Promise<void> {
    if (!this.hubConnection) {
      return;
    }

    await this.hubConnection.stop();
    this.connectionStateSubject.next(signalR.HubConnectionState.Disconnected);
    this.hubConnection = null;
  }

  //  Hub method invoke wrapper (FE -> BE)
  public async invoke<T = any>(methodName: string, ...args: any[]): Promise<T> {
    if (
      !this.hubConnection ||
      this.hubConnection.state !== signalR.HubConnectionState.Connected
    ) {
      throw new Error('Hub connection not initialized');
    }

    return this.hubConnection.invoke<T>(methodName, ...args);
  }

  //  Hub event listener wrapper (BE -> FE)
  public onEvent<T = any>(
    eventName: string,
    callback: (payload: T) => void,
  ): void {
    if (!this.hubConnection) {
      throw new Error('Hub connection not initialized');
    }

    this.hubConnection.off(eventName);
    this.hubConnection.on(eventName, callback);
  }

  //  Event listener eltávolítása
  public offEvent(eventName: string): void {
    if (!this.hubConnection) {
      return;
    }

    this.hubConnection.off(eventName);
  }

  //  Kapcsolati lifecycle események kezelése
  private registerConnectionLifecycleEvents(): void {
    if (!this.hubConnection) {
      return;
    }

    this.hubConnection.onreconnecting((error) => {
      console.warn('SignalR reconnecting...', error);
      this.connectionStateSubject.next(signalR.HubConnectionState.Reconnecting);
    });

    this.hubConnection.onreconnected(() => {
      console.log('SignalR reconnected');
      this.connectionStateSubject.next(signalR.HubConnectionState.Connected);
    });

    this.hubConnection.onclose((error) => {
      console.warn('SignalR closed', error);
      this.connectionStateSubject.next(signalR.HubConnectionState.Disconnected);
    });
  }

  public onSessionUpdated(
    callback: (message: SessionUpdatedMessage) => void,
  ): void {
    this.onEvent<SessionUpdatedMessage>(HUB_EVENTS.SessionUpdated, callback);
  }
}
