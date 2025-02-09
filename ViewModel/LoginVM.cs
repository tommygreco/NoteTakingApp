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
        // Current user.
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

        // Email used for login/registration.
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

        // Password used for login/registration.
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

        // First name used for login/registration.
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

        // Last name used for login/registration.
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

        // Password confirmation used for login/registration.
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

        // Used to determine if the Login window should be closed.
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

        // Used to determine if the register stack panel should be closed.
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

        // Commands for login and registration.
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

        // Occurs on a successful user registration.
        private void RegisterCommand_Authenticated(object? sender, EventArgs e)
        {
            CloseRegister = true;
        }

        // Occurs on a successful login.
        private void LoginCommand_Authenticated(object? sender, EventArgs e)
        {
            CloseLogin = true;
        }

        // Used to notify if a property has changed.
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
