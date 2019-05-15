using System;
using System.Windows.Input;

namespace zbW.ProjNuGet.ViewModel
{
    internal class CommandHandler : ICommand
    {
        private Func<object> p;
        private bool _canLoadExcecute;

        public CommandHandler(Func<object> p, bool canLoadExcecute)
        {
            this.p = p;
            _canLoadExcecute = canLoadExcecute;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            throw new NotImplementedException();
        }

        public void Execute(object parameter)
        {
            throw new NotImplementedException();
        }
    }
}