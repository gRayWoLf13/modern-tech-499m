using System;
using modern_tech_499m.Repositories.Core.Domain;
using NLog;

namespace modern_tech_499m.Logic
{
    class UserPlayer : IPlayer
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        public Guid GlobalId { get; }
        public event EventHandler<CellGetterEventArgs> OnGetCell;
        public IPlayer Enemy { get; set; }
        public string Name { get; set; }
        public bool CanUndoMoves { get; set; }

        public User User { get; set; }
        public int? Id => User.Id;

        public UserPlayer(string name, User currentUser, Guid globalId)
        {
            _logger.Debug($"User player constructor called with name = {name}, GUID = {globalId}");
            GlobalId = globalId.Equals(Guid.Empty) ? Guid.NewGuid() : globalId;
            Name = name;
            CanUndoMoves = true;
            User = currentUser;
        }

        public void GetCell(GameLogic gameLogic)
        { }

        public void MakeMove(int cellNumber)
        {
            _logger.Debug($"User player make move method called with parameter {cellNumber}");
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
