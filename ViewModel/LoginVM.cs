using EvernoteClone.Model;
using EvernoteClone.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EvernoteClone.ViewModel
{
    public class LoginVM : INotifyPropertyChanged
    {
        private User user;
        public User User
        {
            get { return user; }
            set
            { 
                user = value;
                OnPropertyChanged("User");
            }
        }

        private string email;
        public string Email
        {
            get { return email; }
            set
            {
                email = value;
                User = new User
                {
                    Email = email,
                    Password = this.Password,
                    Name = this.Name,
                    LastName = this.LastName,
                    ConfirmPassword = this.ConfirmPassword
                };
                OnPropertyChanged("Username");
            }
        }

        private string password;
        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                User = new User
                {
                    Email = this.Email,
                    Password = password,
                    Name = this.Name,
                    LastName = this.LastName,
                    ConfirmPassword = this.ConfirmPassword
                };
                OnPropertyChanged("Password");
            }
        }

        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                User = new User
                {
                    Email = this.Email,
                    Password = this.Password,
                    Name = name,
                    LastName = this.LastName,
                    ConfirmPassword = this.ConfirmPassword
                };
                OnPropertyChanged("Name");
            }
        }

        private string lastName;
        public string LastName
        {
            get { return lastName; }
            set
            {
                lastName = value;
                User = new User
                {
                    Email = this.Email,
                    Password = this.Password,
                    Name = this.Name,
                    LastName = lastName,
                    ConfirmPassword = this.ConfirmPassword
                };
                OnPropertyChanged("LastName");
            }
        }

        private string confirmPassword;
        public string ConfirmPassword
        {
            get { return confirmPassword; }
            set
            {
                confirmPassword = value;
                User = new User
                {
                    Email = this.Email,
                    Password = this.Password,
                    Name = this.Name,
                    LastName = this.LastName,
                    ConfirmPassword = confirmPassword
                };
                OnPropertyChanged("ConfirmPassword");
            }
        }

        private bool closeLogin;

        public bool CloseLogin
        {
            get { return closeLogin; }
            set
            {
                closeLogin = value;
                OnPropertyChanged("CloseLogin");
            }
        }

        private bool closeRegister;

        public bool CloseRegister
        {
            get { return closeRegister; }
            set
            {
                closeRegister = value;
                OnPropertyChanged("CloseRegister");
            }
        }



        public RegisterCommand RegisterCommand { get; set; }
        public LoginCommand LoginCommand { get; set; }

        public LoginVM()
        {
            RegisterCommand = new RegisterCommand(this);
            LoginCommand = new LoginCommand(this);
            LoginCommand.Authenticated += LoginCommand_Authenticated;
            RegisterCommand.Authenticated += RegisterCommand_Authenticated;

            User = new User();
        }

        private void RegisterCommand_Authenticated(object? sender, EventArgs e)
        {
            CloseRegister = true;
        }

        private void LoginCommand_Authenticated(object? sender, EventArgs e)
        {
            CloseLogin = true;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }
}
