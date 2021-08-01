using System;
using System.Collections.Generic;
using System.Text;

namespace Checkers_Server
{
    class GameMaster
    {
        List<IPlayer> players;
        Board board;
        Stack<Move> moves;
        IRuleSet ruleset;
    }
}
