import { CommonModule } from '@angular/common';
import { Component, OnInit, signal } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { firstValueFrom } from 'rxjs';
import { TournamentsApiService } from '../../../services/tournaments-api.service';
import { TournamentBaseResponse } from '../../../models/api/responses/tournament-base.response';
import { TournamentJoinRequestResponse } from '../../../models/api/responses/tournament-join-request.response';
import { TournamentJoinRequestCardComponent } from '../../../components/tournament-join-request-card/tournament-join-request-card.component';

type TournamentRole = 'Organizer' | 'Participant' | 'None';

@Component({
  selector: 'app-tournament-requests',
  standalone: true,
  imports: [CommonModule, TournamentJoinRequestCardComponent],
  templateUrl: './tournament-requests.component.html',
  styleUrls: ['./tournament-requests.component.css'],
})
export class TournamentRequestsComponent implements OnInit {
  readonly requests = signal<TournamentJoinRequestResponse[]>([]);
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

    await this.loadRequests(tournament.id);
  }

  private async loadRequests(tournamentId: string): Promise<void> {
    this.loading.set(true);

    try {
      const res = await firstValueFrom(
        this.api.getTournamentJoinRequests(tournamentId),
      );

      this.requests.set(res);
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

  canManageRequests = () => this.role() === 'Organizer';

  // =========================
  // ACTIONS
  // =========================

  async onApprove(requestId: string) {
    const tournamentId = this.tournament()?.id;

    if (!tournamentId) return;

    await firstValueFrom(this.api.approveJoinRequest(tournamentId, requestId));

    await this.loadRequests(tournamentId);
  }

  async onReject(requestId: string) {
    const tournamentId = this.tournament()?.id;

    if (!tournamentId) return;

    await firstValueFrom(this.api.rejectJoinRequest(tournamentId, requestId));

    await this.loadRequests(tournamentId);
  }

  trackById(index: number, item: TournamentJoinRequestResponse): string {
    return item.id;
  }
}
