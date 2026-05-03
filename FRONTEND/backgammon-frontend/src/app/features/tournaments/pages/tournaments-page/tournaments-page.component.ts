import { Component } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { AppRoutes } from '../../../../app.routes';

@Component({
  selector: 'app-tournaments-page',
  standalone: true,
  imports: [RouterOutlet, RouterLink, RouterLinkActive],
  templateUrl: './tournaments-page.component.html',
  styleUrls: ['./tournaments-page.component.css'],
})
export class TournamentsPageComponent {
  routes = AppRoutes;
}
