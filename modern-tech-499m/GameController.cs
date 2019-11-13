using modern_tech_499m.Logic;
using modern_tech_499m.AILogic;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace modern_tech_499m
{
    class GameController : INotifyPropertyChanged
    {
        public GameLogic GameLogic { get; private set; }
        private bool _gameStopPending;

        //private Action<string> updateField;
        //private Action<IPlayer> showGameEnding;
        //private Action<IPlayer> showPlayerWorkingMessage, stopPlayerWorkingMessage;

        public GameController(GameLogic gameLogic/*, Action<string> updateField,
            Action<IPlayer> showGameEnding, Action<IPlayer> showPlayerWorkingMessage, Action<IPlayer> stopPlayerWorkingMessage*/)
        {
            this.GameLogic = gameLogic;
            //this.updateField = updateField;
            //this.showGameEnding = showGameEnding;
            //this.showPlayerWorkingMessage = showPlayerWorkingMessage;
            //this.stopPlayerWorkingMessage = stopPlayerWorkingMessage;
            this.GameLogic.Player1.OnGetCell += new EventHandler<CellGetterEventArgs>(RecieveCellNumber);
            this.GameLogic.Player2.OnGetCell += new EventHandler<CellGetterEventArgs>(RecieveCellNumber);
        }

        private string _lastStatus;
        public string LastStatus
        {
            get => _lastStatus;
            set
            {
                _lastStatus = value;
                OnPropertyChanged();
            }
        }

        private string _currentPlayerInfo;
        public string CurrentPlayerInfo
        {
            get => _currentPlayerInfo;
            set
            {
                _currentPlayerInfo = value;
                OnPropertyChanged();
            }
        }

        public void RunGame()
        {
            _gameStopPending = false;
            LastStatus = "Game starting";
            MakeGameStep();
        }

        public void StopGame()
        {
            _gameStopPending = true;
        }

        public void UndoMove()
        {
            if (GameLogic.UndoMove())
            {
                LastStatus = "Undo";
                //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                OnPropertyChanged(nameof(GameLogic));
            }
            else
                LastStatus = "Can't undo";
        }

        public void RedoMove()
        {
            if (GameLogic.RedoMove())
            {
                LastStatus = "Redo";
                //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                OnPropertyChanged(nameof(GameLogic));
            }
            else
                LastStatus = "Can't redo";
        }

        private void MakeGameStep()
        {
            CurrentPlayerInfo = $"Waiting for player '{GameLogic.CurrentPlayer.Name}' to make move";
            //showPlayerWorkingMessage(gameLogic.CurrentPlayer);
            GameLogic.CurrentPlayer.GetCell(GameLogic);
        }

        private void RecieveCellNumber(object sender, CellGetterEventArgs eventArgs)
        {
            MoveResult moveResult = GameLogic.MakeMove(sender as IPlayer, eventArgs.CellNumber);
            LastStatus = Enum.GetName(typeof(MoveResult), moveResult);
            if (moveResult == MoveResult.GameEnded)
            {
                //TODO - Show message box via service
                //showGameEnding(sender as IPlayer);
                CurrentPlayerInfo = "Game ended";
                return;
            }
            if (_gameStopPending)
            {
                LastStatus = "Game interrupted";
                return;
            }
            if (moveResult != MoveResult.ImpossibleMove)
            {
                CurrentPlayerInfo = string.Empty;
                //stopPlayerWorkingMessage(sender as IPlayer);
            }
            MakeGameStep();

            //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            OnPropertyChanged(nameof(GameLogic));
        }

        #region INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}