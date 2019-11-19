using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Newtonsoft.Json;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("modern_tech_499m.Tests")]
namespace modern_tech_499m.Logic
{
    internal class GameLogic : ICloneable
    {
        public readonly int CellsCount;
        private List<Cell> field;
        private Stack<Move> undoMovesHistory;
        private Stack<Move> redoMovesHistory;

        public IPlayer Player1 { get; }
        public IPlayer Player2 { get; }
        public IPlayer CurrentPlayer { get; private set; }
        public bool GameEnded { get; private set; }
        public int Score => field[CellsCount].Value - field[field.Count - 1].Value;
        private int initialValue;
        private List<int> _availableValuesToStealEnemyPoints;

        public IPlayer GetOtherPlayer(IPlayer player)
        {
            if (!player.Equals(Player1) && !player.Equals(Player2))
                throw new ArgumentException("Invalid player", nameof(player));
            return player.Equals(Player1) ? Player2 : Player1;
        }

        public GameLogic(int initialValue, IPlayer player1, IPlayer player2, IPlayer firstPlayer, bool useDefaultSettings = true)
        {
            if (useDefaultSettings)
            {
                string cellsCount = ConfigurationManager.AppSettings["CellsCount"];
                if (cellsCount == null || !int.TryParse(cellsCount, out CellsCount))
                    CellsCount = 6;
                string availableValuesToStealEnemyPoints = ConfigurationManager.AppSettings["AvailableValuesToStealEnemyPoints"];
                if (availableValuesToStealEnemyPoints == null)
                    _availableValuesToStealEnemyPoints = new List<int> { 2, 3 };
                else
                {
                    string[] splittedValues = availableValuesToStealEnemyPoints.Split(new char[] { ' ', ',', ';' });
                    bool isCorrect = splittedValues.All(item => int.TryParse(item, out int value) && value > 0);
                    if (isCorrect)
                        _availableValuesToStealEnemyPoints = Array.ConvertAll(splittedValues, item => int.Parse(item)).ToList();
                    else
                        _availableValuesToStealEnemyPoints = new List<int> { 2, 3 };
                }
            }

            if (player1 == null || player2 == null)
                throw new ArgumentNullException("One of the players is null");
            if (!firstPlayer.Equals(player1) && !firstPlayer.Equals(player2))
                throw new ArgumentException("First player is incorrect");
            this.Player1 = player1;
            this.Player2 = player2;
            CurrentPlayer = firstPlayer;
            undoMovesHistory = new Stack<Move>();
            redoMovesHistory = new Stack<Move>();
            this.initialValue = initialValue;
            GameEnded = false;
            CreateField(initialValue);
        }

        [Obsolete]
        public GameLogic(IPlayer player1, IPlayer player2, IPlayer firstPlayer, int[] initialValues, int endingCellPlayer1Value, int endingCellPlayer2Value, bool useDefaultSettings = true)
        {
            if (useDefaultSettings)
            {
                string cellsCount = ConfigurationManager.AppSettings["CellsCount"];
                if (cellsCount == null || !int.TryParse(cellsCount, out CellsCount))
                    CellsCount = 6;
                string availableValuesToStealEnemyPoints = ConfigurationManager.AppSettings["AvailableValuesToStealEnemyPoints"];
                if (availableValuesToStealEnemyPoints == null)
                    _availableValuesToStealEnemyPoints = new List<int> { 2, 3 };
                else
                {
                    string[] splittedValues = availableValuesToStealEnemyPoints.Split(new char[] { ' ', ',', ';' });
                    bool isCorrect = splittedValues.All(item => int.TryParse(item, out int value) && value > 0);
                    if (isCorrect)
                        _availableValuesToStealEnemyPoints = Array.ConvertAll(splittedValues, item => int.Parse(item)).ToList();
                    else
                        _availableValuesToStealEnemyPoints = new List<int> { 2, 3 };
                }
            }

            if (player1 == null || player2 == null)
                throw new ArgumentNullException("One of the players is null");
            if (initialValues.Length != CellsCount * 2)
                throw new ArgumentException("Initial values array size is incorrect");
            if (!firstPlayer.Equals(player1) && !firstPlayer.Equals(player2))
                throw new ArgumentException("First player is incorrect");
            this.Player1 = player1;
            this.Player2 = player2;
            CurrentPlayer = firstPlayer;
            undoMovesHistory = new Stack<Move>();
            redoMovesHistory = new Stack<Move>();
            initialValue = CreateField(initialValues, endingCellPlayer1Value, endingCellPlayer2Value) / 2 / CellsCount;
            GameEnded = false;
        }

