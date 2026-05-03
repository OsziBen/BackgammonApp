import { Component, signal } from '@angular/core';
import { GroupFormComponent } from '../../components/group-form/group-form.component';
import { Router } from '@angular/router';
import { AppRoutes } from '../../../../app.routes';
import { ToastrService } from 'ngx-toastr';
import { firstValueFrom } from 'rxjs';
import { CreateGroupRequest } from '../../models/api/requests/create-group.request';
import { GroupsApiService } from '../../services/groups-api.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-groups-create',
  standalone: true,
  imports: [GroupFormComponent, CommonModule],
  templateUrl: './groups-create.component.html',
  styleUrls: ['./groups-create.component.css'],
})
export class GroupsCreateComponent {
  // STATE (signals)
  readonly loading = signal(false);
  readonly error = signal<string | null>(null);

  constructor(
    private readonly api: GroupsApiService,
    private readonly router: Router,
    private readonly toastr: ToastrService,
  ) {}

  // CREATE GROUP (GameSession mintára)
  async onSubmit(request: CreateGroupRequest): Promise<void> {
    this.loading.set(true);
    this.error.set(null);

    try {
      const response = await firstValueFrom(this.api.createGroup(request));

      this.toastr.success('Group created successfully', 'Success');

      // opcionális: redirect új group oldalra később
      await this.router.navigate([AppRoutes.groups, AppRoutes.groupsMy]);
    } catch (err) {
      console.error(err);

      this.error.set('Could not create group');

      this.toastr.error('Could not create group', 'Error');
    } finally {
      this.loading.set(false);
    }
  }
}
