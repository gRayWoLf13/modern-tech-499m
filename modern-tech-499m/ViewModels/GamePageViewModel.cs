using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using modern_tech_499m.Commands;
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
            //GameController = new GameController(gameInfoRepository.GetAll().First(), gameInfoRepository);

            _logger.Debug("Main window view model constructor called");
            GameInfoRepository = gameInfoRepository;

            CellClickCommand = new RelayParameterizedCommand(param => CellClick(param), param => CellClickCanExecute);

            StartNewGameCommand =
                new RelayParameterizedCommand(param => StartNewGame(), param => StartNewGameCanExecute);

            UndoRedoMoveCommand = new RelayParameterizedCommand(param => UndoRedo(param), param => UndoRedoCanExecute);
            SelectPlayerCommand =
                new RelayParameterizedCommand(param => SelectPlayer(param), param => SelectPlayerCanExecute);

            SaveGameCommand = new RelayParameterizedCommand(param => SaveGame(), param => SaveGameCanExecute);
        }

        #region Public properties

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

        /// <summary>
        /// First player of the game
        /// </summary>
        public IPlayer Player1 { get; set; }

        /// <summary>
        /// Second player of the game
        /// </summary>
        public IPlayer Player2 { get; set; }

        /// <summary>
        /// Action that will be called from another page to set current page new needed player
        /// </summary>
        [DoNotNotify]
        public Action<IPlayer> CurrentPlayerLoggingAction { get; private set; }

        public ICommand CellClickCommand { get; }
        public ICommand StartNewGameCommand { get; }
        public ICommand UndoRedoMoveCommand { get; }
        public ICommand SelectPlayerCommand { get; }
        public ICommand SaveGameCommand { get; }

        #endregion


        #region Private members

        /// <summary>
        /// Private instance of class that controls the game logic
        /// </summary>
        private GameController _gameController;

        #endregion

        #region Private methods

        private void SyncInfoWithLoadedGameLogic()
        {
            Player1 = GameController.GameLogic.Player1;
            Player2 = GameController.GameLogic.Player2;
        }

        /// <summary>
        /// Method to call from cell click command
        /// </summary>
        /// <param name="parameter">Parameter passed from command</param>
        private void CellClick(object parameter)
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
        }

        /// <summary>
        /// Cell click command can execute condition
        /// </summary>
        private bool CellClickCanExecute => GameController != null && Player1 != null && Player2 != null;

        /// <summary>
        /// Method to call fom start new game command
        /// </summary>
        private void StartNewGame()
        {
            _logger.Debug("Start new game command called");
            GameLogic logic = new GameLogic(6, Player1, Player2, Player1);
            GameController?.StopGame();
            GameController = new GameController(logic, GameInfoRepository);
            GameController.RunGame();
        }

        /// <summary>
        /// Start new game command can execute condition
        /// </summary>
        private bool StartNewGameCanExecute => Player1 != null && Player2 != null;

        /// <summary>
        /// Method to call from undo/redo command
        /// </summary>
        /// <param name="parameter"></param>
        private void UndoRedo(object parameter)
        {
            var param = Convert.ToBoolean(parameter);
            _logger.Debug($"Undo/redo mode command called with parameter {param}");
            if (param)
                GameController?.UndoMove();
            else
                GameController?.RedoMove();
        }

        /// <summary>
        /// Undo/redo command can execute condition
        /// </summary>
        private bool UndoRedoCanExecute => GameController != null && Player1 != null && Player2 != null;

        /// <summary>
        /// Method to call from select player command
        /// </summary>
        /// <param name="parameter"></param>
        private void SelectPlayer(object parameter)
        {
            var param = parameter as string;
            _logger.Debug($"Select player command called with parameter {param}");
            switch (param)
            {
                case "Player1":
                    //Setting action that will be called from login page when player will be loaded
                    CurrentPlayerLoggingAction = player => Player1 = player;
                    //Navigating to login page with current viewmodel navigation source
                    ViewModelLocator.ApplicationViewModel.GoToPageWithNavigationSource(ApplicationPage.Login,
                        ApplicationPage.Game, this, BootStrapper.Resolve<LoginViewModel>());
                    break;
                case "Player2":
                    //Setting action that will be called from login page when player will be loaded
                    CurrentPlayerLoggingAction = player => Player2 = player;
                    //Navigating to login page with current viewmodel navigation source
                    ViewModelLocator.ApplicationViewModel.GoToPageWithNavigationSource(ApplicationPage.Login,
                        ApplicationPage.Game, this, BootStrapper.Resolve<LoginViewModel>());
                    break;
                default:
                    {
                        var exception = new ArgumentException("Wrong player", nameof(parameter));
                        _logger.Fatal(exception, "Select player command caused an exception");
                        throw exception;
                    }
            }
        }

        /// <summary>
        /// Select player command can execute condition
        /// </summary>
        private bool SelectPlayerCanExecute => true;

        /// <summary>
        /// Method to call from save game command
        /// </summary>
        private void SaveGame()
        {
            _logger.Debug("Save game command called");
            GameController.SaveGame();
        }

        /// <summary>
        /// Save game command can execute condition
        /// </summary>
        private bool SaveGameCanExecute => GameController != null && Player1 != null && Player2 != null;

        #endregion

    }
}
