import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { TournamentBaseResponse } from '../../../models/api/responses/tournament-base.response';

@Component({
  selector: 'app-tournament-overview',
  standalone: true,
  templateUrl: './tournament-overview.component.html',
})
export class TournamentOverviewComponent {
  tournament: TournamentBaseResponse;

  constructor(private route: ActivatedRoute) {
    this.tournament = this.route.parent!.snapshot.data['tournament'];
  }

  isOrganizer(): boolean {
    return this.tournament.tournamentUserState === 'ORGANIZER';
  }
}
