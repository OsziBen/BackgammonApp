import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';

import { TournamentParticipantBaseResponse } from '../../models/api/responses/tournament-participant-base.response';

@Component({
  selector: 'app-tournament-participant-card',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './tournament-participant-card.component.html',
  styleUrls: ['./tournament-participant-card.component.css'],
})
export class TournamentParticipantCardComponent {
  @Input({ required: true })
  participant!: TournamentParticipantBaseResponse;

  @Input()
  canManage = false;

  @Output()
  remove = new EventEmitter<string>();

  onRemove(): void {
    this.remove.emit(this.participant.userId);
  }
}
