using System;
using System.Windows.Input;

namespace AISystemsModule.Helpers
{
    public class RelayCommand : ICommand
    {
        private readonly Action execute;

        private readonly Func<bool>? canExecute;

        public event EventHandler? CanExecuteChanged;

        public RelayCommand(Action execute) : this(execute, null)
        { }

        public RelayCommand(Action execute, Func<bool>? canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => canExecute == null || canExecute();

        public void Execute(object parameter) => execute();

        public void OnCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }

    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T> execute;

        private readonly Func<T, bool>? canExecute;

        public event EventHandler? CanExecuteChanged;

        public RelayCommand(Action<T> execute) : this(execute, null)
        { }

        public RelayCommand(Action<T> execute, Func<T, bool>? canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => canExecute == null || canExecute((T)parameter);

        public void Execute(object parameter) => execute((T)parameter);

        public void OnCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
