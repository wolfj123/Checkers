using System;
using System.Collections.Generic;
using System.Text;

namespace Checkers_Server
{
    /// <summary>
    /// Represents a legal move that can be done by a player. A single move is composed by a list of steps.
    /// <para>Command design pattern:
    /// https://refactoring.guru/design-patterns/command </para>
    /// </summary>
    public class Move
    {
        protected Board board;
        protected Queue<IStep> steps;
        protected Stack<IStep> undoSteps;
        protected List<Move> chainedMoves;

        public Move(Board board)
        {
            this.board = board;
            steps = new Queue<IStep>();
            undoSteps = new Stack<IStep>();
        }

        public void DoMove()
        {
            for (int i = 0; i < steps.Count; i++)
            {
                IStep currStep = steps.Dequeue();
                currStep.DoStep();
                undoSteps.Push(currStep);
            }
        }

        public void UndoMove()
        {
            for (int i = 0; i < undoSteps.Count; i++)
            {
                IStep currStep = undoSteps.Pop();
                currStep.UndoStep();
                steps.Enqueue(currStep);
            }
        }

        /// <summary>
        /// Add a step to a move.
        /// <para>returns self to allow for Fluent Interface</para>
        /// </summary>
        /// <param name="step"></param>
        /// <returns>self</returns>
        public Move AddStep(IStep step)
        {
            steps.Enqueue(step);
            return this;
        }

        public List<Move> GetChainedMove()
        {
            return chainedMoves;
        }

        public void SetChainedMove(List<Move> moves)
        {
            this.chainedMoves = moves;
        }

        public static Move AdvanceMove(Board board, Cell cell, Pawn pawn, DirectionX dx, DirectionY dy, int distance)
        {
            Move result = new Move(board)
                .AddStep(new ChangePawnPositionStep(board, cell.x, cell.y, cell.x + (int)dx * distance, cell.y + (int)dx * distance));
            return result;
        }

        public static Move EatMove(Board board, Cell cell, Pawn pawn, DirectionX dx, DirectionY dy)
        {
            var eatDistance = 2;
            var enemyX = cell.x + (int)dx;
            var enemyY = cell.y + (int)dy;
            Move result = new Move(board)
                .AddStep(new RemovePawnStep(board, enemyX, enemyY))
                .AddStep(new ChangePawnPositionStep(board, cell.x, cell.y, cell.x + ((int)dx * eatDistance), cell.y + ((int)dx * eatDistance)));
            return result;
        }
    }


    /// <summary>
    /// Represents a single step as part of a legal that can be done by a player.
    /// <para>Command design pattern:
    /// https://refactoring.guru/design-patterns/command </para>
    /// </summary>
    public interface IStep
    {
        void DoStep();

        void UndoStep();
    }


    public class RemovePawnStep : IStep
    {
        Board board;
        Pawn pawn;
        int x, y;

        public RemovePawnStep(Board board, int x, int y)
        {
            this.board = board;
            this.x = x;
            this.y = y;
        }
        public void DoStep()
        {
            pawn = board.GetPawn(x, y);
            board.RemovePawn(x, y);
        }

        public void UndoStep()
        {
            board.AddPawn(pawn, x, y);
            pawn = null;
        }
    }

    public class ChangePawnPositionStep : IStep
    {
        Board board;
        Pawn pawn;
        int x1, y1, x2, y2;

        public ChangePawnPositionStep(Board board, int x1, int y1, int x2, int y2)
        {
            this.board = board;
            this.x1 = x1;
            this.y1 = y1;
            this.x2 = x2;
            this.y2 = y2;
        }

        public void DoStep()
        {
            pawn = board.GetPawn(x1, y1);
            board.RemovePawn(x1, y1);
            board.AddPawn(pawn, x2, y2);
        }

        public void UndoStep()
        {
            board.RemovePawn(x2, y2);
            board.AddPawn(pawn, x1, y1);
            pawn = null;
        }
    }

    public class UpgradeToQueen : IStep {
        Pawn pawn;

        public UpgradeToQueen(Pawn pawn)
        {
            this.pawn = pawn;
        }

        public void DoStep()
        {
            pawn.type = PawnType.QUEEN;
        }

        public void UndoStep()
        {
            pawn.type = PawnType.NORMAL;
        }
    }

    public enum DirectionX
    {
        RIGHT = 1,
        LEFT = -1,
    }

    public enum DirectionY
    {
        UP = 1 ,
        DOWN = - 1,
    }
}

