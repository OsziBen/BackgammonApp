import { CommonModule } from '@angular/common';
import { Component, OnInit, signal } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { firstValueFrom } from 'rxjs';
import { TournamentsApiService } from '../../../services/tournaments-api.service';
import { TournamentBaseResponse } from '../../../models/api/responses/tournament-base.response';
import { TournamentParticipantsAddComponent } from '../../../components/tournament-participants-add/tournament-participants-add.component';
import { TournamentParticipantBaseResponse } from '../../../models/api/responses/tournament-participant-base.response';

type TournamentRole = 'Organizer' | 'Participant' | 'None';

@Component({
  selector: 'app-tournament-participants',
  standalone: true,
  imports: [CommonModule, TournamentParticipantsAddComponent],
  templateUrl: './tournament-participants.component.html',
  styleUrls: ['./tournament-participants.component.css'],
})
export class TournamentParticipantsComponent implements OnInit {
  // STATE
  readonly participants = signal<TournamentParticipantBaseResponse[]>([]);
  readonly tournament = signal<TournamentBaseResponse | null>(null);
  readonly role = signal<TournamentRole>('None');
  readonly loading = signal(false);

  constructor(
    private route: ActivatedRoute,
    private api: TournamentsApiService,
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
      const res = await firstValueFrom(
        this.api.getTournamentParticipants(tournamentId),
      );

      this.participants.set(res.participants);
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

  isOrganizer = () => this.role() === 'Organizer';

  canManageParticipants = () => this.role() === 'Organizer';

  canAddParticipants = () =>
    this.isOrganizer() && this.tournament()?.visibility === 'Private';

  // =========================
  // ACTIONS
  // =========================

  async onAddParticipant(username: string) {
    const tournamentId = this.tournament()?.id;

    if (!tournamentId) return;

    await firstValueFrom(
      this.api.addTournamentParticipant(tournamentId, username),
    );

    await this.loadParticipants(tournamentId);
  }

  async onRemove(userId: string) {
    const tournamentId = this.tournament()?.id;

    if (!tournamentId) return;

    await firstValueFrom(
      this.api.removeTournamentParticipant(tournamentId, userId),
    );

    await this.loadParticipants(tournamentId);
  }

  trackById(index: number, item: TournamentParticipantBaseResponse): string {
    return item.userId;
  }
}
