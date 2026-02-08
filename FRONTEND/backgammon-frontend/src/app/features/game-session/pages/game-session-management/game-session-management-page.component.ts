import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Observable } from 'rxjs';
import { GameSessionFacade } from '../../../../core/states/game-session/game-session.facade';
import { CreateGameSessionComponent } from '../../components/create-game-session/create-game-session.component';
import { GetActiveSessionResponse } from '../../models/api/responses/get-active-session.response';
import { GameSessionSettings } from '../../../../core/models/game-session/game-session-settings.model';
import { ActiveGameSessionComponent } from '../../components/active-game-session/active-game-session.component';
import { ToastrService } from 'ngx-toastr';

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
  activeSession$!: Observable<GetActiveSessionResponse | null>;
  loading$!: Observable<boolean>;
  error$!: Observable<string | null>;

  // ideiglenes, auth később
  private readonly userId = '11111111-1111-1111-1111-111111111111';

  constructor(
    private readonly facade: GameSessionFacade,
    private readonly toastr: ToastrService,
  ) {}

  ngOnInit(): void {
    this.activeSession$ = this.facade.activeSession$;
    this.loading$ = this.facade.loading$;
    this.error$ = this.facade.error$;

    this.facade.loadActiveSession(this.userId);
  }

  onSessionCreated(settings: GameSessionSettings): void {
    this.facade.createSession(this.userId, settings).subscribe({
      next: () => {
        this.toastr.success(
          'Game session created successfully',
          'Session created',
        );
      },
      error: () => {
        this.toastr.error('Could not create session', 'Error');
      },
    });
  }

  onDeleteSession(sessionId: string): void {
    this.facade.deleteSession(sessionId).subscribe({
      next: () => {
        this.toastr.success(
          'Game session deleted successfully',
          'Session removed',
        );
      },
      error: () => {
        this.toastr.error('Could not delete session', 'Error');
      },
    });
  }
}
