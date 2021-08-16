﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Checkers_Server
{
    public interface IPlayer
    {
        Color GetColor();

        void NotifyTurn();

        void NotifyWinners(List<IPlayer> winners);
    }
}
