using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace modern_tech_499m.Logic
{
    class UserPlayer : IPlayer
    {
        public event EventHandler<CellGetterEventArgs> OnGetCell;
        public IPlayer Enemy { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Name { get; set; }

        public UserPlayer(string name)
        {
            Name = name;
        }

        public void GetCell(GameLogic gameLogic)
        { }

        public void MakeMove(int cellNumber)
        {
            if (OnGetCell == null)
                return;
            CellGetterEventArgs eventArgs = new CellGetterEventArgs(cellNumber);
            OnGetCell(this, eventArgs);
        }
    }
}
