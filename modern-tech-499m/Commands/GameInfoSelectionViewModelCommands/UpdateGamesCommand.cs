using System;
using System.Linq;
using System.Windows.Input;
using modern_tech_499m.Repositories.Core.Repositories;
using modern_tech_499m.ViewModels;

namespace modern_tech_499m.Commands.GameInfoSelectionViewModelCommands
{
    class UpdateGamesCommand : ICommand
    {
        private readonly GameInfoSelectionViewModel _playerSelectionViewModel;
        private readonly IGameInfoRepository _gameInfoRepository;

        public UpdateGamesCommand(GameInfoSelectionViewModel playerSelectionViewModel, IGameInfoRepository gameInfoRepository)
        {
            _playerSelectionViewModel = playerSelectionViewModel;
            _gameInfoRepository = gameInfoRepository;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _playerSelectionViewModel.LoadGames();
            _playerSelectionViewModel.CurrentGameInfoWrapper =
                _playerSelectionViewModel.WrappedGameInfos.FirstOrDefault();
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}
