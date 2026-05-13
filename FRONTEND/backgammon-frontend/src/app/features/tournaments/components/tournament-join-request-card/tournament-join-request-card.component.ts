import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';

import { TournamentJoinRequestResponse } from '../../models/api/responses/tournament-join-request.response';

@Component({
  selector: 'app-tournament-join-request-card',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './tournament-join-request-card.component.html',
  styleUrls: ['./tournament-join-request-card.component.css'],
})
export class TournamentJoinRequestCardComponent {
  @Input({ required: true })
  request!: TournamentJoinRequestResponse;

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
