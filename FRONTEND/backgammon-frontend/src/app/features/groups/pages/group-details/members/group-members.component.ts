import { CommonModule } from '@angular/common';
import { Component, OnInit, signal } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { firstValueFrom } from 'rxjs';

import { GroupsApiService } from '../../../services/groups-api.service';

import { BaseGroupResponse } from '../../../models/api/responses/base-group.response';
import { UserBaseResponse } from '../../../../user/models/api/responses/user-base.response';
import { GroupMembersAddComponent } from '../../../components/group-members-add/group-members-add.component';

type GroupRole = 'Owner' | 'Moderator' | 'Member' | 'None';

@Component({
  selector: 'app-group-members',
  standalone: true,
  imports: [CommonModule, GroupMembersAddComponent],
  templateUrl: './group-members.component.html',
  styleUrls: ['./group-members.component.css'],
})
export class GroupMembersComponent implements OnInit {
  // STATE
  readonly members = signal<UserBaseResponse[]>([]);

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

    await this.loadMembers(group.id);
  }

  private async loadMembers(groupId: string): Promise<void> {
    this.loading.set(true);

    try {
      const res = await firstValueFrom(this.api.getGroupMembers(groupId));

      this.members.set(res.members);
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

  // =========================
  // PERMISSIONS
  // =========================

  isOwner = () => this.role() === 'Owner';

  canManageMembers = () =>
    this.role() === 'Owner' || this.role() === 'Moderator';

  canAddMembers = () =>
    this.canManageMembers() && this.group()?.visibility === 'Private';

  // =========================
  // ACTIONS
  // =========================

  async onAddMember(username: string) {
    const groupId = this.group()?.id;

    if (!groupId) return;

    await firstValueFrom(this.api.addGroupMember(groupId, username));

    await this.loadMembers(groupId);
  }

  async onPromote(userId: string) {
    const groupId = this.group()?.id;

    if (!groupId) return;

    await firstValueFrom(this.api.promoteToModerator(groupId, userId));

    await this.loadMembers(groupId);
  }

  async onDemote(userId: string) {
    const groupId = this.group()?.id;

    if (!groupId) return;

    await firstValueFrom(this.api.demoteModerator(groupId, userId));

    await this.loadMembers(groupId);
  }

  async onRemove(userId: string) {
    const groupId = this.group()?.id;

    if (!groupId) return;

    await firstValueFrom(this.api.removeGroupMember(groupId, userId));

    await this.loadMembers(groupId);
  }

  trackById(index: number, item: UserBaseResponse): string {
    return item.id;
  }
}
