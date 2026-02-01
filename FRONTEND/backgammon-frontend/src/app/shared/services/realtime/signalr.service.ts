import { Injectable } from '@angular/core';
import {
  HubConnection,
  HubConnectionBuilder,
  HubConnectionState,
  LogLevel,
} from '@microsoft/signalr';
import { environment } from '../../../../environments/environment';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class SignalRService {
  private hubConnection!: HubConnection;

  private connectionState$ = new BehaviorSubject<HubConnectionState | null>(
    null,
  );

  get connectionState() {
    return this.connectionState$.asObservable();
  }

  startConnection(): void {
    if (this.hubConnection?.state === HubConnectionState.Connected) return;

    this.hubConnection = new HubConnectionBuilder()
      .withUrl(`${environment.apiBaseUrl}/backgammonHub`)
      .withAutomaticReconnect({
        nextRetryDelayInMilliseconds: (retryContext) => {
          if (retryContext.elapsedMilliseconds > 30_000) return null;
          return Math.min(
            1000 * Math.pow(2, retryContext.previousRetryCount),
            30_000,
          );
        },
      })
      .configureLogging(LogLevel.Information)
      .build();

    this.registerConnectionEvents();

    this.hubConnection
      .start()
      .then(() => {
        console.log('[SignalR] Connected');
        this.connectionState$.next(this.hubConnection.state);
      })
      .catch((err) => console.error('[SignalR] Connection error', err));
  }

  stopConnection(): void {
    if (!this.hubConnection) return;

    this.hubConnection
      .stop()
      .then(() => {
        console.log('[SignalR] Disconnected');
        this.connectionState$.next(HubConnectionState.Disconnected);
      })
      .catch((err) => console.error('[SignalR] Stop error', err));
  }

  invoke<T = any>(methodName: string, ...args: any[]): Promise<T> {
    if (this.hubConnection.state !== HubConnectionState.Connected) {
      console.warn('[SignalR] Not connected');
      return Promise.reject('Not connected');
    }
    return this.hubConnection.invoke<T>(methodName, ...args);
  }

  on<T>(eventName: string, callback: (payload: T) => void): void {
    this.hubConnection.on(eventName, callback);
  }

  private registerConnectionEvents(): void {
    this.hubConnection.onreconnecting((error) => {
      console.warn('[SignalR] Reconnecting...', error);
      this.connectionState$.next(HubConnectionState.Reconnecting);
    });

    this.hubConnection.onreconnected((connectionId) => {
      console.log('[SignalR] Reconnected', connectionId);
      this.connectionState$.next(HubConnectionState.Connected);
    });

    this.hubConnection.onclose((error) => {
      console.warn('[SignalR] Closed', error);
      this.connectionState$.next(HubConnectionState.Disconnected);
    });
  }
}
