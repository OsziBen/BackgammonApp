import { Component } from '@angular/core';

@Component({
  selector: 'app-group-members',
  standalone: true,
  templateUrl: './group-members.component.html',
  styleUrls: ['./group-members.component.css'],
})
export class GroupMembersComponent {
  role: 'Member' | 'Moderator' | 'Owner' = 'Owner';
}
