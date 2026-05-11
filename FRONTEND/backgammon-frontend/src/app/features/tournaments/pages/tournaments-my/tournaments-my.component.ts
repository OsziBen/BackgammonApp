import { CommonModule } from '@angular/common';
import { Component, OnInit, signal } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { TournamentCardComponent } from '../../components/tournament-card/tournament-card.component';
import { TournamentBaseResponse } from '../../models/api/responses/tournament-base.response';
import { UsersApiService } from '../../../user/services/users-api.service';

@Component({
  selector: 'app-tournaments-my',
  standalone: true,
  imports: [CommonModule, TournamentCardComponent],
  templateUrl: './tournaments-my.component.html',
  styleUrls: ['./tournaments-my.component.css'],
})
export class TournamentsMyComponent implements OnInit {
  readonly tournaments = signal<TournamentBaseResponse[]>([]);
  readonly loading = signal(false);
  readonly error = signal<string | null>(null);

  constructor(
    private readonly usersApi: UsersApiService,
    private readonly toastr: ToastrService,
  ) {}

  async ngOnInit(): Promise<void> {
    await this.loadTournaments();
  }

  private async loadTournaments(): Promise<void> {
    this.loading.set(true);
    this.error.set(null);

    try {
      const result = await firstValueFrom(this.usersApi.getMyTournaments());

      this.tournaments.set(result);
    } catch (err) {
      console.error(err);
      this.error.set('Could not load your tournaments');
      this.toastr.error('Could not load your tournaments', 'Error');
    } finally {
      this.loading.set(false);
    }
  }

  trackById(index: number, item: TournamentBaseResponse): string {
    return item.id;
  }
}
