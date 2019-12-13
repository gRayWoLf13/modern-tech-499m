using System.Collections.Generic;

namespace modern_tech_499m.Logic
{
    class Move
    {
        public IPlayer MoveOwner { get; }
        public List<KeyValuePair<Cell, int>> CellValuesChanges { get; }
        public MoveResult Result { get; }

        public Move(IPlayer moveOwner, List<KeyValuePair<Cell, int>> cellValuesChanges, MoveResult result)
        {
            MoveOwner = moveOwner;
            CellValuesChanges = cellValuesChanges;
            Result = result;
        }
    }
}
