using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Checkers_Server
{
    class GameMaster
    {
        List<IPlayer> players;
        Board board;
        IRuleSet ruleset;
        List<IPlayer> winners;
        private static GameMaster gamemaster;

        private GameMaster(List<IPlayer> players, int boardSize, string rules)
        {
            Reset(players, boardSize, rules);
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
            RandomizePlayers();
        }

        /// <summary>
        /// Applies the following side effects:
        /// <para>
        ///     1.  reorganizes the players list in the GameMaster
        /// </para>
        ///     2.  assignes a color to each player
        /// </summary>
        /// <param name="players"></param>
        private void RandomizePlayers()
        {
            if(players.Count != 2)
            {
                throw new ArgumentException("Game supports only 2 players");
            }
            //List<IPlayer> result = new List<IPlayer>();
            int numOfWhite = players.Reduce(0, (sum, player) => player.GetColor() == Color.WHITE ? sum + 1 : sum);
            int numOfBlack = players.Reduce(0, (sum, player) => player.GetColor() == Color.BLACK ? sum + 1 : sum);

            (Color colorToKeep, Color colorToSet) colors =
                numOfWhite == 1 ? (Color.WHITE, Color.BLACK) :
                numOfBlack == 1 ? (Color.BLACK, Color.WHITE) :
                default;

            if (colors == default)
            {
                var rand = new Random();
                players = players.OrderBy(x => rand.Next()).ToList();
            }
            else
            {
                players.ForEach(
                    (player) =>
                    {
                        if (player.GetColor() != colors.colorToKeep)
                        {
                            player.SetColor(colors.colorToSet);
                        }
                    });
                players.Sort((player1, player2) => ((int)player1.GetColor()).CompareTo((int)player2.GetColor()));
            }

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
