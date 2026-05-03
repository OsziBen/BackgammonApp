import { Component, signal } from '@angular/core';
import { RouterOutlet, RouterLink, RouterLinkActive } from '@angular/router';

type GroupRole = 'Member' | 'Moderator' | 'Owner';

@Component({
  selector: 'app-group-details',
  standalone: true,
  imports: [RouterOutlet, RouterLink, RouterLinkActive],
  templateUrl: './group-details.component.html',
  styleUrls: ['./group-details.component.css'],
})
export class GroupDetailsComponent {
  // DUMMY ROLE
  readonly role = signal<GroupRole>('Owner');

  // helper
  isOwner = () => this.role() === 'Owner';
  isModeratorOrOwner = () =>
    this.role() === 'Owner' || this.role() === 'Moderator';
}
