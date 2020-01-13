using System;

namespace modern_tech_499m.Logic
{
    public interface IPlayer : IEquatable<IPlayer>
    {
        Guid GlobalId { get; }
        event EventHandler<CellGetterEventArgs> OnGetCell;
        IPlayer Enemy { get; set; }
        int? Id { get; }
        string Name { get; set; }
        bool CanUndoMoves { get; set; }
        void GetCell(GameLogic gameLogic);
    }
}
