using System;
using System.Collections.Generic;
using System.Text;

namespace Checkers_Server
{
    public class Cell
    {
        Pawn pawn;
        int x;
        int y;

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

        public void SetPawn(Pawn pawn)
        {
            //if (this.pawn != null && this.pawn.cell == this)
            //{
            //    pawn.cell = null;
            //}
            this.pawn = pawn;
            //if(this.pawn != null)
            //{
            //    pawn.cell = this;
            //}
        }
    }
 
}
