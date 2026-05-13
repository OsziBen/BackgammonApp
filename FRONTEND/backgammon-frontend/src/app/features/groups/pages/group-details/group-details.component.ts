import { Component, computed } from '@angular/core';
import {
  ActivatedRoute,
  RouterOutlet,
  RouterLink,
  RouterLinkActive,
} from '@angular/router';
import { CommonModule } from '@angular/common';
import { BaseGroupResponse } from '../../models/api/responses/base-group.response';

type GroupRole = 'OWNER' | 'MODERATOR' | 'MEMBER' | 'NONE';

@Component({
  selector: 'app-group-details',
  standalone: true,
  imports: [CommonModule, RouterOutlet, RouterLink, RouterLinkActive],
  templateUrl: './group-details.component.html',
  styleUrls: ['./group-details.component.css'],
})
export class GroupDetailsComponent {
  constructor(private route: ActivatedRoute) {}

  // RESOLVED DATA
  group = computed(
    () => this.route.snapshot.data['group'] as BaseGroupResponse,
  );

  role = computed<GroupRole>(() => {
    const g = this.group();
    if (!g) return 'NONE';

    switch (g.groupUserState) {
      case 'OWNER':
        return 'OWNER';
      case 'MODERATOR':
        return 'MODERATOR';
      case 'MEMBER':
        return 'MEMBER';
      default:
        return 'NONE';
    }
  });

  isModeratorOrOwner = computed(
    () => this.role() === 'OWNER' || this.role() === 'MODERATOR',
  );

  isPrivateGroup = computed(() => this.group()?.visibility === 'Private');

  showRequestsTab = computed(
    () => this.isModeratorOrOwner() && !this.isPrivateGroup(),
  );
}
