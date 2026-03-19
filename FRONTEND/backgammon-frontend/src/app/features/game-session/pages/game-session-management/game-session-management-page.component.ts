import { CommonModule } from '@angular/common';
import { Component, OnInit, signal } from '@angular/core';
import { CreateGameSessionComponent } from '../../components/create-game-session/create-game-session.component';
import { ActiveGameSessionComponent } from '../../components/active-game-session/active-game-session.component';
import { GetActiveSessionResponse } from '../../models/api/responses/get-active-session.response';
import { GameSessionApiService } from '../../../../core/services/game-session-api.service';
import { ToastrService } from 'ngx-toastr';
import { GameSessionSettings } from '../../models/game-session-settings.model';
import { firstValueFrom } from 'rxjs';

@Component({
  selector: 'app-game-session-management-page',
  standalone: true,
  imports: [
    CommonModule,
    CreateGameSessionComponent,
    ActiveGameSessionComponent,
  ],
  templateUrl: './game-session-management-page.component.html',
})
export class GameSessionManagementPageComponent implements OnInit {
  // STATE
  readonly activeSession = signal<GetActiveSessionResponse | null>(null);
  readonly loading = signal(false);
  readonly error = signal<string | null>(null);

  constructor(
    private readonly api: GameSessionApiService,
    private readonly toastr: ToastrService,
  ) {}

  async ngOnInit(): Promise<void> {
    await this.loadActiveSession();
  }

  // LOAD ACTIVE SESSION
  private async loadActiveSession(): Promise<void> {
    this.loading.set(true);
    this.error.set(null);

    try {
      const session = await firstValueFrom(this.api.getActiveSession());

      this.activeSession.set(session);
    } catch {
      this.error.set('Could not load active session');
    } finally {
      this.loading.set(false);
    }
  }

  // CREATE SESSION
  async onSessionCreated(settings: GameSessionSettings): Promise<void> {
    this.loading.set(true);

    try {
      const response = await firstValueFrom(
        this.api.createSession({ settings }),
      );

      this.activeSession.set({
        sessionId: response.sessionId,
        sessionCode: response.sessionCode,
        settings: response.settings,
        createdAt: response.createdAt,
      });

      this.toastr.success(
        'Game session created successfully',
        'Session created',
      );
    } catch {
      this.toastr.error('Could not create session', 'Error');
    } finally {
      this.loading.set(false);
    }
  }

  // DELETE SESSION
  async onDeleteSession(sessionId: string): Promise<void> {
    this.loading.set(true);

    try {
      await firstValueFrom(this.api.softDeleteSession(sessionId));

      this.activeSession.set(null);

      this.toastr.success(
        'Game session deleted successfully',
        'Session removed',
      );
    } catch {
      this.toastr.error('Could not delete session', 'Error');
    } finally {
      this.loading.set(false);
    }
  }
}
