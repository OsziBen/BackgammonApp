using Common.Enums.BoardState;
using Domain.GameLogic.Constants;
using Domain.GameLogic.Extensions;
using Domain.GameLogic.Rules;

namespace Domain.GameLogic.Generators
{
    public class MoveSequenceGenerator : IMoveSequenceGenerator
    {
        public IReadOnlyList<MoveSequence> Generate(
            BoardState state,
            DiceRoll roll)
        {
            var sequences = new List<MoveSequence>();

            foreach (var diceOrder in roll.GetPermutations())
            {
                GenerateRecursive(
                    state,
                    diceOrder.ToList(),
                    new List<Move>(),
                    sequences);
            }

            sequences = sequences
                .Distinct()
                .ToList();

            var rules = new List<IMoveSequenceRule>
            {
                new MustUseMaxDiceRule()
            };

            if (!roll.IsDouble)
            {
                rules.Add(new PreferHigherDieRule()); 
            }

            foreach (var rule in rules)
            {
                sequences = rule.Apply(sequences, roll).ToList();
            }

            return sequences;
        }

        private static void GenerateRecursive(
            BoardState state,
            List<int> remainingDice,
            List<Move> currentMoves,
            List<MoveSequence> results)
        {
            if (remainingDice.Count == 0)
            {
                results.Add(
                    new MoveSequence(
                        currentMoves.ToList())
                    );

                return;
            }

            var die = remainingDice[0];
            var usedAnyMove = false;

            foreach (var from in GetMovablePoints(state))
            {
                var to = CalculateTarget(from, die, state);

                if (!IsLegalMove(state, from, to, die))
                {
                    continue;
                }

                usedAnyMove = true;

                var move = new Move(from, to, die);
                var nextState = state.Apply(move);

                currentMoves.Add(move);

                GenerateRecursive(
                    nextState,
                    remainingDice.Skip(1).ToList(),
                    currentMoves,
                    results);

                currentMoves.RemoveAt(currentMoves.Count - 1);
            }

            if (!usedAnyMove)
            {
                results.Add(
                    new MoveSequence(
                        currentMoves.ToList())
                );
            }
        }

        private static IEnumerable<int> GetMovablePoints(BoardState state)
        {
            if (state.HasCheckersOnBar(state.CurrentPlayer))
            {
                yield return BoardConstants.BarPosition;
                yield break;
            }

            foreach (var point in state.Points)
            {
                if (point.Value.Owner == state.CurrentPlayer &&
                    point.Value.Count > 0)
                {
                    yield return point.Key;
                }
            }
        }

        private static int CalculateTarget(
            int from,
            int die,
            BoardState state)
        {
            return state.CurrentPlayer == PlayerColor.White
                ? from + die
                : from - die;
        }

        private static bool IsLegalMove(
            BoardState state,
            int from,
            int to,
            int die)
        {
            if (from == BoardConstants.BarPosition)
            {
                if (!state.HasCheckersOnBar(state.CurrentPlayer))
                {
                    return false;
                }

                return IsLegalBarEntry(state, to);
            }

            if (!state.Points.TryGetValue(from, out var fromPoint))
            {
                return false;
            }

            if (fromPoint.Owner != state.CurrentPlayer ||
                fromPoint.Count == 0)
            {
                return false;
            }

            if (state.HasCheckersOnBar(state.CurrentPlayer))
                return false;

            // BEAR OFF
            if (to == BoardConstants.OffBoardPosition)
            {
                return BearOffRules.CanBearOff(state, from, die);
            }

            if (!state.Points.TryGetValue(to, out var target))
            {
                return false;
            }

            if (target.Owner != null &&
                target.Owner != state.CurrentPlayer &&
                target.Count >= 2)
            {
                return false;
            }

            return true;
        }

        private static bool IsLegalBarEntry(
            BoardState state,
            int targetPoint)
        {
            if (!state.Points.TryGetValue(targetPoint, out var target))
            {
                return false;
            }

            if (target.Owner != null &&
                target.Owner != state.CurrentPlayer &&
                target.Count >= 2)
            {
                return false;
            }

            return BoardConstants.IsHomeBoard(
                targetPoint, state.CurrentPlayer);
        }
    }
}
