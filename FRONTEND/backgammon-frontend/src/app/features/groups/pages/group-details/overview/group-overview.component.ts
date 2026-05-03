import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-group-overview',
  standalone: true,
  templateUrl: './group-overview.component.html',
  styleUrls: ['./group-overview.component.css'],
})
export class GroupOverviewComponent {
  role: 'Member' | 'Moderator' | 'Owner' = 'Owner'; // dummy
}
