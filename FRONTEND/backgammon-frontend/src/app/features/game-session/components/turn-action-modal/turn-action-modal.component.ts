import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-turn-action-modal',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './turn-action-modal.component.html',
  styleUrls: ['./turn-action-modal.component.css'],
})
export class TurnActionModalComponent {
  @Input() title: string = 'Choose action';

  @Input() canRoll: boolean = false;
  @Input() canDouble: boolean = false;
  @Input() canRespond: boolean = false;

  @Output() roll = new EventEmitter<void>();
  @Output() offerDouble = new EventEmitter<void>();
  @Output() accept = new EventEmitter<void>();
  @Output() decline = new EventEmitter<void>();

  onRoll() {
    if (!this.canRoll) {
      return;
    }

    this.roll.emit();
  }

  onOfferDouble() {
    if (!this.canDouble) {
      return;
    }

    this.offerDouble.emit();
  }

  onAccept() {
    if (!this.canRespond) {
      return;
    }

    this.accept.emit();
  }

  onDecline() {
    if (!this.canRespond) {
      return;
    }

    this.decline.emit();
  }
}
