import { Component, OnInit } from '@angular/core';
import { AppApiService } from '../../../../../core/services/app-api.service';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { AppRoutes } from '../../../../../app.routes';

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
