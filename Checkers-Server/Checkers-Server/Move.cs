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
        Board board;
        Queue<IStep> steps;
        Stack<IStep> undoSteps;

        public Move(Board board)
        {
            this.board = board;
            steps = new Queue<IStep>();
            undoSteps = new Stack<IStep>();
        }

        void DoMove()
        {
            for (int i = 0; i < steps.Count; i++)
            {
                IStep currStep = steps.Dequeue();
                currStep.DoStep();
                undoSteps.Push(currStep);
            }
        }

        void UndoMove()
        {
            for (int i = 0; i < undoSteps.Count; i++)
            {
                IStep currStep = undoSteps.Pop();
                currStep.UndoStep();
                steps.Enqueue(currStep);
            }
        }

        public void AddStep(IStep step)
        {
            steps.Enqueue(step);
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
}

