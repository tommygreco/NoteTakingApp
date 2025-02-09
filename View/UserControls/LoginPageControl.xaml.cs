using EvernoteClone.Model;
using EvernoteClone.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EvernoteClone.View.UserControls
{
    /// <summary>
    /// Interaction logic for LoginPageControl.xaml
    /// </summary>
    public partial class LoginPageControl : UserControl
    {
        public LoginPageControl()
        {
            InitializeComponent();
        }

        // *** Dependency properties. ***
        public string Email
        {
            get { return (string)GetValue(EmailProperty); }
            set { SetValue(EmailProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EmailProperty =
            DependencyProperty.Register("Email", typeof(string), typeof(LoginPageControl), new PropertyMetadata(string.Empty));

        public string Password
        {
            get { return (string)GetValue(PasswordProperty); }
            set { SetValue(PasswordProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register("Password", typeof(string), typeof(LoginPageControl), new PropertyMetadata(string.Empty));

        public string RegisterFirstName
        {
            get { return (string)GetValue(RegisterFirstNameProperty); }
            set { SetValue(RegisterFirstNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RegisterFirstNameProperty =
            DependencyProperty.Register("RegisterFirstName", typeof(string), typeof(LoginPageControl), new PropertyMetadata(string.Empty));

        public string RegisterLastName
        {
            get { return (string)GetValue(RegisterLastNameProperty); }
            set { SetValue(RegisterLastNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RegisterLastNameProperty =
            DependencyProperty.Register("RegisterLastName", typeof(string), typeof(LoginPageControl), new PropertyMetadata(string.Empty));

        public string RegisterConfirmPassword
        {
            get { return (string)GetValue(RegisterConfirmPasswordProperty); }
            set { SetValue(RegisterConfirmPasswordProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RegisterConfirmPasswordProperty =
            DependencyProperty.Register("RegisterConfirmPassword", typeof(string), typeof(LoginPageControl), new PropertyMetadata(string.Empty));



        public ICommand LoginCommandProp
        {
            get { return (ICommand)GetValue(LoginCommandPropProperty); }
            set { SetValue(LoginCommandPropProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LoginCommandProp.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LoginCommandPropProperty =
            DependencyProperty.Register("LoginCommandProp", typeof(ICommand), typeof(LoginPageControl), new PropertyMetadata(null));

        public ICommand RegisterCommandProp
        {
            get { return (ICommand)GetValue(RegisterCommandPropProperty); }
            set { SetValue(LoginCommandPropProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LoginCommandProp.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RegisterCommandPropProperty =
            DependencyProperty.Register("RegisterCommandProp", typeof(ICommand), typeof(LoginPageControl), new PropertyMetadata(null));



        public object UserCommandParameter
        {
            get { return (object)GetValue(UserCommandParameterProperty); }
            set { SetValue(UserCommandParameterProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UserCommandParameterProperty =
            DependencyProperty.Register("UserCommandParameter", typeof(object), typeof(LoginPageControl), new PropertyMetadata(string.Empty));

        public bool CloseLogin
        {
            get { return (bool)GetValue(CloseLoginProperty); }
            set { SetValue(CloseLoginProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CloseLogin.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CloseLoginProperty =
            DependencyProperty.Register("CloseLogin", typeof(bool), typeof(LoginPageControl), new PropertyMetadata(false, new PropertyChangedCallback(CloseLoginChanged)));

        public bool CloseRegister
        {
            get { return (bool)GetValue(CloseRegisterProperty); }
            set { SetValue(CloseRegisterProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CloseRegister.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CloseRegisterProperty =
            DependencyProperty.Register("CloseRegister", typeof(bool), typeof(LoginPageControl), new PropertyMetadata(false, new PropertyChangedCallback(DisplayLoginWindow)));

        private static void CloseLoginChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // Close the login window when prompted.
            if ((bool)e.NewValue == true)
            {
                LoginPageControl me = (LoginPageControl)d;
                Window parent = Window.GetWindow(me);
                parent.Close();
            }
        }

        private static void DisplayLoginWindow(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // Display the login window when prompted.
            if ((bool)e.NewValue == true)
            {
                LoginPageControl me = (LoginPageControl)d;
                me.loginStackPanel.Visibility = Visibility.Visible;
                me.registerStackPanel.Visibility = Visibility.Collapsed;
            }
        }

        // Toggle the visibility of the login and register stack panels.
        private void registerButton_Click(object sender, RoutedEventArgs e)
        {
            loginStackPanel.Visibility = Visibility.Collapsed;
            registerStackPanel.Visibility = Visibility.Visible;
        }

        // Toggle the visibility of the login and register stack panels.
        private void registerCancelButton_Click(object sender, RoutedEventArgs e)
        {
            loginStackPanel.Visibility = Visibility.Visible;
            registerStackPanel.Visibility = Visibility.Collapsed;
        }
    }
}
