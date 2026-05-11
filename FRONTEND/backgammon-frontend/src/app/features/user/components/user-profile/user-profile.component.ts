import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserProfileResponse } from '../../models/api/responses/user-profile.response';

@Component({
  selector: 'app-user-profile',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.css'],
})
export class UserProfileComponent {
  @Input() user?: UserProfileResponse;
}
