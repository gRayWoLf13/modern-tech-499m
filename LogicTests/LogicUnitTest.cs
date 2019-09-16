using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using modern_tech_499m.Logic;

namespace LogicTests
{
    [TestClass]
    public class LogicUnitTest
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

            CollectionAssert.AreEqual(expectedField, resultField, Comparer<Cell>.Create((x, y) => {
                if (x.IsEndingCell.CompareTo(y.IsEndingCell) != 0)
                    return -1;
                if (x.Number.CompareTo(y.Number) != 0)
                    return -1;
                if ((x.Owner?.Equals(y.Owner)).HasValue && (x.Owner?.Equals(y.Owner)).Value == false)
                    return -1;
                if (x.Value.CompareTo(y.Value) != 0)
                    return -1;
                return 0;
            }));
        }
    }
}
