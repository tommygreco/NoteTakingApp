using EvernoteClone.Model;
using EvernoteClone.ViewModel.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace EvernoteClone.ViewModel.Commands
{
    public class LoginCommand : ICommand
    {
        public LoginVM VM { get; set; }
        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public LoginCommand(LoginVM vm)
        {
            VM = vm;
        }
        public bool CanExecute(object? parameter)
        {
            if (parameter != null)
            {
                User curUser = parameter as User;
                if (!string.IsNullOrEmpty(curUser.Email) && !string.IsNullOrEmpty(curUser.Password))
                {
                    return true;
                }

            }
            return false;
        }

        public async void Execute(object? parameter)
        {
            // TODO: Implement funcitonality.
            if (parameter != null)
            {
                bool result = await FirebaseAuthHelper.Login(parameter as User);
                if (result)
                {
                    OnAuthenticated(new EventArgs());
                }
            }
        }

        public event EventHandler Authenticated;
        private void OnAuthenticated(EventArgs e)
        {
            Authenticated?.Invoke(this, e);
        }
    }
}
