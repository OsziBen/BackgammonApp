export interface GameBoard {
  points: number[][];
  bar: {
    white: number;
    black: number;
  };
  borneOff: {
    white: number;
    black: number;
  };
}
