using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace zbW.ProjNuGet
{
    public class RelayCommand : ICommand
    {
        
        readonly Action targetExecuteMethod;
        readonly Func<bool> targetCanExecuteMethod;

        public RelayCommand(Action executeMethod)
        {
            targetExecuteMethod = executeMethod;
        }

        public RelayCommand(Action executeMethod, Func<bool> canExecuteMethod)
        {
            targetExecuteMethod = executeMethod;
            targetCanExecuteMethod = canExecuteMethod;
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
        #region ICommand Members

        bool ICommand.CanExecute(object parameter)
        {
            if (targetCanExecuteMethod != null)
            {
                return targetCanExecuteMethod();
            }
            if (targetExecuteMethod != null)
            {
                return true;
            }
            return false;
        }

        // Beware - should use weak references if command instance lifetime is longer than lifetime of UI objects that get hooked up to command
        // Prism commands solve this in their implementation
        public event EventHandler CanExecuteChanged;

        void ICommand.Execute(object parameter)
        {
            this.targetExecuteMethod?.Invoke();
        }
        #endregion
    }

    public class RelayCommand<T> : ICommand
    {
        readonly Action<T> targetExecuteMethod;
        readonly Func<T, bool> targetCanExecuteMethod;

        public RelayCommand(Action<T> executeMethod)
        {
            targetExecuteMethod = executeMethod;
        }

        public RelayCommand(Action<T> executeMethod, Func<T, bool> canExecuteMethod)
        {
            targetExecuteMethod = executeMethod;
            targetCanExecuteMethod = canExecuteMethod;
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
        #region ICommand Members

        bool ICommand.CanExecute(object parameter)
        {
            if (targetCanExecuteMethod != null)
            {
                var tparm = (T)parameter;
                return targetCanExecuteMethod(tparm);
            }
            if (targetExecuteMethod != null)
            {
                return true;
            }
            return false;
        }

        // Beware - should use weak references if command instance lifetime is longer than lifetime of UI objects that get hooked up to command
        // Prism commands solve this in their implementation
        public event EventHandler CanExecuteChanged;

        void ICommand.Execute(object parameter)
        {
            this.targetExecuteMethod?.Invoke((T)parameter);
        }
        #endregion
        
    }
}
