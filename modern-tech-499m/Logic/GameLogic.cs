using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Newtonsoft.Json;
using NLog;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("modern_tech_499m.Tests")]
namespace modern_tech_499m.Logic
{
    public class GameLogic : ICloneable
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        public readonly int CellsCount;
        private List<Cell> field;
        private Stack<Move> _undoMovesHistory;
        private Stack<Move> _redoMovesHistory;

        public IPlayer Player1 { get; }
        public IPlayer Player2 { get; }
        public IPlayer CurrentPlayer { get; private set; }
        public bool GameEnded { get; private set; }
        public int Score => field[CellsCount].Value - field[field.Count - 1].Value;
        private int _initialValue;
        private List<int> _availableValuesToStealEnemyPoints;

        public IPlayer GetOtherPlayer(IPlayer player)
        {
            if (!player.Equals(Player1) && !player.Equals(Player2))
                throw new ArgumentException("Invalid player", nameof(player));
            return player.Equals(Player1) ? Player2 : Player1;
        }

        public GameLogic(int initialValue, IPlayer player1, IPlayer player2, IPlayer firstPlayer, bool useDefaultSettings = true)
        {
            _logger.Debug($"Game logic main constructor called with{(useDefaultSettings ? "" : " no")} default settings");
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
            {
                var exception = new ArgumentNullException("One of the players is null");
                _logger.Fatal(exception, "Exception in game logic constructor");
                throw exception;
            }
            if (!firstPlayer.Equals(player1) && !firstPlayer.Equals(player2))
            {
                var exception = new ArgumentException("First player is incorrect");
                _logger.Fatal(exception, "Exception in game logic constructor");
                throw exception;
            }
            Player1 = player1;
            Player2 = player2;
            CurrentPlayer = firstPlayer;
            _undoMovesHistory = new Stack<Move>();
            _redoMovesHistory = new Stack<Move>();
            this._initialValue = initialValue;
            GameEnded = false;
            CreateField(initialValue);
        }

        [Obsolete]
        public GameLogic(IPlayer player1, IPlayer player2, IPlayer firstPlayer, int[] initialValues, int endingCellPlayer1Value, int endingCellPlayer2Value, bool useDefaultSettings = true)
        {
            _logger.Debug($"Game logic obsolete constructor called with{(useDefaultSettings ? "" : " no")} default settings");
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
            {
                var exception = new ArgumentNullException("One of the players is null");
                _logger.Fatal(exception, "Exception in game logic constructor");
                throw exception;
            }
            if (initialValues.Length != CellsCount * 2)
            {
                var exception = new ArgumentException("Initial values array size is incorrect");
                _logger.Fatal(exception, "Exception in game logic constructor");
                throw exception;
            }
            if (!firstPlayer.Equals(player1) && !firstPlayer.Equals(player2))
            {
                var exception = new ArgumentException("First player is incorrect");
                _logger.Fatal(exception, "Exception in game logic constructor");
                throw exception;
            }
            Player1 = player1;
            Player2 = player2;
            CurrentPlayer = firstPlayer;
            _undoMovesHistory = new Stack<Move>();
            _redoMovesHistory = new Stack<Move>();
            _initialValue = CreateField(initialValues, endingCellPlayer1Value, endingCellPlayer2Value) / 2 / CellsCount;
            GameEnded = false;
        }

