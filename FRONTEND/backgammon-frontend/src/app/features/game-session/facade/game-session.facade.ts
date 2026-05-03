import { effect, Injectable } from '@angular/core';
import { ConnectionState } from '../models/enums/connection-state.enum';
import { SignalRService } from '../../../core/services/signalr.service';
import * as signalR from '@microsoft/signalr';
import { SessionUpdatedMessage } from '../models/api/session-updated-message.model';
import { GameSessionStore } from '../state/game-session.store';
import { HUB_METHODS } from '../../../shared/utils/constants/hub.constants';
import { GamePhase } from '../models/enums/game-phase.enum';
import { Move } from '../models/turn/move.model';

@Injectable({
  providedIn: 'root',
})
export class GameSessionFacade {
  private hubEventRegistered = false;
  private isJoining = false;
  private hasJoinedSession = false;

  constructor(
    private readonly signalRService: SignalRService,
    private readonly store: GameSessionStore,
  ) {
    this.setupConnectionEffect();
  }

  // JOIN
  public async joinSession(sessionCode: string, token: string): Promise<void> {
    if (this.isJoining) {
      return;
    }

    this.isJoining = true;
    this.store.reset();
    this.store.setConnectionState(ConnectionState.Connecting);

    try {
      await this.signalRService.startConnection(token);

      this.registerHubEvents();

      await this.signalRService.invoke(HUB_METHODS.JoinSession, sessionCode);

      this.hasJoinedSession = true;
      this.store.setConnectionState(ConnectionState.Connected);
    } catch (error) {
      console.error('Join failed', error);

      this.store.setError('Could not join session');
      this.store.setConnectionState(ConnectionState.Disconnected);
    } finally {
      this.isJoining = false;
    }
  }

  public async leaveSession(): Promise<void> {
    await this.signalRService.stopConnection();

    this.hasJoinedSession = false;
    this.hubEventRegistered = false;

    this.store.reset();
  }

  // SIGNALR EVENTS
  private registerHubEvents(): void {
    if (this.hubEventRegistered) {
      return;
    }

    this.signalRService.onSessionUpdated((message) =>
      this.handleSessionUpdated(message),
    );

    this.hubEventRegistered = true;
  }

  private handleSessionUpdated(message: SessionUpdatedMessage): void {
    const snapshot = message.snapshot;
    console.log('SNAPSHOT RECEIVED', message.snapshot);
    console.log('IDs DEBUG ->', {
      localPlayerId: snapshot.localPlayerId,
      currentPlayerId: snapshot.currentPlayerId,
      players: snapshot.players?.map((p) => ({
        id: p.playerId,
        isHost: p.isHost,
      })),
      phase: snapshot.currentPhase,
    });
    const current = this.store.snapshot();
    const isDifferentSession =
      current !== null && current.sessionId !== message.snapshot.sessionId;

    if (isDifferentSession) {
      console.log('Session changed -> full reset');
    }

    const force = !current || isDifferentSession;

    this.store.setSnapshot(message.snapshot, force);
  }

  // CONNECTION
  private setupConnectionEffect(): void {
    effect(() => {
      const state = this.signalRService.connectionState();

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

  private tryAutoRejoin(): void {
    if (!this.hasJoinedSession) {
      return;
    }

    const snapshot = this.store.snapshot();
    if (!snapshot?.sessionCode) {
      return;
    }
    if (this.store.connectionState() !== ConnectionState.Reconnecting) {
      return;
    }

    console.log('Auto rejoin:', snapshot.sessionCode);

    this.signalRService
      .invoke(HUB_METHODS.JoinSession, snapshot.sessionCode)
      .catch((err) => {
        console.error('Auto rejoin failed', err);
        this.store.setError('Reconnection failed');
      });
  }

  // GUARDS
  private canAct(phases: GamePhase[], requireTurn = true): boolean {
    const snapshot = this.store.snapshot();

    if (!snapshot) {
      return false;
    }

    if (requireTurn && !this.store.isMyTurn()) {
      return false;
    }

    const phase = this.store.gamePhase();

    if (phase === null) {
      return false;
    }

    return phases.includes(phase);
  }

  public canMoveCheckers(): boolean {
    return this.canAct([GamePhase.MoveCheckers]);
  }

  public canRollDice(): boolean {
    return this.canAct([GamePhase.RollDice, GamePhase.TurnStart]);
  }

  public canOfferDoublingCube(): boolean {
    return this.canAct([GamePhase.TurnStart]);
  }

  public canAnswerDoublingCubeOffer(): boolean {
    return this.canAct([GamePhase.CubeOffered]);
  }

  public canForfeit(): boolean {
    return this.canAct(
      [
        GamePhase.TurnStart,
        GamePhase.RollDice,
        GamePhase.MoveCheckers,
        GamePhase.CubeOffered,
      ],
      false,
    );
  }

  //
  public async rollDice(sessionId: string): Promise<void> {
    if (!this.canRollDice()) {
      console.warn('RollDice not allowed');
      return;
    }

    try {
      await this.signalRService.invoke(HUB_METHODS.RollDice, sessionId);
    } catch (error) {
      console.error('Roll dice failed', error);
      this.store.setError('Failed to roll dice');
    }
  }

  public async moveCheckers(sessionId: string, moves: Move[]): Promise<void> {
    if (!this.canMoveCheckers()) {
      console.warn('MoveCheckers not allowed');
      return;
    }

    // if (!moves || moves.length === 0) {
    //   console.warn('No moves provided');
    //   return;
    // }

    try {
      await this.signalRService.invoke(
        HUB_METHODS.MoveCheckers,
        sessionId,
        moves,
      );
    } catch (error) {
      console.error('Move checkers failed', error);
      this.store.setError('Failed to move checkers');
    }
  }

  public async offerDoublingCube(sessionId: string): Promise<void> {
    if (!this.canOfferDoublingCube()) {
      console.warn('OfferDoublingCube not allowed');
      return;
    }

    try {
      await this.signalRService.invoke(
        HUB_METHODS.OfferDoublingCube,
        sessionId,
      );
    } catch (error) {
      console.error('Offer doubling cube failed', error);
      this.store.setError('Failed to offer doubling cube');
    }
  }

  public async acceptDoublingCube(sessionId: string): Promise<void> {
    if (!this.canAnswerDoublingCubeOffer()) {
      console.warn('AcceptDoublingCube not allowed');
      return;
    }

    try {
      await this.signalRService.invoke(
        HUB_METHODS.AcceptDoublingCube,
        sessionId,
      );
    } catch (error) {
      console.error('Accept doubling cube failed', error);
      this.store.setError('Failed to accept doubling cube');
    }
  }

  public async declineDoublingCube(sessionId: string): Promise<void> {
    if (!this.canAnswerDoublingCubeOffer()) {
      console.warn('DeclineDoublingCube not allowed');
      return;
    }

    try {
      await this.signalRService.invoke(
        HUB_METHODS.DeclineDoublingCube,
        sessionId,
      );
    } catch (error) {
      console.error('Decline doubling cube failed', error);
      this.store.setError('Failed to decline doubling cube');
    }
  }

  public async forfeit(sessionId: string): Promise<void> {
    if (!this.canForfeit()) {
      console.warn('Forfeit not allowed');
      return;
    }

    try {
      await this.signalRService.invoke(HUB_METHODS.Forfeit, sessionId);
    } catch (error) {
      console.error('Forfeit failed', error);
      this.store.setError('Failed to forfeit');
    }
  }
}
