import { Component, OnInit } from '@angular/core';
import { AppApiService } from '../../../../../core/services/app-api.service';

@Component({
  selector: 'app-dashboard-home',
  standalone: true,
  templateUrl: './dashboard-home.component.html',
})
export class DashboardHomeComponent implements OnInit {
  status = 'Connecting...';

  constructor(private api: AppApiService) {}

  ngOnInit(): void {
    this.api.ping().subscribe({
      next: () => (this.status = 'Backend reachable ✅'),
      error: () => (this.status = 'Backend not reachable ❌'),
    });
  }
}
