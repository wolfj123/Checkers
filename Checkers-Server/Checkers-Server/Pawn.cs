using System;
using System.Collections.Generic;
using System.Text;

namespace Checkers_Server
{
    public class Pawn
    {
        //public Cell cell { get; set; }
        public Color color { get; set; }
        public PawnType type { get; set; }

        public Pawn(Color color, PawnType type)
        {
            this.color = color;
        }

        //public Color GetColor()
        //{
        //    return color;
        //}

        //public void SetColor(Color color)
        //{
        //    this.color = color;
        //}

        //public PawnType GetPawnType()
        //{
        //    return type;
        //}

        //public void SetPawnType(PawnType type)
        //{
        //    this.type = type;
        //}
    }

    public enum Color
    {
        BLACK,
        WHITE,
    }

    public enum PawnType
    {
        NORMAL,
        QUEEN
    }
}
