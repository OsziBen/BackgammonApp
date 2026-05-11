import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { UserTournamentJoinRequestResponse } from '../../models/api/responses/user-tournament-join-request.response';

@Component({
  selector: 'app-user-tournament-join-request-card',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './user-tournament-join-request-card.component.html',
  styleUrls: ['./user-tournament-join-request-card.component.css'],
})
export class UserTournamentJoinRequestCardComponent {
  @Input({ required: true })
  request!: UserTournamentJoinRequestResponse;
}
