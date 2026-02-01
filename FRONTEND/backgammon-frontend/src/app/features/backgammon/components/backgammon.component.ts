import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-backgammon-realtime',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './backgammon.component.html',
})
export class BackgammonComponent {
  message =
    'Realtime connection not implemented yet (SignalR will come later).';
}
