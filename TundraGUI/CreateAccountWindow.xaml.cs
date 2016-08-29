using TundraCore;

using System.Linq;
using System.Windows;

namespace TundraGUI
{
    /// <summary>
    /// Interaction logic for CreateAccountWindow.xaml
    /// </summary>
    public partial class CreateAccountWindow : Window
    {
        public CreateAccountWindow()
        {
            InitializeComponent();
            UpdatePasswordControls();
        }

        private void requireAuthenticationCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            UpdatePasswordControls();
        }

        private void requireAuthenticationCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            UpdatePasswordControls();
        }

        private void createAccount_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrEmpty(userNameTextbox.Text))
            {
                MessageBox.Show("Please enter a username.");
                return;
            }
            if((bool) requireAuthenticationCheckbox.IsChecked)
            {
                if(string.IsNullOrWhiteSpace(passwordBox.Password))
                {
                    MessageBox.Show("Please enter a password.");
                    return;
                }
                if(passwordBox.Password != passwordConfirmBox.Password)
                {
                    MessageBox.Show("Make sure your passwords match.");
                    return;
                }

                CreateAccount(userNameTextbox.Text, passwordBox.Password);
                Close();
                return;
            }
            CreateAccount(userNameTextbox.Text);
            Close();
            return;
        }

        private void CreateAccount(string name, string password)
        {
            using (var context = new TundraContext())
            {
                if(AccountExists(name))
                {
                    MessageBox.Show("This account name already exists. Please choose another.", "Create Account - Error");
                    return;
                }

                var User = new User
                {
                    Name = name,
                    Password = password,
                    RequiresValidation = true
                };
                
                context.Users.Add(User);
                context.SaveChanges();
            }
        }

        private void CreateAccount(string name)
        {
            using(var context = new TundraContext())
            {
                if (AccountExists(name))
                {
                    MessageBox.Show("This account name already exists. Please choose another.", "Create Account - Error");
                    return;
                }

                var User = new User
                {
                    Name = name,
                    Password = string.Empty,
                    RequiresValidation = false
                };

                context.Users.Add(User);
                context.SaveChanges();
            }
        }

        private bool AccountExists(string name)
        {
            using (var context = new TundraContext())
            {
                var query = from u in context.Users
                            where u.Name.Equals(name)
                            select u;
                if (query.Any())
                    return true;
                return false;
            }
        }

        private void UpdatePasswordControls()
        {
            passwordLabel.IsEnabled = (bool)requireAuthenticationCheckbox.IsChecked;
            passwordConfirmLabel.IsEnabled = (bool)requireAuthenticationCheckbox.IsChecked;
            passwordBox.IsEnabled = (bool)requireAuthenticationCheckbox.IsChecked;
            passwordConfirmBox.IsEnabled = (bool)requireAuthenticationCheckbox.IsChecked;
        }
    }
}
