import { computed, Injectable } from '@angular/core';
import { TurnStateService } from '../state/turn-state.service';
import { UiBoardState } from '../models/board/ui-board-state.model';
import { BoardSelectorsService } from '../selectors/board-selectors.service';

@Injectable({
  providedIn: 'root',
})
export class BoardStateFacadeService {
  constructor(
    private snapshotBoard: BoardSelectorsService,
    private turnState: TurnStateService,
  ) {}

  readonly board = computed<UiBoardState | null>(() => {
    return this.turnState.board() ?? this.snapshotBoard.board();
  });
}
