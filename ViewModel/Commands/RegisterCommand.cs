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
    public class RegisterCommand : ICommand
    {
        public LoginVM VM { get; set; }
        public event EventHandler? CanExecuteChanged;

        public RegisterCommand(LoginVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object? parameter)
        {
            if (parameter != null)
            {
                User curUser = parameter as User;
                if (!string.IsNullOrEmpty(curUser.Name) &&
                    !string.IsNullOrEmpty(curUser.LastName) && 
                    !string.IsNullOrEmpty(curUser.Email) &&
                    curUser.Email.Contains('@') &&
                    curUser.Email.Contains('.') &&
                    !string.IsNullOrEmpty(curUser.Password) &&
                    !string.IsNullOrEmpty(curUser.ConfirmPassword) &&
                    string.Equals(curUser.ConfirmPassword, curUser.Password))
                {
                    return true;
                }

            }
            return false;
        }

        public async void Execute(object? parameter)
        {
            if (parameter != null)
            {
                bool result = await FirebaseAuthHelper.Register(parameter as User);
                if (result)
                {
                    MessageBox.Show("Registered Successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
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
