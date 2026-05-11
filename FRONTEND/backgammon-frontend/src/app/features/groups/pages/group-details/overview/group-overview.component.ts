import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BaseGroupResponse } from '../../../models/api/responses/base-group.response';

@Component({
  selector: 'app-group-overview',
  standalone: true,
  templateUrl: './group-overview.component.html',
})
export class GroupOverviewComponent {
  group: BaseGroupResponse;

  constructor(private route: ActivatedRoute) {
    this.group = this.route.parent!.snapshot.data['group'];
  }

  isOwner(): boolean {
    return this.group.groupUserState === 'OWNER';
  }
}
