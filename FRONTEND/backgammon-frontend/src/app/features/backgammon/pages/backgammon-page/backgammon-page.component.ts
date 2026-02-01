import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { CreateGameSessionComponent } from '../../../game-session/pages/create-game-session/create-game-session.component';

@Component({
  selector: 'app-backgammon-page',
  standalone: true,
  imports: [CommonModule, CreateGameSessionComponent],
  templateUrl: './backgammon-page.component.html',
})
export class BackgammonPageComponent {}
