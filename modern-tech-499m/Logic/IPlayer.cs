using System;

namespace modern_tech_499m.Logic
{
    interface IPlayer
    {
        event EventHandler<CellGetterEventArgs> OnGetCell;
        IPlayer Enemy { get; set; }
        string Name { get; set; }
        bool CanUndoMoves { get; set; }
        void GetCell(GameLogic gameLogic);
    }
}
