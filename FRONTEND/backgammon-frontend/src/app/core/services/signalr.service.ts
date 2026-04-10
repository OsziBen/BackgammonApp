import { Injectable, signal } from '@angular/core';
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

  readonly connectionState = signal<signalR.HubConnectionState>(
    signalR.HubConnectionState.Disconnected,
  );

  private registeredEvents = new Map<string, (...args: any[]) => void>();

  // Kapcsolat indítása + JWT token
  public async startConnection(token: string): Promise<void> {
    if (this.hubConnection) {
      if (
        this.hubConnection.state === signalR.HubConnectionState.Disconnected
      ) {
        await this.hubConnection.start();
        this.connectionState.set(this.hubConnection.state);
      }

      return;
    }

    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(environment.hubUrl, {
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

    this.registerLifecycleEvents();

    await this.hubConnection.start();
    this.connectionState.set(this.hubConnection.state);

    console.log('SignalR connected');
  }

  //  Kapcsolat leállítása
  public async stopConnection(): Promise<void> {
    if (!this.hubConnection) {
      return;
    }

    await this.hubConnection.stop();
    this.connectionState.set(signalR.HubConnectionState.Disconnected);
    this.hubConnection = null;
    this.registeredEvents.clear();
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

    this.registeredEvents.set(eventName, callback);

    this.hubConnection.off(eventName);
    this.hubConnection.on(eventName, callback);
  }

  private reRegisterEvents(): void {
    if (!this.hubConnection) {
      return;
    }

    this.registeredEvents.forEach((callback, eventName) => {
      this.hubConnection!.off(eventName);
      this.hubConnection!.on(eventName, callback);
    });
  }

  //  Kapcsolati lifecycle események kezelése
  private registerLifecycleEvents(): void {
    if (!this.hubConnection) {
      return;
    }

    this.hubConnection.onreconnecting((error) => {
      console.warn('SignalR reconnecting...', error);
      this.connectionState.set(signalR.HubConnectionState.Reconnecting);
    });

    this.hubConnection.onreconnected(() => {
      console.log('SignalR reconnected');
      this.connectionState.set(signalR.HubConnectionState.Connected);

      this.reRegisterEvents();
    });

    this.hubConnection.onclose((error) => {
      console.warn('SignalR closed', error);
      this.connectionState.set(signalR.HubConnectionState.Disconnected);
    });
  }

  public onSessionUpdated(
    callback: (message: SessionUpdatedMessage) => void,
  ): void {
    this.onEvent<SessionUpdatedMessage>(HUB_EVENTS.SessionUpdated, callback);
  }
}
