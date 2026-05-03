import { Component, EventEmitter, Input, Output } from '@angular/core';
import { BaseGroupResponse } from '../../models/api/responses/base-group.response';
import { Router } from '@angular/router';
import { AppRoutes } from '../../../../app.routes';

@Component({
  selector: 'app-group-card',
  standalone: true,
  templateUrl: './group-card.component.html',
  styleUrls: ['./group-card.component.css'],
})
export class GroupCardComponent {
  @Input() group!: BaseGroupResponse;

  @Output() join = new EventEmitter<string>();

  constructor(private router: Router) {}

  onJoin() {
    this.join.emit(this.group.id);
  }

  onView() {
    this.router.navigate([AppRoutes.groups, this.group.id]);
  }
}
