import { CommonModule } from '@angular/common';
import { Component, OnInit, signal } from '@angular/core';
import { UserGroupJoinRequestResponse } from '../../models/api/responses/user-group-join-request.response';
import { GroupsApiService } from '../../services/groups-api.service';
import { ToastrService } from 'ngx-toastr';
import { firstValueFrom } from 'rxjs';

@Component({
  selector: 'app-groups-join-requests',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './groups-join-requests.component.html',
  styleUrls: ['./groups-join-requests.component.css'],
})
export class GroupsJoinRequestsComponent implements OnInit {
  readonly requests = signal<UserGroupJoinRequestResponse[]>([]);
  readonly loading = signal(false);
  readonly error = signal<string | null>(null);

  constructor(
    private readonly api: GroupsApiService,
    private readonly toastr: ToastrService,
  ) {}

  async ngOnInit(): Promise<void> {
    await this.loadRequests();
  }

  private async loadRequests(): Promise<void> {
    this.loading.set(true);
    this.error.set(null);

    try {
      const result = await firstValueFrom(this.api.getMyJoinRequests());

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
