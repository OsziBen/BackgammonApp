import { CommonModule } from '@angular/common';
import { Component, OnInit, signal } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
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
  readonly error = signal<string | null>(null);

  constructor(
    private readonly route: ActivatedRoute,
    private readonly api: TournamentsApiService,
    private readonly toastr: ToastrService,
  ) {}

  async ngOnInit(): Promise<void> {
    const tournament = this.route.parent?.snapshot.data[
      'tournament'
    ] as TournamentBaseResponse;

    if (!tournament) {
      this.error.set('Tournament not found');
      return;
    }

    this.tournament.set(tournament);

    this.role.set(this.mapRole(tournament.tournamentUserState));

    await this.loadRequests(tournament.id);
  }

  private async loadRequests(tournamentId: string): Promise<void> {
    this.loading.set(true);
    this.error.set(null);

    try {
      const result = await firstValueFrom(
        this.api.getTournamentJoinRequests(tournamentId),
      );

      this.requests.set(result);
    } catch (err) {
      console.error(err);

      this.error.set('Could not load join requests');

      this.toastr.error('Could not load join requests', 'Error');
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

  canManageRequests(): boolean {
    return this.role() === 'Organizer';
  }

  async onApprove(requestId: string): Promise<void> {
    const tournamentId = this.tournament()?.id;

    if (!tournamentId) return;

    try {
      await firstValueFrom(
        this.api.approveJoinRequest(tournamentId, requestId),
      );

      this.toastr.success('Join request approved', 'Success');

      await this.loadRequests(tournamentId);
    } catch (err) {
      console.error(err);

      this.toastr.error('Could not approve join request', 'Error');
    }
  }

  async onReject(requestId: string): Promise<void> {
    const tournamentId = this.tournament()?.id;

    if (!tournamentId) return;

    try {
      await firstValueFrom(this.api.rejectJoinRequest(tournamentId, requestId));

      this.toastr.success('Join request rejected', 'Success');

      await this.loadRequests(tournamentId);
    } catch (err) {
      console.error(err);

      this.toastr.error('Could not reject join request', 'Error');
    }
  }
}
