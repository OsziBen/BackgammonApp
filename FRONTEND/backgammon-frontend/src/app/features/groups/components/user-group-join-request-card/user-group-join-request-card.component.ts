import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';

import { UserGroupJoinRequestResponse } from '../../models/api/responses/user-group-join-request.response';

@Component({
  selector: 'app-user-group-join-request-card',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './user-group-join-request-card.component.html',
  styleUrls: ['./user-group-join-request-card.component.css'],
})
export class UserGroupJoinRequestCardComponent {
  @Input({ required: true })
  request!: UserGroupJoinRequestResponse;
}
