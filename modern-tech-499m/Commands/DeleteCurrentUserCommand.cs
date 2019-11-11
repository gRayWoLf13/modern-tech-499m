using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using modern_tech_499m.ViewModels;

namespace modern_tech_499m.Commands
{
    internal class DeleteCurrentUserCommand : ICommand
    {
        private readonly UsersDatabaseViewModel _viewModel;

        public DeleteCurrentUserCommand(UsersDatabaseViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return _viewModel.CurrentUser != null;
        }

        public void Execute(object parameter)
        {
                throw new NotImplementedException();
        }

        public event EventHandler CanExecuteChanged;
    }
}
