using System;
using System.Collections.Generic;
using System.Linq;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("modern_tech_499m.Tests")]
namespace modern_tech_499m.Logic
{
    internal class GameLogic : ICloneable
    {
        public static readonly int CellsCount;
        private List<Cell> field;
        private IPlayer player1, player2, currentPlayer;
        private int initialValue;
        private static List<int> availableValuesToStealEnemyPoints;

        static GameLogic()
        {
            //TODO: Read cellsCount and availableStealingValues from config files...
            CellsCount = 6;
            availableValuesToStealEnemyPoints = new List<int>() { 2, 3 };
        }

        public GameLogic(int initialValue, IPlayer player1, IPlayer player2, IPlayer firstPlayer)
        {
            if (player1 == null || player2 == null)
                throw new ArgumentNullException("One of the players is null");
            if (!firstPlayer.Equals(player1) && !firstPlayer.Equals(player2))
                throw new ArgumentException("First player is incorrect");
            this.player1 = player1;
            this.player2 = player2;
            currentPlayer = firstPlayer;
            this.initialValue = initialValue;
            CreateField(initialValue);
        }

        [Obsolete]
        public GameLogic(IPlayer player1, IPlayer player2, IPlayer firstPlayer, int[] initialValues, int endingCellPlayer1Value, int endingCellPlayer2Value)
        {
            if (player1 == null || player2 == null)
                throw new ArgumentNullException("One of the players is null");
            if (initialValues.Length != CellsCount * 2)
                throw new ArgumentException("Initial values array size is incorrect");
            if (!firstPlayer.Equals(player1) && !firstPlayer.Equals(player2))
                throw new ArgumentException("First player is incorrect");
            this.player1 = player1;
            this.player2 = player2;
            currentPlayer = firstPlayer;
            initialValue = CreateField(initialValues, endingCellPlayer1Value, endingCellPlayer2Value) / 2 / CellsCount;
        }

        public object Clone()
        {
            int[] initialValues = new int[CellsCount * 2];
            for (int i = 0; i < CellsCount; i++)
                initialValues[i] = field[i].Value;
            for (int i = 0; i < CellsCount; i++)
                initialValues[CellsCount + i] = field[CellsCount + 1 + i].Value;
            GameLogic logicClone = new GameLogic(player1, player2, currentPlayer, initialValues, field[CellsCount].Value, field[CellsCount * 2 + 1].Value);
            return logicClone;
        }

        public int GetCellValue(IPlayer player, int cellIndex)
        {
            if (player == null)
                throw new ArgumentNullException("Passed player is null");
            if (cellIndex < 0 || cellIndex > CellsCount)
                throw new ArgumentOutOfRangeException(nameof(cellIndex), "Param value is outside of the range");
            int indexOnField = player.Equals(player1) ? cellIndex : CellsCount + 1 + cellIndex;
            return field[indexOnField].Value;
        }

        public MoveResult MakeMove(IPlayer player, int cellIndex)
        {
            if (player != currentPlayer)
                return MoveResult.ImpossibleMove;
            if (cellIndex < 0 || cellIndex >= CellsCount)
                return MoveResult.ImpossibleMove;
            if (!CheckMovePossible())
            {
                ClearAllEnemyNonEndingCellsOnGameEnd();
                return MoveResult.GameEnded;
            }
            int indexOnField = player.Equals(player1) ? cellIndex : CellsCount + 1 + cellIndex;
            if (field[indexOnField].Value == 0)
                return MoveResult.ImpossibleMove;
            (MoveResult moveResult, int lastCellNumber) result = MakeSingleMove(player, indexOnField);
            while (result.moveResult == MoveResult.ContinuousMove)
            {
                indexOnField = player.Equals(player1) ? result.lastCellNumber : CellsCount + 1 + result.lastCellNumber;
                result = MakeSingleMove(player, indexOnField);
            }
            if (CheckGameEnding())
                return MoveResult.GameEnded;
            currentPlayer = currentPlayer.Equals(player1) ? player2 : player1;
            return MoveResult.EndedMove;
        }

