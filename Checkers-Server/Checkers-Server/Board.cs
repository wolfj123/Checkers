using System;
using System.Collections.Generic;
using System.Text;

namespace Checkers_Server
{
    public class Board
    {
        Cell[][] cells;

        Board(int size)
        {
            cells = new Cell[size][];
            for (int x = 0; x < size; x++)
            {
                cells[x] = new Cell[size];
                for (int y = 0; y < size; y++)
                {
                    //Color color = (x + y) % 
                    cells[x][y] = new Cell(x, y);
                }
            }
        }

        public Cell GetCell(int x, int y)
        {
            return cells[x][y];
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
