using System;
using modern_tech_499m.AILogic;

namespace modern_tech_499m.Logic
{
    internal class AIPlayer : IPlayer
    {
        public event EventHandler<CellGetterEventArgs> OnGetCell;
        public IPlayer Enemy { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Name { get; set; }

        public bool CanUndoMoves { get; set; }

        public AIPlayer(string name)
        {
            Name = name;
            CanUndoMoves = false;
        }

        public void GetCell(GameLogic gameLogic)
        {
            AISolver solver = new AISolver(gameLogic);
            int cellNumber = solver.GetCell();
            if (OnGetCell == null)
                return;
            CellGetterEventArgs eventArgs = new CellGetterEventArgs(cellNumber);
            OnGetCell(this, eventArgs);
        }
    }
}
