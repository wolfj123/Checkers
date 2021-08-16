using System;
using System.Collections.Generic;
using System.Text;

namespace Checkers_Server
{
    public interface IRuleSet
    {
        //string GetName();
        //string GetDescription();

        ///<returns>Returns Null in case of no winner</returns>
        List<IPlayer> GetWinners(List<IPlayer> players, Board board);

        List<Move> GetAllMoves(IPlayer player, Board board);

        List<Move> GetAllMovesForPawn(IPlayer player, Board board, Pawn pawn);
    }
}
