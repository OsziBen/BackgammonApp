import { Component, signal } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { firstValueFrom } from 'rxjs';
import { TournamentFormComponent } from '../../components/tournament-form/tournament-form.component';
import { CreateTournamentRequest } from '../../models/api/requests/create-tournament.request';
import { TournamentsApiService } from '../../services/tournaments-api.service';
import { AppRoutes } from '../../../../shared/constants/app-routes.constants';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-tournaments-create',
  standalone: true,
  imports: [CommonModule, TournamentFormComponent],
  templateUrl: './tournaments-create.component.html',
  styleUrls: ['./tournaments-create.component.css'],
})
export class TournamentsCreateComponent {
  // STATE
  readonly loading = signal(false);
  readonly error = signal<string | null>(null);

  constructor(
    private readonly api: TournamentsApiService,
    private readonly router: Router,
    private readonly toastr: ToastrService,
  ) {}

  // CREATE TOURNAMENT
  async onSubmit(request: CreateTournamentRequest): Promise<void> {
    this.loading.set(true);
    this.error.set(null);

    try {
      const response = await firstValueFrom(this.api.createTournament(request));

      this.toastr.success('Tournament created successfully', 'Success');

      await this.router.navigate([AppRoutes.tournaments, response.id]);

      // alternatíva:
      // await this.router.navigate([AppRoutes.tournaments, AppRoutes.tournamentsMy]);
    } catch (err) {
      console.error(err);

      this.error.set('Could not create tournament');

      this.toastr.error('Could not create tournament', 'Error');
    } finally {
      this.loading.set(false);
    }
  }
}
