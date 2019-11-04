using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace modern_tech_499m.Logic
{
    class Move
    {
        public IPlayer _moveOwner;
        public Dictionary<Cell, int> _cellValuesChanges;

        public Move(IPlayer moveOwner, Dictionary<Cell, int> cellValuesChanges)
        {
            _moveOwner = moveOwner;
            _cellValuesChanges = cellValuesChanges;
        }

        public IPlayer MoveOwner
        {
            get => _moveOwner;
            private set => _moveOwner = value;
        }

        public Dictionary<Cell, int> CellValuesChanges
        {
            get => _cellValuesChanges;
            private set => _cellValuesChanges = value;
        }
    }
}
