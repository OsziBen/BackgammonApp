import { CommonModule } from '@angular/common';
import { Component, OnInit, signal } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { firstValueFrom } from 'rxjs';
import { TournamentsApiService } from '../../../services/tournaments-api.service';
import { TournamentBaseResponse } from '../../../models/api/responses/tournament-base.response';
import { TournamentParticipantBaseResponse } from '../../../models/api/responses/tournament-participant-base.response';
import { TournamentParticipantsAddComponent } from '../../../components/tournament-participants-add/tournament-participants-add.component';
import { TournamentParticipantCardComponent } from '../../../components/tournament-participant-card/tournament-participant-card.component';

type TournamentRole = 'Organizer' | 'Participant' | 'None';

@Component({
  selector: 'app-tournament-participants',
  standalone: true,
  imports: [
    CommonModule,
    TournamentParticipantsAddComponent,
    TournamentParticipantCardComponent,
  ],
  templateUrl: './tournament-participants.component.html',
  styleUrls: ['./tournament-participants.component.css'],
})
export class TournamentParticipantsComponent implements OnInit {
  readonly participants = signal<TournamentParticipantBaseResponse[]>([]);
  readonly tournament = signal<TournamentBaseResponse | null>(null);

  readonly role = signal<TournamentRole>('None');

  readonly loading = signal(false);

  constructor(
    private readonly route: ActivatedRoute,
    private readonly api: TournamentsApiService,
  ) {}

  async ngOnInit(): Promise<void> {
    const tournament = this.route.parent?.snapshot.data[
      'tournament'
    ] as TournamentBaseResponse;

    if (!tournament) return;

    this.tournament.set(tournament);

    this.role.set(this.mapRole(tournament.tournamentUserState));

    await this.loadParticipants(tournament.id);
  }

  private async loadParticipants(tournamentId: string): Promise<void> {
    this.loading.set(true);

    try {
      const result = await firstValueFrom(
        this.api.getTournamentParticipants(tournamentId),
      );

      this.participants.set(result.participants);
    } finally {
      this.loading.set(false);
    }
  }

  private mapRole(state: string | null): TournamentRole {
    switch (state) {
      case 'ORGANIZER':
        return 'Organizer';

      case 'PARTICIPANT':
        return 'Participant';

      default:
        return 'None';
    }
  }

  // =========================
  // PERMISSIONS
  // =========================

  isOrganizer(): boolean {
    return this.role() === 'Organizer';
  }

  canManageParticipants(): boolean {
    return this.role() === 'Organizer';
  }

  canAddParticipants(): boolean {
    return this.isOrganizer() && this.tournament()?.visibility === 'Private';
  }

  // =========================
  // ACTIONS
  // =========================

  async onAddParticipant(username: string): Promise<void> {
    const tournamentId = this.tournament()?.id;

    if (!tournamentId) return;

    await firstValueFrom(
      this.api.addTournamentParticipant(tournamentId, username),
    );

    await this.loadParticipants(tournamentId);
  }

  async onRemove(userId: string): Promise<void> {
    const tournamentId = this.tournament()?.id;

    if (!tournamentId) return;

    await firstValueFrom(
      this.api.removeTournamentParticipant(tournamentId, userId),
    );

    await this.loadParticipants(tournamentId);
  }
}
