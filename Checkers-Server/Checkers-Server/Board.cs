using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Checkers_Server
{
    public class Board
    {
        static (DirectionX, DirectionY)[] directions = {
            (DirectionX.RIGHT, DirectionY.UP),
            (DirectionX.RIGHT, DirectionY.DOWN),
            (DirectionX.LEFT, DirectionY.UP),
            (DirectionX.LEFT, DirectionY.DOWN) };
        Cell[,] cellsArray;
        List<Cell> cellsList;
        int size;

        public Board(int size)
        {
            if(size <= 6)
            {
                throw new ArgumentException("size of board must be larger than 6");
            }
            if(size % 2 != 0)
            {
                throw new ArgumentException("size of board must be an even number");
            }
            createCellMatrix(size);
            populateMatrixWithPawns();
        }

        private void createCellMatrix(int size)
        {
            this.size = size;
            cellsArray = new Cell[size, size];
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    cellsArray[x, y] = new Cell(x, y);
                    cellsList.Add(cellsArray[x, y]);
                };
            };
        }

        //TODO: maybe move to to ruleset?
        private void populateMatrixWithPawns()
        {
            (int, int) linesToSkip = (size / 2, size / 2 + 1);
            for (int x = 0; x < size; x++)
            {
                //var firstCellHasPawn = (x % 2 != 0);
                for (int y = 0; y < size; y++)
                {
                    if (y == linesToSkip.Item1 || y == linesToSkip.Item2) {continue;}

                    var cellHasPawn = ((x + y) % 2 != 0);
                    var color = (y < linesToSkip.Item1) ? Color.WHITE : Color.BLACK;
                    if (cellHasPawn)
                    {
                        cellsArray[x, y].SetPawn(new Pawn(color, PawnType.NORMAL));
                    }
                };
            };
        }

        public Cell GetCell(int x, int y)
        {
            return cellsArray[x,y];
        }

        public List<Cell> GetAllCells()
        {
            return cellsList;
        }

        public static (DirectionX, DirectionY)[] GetAllDirections()
        {
            return directions;
        }

        public List<Pawn> GetAllPawns()
        {
            List<Pawn> result = cellsList.Map(cell => cell.GetPawn()).ToList();
            result.RemoveAll(pawn => pawn == null);
            return result;
        }

        public List<(Cell, Pawn)> GetAllCellsAndPawns()
        {
            var cells = GetAllCells();
            var result = new List<(Cell, Pawn)>();
            cells.ForEach(delegate (Cell cell)
            {
                result.Add((cell, cell.GetPawn()));
            });
            return result;
        }

        public Pawn ScanInDirection(int x, int y, (DirectionX x, DirectionY y) direction)
        {
            var currCell = GetCell(x + (int)direction.x, y + (int)direction.y);
            while(currCell != null && currCell.isEmpty())
            {
                currCell = GetCell(currCell.x + (int)direction.x, currCell.y + (int)direction.y);
            }
            if(currCell == null)
            {
                return null;
            }
            else
            {
                return currCell.GetPawn();
            }
        }

        ///<returns>Returns Null in case of no Pawn</returns>
        public Pawn GetPawn(int x, int y)
        {
            return GetCell(x, y).GetPawn();
        }

        public void AddPawn(Pawn pawn, int x, int y)
        {
            GetCell(x, y).SetPawn(pawn);
        }

        public void RemovePawn(int x, int y)
        {
            GetCell(x, y).SetPawn(null);
        }
    }
}
