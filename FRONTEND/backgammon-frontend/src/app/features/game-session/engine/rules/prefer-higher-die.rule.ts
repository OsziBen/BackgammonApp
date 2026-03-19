import { MoveSequence } from '../../models/turn/move-sequence.model';
import { MoveSequenceRule } from './move-sequence-rule.model';

export class PreferHigherDieRule implements MoveSequenceRule {
  apply(sequences: MoveSequence[], dice: number[]): MoveSequence[] {
    if (sequences.length === 0) {
      return sequences;
    }

    const maxLength = Math.max(...sequences.map((s) => s.moves.length));

    if (maxLength !== 1) {
      return sequences;
    }

    const highestDice = Math.max(...dice);

    return sequences.filter((seq) =>
      seq.moves.some((m) => m.die === highestDice),
    );
  }
}
