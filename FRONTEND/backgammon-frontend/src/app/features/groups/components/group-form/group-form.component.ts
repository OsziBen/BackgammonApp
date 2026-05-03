import { Component, EventEmitter, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CreateGroupRequest } from '../../models/api/requests/create-group.request';
import { GroupVisibility } from '../../models/enums/group-visibility.type';
import { GroupSizePreset } from '../../models/enums/group-size-preset.type';

@Component({
  selector: 'app-group-form',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './group-form.component.html',
  styleUrls: ['./group-form.component.css'],
})
export class GroupFormComponent {
  @Output() submitted = new EventEmitter<CreateGroupRequest>();

  visibilityOptions: GroupVisibility[] = ['Public', 'Private'];

  sizeOptions: GroupSizePreset[] = ['Small', 'Medium', 'Large'];

  model: CreateGroupRequest = {
    name: '',
    description: '',
    visibility: 'Public',
    sizePreset: 'Small',
  };

  submit() {
    this.submitted.emit(this.model);
  }
}
