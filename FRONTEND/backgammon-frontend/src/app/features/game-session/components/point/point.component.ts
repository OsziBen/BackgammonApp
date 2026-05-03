import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { UiPoint } from '../../models/board/ui-point.model';

@Component({
  selector: 'app-point',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './point.component.html',
  styleUrls: ['./point.component.css'],
})
export class PointComponent {
  @Input({ required: true }) point!: UiPoint;
  @Input() top = false;
  @Input() clickable = false;
  @Input() target = false;
  @Input() selected = false;
  @Input() index!: number;

  @Output() pointClick = new EventEmitter<number>();

  onClick() {
    this.pointClick.emit(this.point.index);
  }

  getCheckers(count: number, isTop: boolean) {
    const maxVisible = 5;

    if (count <= maxVisible) {
      return Array(count).fill({ isCount: false });
    }

    const arr = Array(maxVisible).fill({ isCount: false });

    if (isTop) {
      arr[maxVisible - 1] = { isCount: true, count };
    } else {
      arr[0] = { isCount: true, count };
    }

    return arr;
  }

  get isEven(): boolean {
    return this.point.index % 2 === 0;
  }

  get checkers() {
    return this.getCheckers(this.point.count, this.top);
  }
}
