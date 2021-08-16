using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Checkers_Server
{
    class GameMaster
    {
        //IPlayer currentPlayer;
        List<IPlayer> players;
        Board board;
        Stack<Move> moves;
        IRuleSet ruleset;
        List<IPlayer> winners;
        private static GameMaster gamemaster;

        private GameMaster(List<IPlayer> players, int boardSize, string rules)
        {
            reset(players, boardSize, rules);
        }

        public static GameMaster GetGameMaster()
        {
            return gamemaster;
        }

        public static GameMaster GetGameMaster(List<IPlayer> players, int boardSize, string rules)
        {
            if(gamemaster == null)
            {
                gamemaster = new GameMaster(players, boardSize, rules);
            }
            return GetGameMaster();
        }

        public void reset(List<IPlayer> players, int boardSize, string rules)
        {
            board = new Board(boardSize);
            this.players = players;
            //TODO: get createrulset
        }


        public void notifyTurn()
        {
            players[0].NotifyTurn();
        }

        public List<IPlayer> GetWinner()
        {
            return winners;
        }

        public (Board, List<Move>) GetBoardAndMoves(IPlayer player)
        {
            var moves = ruleset.GetAllMoves(player, board);
            return (board, moves);
        }

        List<Move> MakeMove(Move move)
        {
            move.DoMove();
            return move.GetChainedMove();
        }

        public void endTurn()
        {
            winners = ruleset.GetWinners(players.ToList(), board);
            if (winners != null)
            {
                players.ForEach(delegate (IPlayer player)
                {
                    player.NotifyWinners(winners);
                });
            }

            var currplayer = players[0];
            players.RemoveAt(0);
            players.Add(currplayer);
        }
    }
}