        public object Clone()
        {
            int[] initialValues = new int[CellsCount * 2];
            for (int i = 0; i < CellsCount; i++)
                initialValues[i] = field[i].Value;
            for (int i = 0; i < CellsCount; i++)
                initialValues[CellsCount + i] = field[CellsCount + 1 + i].Value;
            GameLogic logicClone = new GameLogic(Player1, Player2, CurrentPlayer, initialValues, field[CellsCount].Value, field[CellsCount * 2 + 1].Value);
            return logicClone;
        }

        private int GetCellValue(IPlayer player, int cellIndex)
        {
            if (player == null)
                throw new ArgumentNullException("Passed player is null");
            if (cellIndex < 0 || cellIndex > CellsCount)
                throw new ArgumentOutOfRangeException(nameof(cellIndex), "Param value is outside of the range");
            int indexOnField = player.Equals(Player1) ? cellIndex : CellsCount + 1 + cellIndex;
            return field[indexOnField].Value;
        }

        public int this[string player, int cellIndex]
        {
            get
            {
                IPlayer pl;
                switch (player)
                {
                    case "Player1":
                        pl = Player1;
                        break;
                    case "Player2":
                        pl = Player2;
                        break;
                    default: throw new ArgumentException(nameof(player));
                }
                return GetCellValue(pl, cellIndex);
            }
        }

        public int this[IPlayer player, int cellIndex] => GetCellValue(player, cellIndex);

        public MoveResult MakeMove(IPlayer player, int cellIndex)
        {
            if (!player.Equals(CurrentPlayer))
                return MoveResult.ImpossibleMove;
            if (cellIndex < 0 || cellIndex >= CellsCount)
                return MoveResult.ImpossibleMove;
            if (!CheckMovePossible())
            {
                ClearAllEnemyNonEndingCellsOnGameEnd();
                return MoveResult.GameEnded;
            }
            int indexOnField = player.Equals(Player1) ? cellIndex : CellsCount + 1 + cellIndex;
            if (field[indexOnField].Value == 0)
                return MoveResult.ImpossibleMove;

            int[] gameFieldCopy = GetFieldValuesCopy();

            (MoveResult moveResult, int lastCellNumber) result = MakeSingleMove(player, indexOnField);
            while (result.moveResult == MoveResult.ContinuousMove)
            {
                indexOnField = player.Equals(Player1) ? result.lastCellNumber : CellsCount + 1 + result.lastCellNumber;
                result = MakeSingleMove(player, indexOnField);
            }

            List<KeyValuePair<Cell, int>> cellValuesChanges = GetFieldValuesChanges(gameFieldCopy);
            Move madeMove = new Move(CurrentPlayer, cellValuesChanges, result.moveResult);
            undoMovesHistory.Push(madeMove);
            redoMovesHistory.Clear();

            if (CheckGameEnding())
            {
                GameEnded = true;
                return MoveResult.GameEnded;
            }
            CurrentPlayer = GetOtherPlayer(CurrentPlayer);
            return MoveResult.EndedMove;
        }

