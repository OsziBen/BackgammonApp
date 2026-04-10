import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { PlayerColor } from '../../models/enums/player-color.enum';

@Component({
  selector: 'app-doubling-cube',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './doubling-cube.component.html',
  styleUrls: ['./doubling-cube.component.css'],
})
export class DoublingCubeComponent {
  @Input() value: number = 2;
  @Input() owner: PlayerColor | null = null;
  @Input() enabled: boolean = true;

  readonly PlayerColor = PlayerColor;
}
