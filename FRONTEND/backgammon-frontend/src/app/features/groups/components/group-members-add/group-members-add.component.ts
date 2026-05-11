import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';

import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-group-members-add',
  standalone: true,

  imports: [CommonModule, FormsModule],

  templateUrl: './group-members-add.component.html',
  styleUrls: ['./group-members-add.component.css'],
})
export class GroupMembersAddComponent {
  @Input() canAdd = false;

  @Output() add = new EventEmitter<string>();

  username = '';

  onAdd(): void {
    if (!this.username.trim()) return;

    this.add.emit(this.username);

    this.username = '';
  }
}
