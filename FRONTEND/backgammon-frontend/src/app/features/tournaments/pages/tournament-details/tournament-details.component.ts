import { Component, computed } from '@angular/core';
import {
  ActivatedRoute,
  RouterOutlet,
  RouterLink,
  RouterLinkActive,
} from '@angular/router';
import { CommonModule } from '@angular/common';
import { TournamentBaseResponse } from '../../models/api/responses/tournament-base.response';

type TournamentRole = 'Organizer' | 'Participant' | 'None';

@Component({
  selector: 'app-tournament-details',
  standalone: true,
  imports: [CommonModule, RouterOutlet, RouterLink, RouterLinkActive],
  templateUrl: './tournament-details.component.html',
  styleUrls: ['./tournament-details.component.css'],
})
export class TournamentDetailsComponent {
  constructor(private route: ActivatedRoute) {}

  // RESOLVED DATA
  tournament = computed(
    () => this.route.snapshot.data['tournament'] as TournamentBaseResponse,
  );

  role = computed<TournamentRole>(() => {
    const t = this.tournament();
    if (!t) return 'None';

    switch (t.tournamentUserState) {
      case 'ORGANIZER':
        return 'Organizer';
      case 'PARTICIPANT':
        return 'Participant';
      default:
        return 'None';
    }
  });

  isOrganizer = computed(() => this.role() === 'Organizer');

  isParticipantOrOrganizer = computed(
    () => this.role() === 'Organizer' || this.role() === 'Participant',
  );
}