        public bool UndoMove()
        {
            if (!CurrentPlayer.CanUndoMoves)
                return false;
            if (undoMovesHistory.Count == 0)
                return false;
            Move lastMove = undoMovesHistory.Pop();
            redoMovesHistory.Push(lastMove);
            UndoCellsValues(lastMove.CellValuesChanges);
            CurrentPlayer = lastMove.MoveOwner;
            if (!CurrentPlayer.CanUndoMoves)
            {
                if (undoMovesHistory.Count == 0)
                {
                    lastMove = redoMovesHistory.Pop();
                    RedoCellsValues(lastMove.CellValuesChanges);
                    CurrentPlayer = GetOtherPlayer(lastMove.MoveOwner);
                    return false;
                }
                lastMove = undoMovesHistory.Pop();
                redoMovesHistory.Push(lastMove);
                UndoCellsValues(lastMove.CellValuesChanges);
                CurrentPlayer = lastMove.MoveOwner;
            }
            GameEnded = false;
            return true;
        }

        public bool RedoMove()
        {
            if (!CurrentPlayer.CanUndoMoves)
                return false;
            if (redoMovesHistory.Count == 0)
                return false;
            Move lastMove = redoMovesHistory.Pop();
            undoMovesHistory.Push(lastMove);
            RedoCellsValues(lastMove.CellValuesChanges);
            CurrentPlayer = GetOtherPlayer(lastMove.MoveOwner);
            if (!CurrentPlayer.CanUndoMoves)
            {
                if (redoMovesHistory.Count == 0)
                    throw new Exception("Something is really wrong here");
                lastMove = redoMovesHistory.Pop();
                undoMovesHistory.Push(lastMove);
                RedoCellsValues(lastMove.CellValuesChanges);
                CurrentPlayer = GetOtherPlayer(lastMove.MoveOwner);
            }
            if (lastMove.Result == MoveResult.GameEnded)
                GameEnded = true;
            return true;
        }

        #region Serialization
        private class SerializableGameLogic
        {
            public int CellsCount { get; set; }
            public List<Cell> Field { get; set; }
            public Stack<Move> UndoMovesHistory { get; set; }
            public Stack<Move> RedoMovesHistory { get; set; }
            public IPlayer Player1 { get; set; }
            public IPlayer Player2 { get; set; }
            public IPlayer CurrentPlayer { get; set; }
            public int InitialValue { get; set; }
            public List<int> AvailableValuesToStealEnemyPoints { get; set; }

        }
        public string Serialize()
        {
            var settings = new JsonSerializerSettings {TypeNameHandling = TypeNameHandling.Objects};
            return JsonConvert.SerializeObject(new SerializableGameLogic
            {
                CellsCount = CellsCount,
                Field = field,
                UndoMovesHistory = undoMovesHistory,
                RedoMovesHistory = redoMovesHistory,
                Player1 = Player1,
                Player2 = Player2,
                CurrentPlayer = CurrentPlayer,
                InitialValue = initialValue,
                AvailableValuesToStealEnemyPoints = _availableValuesToStealEnemyPoints
            }, settings);
        }

        public static GameLogic Deserialize(string data)
        {
            var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects };
            var result = JsonConvert.DeserializeObject<SerializableGameLogic>(data, settings);
            return new GameLogic(result);
        }

        //Private constructor for deserialization
        private GameLogic(SerializableGameLogic obj)
        {
            CellsCount = obj.CellsCount;
            field = obj.Field;
            undoMovesHistory = obj.UndoMovesHistory;
            redoMovesHistory = obj.RedoMovesHistory;
            Player1 = obj.Player1;
            Player2 = obj.Player2;
            CurrentPlayer = obj.CurrentPlayer;
            initialValue = obj.InitialValue;
            _availableValuesToStealEnemyPoints = obj.AvailableValuesToStealEnemyPoints;
        }
        #endregion

