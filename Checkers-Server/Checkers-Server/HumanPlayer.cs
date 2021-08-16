using System;
using System.Collections.Generic;
using System.Text;

namespace Checkers_Server
{
    class HumanPlayer : IPlayer
    {
        Color? color = null;

        public Color? GetColor()
        {
            //TODO: HumanPlayer.GetColor
            throw new NotImplementedException();
        }

        public void NotifyTurn()
        {
            //TODO: HumanPlayer.NotifyTurn
            throw new NotImplementedException();
        }

        public void NotifyWinners(List<IPlayer> winners)
        {
            //TODO: HumanPlayer.NotifyWinners
            throw new NotImplementedException();
        }
    }
}
