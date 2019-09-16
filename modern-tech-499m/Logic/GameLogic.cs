﻿using System;
using System.Collections.Generic;
using System.Linq;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("modern_tech_499m.Tests")]
namespace modern_tech_499m.Logic
{
    internal class GameLogic
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

        public GameLogic(int initialValue, IPlayer player1, IPlayer player2)
        {
            if (player1 == null || player2 == null)
                throw new ArgumentNullException("One of the players is null");
            currentPlayer = this.player1 = player1;
            this.player2 = player2;
            this.initialValue = initialValue;
            CreateField(initialValue);
        }

        [Obsolete]
        public GameLogic(IPlayer player1, IPlayer player2, int[] initialValues)
        {
            if (player1 == null || player2 == null)
                throw new ArgumentNullException("One of the players is null");
            if (initialValues.Length != CellsCount * 2)
                throw new ArgumentException("Initial values array size is incorrect");
            currentPlayer = this.player1 = player1;
            this.player2 = player2;
            initialValue = CreateField(initialValues) / 2 / CellsCount;
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
        private int CreateField(int[] initialvalues)
        {
            field = new List<Cell>();
            int counter = 0;
            for (int i = 0; i < CellsCount; i++)
            {
                field.Add(new Cell() { Owner = player1, Value = initialvalues[counter], IsEndingCell = false, Number = i });
                counter++;
            }
            field.Add(new Cell() { Owner = player1, Value = 0, IsEndingCell = true, Number = CellsCount });
            for (int i = 0; i < CellsCount; i++)
            {
                field.Add(new Cell() { Owner = player2, Value = initialvalues[counter], IsEndingCell = false, Number = i });
                counter++;
            }
            field.Add(new Cell() { Owner = player2, Value = 0, IsEndingCell = true, Number = CellsCount });
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
            bool endedOnPlayerCell = true;
            bool endedOnEnemyCell = false;
            int lastCellNumber = field[indexOnField].Number;
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
                if (cell.Owner == player && !cell.IsEndingCell)
                    endedOnPlayerCell = true;
                else
                    endedOnPlayerCell = false;
                if (cell.Owner != player && !cell.IsEndingCell)
                    endedOnEnemyCell = true;
                else
                    endedOnEnemyCell = false;
                lastCellNumber = cell.Number;
                cell.Value++;
                value--;
            }
            if (passedEnemyCell && endedOnPlayerCell)
                return (MoveResult.ContinuousMove, lastCellNumber);
            if (endedOnEnemyCell)
            {
                int lastCellIndexOnField;
                if (player.Equals(player1))
                    lastCellIndexOnField = lastCellNumber + CellsCount + 1;
                else
                    lastCellIndexOnField = lastCellNumber;
                if (availableValuesToStealEnemyPoints.Contains(field[lastCellIndexOnField].Value))
                    StealEnemyPoints(player, lastCellNumber);
            }
            return (MoveResult.EndedMove, lastCellNumber);
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
