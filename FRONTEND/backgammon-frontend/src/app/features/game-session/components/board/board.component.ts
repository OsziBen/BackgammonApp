import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { PointComponent } from '../point/point.component';
import { UiBoardState } from '../../models/board/ui-board-state.model';
import { PlayerColor } from '../../models/enums/player-color.enum';
import * as BoardConsts from '../../../../shared/utils/constants/board.constants';
import { BarComponent } from '../bar/bar.component';

@Component({
  selector: 'app-board',
  standalone: true,
  imports: [CommonModule, PointComponent, BarComponent],
  templateUrl: './board.component.html',
  styleUrls: ['./board.component.css'],
})
export class BoardComponent {
  @Input({ required: true }) board!: UiBoardState;
  @Input() enabled = true;
  @Input() currentPlayer = 0;

  @Input() clickablePoints: number[] = [];
  @Input() targetPoints: number[] = [];
  @Input() selectedPoint: number | null = null;

  @Output() pointClick = new EventEmitter<number>();
  @Output() barClick = new EventEmitter<PlayerColor>();

  public PlayerColor = PlayerColor;
  public OFF = BoardConsts.OFF;

  constructor() {}

  // 4 negyed
  topLeft() {
    return this.board.points.slice(12, 18);
  }
  topRight() {
    return this.board.points.slice(18, 24);
  }
  bottomLeft() {
    return [...this.board.points.slice(6, 12)].reverse();
  }
  bottomRight() {
    return [...this.board.points.slice(0, 6)].reverse();
  }

  isClickable(index: number) {
    if (!this.enabled) return false;
    return this.clickablePoints.includes(index);
  }

  isTarget(index: number) {
    if (!this.enabled || this.selectedPoint === null) return false;
    return this.targetPoints.includes(index);
  }

  isSelected(index: number) {
    return this.selectedPoint === index;
  }

  hasSelection() {
    return this.selectedPoint !== null;
  }

  getCheckers(count: number) {
    const maxVisible = 5;
    if (count <= maxVisible) {
      return Array(count).fill({ isCount: false });
    } else {
      const arr = Array(maxVisible).fill({ isCount: false });
      arr[maxVisible - 1] = { isCount: true, count };
      return arr;
    }
  }

  getOffCheckers(player: PlayerColor) {
    const count = this.board.off[player] || 0;
    const maxVisible = 5;
    if (count <= maxVisible) {
      return Array(count).fill({ count: 1 });
    } else {
      const arr = Array(maxVisible).fill({ count: 1 });
      arr[maxVisible - 1] = { count };
      return arr;
    }
  }
}
