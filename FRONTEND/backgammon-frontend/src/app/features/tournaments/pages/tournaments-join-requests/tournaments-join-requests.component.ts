import { CommonModule } from '@angular/common';
import { Component, OnInit, signal } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { UsersApiService } from '../../../user/services/users-api.service';
import { UserTournamentJoinRequestResponse } from '../../models/api/responses/user-tournament-join-request.response';
import { UserTournamentJoinRequestCardComponent } from '../../components/user-tournament-join-request-card/user-tournament-join-request-card.component';

@Component({
  selector: 'app-tournaments-join-requests',
  standalone: true,
  imports: [CommonModule, UserTournamentJoinRequestCardComponent],
  templateUrl: './tournaments-join-requests.component.html',
  styleUrls: ['./tournaments-join-requests.component.css'],
})
export class TournamentsJoinRequestsComponent implements OnInit {
  readonly requests = signal<UserTournamentJoinRequestResponse[]>([]);
  readonly loading = signal(false);
  readonly error = signal<string | null>(null);

  constructor(
    private readonly usersApi: UsersApiService,
    private readonly toastr: ToastrService,
  ) {}

  async ngOnInit(): Promise<void> {
    await this.loadRequests();
  }

  private async loadRequests(): Promise<void> {
    this.loading.set(true);
    this.error.set(null);

    try {
      const result = await firstValueFrom(
        this.usersApi.getMyTournamentJoinRequests(),
      );

      this.requests.set(result);
    } catch (err) {
      console.error(err);

      this.error.set('Could not load tournament join requests');

      this.toastr.error('Could not load tournament join requests', 'Error');
    } finally {
      this.loading.set(false);
    }
  }
}
