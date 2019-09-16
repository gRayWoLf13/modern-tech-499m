using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using modern_tech_499m.Logic;

namespace modern_tech_499m.Tests
{
    [TestClass]
    public class modern_tech_499m_Tests_GameLogic
    {
        [TestMethod]
        public void FieldCreation()
        {
            const int initialCount = 10;
            GameLogic logic = new GameLogic(initialCount, null, null);
            PrivateObject prLogic = new PrivateObject(logic);
            var resultField = prLogic.GetField("Field") as List<Cell>;
            List<Cell> expectedField = new List<Cell>() {
            new Cell() {Number = 0, Owner = null, IsEndingCell = false, Value = initialCount },
            new Cell(){Number = 1, Owner = null, IsEndingCell = false, Value = initialCount},
            new Cell(){Number = 2, Owner = null, IsEndingCell = false, Value = initialCount},
            new Cell(){Number = 3, Owner = null, IsEndingCell = false, Value = initialCount},
            new Cell(){Number = 4, Owner = null, IsEndingCell = false, Value = initialCount},
            new Cell(){Number = 5, Owner = null, IsEndingCell = false, Value = initialCount},
            new Cell(){Number = 6, Owner = null, IsEndingCell = true, Value = 0},

            new Cell() {Number = 0, Owner = null, IsEndingCell = false, Value = initialCount },
            new Cell(){Number = 1, Owner = null, IsEndingCell = false, Value = initialCount},
            new Cell(){Number = 2, Owner = null, IsEndingCell = false, Value = initialCount},
            new Cell(){Number = 3, Owner = null, IsEndingCell = false, Value = initialCount},
            new Cell(){Number = 4, Owner = null, IsEndingCell = false, Value = initialCount},
            new Cell(){Number = 5, Owner = null, IsEndingCell = false, Value = initialCount},
            new Cell(){Number = 6, Owner = null, IsEndingCell = true, Value = 0} };

            CollectionAssert.AreEqual(expectedField, resultField, Comparer<Cell>.Create((x, y) =>
            {
                if (x.IsEndingCell.CompareTo(y.IsEndingCell) != 0)
                {
                    return -1;
                }
                if (x.Number.CompareTo(y.Number) != 0)
                {
                    return -1;
                }
                if (x.Owner == null ^ y.Owner == null)
                {
                    return -1;
                }
                else
                {
                    if (x.Owner != null && !x.Owner.Equals(y.Owner))
                    {
                        return -1;
                    }
                }
                if (x.Value.CompareTo(y.Value) != 0)
                {
                    return -1;
                }
                return 0;
            }));
        }
    }
}
