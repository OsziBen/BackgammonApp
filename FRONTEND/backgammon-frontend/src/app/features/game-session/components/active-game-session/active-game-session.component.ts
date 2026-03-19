import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { GetActiveSessionResponse } from '../../models/api/responses/get-active-session.response';

@Component({
  selector: 'app-active-game-session',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './active-game-session.component.html',
})
export class ActiveGameSessionComponent {
  @Input({ required: true })
  session!: GetActiveSessionResponse;

  @Output()
  deleteSession = new EventEmitter<string>();

  onDelete(): void {
    this.deleteSession.emit(this.session.sessionId);
  }
}
