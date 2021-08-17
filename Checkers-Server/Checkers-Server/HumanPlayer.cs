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
            return color;
        }

        public void SetColor(Color? color)
        {
            this.color = color;
        }

        public void NotifyTurn()
        {
            //TODO: HumanPlayer notify turn
        }

        public void NotifyWinners(List<IPlayer> winners)
        {
            //HumanPlayer notify winners
        }
    }
}
