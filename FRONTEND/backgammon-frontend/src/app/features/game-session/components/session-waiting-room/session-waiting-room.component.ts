import { CommonModule } from '@angular/common';
import { Component, effect } from '@angular/core';
import { ConnectionState } from '../../models/enums/connection-state.enum';
import { GameSessionFacade } from '../../facade/game-session.facade';
import { GameSessionStore } from '../../state/game-session.store';
import { Router } from '@angular/router';
import { AppRoutes } from '../../../../shared/constants/app-routes.constants';

@Component({
  selector: 'app-session-waiting-room',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './session-waiting-room.component.html',
  styleUrls: ['./session-waiting-room.component.css'],
})
export class SessionWaitingRoomComponent {
  readonly ConnectionState = ConnectionState;

  constructor(
    private readonly facade: GameSessionFacade,
    private readonly router: Router,
    public readonly store: GameSessionStore,
  ) {
    effect(() => {
      const conn = this.store.connectionState();
      console.log('UI connectionState changed:', conn);
      // itt Angular automatikusan újrarenderel
    });
  }

  public leave(): void {
    this.facade.leaveSession();

    this.router.navigate([AppRoutes.sessions]);
  }

  public connectionState(): ConnectionState {
    return this.store.connectionState();
  }

  public snapshot() {
    return this.store.snapshot();
  }

  public players() {
    return this.store.players();
  }

  public localPlayerId() {
    return this.store.localPlayerId();
  }

  public isHost(): boolean {
    return this.store.isHost();
  }
}
