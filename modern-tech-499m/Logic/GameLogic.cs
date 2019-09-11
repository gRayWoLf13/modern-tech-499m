using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("LogicTests")]
namespace modern_tech_499m.Logic
{
    class GameLogic
    {
        const int CellsCount = 6;

        List<Cell> Field;
        IPlayer Player1, Player2, CurrentPlayer;
        int InitialValue;

        public GameLogic(int initialValue, IPlayer player1, IPlayer player2)
        {
            Player1 = player1;
            CurrentPlayer = Player2 = player2;
            InitialValue = initialValue;
            CreateField(initialValue);
        }

        public GameLogic(IPlayer player1, IPlayer player2, params int[] initialValues)
        {
            CurrentPlayer = Player1 = player1;
            Player2 = player2;
            InitialValue = CreateField(initialValues) / 2 / CellsCount;
        }

        int CreateField(params int[] initialvalues)
        {
            Field = new List<Cell>();
            int counter = 0;
            for (int i = 0; i < CellsCount; i++)
                Field.Add(new Cell() { Owner = Player1, Value = initialvalues[counter++], IsEndingCell = false, Number = i });
            Field.Add(new Cell() { Owner = Player1, Value = 0, IsEndingCell = true, Number = CellsCount });
            for (int i = 0; i < CellsCount; i++)
                Field.Add(new Cell() { Owner = Player2, Value = initialvalues[counter++], IsEndingCell = false, Number = i });
            Field.Add(new Cell() { Owner = Player2, Value = 0, IsEndingCell = true, Number = CellsCount });
            int sum = 0;
            for (int i = 0; i < counter; sum += initialvalues[i++]) ;
            return sum;
        }

        void CreateField(int initialValue)
        {
            Field = new List<Cell>();
            for (int i = 0; i < CellsCount; i++)
                Field.Add(new Cell() { Owner = Player1, Value = initialValue, IsEndingCell = false, Number = i });
            Field.Add(new Cell() { Owner = Player1, Value = 0, IsEndingCell = true, Number = CellsCount });
            for (int i = 0; i < CellsCount; i++)
                Field.Add(new Cell() { Owner = Player2, Value = initialValue, IsEndingCell = false, Number = i });
            Field.Add(new Cell() { Owner = Player2, Value = 0, IsEndingCell = true, Number = CellsCount });
        }

        public MoveResult MakeMove(IPlayer player, int cellIndex)
        {
            if (player != CurrentPlayer)
                return MoveResult.ImpossibleMove;
            if (cellIndex < 0 || cellIndex >= 6)
                return MoveResult.ImpossibleMove;
            int indexOnField = player.Equals(Player1) ? cellIndex : CellsCount + 1 + cellIndex;
            if (Field[indexOnField].Value == 0)
                return MoveResult.ImpossibleMove;
            var result = MakeSingleMove(player, indexOnField);
            while (result.moveResult == MoveResult.ContinuousMove)
            {
                indexOnField = player.Equals(Player1) ? result.lastCellNumber : CellsCount + 1 + result.lastCellNumber;
                result = MakeSingleMove(player, indexOnField);
            }
            if (CheckGameEnding())
                return MoveResult.GameEnded;
            CurrentPlayer = CurrentPlayer.Equals(Player1) ? Player2 : Player1;
            return MoveResult.EndedMove;
        }

        (MoveResult moveResult, int lastCellNumber) MakeSingleMove(IPlayer player, int indexOnField)
        {
            bool passedEnemyCell = false;
            bool endedOnPlayerCell = true;
            bool endedOnEnemyCell = false;
            int lastCellNumber = Field[indexOnField].Number;
            int value = Field[indexOnField].Value;
            Field[indexOnField].Value = 0;
            foreach (var cell in Field.Cycle(indexOnField + 1))
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
                int lastCellIndexInField;
                if (player.Equals(Player1))
                    lastCellIndexInField = lastCellNumber + CellsCount + 1;
                else
                    lastCellIndexInField = lastCellNumber;
                if (Field[lastCellIndexInField].Value == 2 || Field[lastCellIndexInField].Value == 3)
                    StealEnemyPoints(player, lastCellNumber);
            }
            return (MoveResult.EndedMove, lastCellNumber);
        }

        bool CheckGameEnding()
        {
            if (Field[CellsCount].Value >= InitialValue * CellsCount)
                return true;
            if (Field.Last().Value >= InitialValue * CellsCount)
                return true;
            return false;
        }

        void StealEnemyPoints(IPlayer currentPlayer, int endedCellNumber)
        {
            int endedCellIndexOnField;
            int targetEndingCellIndexOfField;
            if (currentPlayer.Equals(Player1))
            {
                endedCellIndexOnField = endedCellNumber + CellsCount + 1;
                targetEndingCellIndexOfField = CellsCount;
            }
            else
            {
                endedCellIndexOnField = endedCellNumber;
                targetEndingCellIndexOfField = Field.Count - 1;
            }
            while (endedCellIndexOnField >= 0 && !Field[endedCellIndexOnField].IsEndingCell && (Field[endedCellIndexOnField].Value == 2 || Field[endedCellIndexOnField].Value == 3))
            {
                Field[targetEndingCellIndexOfField].Value += Field[endedCellIndexOnField].Value;
                Field[endedCellIndexOnField].Value = 0;
                endedCellIndexOnField--;
            }
        }
    }
}
