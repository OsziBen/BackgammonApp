import { MoveSequence } from '../../models/turn/move-sequence.model';

export interface MoveSequenceRule {
  apply(sequences: MoveSequence[], dice: number[]): MoveSequence[];
}
