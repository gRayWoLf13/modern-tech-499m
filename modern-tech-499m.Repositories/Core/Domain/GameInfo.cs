using System;

namespace modern_tech_499m.Repositories.Core.Domain
{
    public class GameInfo
    {
        public int Id { get; set; }
        public DateTime GameDate { get; set; }
        public int Score { get; set; }
        public bool GameFinished { get; set; }
        public string InternalGameData { get; set; }
        public string InternalSolverData { get; set; }
    }
}
