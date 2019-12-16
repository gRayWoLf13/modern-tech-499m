using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using modern_tech_499m.Commands;
using modern_tech_499m.Commands.MainWindowViewModelCommands;
using modern_tech_499m.Logic;
using modern_tech_499m.Repositories.Core.Repositories;
using modern_tech_499m.ViewModels.Base;
using NLog;
using PropertyChanged;

namespace modern_tech_499m.ViewModels
{
    public class GamePageViewModel : BaseViewModel
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        public IGameInfoRepository GameInfoRepository { get; }
        public GamePageViewModel(IGameInfoRepository gameInfoRepository)
        {
            GameController = new GameController(gameInfoRepository.GetAll().First(), gameInfoRepository);

            _logger.Debug("Main window view model constructor called");
            GameInfoRepository = gameInfoRepository;

            CellClickCommand = new RelayParameterizedCommand(parameter =>
            {
                try
                {
                    var values = (object[])parameter;
                    IPlayer player = (IPlayer)values[0];
                    int cellIndex = Convert.ToInt32(values[1]);
                    _logger.Debug($"Cell click command called with parameters {player}, {cellIndex}");
                    (player as UserPlayer)?.MakeMove(cellIndex);
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Cell click command caused the exception");
                }
            }, parameter => GameController != null && Player1 != null && Player2 != null);

            StartNewGameCommand = new RelayParameterizedCommand(parameter =>
            {
                _logger.Debug("Start new game command called");
                GameLogic logic = new GameLogic(6, Player1, Player2, Player1);
                GameController?.StopGame();
                GameController = new GameController(logic, GameInfoRepository);
                GameController.RunGame();
            }, parameter => Player1 != null && Player2 != null);

            UndoRedoMoveCommand = new RelayParameterizedCommand(parameter =>
            {
                var param = Convert.ToBoolean(parameter);
                _logger.Debug($"Undo/redo mode command called with parameter {param}");
                if (param)
                    GameController?.UndoMove();
                else
                    GameController?.RedoMove();
            }, parameter => GameController != null && Player1 != null && Player2 != null);

            SelectPlayerCommand = new RelayParameterizedCommand(parameter =>
            {
                var param = parameter as string;
                _logger.Debug($"Select player command called with parameter {param}");
                switch (param)
                {
                    case "Player1":
                        Player1 = Services.SelectPlayer();
                        break;
                    case "Player2":
                        Player2 = Services.SelectPlayer();
                        break;
                    default:
                        {
                            var exception = new ArgumentException("Wrong player", nameof(parameter));
                            _logger.Fatal(exception, "Select player command caused an exception");
                            throw exception;
                        }
                }
            }, parameter => true);

            SaveGameCommand = new RelayParameterizedCommand(parameter =>
            {
                _logger.Debug("Save game command called");
                GameController.SaveGame();
            }, parameter => GameController != null && Player1 != null && Player2 != null);
        }

        private GameController _gameController;

        [DoNotNotify]
        public GameController GameController
        {
            get => _gameController;
            set
            {
                _gameController = value;
                SyncInfoWithLoadedGameLogic();
                OnPropertyChanged();
            }
        }

        public IPlayer Player1 { get; set; }

        public IPlayer Player2 { get; set; }

        public ICommand CellClickCommand { get; }
        public ICommand StartNewGameCommand { get; }
        public ICommand UndoRedoMoveCommand { get; }
        public ICommand SelectPlayerCommand { get; }
        public ICommand SaveGameCommand { get; }

        private void SyncInfoWithLoadedGameLogic()
        {
            Player1 = GameController.GameLogic.Player1;
            Player2 = GameController.GameLogic.Player2;
        }
    }
}
