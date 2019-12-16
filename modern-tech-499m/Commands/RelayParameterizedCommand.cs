using System;
using System.Windows.Input;

namespace modern_tech_499m.Commands
{
    /// <summary>
    /// A basic command that runs an Action
    /// </summary>
    class RelayParameterizedCommand : ICommand
    {
        #region Private Members

        /// <summary>
        /// The action to run
        /// </summary>
        private Action<object> _action;

        private Func<object, bool> _canExecute;

        #endregion

        #region Public Events

        /// <summary>
        /// The event fired when the <see cref="CanExecute(object)"/> value has changed
        /// </summary>
        public event EventHandler CanExecuteChanged = (sender, e) => { };

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public RelayParameterizedCommand(Action<object> action, Func<object, bool> canExecute = null)
        {
            _action = action;
            _canExecute = canExecute;
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// A relay command can always execute
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object parameter)
        {
            return _canExecute?.Invoke(parameter) ?? true;
        }

        /// <summary>
        /// Executes the commands Action
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            _action(parameter);
        }

        #endregion

    }
}
