import { CommonModule } from '@angular/common';
import { Component, OnInit, signal } from '@angular/core';
import { BaseGroupResponse } from '../../models/api/responses/base-group.response';
import { GroupsApiService } from '../../services/groups-api.service';
import { ToastrService } from 'ngx-toastr';
import { firstValueFrom } from 'rxjs';
import { GroupCardComponent } from '../../components/group-card/group-card.component';
import { UsersApiService } from '../../../user/services/users-api.service';

@Component({
  selector: 'app-groups-my',
  standalone: true,
  imports: [CommonModule, GroupCardComponent],
  templateUrl: './groups-my.component.html',
  styleUrls: ['./groups-my.component.css'],
})
export class GroupsMyComponent implements OnInit {
  readonly groups = signal<BaseGroupResponse[]>([]);
  readonly loading = signal(false);
  readonly error = signal<string | null>(null);

  constructor(
    private readonly usersApi: UsersApiService,
    private readonly toastr: ToastrService,
  ) {}

  async ngOnInit(): Promise<void> {
    await this.loadGroups();
  }

  private async loadGroups(): Promise<void> {
    this.loading.set(true);
    this.error.set(null);

    try {
      const result = await firstValueFrom(this.usersApi.getMyGroups());
      this.groups.set(result);
    } catch (err) {
      console.error(err);
      this.error.set('Could not load your groups');
      this.toastr.error('Could not load your groups', 'Error');
    } finally {
      this.loading.set(false);
    }
  }

  trackById(index: number, item: BaseGroupResponse): string {
    return item.id;
  }
}