        public object Clone()
        {
            _logger.Debug("Game logic clone method called");
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
                    default:
                        {
                            var exception = new ArgumentException(nameof(player));
                            _logger.Fatal(exception, "Cell value getter called");
                            throw exception;
                        }
                }
                return GetCellValue(pl, cellIndex);
            }
        }

        public int this[IPlayer player, int cellIndex] => GetCellValue(player, cellIndex);

        public MoveResult MakeMove(IPlayer player, int cellIndex)
        {
            _logger.Debug($"Make move called with parameters {player} {cellIndex}");
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
            _undoMovesHistory.Push(madeMove);
            _redoMovesHistory.Clear();

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
            _logger.Debug("Undo move called");
            if (!CurrentPlayer.CanUndoMoves)
                return false;
            if (_undoMovesHistory.Count == 0)
                return false;
            Move lastMove = _undoMovesHistory.Pop();
            _redoMovesHistory.Push(lastMove);
            UndoCellsValues(lastMove.CellValuesChanges);
            CurrentPlayer = lastMove.MoveOwner;
            if (!CurrentPlayer.CanUndoMoves)
            {
                if (_undoMovesHistory.Count == 0)
                {
                    lastMove = _redoMovesHistory.Pop();
                    RedoCellsValues(lastMove.CellValuesChanges);
                    CurrentPlayer = GetOtherPlayer(lastMove.MoveOwner);
                    return false;
                }
                lastMove = _undoMovesHistory.Pop();
                _redoMovesHistory.Push(lastMove);
                UndoCellsValues(lastMove.CellValuesChanges);
                CurrentPlayer = lastMove.MoveOwner;
            }
            GameEnded = false;
            return true;
        }

        public bool RedoMove()
        {
            _logger.Debug("Redo move called");
            if (!CurrentPlayer.CanUndoMoves)
                return false;
            if (_redoMovesHistory.Count == 0)
                return false;
            Move lastMove = _redoMovesHistory.Pop();
            _undoMovesHistory.Push(lastMove);
            RedoCellsValues(lastMove.CellValuesChanges);
            CurrentPlayer = GetOtherPlayer(lastMove.MoveOwner);
            if (!CurrentPlayer.CanUndoMoves)
            {
                if (_redoMovesHistory.Count == 0)
                    throw new Exception("Something is really wrong here");
                lastMove = _redoMovesHistory.Pop();
                _undoMovesHistory.Push(lastMove);
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
            _logger.Debug("Game logic Serialize method called");
            var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects };
            return JsonConvert.SerializeObject(new SerializableGameLogic
            {
                CellsCount = CellsCount,
                Field = field,
                UndoMovesHistory = _undoMovesHistory,
                RedoMovesHistory = _redoMovesHistory,
                Player1 = Player1,
                Player2 = Player2,
                CurrentPlayer = CurrentPlayer,
                InitialValue = _initialValue,
                AvailableValuesToStealEnemyPoints = _availableValuesToStealEnemyPoints
            }, settings);
        }

        public static GameLogic Deserialize(string data)
        {
            var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects };
            var result = JsonConvert.DeserializeObject<SerializableGameLogic>(data, settings);

            //Reversing redoMovesHistory and UndoMovesHistory stacks after deserialization
            result.UndoMovesHistory = new Stack<Move>(result.UndoMovesHistory.ToList());
            result.RedoMovesHistory = new Stack<Move>(result.RedoMovesHistory.ToList());
            return new GameLogic(result);
        }

        //Private constructor for deserialization
        private GameLogic(SerializableGameLogic obj)
        {
            CellsCount = obj.CellsCount;
            field = obj.Field;
            _undoMovesHistory = obj.UndoMovesHistory;
            _redoMovesHistory = obj.RedoMovesHistory;
            Player1 = obj.Player1;
            Player2 = obj.Player2;
            CurrentPlayer = obj.CurrentPlayer;
            _initialValue = obj.InitialValue;
            _availableValuesToStealEnemyPoints = obj.AvailableValuesToStealEnemyPoints;
        }
        #endregion

        [Obsolete]
        private int CreateField(int[] initialvalues, int endingCellPlayer1Value, int endingCellPlayer2Value)
        {
            _logger.Debug("Create field from mjltiple values method called");
            field = new List<Cell>();
            int counter = 0;
            for (int i = 0; i < CellsCount; i++)
            {
                field.Add(new Cell(Player1, i) { Value = initialvalues[counter], IsEndingCell = false });
                counter++;
            }
            field.Add(new Cell(Player1, CellsCount) { Value = endingCellPlayer1Value, IsEndingCell = true });
            for (int i = 0; i < CellsCount; i++)
            {
                field.Add(new Cell(Player2, i) { Value = initialvalues[counter], IsEndingCell = false });
                counter++;
            }

            field.Add(new Cell(Player2, CellsCount) { Value = endingCellPlayer2Value, IsEndingCell = true });
            return initialvalues.Sum();
        }

        private void CreateField(int initialValue)
        {
            _logger.Debug("Create field from single value method called");
            field = new List<Cell>();
            for (int i = 0; i < CellsCount; i++)
            {
                field.Add(new Cell(Player1, i) { Value = initialValue, IsEndingCell = false });
            }
            field.Add(new Cell(Player1, CellsCount) { Value = 0, IsEndingCell = true });
            for (int i = 0; i < CellsCount; i++)
            {
                field.Add(new Cell(Player2, i) { Value = initialValue, IsEndingCell = false });
            }
            field.Add(new Cell(Player2, CellsCount) { Value = 0, IsEndingCell = true });
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
                if (!cell.Owner.Equals(player) && cell.IsEndingCell)
                    continue;
                if (!passedEnemyCell && !cell.Owner.Equals(player) && !cell.IsEndingCell)
                    passedEnemyCell = true;
                lastCell = cell;
                cell.Value++;
                value--;
            }
            bool endedOnPlayerCell = lastCell.Owner.Equals(player) && !lastCell.IsEndingCell;
            bool endedOnEnemyCell = !lastCell.Owner.Equals(player) && !lastCell.IsEndingCell;
            if (passedEnemyCell && endedOnPlayerCell && lastCell.Value > 1)
                return (MoveResult.ContinuousMove, lastCell.Number);
            if (endedOnEnemyCell && _availableValuesToStealEnemyPoints.Contains(lastCell.Value))
                StealEnemyPoints(player, lastCell.Number);
            return (MoveResult.EndedMove, lastCell.Number);
        }

        private bool CheckGameEnding()
        {
            if (field[CellsCount].Value >= _initialValue * CellsCount)
                return true;
            if (field[CellsCount * 2 + 1].Value >= _initialValue * CellsCount)
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
            for (int i = 0; i < CellsCount; i++)
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
            {
                foreach (var cell in field)
                {
                    if (keyValuePair.Key.Equals(cell))
                    {
                        cell.Value -= keyValuePair.Value;
                        break;
                    }
                }
            }
        }

        private void RedoCellsValues(List<KeyValuePair<Cell, int>> deltaValues)
        {
            foreach (var keyValuePair in deltaValues)
            {
                foreach (var cell in field)
                {
                    if (keyValuePair.Key.Equals(cell))
                    {
                        cell.Value += keyValuePair.Value;
                        break;
                    }
                }
            }
        }
    }
}
