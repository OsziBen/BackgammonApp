import { CommonModule } from '@angular/common';
import { Component, effect } from '@angular/core';
import { SessionWaitingRoomComponent } from '../../components/session-waiting-room/session-waiting-room.component';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '../../../../shared/services/auth.service';
import { GameSessionStore } from '../../state/game-session.store';
import { GameSessionFacade } from '../../facade/game-session.facade';
import { GamePhase } from '../../models/enums/game-phase.enum';

@Component({
  selector: 'app-session-room-page',
  standalone: true,
  imports: [CommonModule, SessionWaitingRoomComponent],
  templateUrl: './session-room.page.component.html',
})
export class SessionRoomPageComponent {
  constructor(
    private route: ActivatedRoute,
    private auth: AuthService,
    private facade: GameSessionFacade,
    private router: Router,
    public store: GameSessionStore,
  ) {
    const code = this.route.snapshot.paramMap.get('code');
    const token = this.auth.getToken();

    if (code && token) {
      this.facade.joinSession(code, token);
    }

    effect(() => {
      const snapshot = this.store.snapshot();

      if (!snapshot) {
        return;
      }

      if (snapshot.currentPhase !== GamePhase.WaitingForPlayers) {
        this.router.navigate(['/sessions', snapshot.sessionCode, 'game']);
      }
    });
  }
}
