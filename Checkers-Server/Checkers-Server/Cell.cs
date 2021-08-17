using System;
using System.Collections.Generic;
using System.Text;

namespace Checkers_Server
{
    public class Cell
    {
        Pawn pawn;
        public bool homebase { get; set; }

        public int x { get; set; }
        public int y { get; set; }

        public Cell(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        ///<returns>Returns Null in case of no Pawn</returns>
        public Pawn GetPawn()
        {
            return pawn;
        }

        public bool isEmpty()
        {
            return GetPawn() == null;
        }

        public void SetPawn(Pawn pawn)
        {
            this.pawn = pawn;
        }
    }
 
}
