import { computed, Injectable } from '@angular/core';
import { BoardStateDto } from '../models/board/board-state.model';
import { UiBoardState } from '../models/board/ui-board-state.model';
import { mapBoardState } from '../../../shared/utils/board-state.mapper';
import { GameSessionStore } from '../state/game-session.store';

@Injectable({
  providedIn: 'root',
})
export class BoardSelectorsService {
  constructor(private readonly sessionStore: GameSessionStore) {}

  // RAW DTO
  readonly boardDto = computed<BoardStateDto | null>(() => {
    const snapshot = this.sessionStore.snapshot();

    if (!snapshot?.boardStateJson) {
      return null;
    }

    try {
      return JSON.parse(snapshot.boardStateJson) as BoardStateDto;
    } catch {
      console.error('Invalid board JSON');
      return null;
    }
  });

  // UI MODEL
  readonly board = computed<UiBoardState | null>(() => {
    const dto = this.boardDto();

    return dto ? mapBoardState(dto) : null;
  });

  // SELECTORS
  readonly points = computed(() => this.board()?.points ?? []);

  readonly bar = computed(() => this.board()?.bar ?? { 0: 0, 1: 0 });

  readonly off = computed(() => this.board()?.off ?? { 0: 0, 1: 0 });

  readonly currentPlayer = computed(() => this.board()?.currentPlayer ?? null);
}
