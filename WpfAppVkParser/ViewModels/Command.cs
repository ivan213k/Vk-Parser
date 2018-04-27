using System;
using System.Threading;
using System.Windows.Input;
using System.Windows.Threading;

namespace WpfAppVkParser.ViewModels
{
    class Command : ICommand
    {
        public event EventHandler CanExecuteChanged; // подія що повідомляє інтерфейс про зміну умови можливості виконання команди
        private readonly Action<object> execute; // делегат з яким буде зв'язаний метод виконання команди
        private Func<object, bool> canExecute;

        public bool CanExecute(object parameter)
        {
            return this.canExecute == null || this.canExecute(parameter);
        }
        public void OneExecuteChaneged() // метод виклику події CanExecuteChanged
        {
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }

        public Command(Action<object> execute, Func<object, bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public void Execute(object parameter)
        {
            execute(parameter);
        }
    }
}
