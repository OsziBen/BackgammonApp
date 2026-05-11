import { Component } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { AppRoutes } from '../../../../../shared/constants/app-routes.constants';

@Component({
  selector: 'app-dashboard-home',
  standalone: true,
  templateUrl: './dashboard-home.component.html',
  styleUrls: ['./dashboard-home.component.css'],
  imports: [RouterLink, RouterLinkActive],
})
export class DashboardHomeComponent {
  AppRoutes = AppRoutes;
  status = 'Connecting...';

  constructor() {}
}