        [Obsolete]
        private int CreateField(int[] initialvalues, int endingCellPlayer1Value, int endingCellPlayer2Value)
        {
            field = new List<Cell>();
            int counter = 0;
            for (int i = 0; i < CellsCount; i++)
            {
                field.Add(new Cell() { Owner = Player1, Value = initialvalues[counter], IsEndingCell = false, Number = i });
                counter++;
            }
            field.Add(new Cell() { Owner = Player1, Value = endingCellPlayer1Value, IsEndingCell = true, Number = CellsCount });
            for (int i = 0; i < CellsCount; i++)
            {
                field.Add(new Cell() { Owner = Player2, Value = initialvalues[counter], IsEndingCell = false, Number = i });
                counter++;
            }
            field.Add(new Cell() { Owner = Player2, Value = endingCellPlayer2Value, IsEndingCell = true, Number = CellsCount });
            return initialvalues.Sum();
        }

        private void CreateField(int initialValue)
        {
            field = new List<Cell>();
            for (int i = 0; i < CellsCount; i++)
            {
                field.Add(new Cell() { Owner = Player1, Value = initialValue, IsEndingCell = false, Number = i });
            }
            field.Add(new Cell() { Owner = Player1, Value = 0, IsEndingCell = true, Number = CellsCount });
            for (int i = 0; i < CellsCount; i++)
            {
                field.Add(new Cell() { Owner = Player2, Value = initialValue, IsEndingCell = false, Number = i });
            }
            field.Add(new Cell() { Owner = Player2, Value = 0, IsEndingCell = true, Number = CellsCount });
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
            if (endedOnEnemyCell && _availableValuesToStealEnemyPoints.Contains(lastCell.Value))
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
            int cellToStartSearching = CurrentPlayer.Equals(Player1) ? 0 : CellsCount + 1;
            for (int i = 0; i < CellsCount; i++)
                if (field[cellToStartSearching + i].Value != 0)
                    return true;
            return false;
        }

        private void ClearAllEnemyNonEndingCellsOnGameEnd()
        {
            int cellToStartCleaning = CurrentPlayer.Equals(Player1) ? CellsCount + 1 : 0;
            int targetEngingCell = CurrentPlayer.Equals(Player1) ? CellsCount * 2 + 1 : CellsCount;
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
            if (currentPlayer.Equals(Player1))
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
            while (haveFreeCells && _availableValuesToStealEnemyPoints.Contains(field[endedCellIndexOnField].Value))
            {
                field[targetEndingCellIndexOnField].Value += field[endedCellIndexOnField].Value;
                field[endedCellIndexOnField].Value = 0;
                endedCellIndexOnField--;
                haveFreeCells = endedCellIndexOnField >= 0 && !field[endedCellIndexOnField].IsEndingCell;
            }
        }

        private int[] GetFieldValuesCopy()
        {
            int[] copy = new int[field.Count];
            for (int i = 0; i < field.Count; i++)
                copy[i] = field[i].Value;
            return copy;
        }

        private List<KeyValuePair<Cell, int>> GetFieldValuesChanges(int[] previousFieldValues)
        {
            List<KeyValuePair<Cell, int>> fieldValuesChanges = new List<KeyValuePair<Cell, int>>();
            for (int i = 0; i < field.Count; i++)
                if (previousFieldValues[i] != field[i].Value)
                    fieldValuesChanges.Add(new KeyValuePair<Cell, int>(field[i],
                        field[i].Value - previousFieldValues[i]));
            return fieldValuesChanges;
        }

        private void UndoCellsValues(List<KeyValuePair<Cell, int>> deltaValues)
        {
            foreach (var keyValuePair in deltaValues)
                keyValuePair.Key.Value -= keyValuePair.Value;
        }

        private void RedoCellsValues(List<KeyValuePair<Cell, int>> deltaValues)
        {
            foreach (var keyValuePair in deltaValues)
                keyValuePair.Key.Value += keyValuePair.Value;
        }
    }
}