        [Obsolete]
        private int CreateField(int[] initialvalues, int endingCellPlayer1Value, int endingCellPlayer2Value)
        {
            field = new List<Cell>();
            int counter = 0;
            for (int i = 0; i < CellsCount; i++)
            {
                field.Add(new Cell() { Owner = player1, Value = initialvalues[counter], IsEndingCell = false, Number = i });
                counter++;
            }
            field.Add(new Cell() { Owner = player1, Value = endingCellPlayer1Value, IsEndingCell = true, Number = CellsCount });
            for (int i = 0; i < CellsCount; i++)
            {
                field.Add(new Cell() { Owner = player2, Value = initialvalues[counter], IsEndingCell = false, Number = i });
                counter++;
            }
            field.Add(new Cell() { Owner = player2, Value = endingCellPlayer2Value, IsEndingCell = true, Number = CellsCount });
            return initialvalues.Sum();
        }

        private void CreateField(int initialValue)
        {
            field = new List<Cell>();
            for (int i = 0; i < CellsCount; i++)
            {
                field.Add(new Cell() { Owner = player1, Value = initialValue, IsEndingCell = false, Number = i });
            }
            field.Add(new Cell() { Owner = player1, Value = 0, IsEndingCell = true, Number = CellsCount });
            for (int i = 0; i < CellsCount; i++)
            {
                field.Add(new Cell() { Owner = player2, Value = initialValue, IsEndingCell = false, Number = i });
            }
            field.Add(new Cell() { Owner = player2, Value = 0, IsEndingCell = true, Number = CellsCount });
        }

        private (MoveResult moveResult, int lastCellNumber) MakeSingleMove(IPlayer player, int indexOnField)
        {
            bool passedEnemyCell = false;
            Cell lastCell = field[indexOnField];
            int value = field[indexOnField].Value;
            field[indexOnField].Value = 0;
            foreach (Cell cell in field.Cycle(indexOnField + 1))
            {
                if (value == 0)
                    break;
                if (cell.Owner != player && cell.IsEndingCell)
                    continue;
                if (!passedEnemyCell && cell.Owner != player && !cell.IsEndingCell)
                    passedEnemyCell = true;
                lastCell = cell;
                cell.Value++;
                value--;
            }
            bool endedOnPlayerCell = lastCell.Owner == player && !lastCell.IsEndingCell;
            bool endedOnEnemyCell = lastCell.Owner != player && !lastCell.IsEndingCell;
            if (passedEnemyCell && endedOnPlayerCell && lastCell.Value > 1)
                return (MoveResult.ContinuousMove, lastCell.Number);
            if (endedOnEnemyCell && availableValuesToStealEnemyPoints.Contains(lastCell.Value))
                StealEnemyPoints(player, lastCell.Number);
            return (MoveResult.EndedMove, lastCell.Number);
        }

        private bool CheckGameEnding()
        {
            if (field[CellsCount].Value >= initialValue * CellsCount)
                return true;
            if (field[CellsCount * 2 + 1].Value >= initialValue * CellsCount)
                return true;
            return false;
        }

        private bool CheckMovePossible()
        {
            int cellToStartSearching = currentPlayer.Equals(player1) ? 0 : CellsCount + 1;
            for (int i = 0; i < CellsCount; i++)
                if (field[cellToStartSearching + i].Value != 0)
                    return true;
            return false;
        }

        private void ClearAllEnemyNonEndingCellsOnGameEnd()
        {
            int cellToStartCleaning = currentPlayer.Equals(player1) ? CellsCount + 1 : 0;
            int targetEngingCell = currentPlayer.Equals(player1) ? CellsCount * 2 + 1 : CellsCount;
            for(int i = 0; i < CellsCount; i++)
            {
                field[targetEngingCell].Value += field[cellToStartCleaning + i].Value;
                field[cellToStartCleaning + i].Value = 0;
            }
        }

        private void StealEnemyPoints(IPlayer currentPlayer, int endedCellNumber)
        {
            int endedCellIndexOnField;
            int targetEndingCellIndexOnField;
            if (currentPlayer.Equals(player1))
            {
                endedCellIndexOnField = endedCellNumber + CellsCount + 1;
                targetEndingCellIndexOnField = CellsCount;
            }
            else
            {
                endedCellIndexOnField = endedCellNumber;
                targetEndingCellIndexOnField = field.Count - 1;
            }
            bool haveFreeCells = endedCellIndexOnField >= 0 && !field[endedCellIndexOnField].IsEndingCell;
            while (haveFreeCells && availableValuesToStealEnemyPoints.Contains(field[endedCellIndexOnField].Value))
            {
                field[targetEndingCellIndexOnField].Value += field[endedCellIndexOnField].Value;
                field[endedCellIndexOnField].Value = 0;
                endedCellIndexOnField--;
                haveFreeCells = endedCellIndexOnField >= 0 && !field[endedCellIndexOnField].IsEndingCell;
            }
        }
    }
}
