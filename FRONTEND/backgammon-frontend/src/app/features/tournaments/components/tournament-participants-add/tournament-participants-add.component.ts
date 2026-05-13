import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-tournament-participants-add',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './tournament-participants-add.component.html',
  styleUrls: ['./tournament-participants-add.component.css'],
})
export class TournamentParticipantsAddComponent {
  @Input() canAdd = false;
  @Output() add = new EventEmitter<string>();

  username = '';

  onAdd(): void {
    if (!this.username.trim()) return;

    this.add.emit(this.username);
    this.username = '';
  }
}
