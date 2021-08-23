using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Checkers_Server
{
    public class ClassicCheckersRuleSet : IRuleSet
    {

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
            var nextRightCell = board.GetCell(cell.x + (int)DirectionX.RIGHT, cell.y + (int)ydirection);

            List<Move> result = EatingSequence(board, cell, pawn, new List<Pawn>());


            if (result.IsEmpty()) //we only add normal movement if the pawn cannot eat - as eating is a must if possible
            {
                result.AddRange(NormalMove(board, cell, pawn));

                //if(nextLeftCell != null && nextLeftCell.isEmpty())
                //{
                //    Move moveLeft = Move.AdvanceMove(board, cell, pawn, DirectionX.LEFT, ydirection, 1);
                //    result.Add(moveLeft);
                //}
                //if (nextRightCell != null && nextRightCell.isEmpty())
                //{
                //    Move moveRight = Move.AdvanceMove(board, cell, pawn, DirectionX.RIGHT, ydirection, 1);
                //    result.Add(moveRight);
                //}
            }
            return result;
        }

        
        private List<Move> NormalMove(Board board, Cell cell, Pawn pawn)
        {
            List<Move> result = new List<Move>();
            List<Cell> MoveCells = new List<Cell>();
            foreach (Cell mc in MoveCells)
            {
                var distance = Cell.Distance(cell, mc);
                var move = Move.AdvanceMove(board,
                                            cell,
                                            pawn,
                                            (DirectionX)(distance.dx / Math.Abs(distance.dx)),
                                            (DirectionY)(distance.dy / Math.Abs(distance.dy)),
                                            distance.dx);
                result.Add(move);
            }
            return result;
        }

        private List<Cell> MoveCells(Board board, Cell cell, Pawn pawn)
        {
            List<Cell> result = new List<Cell>();

            var playerColor = pawn.color;

            if (pawn.type == PawnType.QUEEN)
            {
                var ydirection = playerColor == Color.WHITE ? DirectionY.UP : DirectionY.DOWN;
                result.Add(
                    board.GetCell(cell.x + (int)DirectionX.LEFT, cell.y + (int)ydirection));
                result.Add(
                    board.GetCell(cell.x + (int)DirectionX.RIGHT, cell.y + (int)ydirection));
            }
            else if(pawn.type == PawnType.QUEEN)
            {
                (DirectionX, DirectionY)[] allDirections = Board.GetAllDirections();
                foreach ((DirectionX, DirectionY) dxdy in allDirections)
                {
                    result.AddRange(board.ScanInDirection(cell.x, cell.y, dxdy).emptyCells);
                }
            }

            return result;
        }


        //TODO: finish this... should driections be a list ?
        private List<(Cell eatCell, Cell landingCell)> EatCells(Board board, Cell cell, Pawn pawn, (DirectionX x, DirectionY y)[] directions)
        {
            List<(Cell eatCell, Cell landingCell)> result = new List<(Cell eatCell, Cell landingCell)>();

            foreach ((DirectionX x, DirectionY y) direction in directions)
            {
                Cell eatCell;
                if (pawn.type == PawnType.NORMAL)
                {
                    eatCell = board.GetCell(cell.x + (int)direction.x, cell.y + (int)direction.y);
                }
                else
                {
                    eatCell = board.ScanInDirection(cell.x, cell.y, direction).occupiedCell;
                }
                if (eatCell == null) { continue; };

                var landingCell = board.GetCell(eatCell.x + (int)direction.x, eatCell.y + (int)direction.y);
                if (landingCell == null) { continue; };
                result.Add((eatCell, landingCell));
            }
            return result;
        }

        //TODO: queen - can eat any distance
        private List<Move> EatingSequence(Board board, Cell cell, Pawn pawn, List<Pawn> removedPawns)
        {
            var result = new List<Move>();

            (DirectionX x, DirectionY y)[] directions = Board.GetAllDirections();
            if (removedPawns.IsEmpty() && pawn.type == PawnType.NORMAL) //first eat move, which is restricted to only forward direction (if normal pawn)
            {
                var forward = GameMaster.GetDirectionByColor(pawn.color);
                directions = directions.Filter(d => d.y == forward).ToArray();
            }

            foreach ((DirectionX x, DirectionY y) direction in directions)
            {
                var nextCell = board.GetCell(cell.x + (int)direction.x, cell.y + (int)direction.y);
                if (nextCell == null) { continue; };

                var nextNextCell = board.GetCell(nextCell.x + (int)direction.x, nextCell.y + (int)direction.y);
                if (nextNextCell == null) { continue; };

                var adjacentPawn = nextCell.GetPawn();
                var nextAdjacentPawn = nextNextCell.GetPawn();
                if (adjacentPawn != null && adjacentPawn.color != pawn.color && !removedPawns.Contains(adjacentPawn)
                    && (nextAdjacentPawn == null || removedPawns.Contains(nextAdjacentPawn)))
                {
                    var eatMove = Move.EatMove(board, cell, pawn, direction.x, direction.y);
                    List<Pawn> removedPawnsCopy = removedPawns.ToList().AddFluent(adjacentPawn);
                    var chainedMoves = EatingSequence(board, nextNextCell, pawn, removedPawnsCopy);
                    eatMove.AddChainedMove(chainedMoves);
                    result.Add(eatMove);
                }
            }
            return result;
        }
        

        //maybe add victory type to explain to players ?

        public List<IPlayer> GetWinners(List<IPlayer> players, Board board)
        {
            List<IPlayer> result = new List<IPlayer>();
            var playerStatuses = new List<(IPlayer player, int numOfPawns, int numOfQueens, int numOfMoves)>();
            var allPawns = board.GetAllPawns();

            foreach (Color color in Enum.GetValues(typeof(Color)))
            {
                var player = players.First(p => p.GetColor() == color);
                var pawns = allPawns.Filter(pawn => pawn.color == color);
                var numOfPawns = pawns.Count(p => true);
                var numOfQueens = pawns.Count(p => p.type == PawnType.QUEEN);
                var numOfMoves = GetAllMovesForPlayer(player, board).Count;
                playerStatuses.Add((player, numOfPawns, numOfQueens, numOfMoves));
            }

            if (playerStatuses.Count(ps => ps.numOfPawns == 0) == 1) //one player has no pawns
            {
                var winner = playerStatuses.First(ps => ps.numOfPawns > 0).player;
                result.Add(winner);
            }
            else if (playerStatuses.Count(ps => ps.numOfMoves == 0) == 1) //one player has no moves
            {
                var winner = playerStatuses.First(ps => ps.numOfMoves > 0).player;
                result.Add(winner);
            }
            else if (playerStatuses.Count(ps => ps.numOfMoves == 0) == 2) //both players have no moves
            {
                var maxPawns = playerStatuses.Max(ps => ps.numOfPawns);
                if (playerStatuses.Count(ps => ps.numOfPawns == maxPawns) == 1) //one player has more pawns
                {
                    var winner = playerStatuses.First(ps => ps.numOfPawns == maxPawns).player;
                    result.Add(winner);
                }
                else //both players have the same number of pawns
                {
                    var maxQueens = playerStatuses.Max(ps => ps.numOfQueens);
                    if (playerStatuses.Count(ps => ps.numOfQueens == maxQueens) == 1) //one player has more queens
                    {
                        var winner = playerStatuses.First(ps => ps.numOfQueens == maxQueens).player;
                        result.Add(winner);
                    }
                    else
                    {
                        result.AddRange(players); //stalemate
                    }
                }

            }
            return result;
        }
    }
}
