using System;
using modern_tech_499m.AILogic;
using modern_tech_499m.Repositories.Core.Domain;

namespace modern_tech_499m.Logic
{
    internal class AIPlayer : IPlayer
    {
        public event EventHandler<CellGetterEventArgs> OnGetCell;
        public IPlayer Enemy { get; set; }
        public string Name { get; set; }
        public int? Id => null;

        public AISolver Solver { get; }

        public bool CanUndoMoves { get; set; }

        public AIPlayer(string name)
        {
            Name = name;
            CanUndoMoves = false;
            Solver = new AISolver(true);
        }

        public void GetCell(GameLogic gameLogic)
        {
            int cellNumber = Solver.GetCell(gameLogic);
            if (OnGetCell == null)
                return;
            CellGetterEventArgs eventArgs = new CellGetterEventArgs(cellNumber);
            OnGetCell(this, eventArgs);
        }
    }
}
