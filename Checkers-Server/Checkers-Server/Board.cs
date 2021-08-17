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
        Cell[][] cellsArray;
        List<Cell> cellsList;
        int size;

        public Board(int size)
        {
            if(size <= 0)
            {
                throw new ArgumentException("size of board must be larger than 0");
            }
            createCellMatrix(size);
            populateMatrixWithPawns();
        }

        private void createCellMatrix(int size)
        {
            this.size = size;
            cellsArray = new Cell[size][];
            for (int x = 0; x < size; x++)
            {
                cellsArray[x] = new Cell[size];
                for (int y = 0; y < size; y++)
                {
                    //Color color = (x + y) % 
                    cellsArray[x][y] = new Cell(x, y);
                    cellsList.Add(cellsArray[x][y]);
                }
            }
        }

        private void populateMatrixWithPawns()
        {
            //TODO: add pawns
        }

        public Cell GetCell(int x, int y)
        {
            return cellsArray[x][y];
        }

        public List<Cell> GetAllCells()
        {
            return cellsList;
        }

        public static (DirectionX, DirectionY)[] GetAllDirections()
        {
            return directions;
            //return new List<(DirectionX, DirectionY)>()
            //    .AddFluent((DirectionX.RIGHT, DirectionY.UP))
            //    .AddFluent((DirectionX.RIGHT, DirectionY.DOWN))
            //    .AddFluent((DirectionX.LEFT, DirectionY.UP))
            //    .AddFluent((DirectionX.LEFT, DirectionY.DOWN));
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

        ///<returns>Returns Null in case of no Pawn</returns>
        public Pawn GetPawn(int x, int y)
        {
            return GetCell(x, y).GetPawn();
        }

        public void AddPawn(Pawn pawn, int x, int y)
        {
            //if(GetPawn(x,y) == null)
           // {
                //TODO: error
            //    return;
            //}

            GetCell(x, y).SetPawn(pawn);
        }

        public void RemovePawn(int x, int y)
        {
            GetCell(x, y).SetPawn(null);
        }
    }
}
