using System;
using System.Windows.Input;
using modern_tech_499m.Logic;
using modern_tech_499m.ViewModels;

namespace modern_tech_499m.Commands
{
    class StartNewGameCommand : ICommand
    {
        private readonly MainWindowViewModel _viewModel;

        public StartNewGameCommand(MainWindowViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            IPlayer player1, player2;
            string param = (string)parameter;
            switch (param)
            {
                case "UserVsUser":
                    {
                        player1 = new UserPlayer("User11");
                        player2 = new UserPlayer("User2");
                        break;
                    }
                case "UserVsAI":
                    {
                        player1 = new UserPlayer("User1");
                        player2 = new AIPlayer("Bot2");
                        break;
                    }
                case "AIVsUser":
                    {
                        player1 = new AIPlayer("Bot1");
                        player2 = new UserPlayer("User2");
                        break;
                    }
                case "AIVsAI":
                default:
                    {
                        player1 = new AIPlayer("Bot1");
                        player2 = new AIPlayer("Bot2");
                        break;
                    }
            }
            GameLogic logic = new GameLogic(6, player1, player2, player1);
            _viewModel.GameController?.StopGame();
            _viewModel.GameController = new GameController(logic);
            _viewModel.GameController.RunGame();
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}
