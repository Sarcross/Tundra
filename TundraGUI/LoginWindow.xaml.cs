using TundraCore;

using System;
using System.Linq;
using System.Windows;

namespace TundraGUI
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private MainWindow MainWindow;

        public LoginWindow(MainWindow main)
        {
            InitializeComponent();
            MainWindow = main;
        }

        private void newUserButton_Click(object sender, RoutedEventArgs e)
        {
            new CreateAccountWindow().Show();
        }

        private void submitButton_Click(object sender, RoutedEventArgs e)
        {
            TryLogin();
        }

        private void TryLogin()
        {
            using (var context = new TundraContext())
            {
                var user = (from u in context.Users
                           where u.Name.Equals(userNameTextBox.Text) &&
                                 u.Password.Equals(passwordBox.Password)
                           select u).ToList();
                if(!user.Any())
                {
                    MessageBox.Show("Could not validate credentials.", "Login Error");
                }
                foreach(User u in user)
                {
                    Console.WriteLine(u.Name);
                    MainWindow.LoggedIn = true;
                    MainWindow.ActiveUser = u;
                    MainWindow.Title = "Tundra - " + u.Name;

                    MainWindow.LoginWindow = null;
                    Close();
                }
            }
        }
    }
}
