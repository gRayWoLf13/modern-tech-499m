using System;

namespace modern_tech_499m.Logic
{
    class Cell : IEquatable<Cell>
    {
        public IPlayer Owner { get; }
        public int Value { get; set; }
        public bool IsEndingCell { get; set; }
        public int Number { get; }

        public Cell(IPlayer owner, int number)
        {
            Owner = owner;
            Number = number;
        }

        public bool Equals(Cell other)
        {
            if (other == null)
                return false;
            return Owner.Equals(other.Owner) && Number == other.Number;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Cell);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + Owner.GetHashCode();
                hash = hash * 23 + Number.GetHashCode();
                return hash;
            }
        }
    }
}