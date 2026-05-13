import { Component } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { BaseGroupResponse } from '../../../models/api/responses/base-group.response';
import { GroupsApiService } from '../../../services/groups-api.service';
import { ToastrService } from 'ngx-toastr';
import { firstValueFrom } from 'rxjs';

@Component({
  selector: 'app-group-overview',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './group-overview.component.html',
  styleUrls: ['./group-overview.component.css'],
})
export class GroupOverviewComponent {
  group: BaseGroupResponse;

  showDeleteConfirm = false;

  constructor(
    private readonly route: ActivatedRoute,
    private readonly router: Router,
    private readonly groupsApi: GroupsApiService,
    private readonly toastr: ToastrService,
  ) {
    this.group = this.route.parent!.snapshot.data['group'];
  }

  isOwner(): boolean {
    return this.group.groupUserState === 'OWNER';
  }

  openDeleteConfirm(): void {
    this.showDeleteConfirm = true;
  }

  closeDeleteConfirm(): void {
    this.showDeleteConfirm = false;
  }

  async confirmDelete(): Promise<void> {
    this.showDeleteConfirm = false;

    try {
      await firstValueFrom(this.groupsApi.deleteGroup(this.group.id));

      this.toastr.success('Group deleted');

      await this.router.navigate(['/groups']);
    } catch (err) {
      console.error(err);

      this.toastr.error('Could not delete group');
    }
  }
}
