import { Component } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { firstValueFrom } from 'rxjs';
import { ToastrService } from 'ngx-toastr';

import { TournamentBaseResponse } from '../../../models/api/responses/tournament-base.response';
import { TournamentsApiService } from '../../../services/tournaments-api.service';

@Component({
  selector: 'app-tournament-overview',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './tournament-overview.component.html',
  styleUrls: ['./tournament-overview.component.css'],
})
export class TournamentOverviewComponent {
  tournament: TournamentBaseResponse;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private api: TournamentsApiService,
    private toastr: ToastrService,
  ) {
    this.tournament = this.route.parent!.snapshot.data['tournament'];
  }

  showDeleteConfirm = false;

  isOrganizer(): boolean {
    return this.tournament.tournamentUserState === 'ORGANIZER';
  }

  openDeleteConfirm() {
    this.showDeleteConfirm = true;
  }

  closeDeleteConfirm() {
    this.showDeleteConfirm = false;
  }

  async confirmDelete() {
    this.showDeleteConfirm = false;

    try {
      await firstValueFrom(this.api.deleteTournament(this.tournament.id));

      this.toastr.success('Tournament deleted');

      await this.router.navigate(['/tournaments']);
    } catch (err) {
      console.error(err);
      this.toastr.error('Could not delete tournament');
    }
  }
}
