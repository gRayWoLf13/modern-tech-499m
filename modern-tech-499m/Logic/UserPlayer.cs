using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace modern_tech_499m.Logic
{
    class UserPlayer : IPlayer
    {
        public IPlayer Enemy { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public MoveResult MakeMove(int cellIndex)
        {
            throw new NotImplementedException();
        }
    }
}
