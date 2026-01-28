import { Component, OnInit } from '@angular/core';
import { AppApiService } from '../../shared/services/app-api.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  templateUrl: './dashboard.component.html',
})
export class DashboardComponent implements OnInit {
  status = 'Connecting...';

  constructor(
    private api: AppApiService,
    private toastr: ToastrService,
  ) {}

  ngOnInit(): void {
    this.api.ping().subscribe({
      next: () => {
        this.status = 'Backend reachable ✅';
        this.toastr.success('Backend reachable', 'Connection OK');
      },
      error: () => {
        this.status = 'Backend not reachable ❌';
        this.toastr.error('Backend not reachable', 'Connection error');
      },
    });
  }
}
