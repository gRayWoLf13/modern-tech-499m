using modern_tech_499m.Logic;
using modern_tech_499m.AILogic;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using modern_tech_499m.Repositories.Core.Domain;
using modern_tech_499m.Repositories.Core.Repositories;

namespace modern_tech_499m
{
    class GameController : INotifyPropertyChanged
    {
        public GameLogic GameLogic { get; private set; }
        private readonly IGameInfoRepository _gameInfoRepository;
        private bool _gameStopPending;

        public GameController(GameLogic gameLogic, IGameInfoRepository gameInfoRepository)
        {
            _gameInfoRepository = gameInfoRepository;
            GameLogic = gameLogic;
            GameLogic.Player1.OnGetCell += RecieveCellNumber;
            GameLogic.Player2.OnGetCell += RecieveCellNumber;
        }

        public GameController(GameInfo info, IGameInfoRepository repository)
        {
            _gameInfoRepository = repository;
            LoadGame(info);
            GameLogic.Player1.OnGetCell += RecieveCellNumber;
            GameLogic.Player2.OnGetCell += RecieveCellNumber;
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
                OnPropertyChanged(nameof(GameLogic));
            }
            else
                LastStatus = "Can't redo";
        }

        public void SaveGame()
        {
            _gameInfoRepository.Add(new GameInfo
            {
                GameDate = DateTime.Now,
                GameFinished = GameLogic.GameEnded,
                InternalGameData = GameLogic.Serialize(),
                Player1Id = GameLogic.Player1.Id,
                Player2Id = GameLogic.Player2.Id,
                Score = GameLogic.Score
            });
        }

        private void LoadGame(GameInfo info)
        {
            GameLogic = GameLogic.Deserialize(info.InternalGameData);
        }

        private void MakeGameStep()
        {
            CurrentPlayerInfo = $"Waiting for player '{GameLogic.CurrentPlayer.Name}' to make move";
            GameLogic.CurrentPlayer.GetCell(GameLogic);
        }

        private void RecieveCellNumber(object sender, CellGetterEventArgs eventArgs)
        {
            MoveResult moveResult = GameLogic.MakeMove(sender as IPlayer, eventArgs.CellNumber);
            LastStatus = Enum.GetName(typeof(MoveResult), moveResult);
            if (moveResult == MoveResult.GameEnded)
            {
                //TODO - Show message box via service
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
            }
            MakeGameStep();
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