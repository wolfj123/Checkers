using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Checkers_Server
{
    public class Board
    {
        Cell[][] cellsArray;
        List<Cell> cellsList;
        int size;

        public Board(int size)
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

        public Cell GetCell(int x, int y)
        {
            return cellsArray[x][y];
        }

        public List<Cell> GetAllCells()
        {
            return cellsList;
        }

        public List<Pawn> GetAllPawns()
        {
            List<Pawn> result = cellsList.ConvertAll<Pawn>(cell => cell.GetPawn());
            result.RemoveAll(pawn => pawn == null);
            return result;

            //List<Pawn> result = new List<Pawn>();
            //for (int x = 0; x < size; x++)
            //{
            //    for (int y = 0; y < size; y++)
            //    {
            //        Pawn currPawn = cells[x][y].GetPawn();
            //        if(currPawn != null)
            //        {
            //            result.Add(currPawn);
            //        }
            //    }
            //}
            //return result;
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
