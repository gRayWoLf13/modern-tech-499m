using System.Windows.Input;
using modern_tech_499m.Commands.MainWindowViewModelCommands;
using modern_tech_499m.Logic;
using modern_tech_499m.Repositories.Core.Repositories;
using modern_tech_499m.ViewModels.Base;
using NLog;

namespace modern_tech_499m.ViewModels
{
    class MainWindowViewModel : BaseViewModel
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        public IGameInfoRepository GameInfoRepository { get; }
        public MainWindowViewModel(IGameInfoRepository gameInfoRepository)
        {
            _logger.Debug("Main window view model constructor called");
            GameInfoRepository = gameInfoRepository;
            CellClickCommand = new CellClickCommand(this);
            StartNewGameCommand = new StartNewGameCommand(this);
            UndoRedoMoveCommand = new UndoRedoMoveCommand(this);
            OpenUsersDatabaseCommand = new OpenUsersDatabaseCommand(this);
            SelectPlayerCommand = new SelectPlayerCommand(this);
            OpenGamesDatabaseCommand = new OpenGamesDatabaseCommand(this);
            SaveGameCommand = new SaveGameCommand(this);
        }

        private GameController _gameController;
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

        private bool _usersDatabaseViewOpen;
        public bool UsersDatabaseViewOpen
        {
            get => _usersDatabaseViewOpen;
            set
            {
                _usersDatabaseViewOpen = value;
                OnPropertyChanged();
            }
        }

        private IPlayer _player1;
        public IPlayer Player1
        {
            get => _player1;
            set
            {
                _player1 = value;
                OnPropertyChanged();
            }
        }

        private IPlayer _player2;
        public IPlayer Player2
        {
            get => _player2;
            set
            {
                _player2 = value;
                OnPropertyChanged();
            }
        }

        public ICommand CellClickCommand { get; }
        public ICommand StartNewGameCommand { get; }
        public ICommand UndoRedoMoveCommand { get; }
        public ICommand OpenUsersDatabaseCommand { get; }
        public ICommand SelectPlayerCommand { get; }
        public ICommand OpenGamesDatabaseCommand { get; }
        public ICommand SaveGameCommand { get; }

        private void SyncInfoWithLoadedGameLogic()
        {
            Player1 = GameController.GameLogic.Player1;
            Player2 = GameController.GameLogic.Player2;
        }
    }
}
