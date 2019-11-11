using System;
using modern_tech_499m.Repositories.Core.Repositories;

namespace modern_tech_499m.Repositories.Core.Domain
{
    public class GameInfo : IEntity
    {
        public int Id { get; set; }
        //Real - OLE Automation Date
        public DateTime GameDate { get; set; }
        public int? Player1Id { get; set; }
        public int? Player2Id { get; set; }
        public int Score { get; set; }
        //Integer - 0/1
        public bool GameFinished { get; set; }
        public string InternalGameData { get; set; }
        public string InternalSolverData { get; set; }
    }
}
