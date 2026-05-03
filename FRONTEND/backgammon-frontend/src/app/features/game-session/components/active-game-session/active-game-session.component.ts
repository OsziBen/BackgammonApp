import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { GetActiveSessionResponse } from '../../models/api/responses/get-active-session.response';

@Component({
  selector: 'app-active-game-session',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './active-game-session.component.html',
  styleUrls: ['./active-game-session.component.css'],
})
export class ActiveGameSessionComponent {
  @Input({ required: true })
  session!: GetActiveSessionResponse;

  @Output()
  deleteSession = new EventEmitter<string>();

  @Output() joinSession = new EventEmitter<string>();

  join() {
    this.joinSession.emit(this.session.sessionCode);
  }

  onDelete(): void {
    console.log('JOIN CLICKED:', this.session.sessionCode);
    this.deleteSession.emit(this.session.sessionId);
  }
}
