using Impure.Object_Classes;
using System;
using System.Windows;
using System.Windows.Input;

namespace Impure
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public static bool initialized = false;
        public LoginWindow()
        {
            if (!initialized)
            {
                OnProgramStart.Initialize("Impure", "619145", "UVIddHh5qNA0PtnctCqnIqvSj184CJvKNpW", "1.0");
                initialized = true;
            }
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if(API.Login(UsernameBox.Text, PasswordBox.SecurePassword.ToString()))
            {
                var CheatUI = new MainWindow();
                CheatUI.Show();
                Close();
            }else
            {
                MessageBox.Show("Invalid credentials!");
            }
        }

        private void MiniButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
            Environment.Exit(0);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Title = Helpers.RandomString(Helpers.Rnd.Next(8, 32));
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && e.ButtonState == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            var RegisterUI = new RegisterWindow();
            RegisterUI.Show();
            Close();
        }
    }
}
