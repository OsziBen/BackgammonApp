import { computed, Injectable, signal } from '@angular/core';
import { ConnectionState } from '../models/enums/connection-state.enum';
import { GameSessionSnapshotResponse } from '../models/api/game-session-snapshot-response.model';
import { GamePhase } from '../models/enums/game-phase.enum';

interface InternalState {
  snapshot: GameSessionSnapshotResponse | null;
  connectionState: ConnectionState;
  error: string | null;
}

@Injectable({
  providedIn: 'root',
})
export class GameSessionStore {
  private readonly _state = signal<InternalState>({
    snapshot: null,
    connectionState: ConnectionState.Disconnected,
    error: null,
  });

  // SNAPSHOT
  readonly snapshot = computed(() => this._state().snapshot);
  readonly hasSnapshot = computed(() => !!this._state().snapshot);

  // CONNECTION
  readonly connectionState = computed(() => this._state().connectionState);
  readonly error = computed(() => this._state().error);

  // SESSION DATA
  readonly players = computed(() => this._state().snapshot?.players ?? []);

  readonly currentPlayerId = computed(
    () => this._state().snapshot?.currentPlayerId ?? null,
  );

  readonly gamePhase = computed(
    () => this._state().snapshot?.currentPhase ?? null,
  );

  readonly isFinished = computed(
    () => this._state().snapshot?.isFinished ?? false,
  );

  readonly isWaitingRoom = computed(
    () => this.gamePhase() === GamePhase.WaitingForPlayers,
  );

  // PLAYER SELECTORS
  readonly localPlayerId = computed(
    () => this._state().snapshot?.localPlayerId ?? null,
  );

  readonly localPlayer = computed(() => {
    const snapshot = this._state().snapshot;

    if (!snapshot) {
      return null;
    }

    return (
      snapshot.players.find((p) => p.playerId === snapshot.localPlayerId) ??
      null
    );
  });

  readonly opponentPlayer = computed(() => {
    const snapshot = this._state().snapshot;

    if (!snapshot) {
      return null;
    }

    return (
      snapshot.players.find((p) => p.playerId !== snapshot.localPlayerId) ??
      null
    );
  });

  readonly isMyTurn = computed(() => {
    const snapshot = this._state().snapshot;

    if (!snapshot) {
      return false;
    }

    return snapshot.currentPlayerId === snapshot.localPlayerId;
  });

  readonly isHost = computed(() => {
    const player = this.localPlayer();

    return player?.isHost ?? false;
  });

  // STATE UPDATES
  setSnapshot(newSnapshot: GameSessionSnapshotResponse, force = false) {
    const current = this._state().snapshot;
    console.log('SET SNAPSHOT CALLED');
    console.log('Incoming version:', newSnapshot.version);
    console.log('Current version:', current?.version);
    const isDifferentSession =
      current && current.sessionId !== newSnapshot.sessionId;

    if (
      force ||
      isDifferentSession ||
      !current ||
      newSnapshot.version >= current.version
    ) {
      if (isDifferentSession) {
        console.log('New session detected → resetting state');
      }

      this._state.update((state) => ({
        ...state,
        snapshot: newSnapshot,
        error: null,
      }));
    } else {
      console.warn(
        `Ignored snapshot: current=${current.version}, incoming=${newSnapshot.version}`,
      );
    }
  }

  setConnectionState(connectionState: ConnectionState) {
    this._state.update((state) => ({
      ...state,
      connectionState,
    }));
  }

  setError(message: string | null) {
    this._state.update((state) => ({
      ...state,
      error: message,
    }));
  }

  reset() {
    console.log('Store reset');

    this._state.set({
      snapshot: null,
      connectionState: ConnectionState.Disconnected,
      error: null,
    });
  }
}
