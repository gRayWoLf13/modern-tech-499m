using System;

namespace modern_tech_499m.Logic
{
    class CellGetterEventArgs : EventArgs
    {
        public int CellNumber { get; set; }
        public CellGetterEventArgs(int number)
        {
            CellNumber = number;
        }
    }
}
