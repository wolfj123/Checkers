using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Checkers_Server
{
    public class ClassicCheckersRuleSet : IRuleSet
    {
        //string name = "Classic Checkers";
        //string description = "TODO";

        public List<Move> GetAllMovesForPlayer(IPlayer player, Board board)
        {
            var result = new List<Move>();
            Color? color = player.GetColor();
            if (color == null) {
                return result;
            }
            List<(Cell, Pawn)> cellsAndPawns = board.GetAllCellsAndPawns();
            cellsAndPawns.ForEach(cp => {
                var cell = cp.Item1;
                var pawn = cp.Item2;
                var moves = GetAllMovesForPawn(board, cell, pawn);
                result.AddRange(moves);
            });
            return result;
        }

        public List<Move> GetAllMovesForPawn(Board board, Cell cell, Pawn pawn)
        {
            var playerColor = pawn.color;
            var ydirection = playerColor == Color.WHITE ? DirectionY.UP : DirectionY.DOWN;
            var nextLeftCell = board.GetCell(cell.x + (int) DirectionX.LEFT, cell.y + (int)ydirection);
            var nextRightCell = board.GetCell(cell.x + (int)DirectionX.LEFT, cell.y + (int)ydirection);

            List<Move> result = new List<Move>();



            if(nextLeftCell != null)
            {
                if(nextLeftCell.isEmpty()) //left cell is clear
                {
                    result.Add(Move.AdvanceMove(board, cell, pawn, DirectionX.LEFT, ydirection, 1));
                }
                else if(nextLeftCell.GetPawn().color != playerColor) //left cell has enemy
                {
                    List<Pawn> removedPawns = new List<Pawn>();
                }
            }




            if(nextRightCell != null)
            {
                if (nextRightCell.isEmpty()) //right cell is clear
                {
                    result.Add(Move.AdvanceMove(board, cell, pawn, DirectionX.RIGHT, ydirection, 1));
                }
                else if (nextRightCell.GetPawn().color != playerColor) //
                {

                }
            }





            return result;
        }

        //TODO: queen
        private List<Move> EatingSequence(Board board, Cell cell, Pawn pawn, List<Pawn> removedPawns)
        {
            (DirectionX x, DirectionY y)[] directions = Board.GetAllDirections();
            if (removedPawns.IsEmpty()) //first eat move, which is restricted to only forward direction
            {
                var forward = GameMaster.GetDirectionByColor(pawn.color);
                directions = directions.Filter(d => d.y == forward).ToArray();
            }

            var result = new List<Move>();

            foreach ((DirectionX x, DirectionY y) direction in directions)
            {
                var nextCell = board.GetCell(cell.x + (int)direction.x, cell.y + (int)direction.y);
                if(nextCell == null) { continue; };
                var nextNextCell = board.GetCell(nextCell.x + (int)direction.x, nextCell.y + (int)direction.y);
                if (nextNextCell == null) { continue; };
                var adjacentPawn = nextCell.GetPawn();
                var nextAdjacentPawn = nextNextCell.GetPawn();
                if(adjacentPawn != null && adjacentPawn.color != pawn.color && !removedPawns.Contains(adjacentPawn)
                    && (nextAdjacentPawn == null || removedPawns.Contains(nextAdjacentPawn)))
                {
                    var eatMove = Move.EatMove(board, cell, pawn, direction.x, direction.y);
                    List<Pawn> removedPawnsCopy = removedPawns.ToList().AddFluent(adjacentPawn);


                    result.Add(eatMove);
                }
            }
            
        }
        


        public List<IPlayer> GetWinners(List<IPlayer> players, Board board)
        {
            //TODO: win conditions. take into account no more moves available (stalemate ?)
            throw new NotImplementedException();
        }


    }
}
