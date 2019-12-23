using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using modern_tech_499m.Logic;

namespace modern_tech_499m.Tests
{
    [TestClass]
    public class modern_tech_499m_Tests_GameLogic
    {
        private static readonly Lazy<Comparer<Cell>> cellComparer = new Lazy<Comparer<Cell>>(CreateCellComparer);

        [TestMethod]
        public void FieldCreation()
        {
            IPlayer player1 = new UserPlayer("player1", null, Guid.Empty), player2 = new AIPlayer("player2", Guid.Empty);
            GameLogic logic = new GameLogic(10, player1, player2, player1);
            PrivateObject prLogic = new PrivateObject(logic);
            List<Cell> resultField = prLogic.GetField("field") as List<Cell>;
            List<Cell> expectedField = GenerateField(6, player1, player2, new int[] { 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10 }, 0, 0);
            CollectionAssert.AreEqual(expectedField, resultField, cellComparer.Value);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void FieldCreationWithNullPlayer()
        {
            new GameLogic(0, null, null, null);
        }

        [TestMethod]
        public void GeneralStartMove1()
        {
            IPlayer player1 = new UserPlayer("player1", null, Guid.Empty), player2 = new UserPlayer("player2", null, Guid.Empty);
            GameLogic logic = new GameLogic(6, player1, player2, player1);
            MoveResult move = logic.MakeMove(player1, 0);
            PrivateObject prLogic = new PrivateObject(logic);
            List<Cell> resultField = prLogic.GetField("field") as List<Cell>;
            List<Cell> expectedField = GenerateField(6, player1, player2, new int[] { 0, 7, 7, 7, 7, 7, 6, 6, 6, 6, 6, 6 }, 1, 0);
            Assert.AreEqual(move, MoveResult.EndedMove);
            CollectionAssert.AreEqual(expectedField, resultField, cellComparer.Value);
        }

        [TestMethod]
        public void WrongPlayerStartMove()
        {
            IPlayer player1 = new UserPlayer("player1", null, Guid.Empty), player2 = new UserPlayer("player2", null, Guid.Empty);
            GameLogic logic = new GameLogic(6, player1, player2, player1);
            MoveResult move = logic.MakeMove(player2, 0);
            PrivateObject prLogic = new PrivateObject(logic);
            List<Cell> resultField = prLogic.GetField("field") as List<Cell>;
            List<Cell> expectedField = GenerateField(6, player1, player2, new int[] { 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6 }, 0, 0);
            Assert.AreEqual(move, MoveResult.ImpossibleMove);
            CollectionAssert.AreEqual(expectedField, resultField, cellComparer.Value);
        }

        [TestMethod]
        public void WrongCellNumberMove()
        {
            IPlayer player1 = new UserPlayer("player1", null, Guid.Empty), player2 = new UserPlayer("player2", null, Guid.Empty);
            GameLogic logic = new GameLogic(6, player1, player2, player1);
            MoveResult move = logic.MakeMove(player1, 7);
            PrivateObject prLogic = new PrivateObject(logic);
            List<Cell> resultField = prLogic.GetField("field") as List<Cell>;
            List<Cell> expectedField = GenerateField(6, player1, player2, new int[] { 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6 }, 0, 0);
            Assert.AreEqual(move, MoveResult.ImpossibleMove);
            CollectionAssert.AreEqual(expectedField, resultField, cellComparer.Value);
        }

        [TestMethod]
        public void CycleMove1()
        {
            IPlayer player1 = new UserPlayer("player1", null, Guid.Empty), player2 = new UserPlayer("player2", null, Guid.Empty);
            GameLogic logic = new GameLogic(player1, player2, player1, new int[] {3, 4, 6, 2, 4, 14, 4, 7, 10, 2, 0, 1 }, 0, 0);
            MoveResult move = logic.MakeMove(player1, 5);
            PrivateObject prLogic = new PrivateObject(logic);
            List<Cell> resultField = prLogic.GetField("field") as List<Cell>;
            List<Cell> expectedField = GenerateField(6, player1, player2, new int[] { 4, 5, 7, 3, 5, 1, 5, 8, 11, 3, 1, 2 }, 2, 0);
            Assert.AreEqual(move, MoveResult.EndedMove);
            CollectionAssert.AreEqual(expectedField, resultField, cellComparer.Value);
        }

        [TestMethod]
        public void ContinuousMove1()
        {
            IPlayer player1 = new UserPlayer("player1", null, Guid.Empty), player2 = new UserPlayer("player2", null, Guid.Empty);
            GameLogic logic = new GameLogic(player1, player2, player1, new int[] { 3, 4, 6, 2, 4, 11, 4, 7, 10, 2, 0, 1 }, 0, 0);
            MoveResult move = logic.MakeMove(player1, 5);
            PrivateObject prLogic = new PrivateObject(logic);
            List<Cell> resultField = prLogic.GetField("field") as List<Cell>;
            List<Cell> expectedField = GenerateField(6, player1, player2, new int[] { 4, 5, 7, 0, 5, 1, 5, 8, 11, 3, 1, 2 }, 2, 0);
            Assert.AreEqual(move, MoveResult.EndedMove);
            CollectionAssert.AreEqual(expectedField, resultField, cellComparer.Value);
        }

        [TestMethod]
        public void ContinuousMove2()
        {
            IPlayer player1 = new UserPlayer("player1", null, Guid.Empty), player2 = new UserPlayer("player2", null, Guid.Empty);
            GameLogic logic = new GameLogic(player1, player2, player1, new int[] { 3, 4, 6, 2, 4, 8, 4, 7, 10, 2, 0, 1 }, 0, 0);
            MoveResult move = logic.MakeMove(player1, 5);
            PrivateObject prLogic = new PrivateObject(logic);
            List<Cell> resultField = prLogic.GetField("field") as List<Cell>;
            List<Cell> expectedField = GenerateField(6, player1, player2, new int[] { 0, 5, 7, 3, 5, 0, 5, 8, 11, 3, 1, 2 }, 1, 0);
            Assert.AreEqual(move, MoveResult.EndedMove);
            CollectionAssert.AreEqual(expectedField, resultField, cellComparer.Value);
        }

        [TestMethod]
        public void MoveWithStealigValues1()
        {
            IPlayer player1 = new UserPlayer("player1", null, Guid.Empty), player2 = new UserPlayer("player2", null, Guid.Empty);
            GameLogic logic = new GameLogic(player1, player2, player1, new int[] { 1, 0, 1, 0, 6, 5, 3, 1, 2, 1, 4, 3 }, 0, 0);
            MoveResult move = logic.MakeMove(player1, 5);
            PrivateObject prLogic = new PrivateObject(logic);
            List<Cell> resultField = prLogic.GetField("field") as List<Cell>;
            List<Cell> expectedField = GenerateField(6, player1, player2, new int[] { 1, 0, 1, 0, 6, 0, 4, 0, 0, 0, 4, 3 }, 8, 0);
            Assert.AreEqual(move, MoveResult.EndedMove);
            CollectionAssert.AreEqual(expectedField, resultField, cellComparer.Value);
        }

        private static Comparer<Cell> CreateCellComparer()
        {
            return Comparer<Cell>.Create((x, y) =>
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
            });
        }

        private static List<Cell> GenerateField(int cellsCount, IPlayer player1, IPlayer player2, int[] initialvalues, int endingCellPlayer1Value, int endingCellPlayer2Value)
        {
            List<Cell> field = new List<Cell>();
            int counter = 0;
            for (int i = 0; i < cellsCount; i++)
            {
                field.Add(new Cell(player1, i) {Value = initialvalues[counter], IsEndingCell = false});
                counter++;
            }
            field.Add(new Cell(player1, cellsCount) { Value = endingCellPlayer1Value, IsEndingCell = true });
            for (int i = 0; i < cellsCount; i++)
            {
                field.Add(new Cell(player2, i) {Value = initialvalues[counter], IsEndingCell = false });
                counter++;
            }
            field.Add(new Cell(player2, cellsCount) { Value = endingCellPlayer2Value, IsEndingCell = true });
            return field;
        }
    }
}
