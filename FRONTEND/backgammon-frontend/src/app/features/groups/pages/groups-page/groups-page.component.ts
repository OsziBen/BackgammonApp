import { Component } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { AppRoutes } from '../../../../shared/constants/app-routes.constants';

@Component({
  selector: 'app-groups-page',
  standalone: true,
  imports: [RouterOutlet, RouterLink, RouterLinkActive],
  templateUrl: './groups-page.component.html',
  styleUrls: ['./groups-page.component.css'],
})
export class GroupsPageComponent {
  routes = AppRoutes;
}
