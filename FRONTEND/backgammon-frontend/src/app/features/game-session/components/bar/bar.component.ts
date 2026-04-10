import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { PlayerColor } from '../../models/enums/player-color.enum';
import { PointComponent } from '../point/point.component';
import { UiPoint } from '../../models/board/ui-point.model';

@Component({
  selector: 'app-bar',
  standalone: true,
  imports: [CommonModule, PointComponent],
  templateUrl: './bar.component.html',
  styleUrls: ['./bar.component.css'],
})
export class BarComponent {
  @Input() barWhite = 0;
  @Input() barBlack = 0;

  @Input() clickablePoints: number[] = [];
  @Input() targetPoints: number[] = [];
  @Input() selectedPoint: number | null = null;

  @Input() enabled = true;

  @Output() pointClick = new EventEmitter<number>();

  PlayerColor = PlayerColor;

  get blackPoint(): UiPoint {
    return {
      index: 0,
      color: PlayerColor.Black,
      count: this.barBlack,
    };
  }

  get whitePoint(): UiPoint {
    return {
      index: 0,
      color: PlayerColor.White,
      count: this.barWhite,
    };
  }

  isClickable(): boolean {
    return this.enabled && this.clickablePoints.includes(0);
  }

  isTarget(): boolean {
    return (
      this.enabled && this.selectedPoint === 0 && this.targetPoints.length > 0
    );
  }

  isSelected(): boolean {
    return this.selectedPoint === 0;
  }

  onPointClick(index: number) {
    this.pointClick.emit(index);
  }
}
