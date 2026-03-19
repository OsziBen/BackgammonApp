import { MoveSequence } from '../../models/turn/move-sequence.model';
import { MoveSequenceRule } from './move-sequence-rule.model';

export class MustUseMaxDiceRule implements MoveSequenceRule {
  apply(sequences: MoveSequence[], _dice: number[]): MoveSequence[] {
    if (sequences.length === 0) {
      return sequences;
    }

    const max = Math.max(...sequences.map((s) => s.moves.length));

    return sequences.filter((s) => s.moves.length === max);
  }
}
