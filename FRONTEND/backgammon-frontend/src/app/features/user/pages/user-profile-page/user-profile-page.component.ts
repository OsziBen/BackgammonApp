import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserProfileComponent } from '../../components/user-profile/user-profile.component';
import { UserProfileResponse } from '../../models/api/responses/user-profile.response';
import { UsersApiService } from '../../services/users-api.service';

@Component({
  selector: 'app-user-profile-page',
  standalone: true,
  imports: [CommonModule, UserProfileComponent],
  templateUrl: './user-profile-page.component.html',
  styleUrls: ['./user-profile-page.component.css'],
})
export class UserProfilePageComponent implements OnInit {
  user?: UserProfileResponse;

  constructor(private usersApi: UsersApiService) {}

  ngOnInit(): void {
    this.usersApi.getMe().subscribe({
      next: (res) => (this.user = res),
    });
  }
}
