import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';

import { GroupJoinRequestResponse } from '../../models/api/responses/group-join-request.response';

@Component({
  selector: 'app-group-join-request-card',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './group-join-request-card.component.html',
  styleUrls: ['./group-join-request-card.component.css'],
})
export class GroupJoinRequestCardComponent {
  @Input({ required: true })
  request!: GroupJoinRequestResponse;

  @Input()
  canManage = false;

  @Output()
  approve = new EventEmitter<string>();

  @Output()
  reject = new EventEmitter<string>();

  onApprove(): void {
    this.approve.emit(this.request.id);
  }

  onReject(): void {
    this.reject.emit(this.request.id);
  }

  isPending(): boolean {
    return this.request.status === 'Pending';
  }
}
