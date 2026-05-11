import { Component, EventEmitter, Input, Output } from '@angular/core';
import { BaseGroupResponse } from '../../models/api/responses/base-group.response';
import { Router } from '@angular/router';
import { AppRoutes } from '../../../../shared/constants/app-routes.constants';
import { GroupUserState } from '../../models/enums/group-user-state.type';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-group-card',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './group-card.component.html',
  styleUrls: ['./group-card.component.css'],
})
export class GroupCardComponent {
  @Input() group!: BaseGroupResponse;

  @Output() join = new EventEmitter<string>();

  readonly states = {
    owner: 'OWNER' as GroupUserState,
    moderator: 'MODERATOR' as GroupUserState,
    member: 'MEMBER' as GroupUserState,
    pending: 'PENDING' as GroupUserState,
  };

  constructor(private router: Router) {}

  onJoin() {
    this.join.emit(this.group.id);
  }

  onView() {
    this.router.navigate([AppRoutes.groups, this.group.id]);
  }

  canView(): boolean {
    return (
      this.group.groupUserState === this.states.owner ||
      this.group.groupUserState === this.states.moderator ||
      this.group.groupUserState === this.states.member
    );
  }

  canJoin(): boolean {
    return this.group.groupUserState === null;
  }

  isPending(): boolean {
    return this.group.groupUserState === this.states.pending;
  }
}
