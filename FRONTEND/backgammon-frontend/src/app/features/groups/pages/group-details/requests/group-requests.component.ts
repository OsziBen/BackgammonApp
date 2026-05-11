import { CommonModule } from '@angular/common';
import { Component, OnInit, signal } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { firstValueFrom } from 'rxjs';
import { GroupsApiService } from '../../../services/groups-api.service';
import { BaseGroupResponse } from '../../../models/api/responses/base-group.response';
import { GroupJoinRequestResponse } from '../../../models/api/responses/group-join-request.response';
import { GroupJoinRequestCardComponent } from '../../../components/group-join-request-card/group-join-request-card.component';

type GroupRole = 'Owner' | 'Moderator' | 'Member' | 'None';

@Component({
  selector: 'app-group-requests',
  standalone: true,
  imports: [CommonModule, GroupJoinRequestCardComponent],
  templateUrl: './group-requests.component.html',
  styleUrls: ['./group-requests.component.css'],
})
export class GroupRequestsComponent implements OnInit {
  readonly requests = signal<GroupJoinRequestResponse[]>([]);

  readonly group = signal<BaseGroupResponse | null>(null);

  readonly role = signal<GroupRole>('None');

  readonly loading = signal(false);

  constructor(
    private route: ActivatedRoute,
    private api: GroupsApiService,
  ) {}

  async ngOnInit(): Promise<void> {
    const group = this.route.parent?.snapshot.data[
      'group'
    ] as BaseGroupResponse;

    if (!group) return;

    this.group.set(group);

    this.role.set(this.mapRole(group.groupUserState));

    await this.loadRequests(group.id);
  }

  private async loadRequests(groupId: string): Promise<void> {
    this.loading.set(true);

    try {
      const res = await firstValueFrom(this.api.getGroupJoinRequests(groupId));

      this.requests.set(res);
    } finally {
      this.loading.set(false);
    }
  }

  private mapRole(state: string | null): GroupRole {
    switch (state) {
      case 'OWNER':
        return 'Owner';

      case 'MODERATOR':
        return 'Moderator';

      case 'MEMBER':
        return 'Member';

      default:
        return 'None';
    }
  }

  canManageRequests = () =>
    this.role() === 'Owner' || this.role() === 'Moderator';

  async onApprove(requestId: string) {
    const groupId = this.group()?.id;

    if (!groupId) return;

    await firstValueFrom(this.api.approveJoinRequest(groupId, requestId));

    await this.loadRequests(groupId);
  }

  async onReject(requestId: string) {
    const groupId = this.group()?.id;

    if (!groupId) return;

    await firstValueFrom(this.api.rejectJoinRequest(groupId, requestId));

    await this.loadRequests(groupId);
  }

  trackById(index: number, item: GroupJoinRequestResponse): string {
    return item.id;
  }
}
