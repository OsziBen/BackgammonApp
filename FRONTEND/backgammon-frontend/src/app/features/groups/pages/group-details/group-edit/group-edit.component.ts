import { CommonModule } from '@angular/common';
import { Component, OnInit, signal } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { firstValueFrom } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { GroupsApiService } from '../../../services/groups-api.service';
import { EditGroupRequest } from '../../../models/api/requests/edit-group.request';
import { BaseGroupResponse } from '../../../models/api/responses/base-group.response';

type GroupVisibility = 'Public' | 'Private';

type GroupSizePreset = 'Small' | 'Medium' | 'Large';

@Component({
  selector: 'app-group-edit',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './group-edit.component.html',
  styleUrls: ['./group-edit.component.css'],
})
export class GroupEditComponent implements OnInit {
  readonly loading = signal(false);

  group!: BaseGroupResponse;

  form!: FormGroup;

  visibilityOptions: GroupVisibility[] = ['Public', 'Private'];

  sizePresetOptions: GroupSizePreset[] = ['Small', 'Medium', 'Large'];

  constructor(
    private readonly fb: FormBuilder,
    private readonly route: ActivatedRoute,
    private readonly router: Router,
    private readonly api: GroupsApiService,
    private readonly toastr: ToastrService,
  ) {}

  ngOnInit(): void {
    this.group = this.route.parent!.snapshot.data['group'] as BaseGroupResponse;

    this.initForm();
  }

  private initForm(): void {
    this.form = this.fb.group({
      name: [this.group.name, Validators.required],

      description: [this.group.description ?? ''],

      visibility: [
        this.normalizeVisibility(this.group.visibility),
        Validators.required,
      ],

      sizePreset: [
        this.normalizeSizePreset(this.group.sizePreset),
        Validators.required,
      ],
    });
  }

  async onSubmit(): Promise<void> {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.loading.set(true);

    try {
      const request: EditGroupRequest = {
        name: this.form.value.name!,
        description: this.form.value.description ?? '',
        visibility: this.form.value.visibility!,
        sizePreset: this.form.value.sizePreset!,
      };

      await firstValueFrom(this.api.editGroup(this.group.id, request));

      this.toastr.success('Group updated');

      // forced refresh marad
      await this.router.navigateByUrl('/', {
        skipLocationChange: true,
      });

      await this.router.navigate(['/groups', this.group.id, 'overview']);
    } catch (err) {
      console.error(err);

      this.toastr.error('Could not update group');
    } finally {
      this.loading.set(false);
    }
  }

  private normalizeVisibility(visibility: string): GroupVisibility {
    switch (visibility?.toLowerCase()) {
      case 'public':
        return 'Public';

      case 'private':
        return 'Private';

      default:
        return 'Public';
    }
  }

  private normalizeSizePreset(preset: string): GroupSizePreset {
    switch (preset?.toLowerCase()) {
      case 'small':
        return 'Small';

      case 'medium':
        return 'Medium';

      case 'large':
        return 'Large';

      default:
        return 'Medium';
    }
  }
}
