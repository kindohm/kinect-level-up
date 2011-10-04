using System;
using System.Windows.Input;

namespace KinectLevelUp.CustomSwipe.ViewModel
{
    public class RelayCommand : ICommand
    {
        Action action;

        public RelayCommand(Action action)
        {
            this.action = action;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            this.action.BeginInvoke(null, null);
        }
    }
}
