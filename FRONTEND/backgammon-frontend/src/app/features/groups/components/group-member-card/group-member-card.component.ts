import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { UserBaseResponse } from '../../../user/models/api/responses/user-base.response';

@Component({
  selector: 'app-group-member-card',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './group-member-card.component.html',
  styleUrls: ['./group-member-card.component.css'],
})
export class GroupMemberCardComponent {
  @Input({ required: true })
  member!: UserBaseResponse;

  @Input()
  isOwner = false;

  @Input()
  canManage = false;

  @Output()
  promote = new EventEmitter<string>();

  @Output()
  demote = new EventEmitter<string>();

  @Output()
  remove = new EventEmitter<string>();

  onPromote(): void {
    this.promote.emit(this.member.id);
  }

  onDemote(): void {
    this.demote.emit(this.member.id);
  }

  onRemove(): void {
    this.remove.emit(this.member.id);
  }

  getRole(): string {
    return (this.member.groupRoleName ?? 'Member').toUpperCase();
  }

  showActions(): boolean {
    return this.canShowRemove() || this.canShowPromoteDemote();
  }

  canShowPromoteDemote(): boolean {
    if (!this.isOwner) return false;

    return this.getRole() === 'Member' || this.getRole() === 'Moderator';
  }

  canShowRemove(): boolean {
    if (!this.canManage) return false;

    const role = this.getRole();

    // owner soha nem törölhető
    if (role === 'OWNER') return false;

    // moderator csak MEMBER-t törölhet
    if (!this.isOwner) {
      return role === 'MEMBER';
    }

    // owner minden nem-owner-t törölhet
    return role === 'MEMBER' || role === 'MODERATOR';
  }
}
