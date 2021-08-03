using System;
using System.Collections.Generic;
using System.Text;

namespace Checkers_Server
{
    public class ClassicCheckersRuleSet : IRuleSet
    {
        public List<Move> GetAllMoves(IPlayer player, Board board)
        {
            var result = new List<Move>();
            Color color = player.GetColor();
            List<(Cell, Pawn)> cellsAndPawns = board.GetAllCellsAndPawns();
            cellsAndPawns.ForEach(cp => {
                var cell = cp.Item1;
                var pawn = cp.Item2;
                var moves = GetAllMovesForPawn(player, board, pawn);
                result.AddRange(moves);
            });
            return result;
        }

        public List<Move> GetAllMovesForPawn(IPlayer player, Board board, Pawn pawn)
        {
            throw new NotImplementedException();
        }

        public string GetGuide()
        {
            //TODO: guide for classic rules
            return "TODO:";
        }

        public IPlayer Winner(List<IPlayer> players, Board board)
        {
            //TODO: win conditions. take into account no more moves available (stalemate ?)
            throw new NotImplementedException();
        }


        public static Move AdvanceLeft(Board board, Pawn pawn)
        {
            //TODO:
            return null;
        }

        public static Move AdvanceRight(Board board, Pawn pawn)
        {
            //TODO:
            return null;
        }
    }
}
