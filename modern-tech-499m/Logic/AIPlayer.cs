using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace modern_tech_499m.Logic
{
    class AIPlayer : IPlayer
    {
        public IPlayer Enemy { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Name { get; set; }

        public AIPlayer(string name)
        {
            Name = name;
        }

        public MoveResult MakeMove(int cellIndex)
        {
            throw new NotImplementedException();
        }
    }
}
