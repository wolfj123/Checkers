using System;
using System.Collections.Generic;
using System.Text;

namespace Checkers_Server
{
    public interface IRuleSet
    {
        ///<returns>Returns Null in case of no winner</returns>
        List<IPlayer> GetWinners(List<IPlayer> players, Board board);

        List<Move> GetAllMovesForPlayer(IPlayer player, Board board);

        List<Move> GetAllMovesForPawn(Board board, Cell cell, Pawn pawn);
    }
}
