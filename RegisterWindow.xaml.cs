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
using System.Windows.Shapes;

namespace Impure
{
    /// <summary>
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            if(API.Register(UsernameBox.Text,PasswordBox.SecurePassword.ToString(), EmailBox.Text, SerialBox.Text))
            {
                MessageBox.Show("Successfully registered!");
                var LoginUI = new LoginWindow();
                LoginUI.Show();
                Close();
            }
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var LoginUI = new LoginWindow();
            LoginUI.Show();
            Close();
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
    }
}
