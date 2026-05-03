import { Component } from '@angular/core';

@Component({
  selector: 'app-group-requests',
  standalone: true,
  templateUrl: './group-requests.component.html',
  styleUrls: ['./group-requests.component.css'],
})
export class GroupRequestsComponent {
  role: 'Member' | 'Moderator' | 'Owner' = 'Moderator';
}
