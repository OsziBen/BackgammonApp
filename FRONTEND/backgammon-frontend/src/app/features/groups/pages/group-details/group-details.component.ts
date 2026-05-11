import { Component, computed } from '@angular/core';
import {
  ActivatedRoute,
  RouterOutlet,
  RouterLink,
  RouterLinkActive,
} from '@angular/router';
import { CommonModule } from '@angular/common';
import { BaseGroupResponse } from '../../models/api/responses/base-group.response';

type GroupRole = 'Owner' | 'Moderator' | 'Member' | 'None';

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
    if (!g) return 'None';

    switch (g.groupUserState) {
      case 'OWNER':
        return 'Owner';
      case 'MODERATOR':
        return 'Moderator';
      case 'MEMBER':
        return 'Member';
      default:
        return 'None';
    }
  });

  isModeratorOrOwner = computed(
    () => this.role() === 'Owner' || this.role() === 'Moderator',
  );
}
