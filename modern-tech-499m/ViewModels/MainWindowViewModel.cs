using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using modern_tech_499m.Commands.MainWindowViewModelCommands;
using modern_tech_499m.Logic;

namespace modern_tech_499m.ViewModels
{
    class MainWindowViewModel : BaseViewModel
    {
        public MainWindowViewModel()
        {
            CellClickCommand = new CellClickCommand(this);
            StartNewGameCommand = new StartNewGameCommand(this);
            UndoRedoMoveCommand = new UndoRedoMoveCommand(this);
            OpenUsersDatabaseCommand = new OpenUsersDatabaseCommand(this);
            SelectPlayerCommand = new SelectPlayerCommand(this);
           // InitGame();
        }

        private void InitGame()
        {
            IPlayer player1 = new UserPlayer("Player1");
            IPlayer player2 = new UserPlayer("Player2");
            var logic = new GameLogic(6, player1, player2, player1);
            GameController = new GameController(logic);
            GameController.RunGame();
        }

        private GameController _gameController;
        public GameController GameController
        {
            get => _gameController;
            set
            {
                _gameController = value;
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

        public ICommand CellClickCommand { get; private set; }
        public ICommand StartNewGameCommand { get; private set; }
        public ICommand UndoRedoMoveCommand { get; private set; }
        public ICommand OpenUsersDatabaseCommand { get; private set; }
        public ICommand SelectPlayerCommand { get; private set; }
    }
}
