import { computed, Injectable, signal } from '@angular/core';
import { ConnectionState } from '../models/enums/connection-state.enum';
import { GameSessionSnapshotResponse } from '../models/api/game-session-snapshot-response.model';

interface InternalState {
  snapshot: GameSessionSnapshotResponse | null;
  connectionState: ConnectionState;
}

@Injectable({
  providedIn: 'root',
})
export class GameSessionStore {
  private readonly _state = signal<InternalState>({
    snapshot: null,
    connectionState: ConnectionState.Disconnected,
  });

  // SNAPSHOT
  readonly snapshot = computed(() => this._state().snapshot);

  readonly hasSnapshot = computed(() => !!this._state().snapshot);

  // CONNECTION
  readonly connectionState = computed(() => this._state().connectionState);

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
  setSnapshot(newSnapshot: GameSessionSnapshotResponse) {
    const current = this._state().snapshot;

    if (!current || newSnapshot.version > current.version) {
      this._state.update((state) => ({
        ...state,
        snapshot: newSnapshot,
      }));
    } else {
      console.warn(
        `Ignored snapshot update: current version=${current.version}, incoming version=${newSnapshot.version}`,
      );
    }
  }

  setConnectionState(connectionState: ConnectionState) {
    this._state.update((state) => ({
      ...state,
      connectionState,
    }));
  }

  reset() {
    this._state.set({
      snapshot: null,
      connectionState: ConnectionState.Disconnected,
    });
  }
}
