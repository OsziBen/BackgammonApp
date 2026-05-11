import { CommonModule } from '@angular/common';
import { Component, OnInit, signal } from '@angular/core';
import { UserGroupJoinRequestResponse } from '../../models/api/responses/user-group-join-request.response';
import { ToastrService } from 'ngx-toastr';
import { firstValueFrom } from 'rxjs';
import { UsersApiService } from '../../../user/services/users-api.service';
import { UserGroupJoinRequestCardComponent } from '../../components/user-group-join-request-card/user-group-join-request-card.component';

@Component({
  selector: 'app-groups-join-requests',
  standalone: true,
  imports: [CommonModule, UserGroupJoinRequestCardComponent],
  templateUrl: './groups-join-requests.component.html',
  styleUrls: ['./groups-join-requests.component.css'],
})
export class GroupsJoinRequestsComponent implements OnInit {
  readonly requests = signal<UserGroupJoinRequestResponse[]>([]);
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
        this.usersApi.getMyGroupJoinRequests(),
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
}
