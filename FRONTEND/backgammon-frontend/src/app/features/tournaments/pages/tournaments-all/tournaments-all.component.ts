import { CommonModule } from '@angular/common';
import { Component, OnInit, signal } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { firstValueFrom } from 'rxjs';

import { TournamentBaseResponse } from '../../models/api/responses/tournament-base.response';
import { TournamentsApiService } from '../../services/tournaments-api.service';
import { TournamentCardComponent } from '../../components/tournament-card/tournament-card.component';

@Component({
  selector: 'app-tournaments-all',
  standalone: true,
  imports: [CommonModule, TournamentCardComponent],
  templateUrl: './tournaments-all.component.html',
  styleUrls: ['./tournaments-all.component.css'],
})
export class TournamentsAllComponent implements OnInit {
  readonly tournaments = signal<TournamentBaseResponse[]>([]);
  readonly loading = signal(false);
  readonly error = signal<string | null>(null);

  constructor(
    private readonly api: TournamentsApiService,
    private readonly toastr: ToastrService,
  ) {}

  async ngOnInit(): Promise<void> {
    await this.loadTournaments();
  }

  private async loadTournaments(): Promise<void> {
    this.loading.set(true);
    this.error.set(null);

    try {
      const result = await firstValueFrom(this.api.getAllTournaments());

      this.tournaments.set(result);
    } catch (err) {
      console.error(err);

      this.error.set('Could not load tournaments');

      this.toastr.error('Could not load tournaments', 'Error');
    } finally {
      this.loading.set(false);
    }
  }

  async onJoin(tournamentId: string): Promise<void> {
    try {
      //await firstValueFrom(this.api.joinTournament(tournamentId));

      this.toastr.success('Joined tournament', 'Success');

      // ha nincs canJoin meződ, akkor status-t frissítünk
      this.tournaments.update((tournaments) =>
        tournaments.map((t) =>
          t.id === tournamentId ? { ...t, status: 'RegistrationClosed' } : t,
        ),
      );
    } catch {
      this.toastr.error('Could not join tournament', 'Error');
    }
  }
}
