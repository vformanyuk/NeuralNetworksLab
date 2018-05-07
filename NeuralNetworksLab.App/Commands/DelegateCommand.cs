using System;
using System.Windows.Input;

namespace NeuralNetworksLab.App.Commands
{
    public class DelegateCommand<T> : ICommand
        where T : class
    {
        private readonly Action<T> _executeAction;
        private readonly Func<T, bool> _canExecute;

        public DelegateCommand(Action<T> executeAction)
            : this(executeAction, _ => true)
        {
        }

        public DelegateCommand(Action<T> executeAction, Func<T, bool> canExecuteFunc)
        {
            _executeAction = executeAction;
            _canExecute = canExecuteFunc;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute.Invoke((T)parameter);
        }

        public void Execute(object parameter)
        {
            _executeAction.Invoke((T)parameter);
        }

        public event EventHandler CanExecuteChanged;
    }

    public class DelegateCommand : ICommand
    {
        private readonly Action _executeAction;
        private readonly Func<object, bool> _canExecute;

        public DelegateCommand(Action executeAction)
            : this(executeAction, _ => true)
        {
        }

        public DelegateCommand(Action executeAction, Func<object, bool> canExecuteFunc)
        {
            _executeAction = executeAction;
            _canExecute = canExecuteFunc;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute.Invoke(parameter);
        }

        public void Execute(object parameter)
        {
            _executeAction.Invoke();
        }

        public event EventHandler CanExecuteChanged;
    }
}
