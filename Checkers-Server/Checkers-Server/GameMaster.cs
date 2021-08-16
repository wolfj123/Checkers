using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Checkers_Server
{
    class GameMaster
    {
        //IPlayer currentPlayer;
        //Stack<Move> moves;
        List<IPlayer> players;
        Board board;
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

        ///<exception cref="ArgumentException">This exception is thrown if the ruleset name is not a valid name</exception>
        public void Reset(List<IPlayer> players, int boardSize, string rulesetName)
        {
            board = new Board(boardSize);
            winners = new List<IPlayer>();
            ruleset = RuleSetFactory.CreateRuleSet(rulesetName);
            if(ruleset == null)
            {
                throw new ArgumentException(String.Format("Ruleset name is not valid: {0}", rulesetName));
            }
            this.players = players;
            //TODO: randomze players 
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
