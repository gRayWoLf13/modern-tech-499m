using System;
using modern_tech_499m.AILogic;
using NLog;

namespace modern_tech_499m.Logic
{
    internal class AIPlayer : IPlayer
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        public Guid GlobalId { get; }
        public event EventHandler<CellGetterEventArgs> OnGetCell;
        public IPlayer Enemy { get; set; }
        public string Name { get; set; }
        public int? Id => null;

        public AISolver Solver { get; }

        public bool CanUndoMoves { get; set; }

        public AIPlayer(string name, Guid globalId)
        {
            _logger.Debug($"AI player constructor called with name = {name}, GUID = {globalId}");
            GlobalId = globalId.Equals(Guid.Empty) ? Guid.NewGuid() : globalId;
            Name = name;
            CanUndoMoves = false;
            Solver = new AISolver(true);
        }

        public void GetCell(GameLogic gameLogic)
        {
            _logger.Debug("AI player get cell method called");
            int cellNumber = Solver.GetCell(gameLogic);
            if (OnGetCell == null)
                return;
            CellGetterEventArgs eventArgs = new CellGetterEventArgs(cellNumber);
            OnGetCell(this, eventArgs);
        }

        public bool Equals(IPlayer other)
        {
            return GlobalId.Equals(other?.GlobalId);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as IPlayer);
        }

        public override int GetHashCode()
        {
            return GlobalId.GetHashCode();
        }
    }
}
