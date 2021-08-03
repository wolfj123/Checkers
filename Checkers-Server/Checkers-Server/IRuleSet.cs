using System;
using System.Collections.Generic;
using System.Text;

namespace Checkers_Server
{
    public interface IRuleSet
    {
        string GetGuide();

        ///<returns>Returns Null in case of no winner</returns>
        IPlayer Winner(List<IPlayer> players, Board board);

        List<Move> GetAllMoves(IPlayer player, Board board);

        List<Move> GetAllMovesForPawn(IPlayer player, Board board, Pawn pawn);
    }
}
