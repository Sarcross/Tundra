using TundraCore;

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Input;

namespace TundraGUI
{
    /// <summary>
    /// Interaction logic for CreateFinancialAccountWindow.xaml
    /// </summary>
    public partial class CreateFinancialAccountWindow : Window
    {
        private MainWindow MainWindow;
        public CreateFinancialAccountWindow(MainWindow win)
        {
            InitializeComponent();
            MainWindow = win;
        }

        private void createButton_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrWhiteSpace(nameTextBox.Text))
            {
                MessageBox.Show("Please enter an account name.", "Create Account - Account Name Error");
                return;
            }
            if(string.IsNullOrWhiteSpace(balanceTextBox.Text))
            {
                MessageBox.Show("Please enter balance.", "Create Account - Balance Error");
                return;
            }

            switch(accountTypeComboBox.SelectedIndex)
            {
                case 0:
                    using (var context = new TundraContext())
                    {
                        SavingsAccount acc = new SavingsAccount
                        {
                            UserId = MainWindow.ActiveUser.Id,
                            Name = nameTextBox.Text,
                            Balance = Convert.ToDecimal(balanceTextBox.Text)
                        };

                        context.Accounts.Add(acc);
                        context.SaveChanges();
                    }
                    break;
                case 1:
                    using (var context = new TundraContext())
                    {
                        CheckingAccount acc = new CheckingAccount
                        {
                            UserId = MainWindow.ActiveUser.Id,
                            Name = nameTextBox.Text,
                            Balance = Convert.ToDecimal(balanceTextBox.Text)
                        };

                        context.Accounts.Add(acc);
                        context.SaveChanges();
                    }
                    break;
            }
            MainWindow.UpdateAccountsList();
            Close();
        }

        private void balanceTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            try
            {
                Convert.ToDecimal(balanceTextBox.Text, new CultureInfo("en-US"));
            }
            catch(Exception)
            {
                if (balanceTextBox.Text.Length == 0)
                {
                    balanceTextBox.Text = string.Empty;
                    balanceTextBox.CaretIndex = 1;
                } 
                else
                {
                    balanceTextBox.Text = balanceTextBox.Text.Substring(0, balanceTextBox.Text.Length - 1);
                    balanceTextBox.CaretIndex = balanceTextBox.Text.Length;
                }
                    
            }
        }
    }
}
