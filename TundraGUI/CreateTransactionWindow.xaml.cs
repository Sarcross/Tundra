using TundraCore;

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Input;

namespace TundraGUI
{
    /// <summary>
    /// Interaction logic for CreateTransactionWindow.xaml
    /// </summary>
    public partial class CreateTransactionWindow : Window
    {
        private MainWindow MainWindow;
        public CreateTransactionWindow(MainWindow mw)
        {
            InitializeComponent();
            MainWindow = mw;
        }

        private void amountTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            try
            {
                Convert.ToDecimal(amountTextBox.Text, new CultureInfo("en-US"));
            }
            catch (Exception)
            {
                if (amountTextBox.Text.Length == 0)
                {
                    amountTextBox.Text = string.Empty;
                    amountTextBox.CaretIndex = 1;
                }
                else
                {
                    amountTextBox.Text = amountTextBox.Text.Substring(0, amountTextBox.Text.Length - 1);
                    amountTextBox.CaretIndex = amountTextBox.Text.Length;
                }

            }
        }

        private void submitButton_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrWhiteSpace(nameTextBox.Text))
            {
                MessageBox.Show("Please enter a name.", "Create Transaction - Name Error");
                return;
            }
            if(string.IsNullOrWhiteSpace(descriptionTextBox.Text))
            {
                MessageBox.Show("Please enter a description.", "Create Transaction - Description Error");
                return;
            }
            if(string.IsNullOrWhiteSpace(amountTextBox.Text))
            {
                MessageBox.Show("Please enter an amount.", "Create Transaction - Amount Error");
                return;
            }

            switch(typeComboBox.SelectedIndex)
            {
                case 0:
                    using (var context = new TundraContext())
                    {
                        Income inc = new Income
                        {
                            UserId = MainWindow.ActiveUser.Id,
                            Name = nameTextBox.Text,
                            Description = descriptionTextBox.Text,
                            Amount = Convert.ToDecimal(amountTextBox.Text)
                        };
                        context.Transactions.Add(inc);
                        context.SaveChanges();
                    }
                    break;

                case 1:
                    using (var context = new TundraContext())
                    {
                        Expense inc = new Expense
                        {
                            UserId = MainWindow.ActiveUser.Id,
                            Name = nameTextBox.Text,
                            Description = descriptionTextBox.Text,
                            Amount = Convert.ToDecimal(amountTextBox.Text)
                        };
                        context.Transactions.Add(inc);
                        context.SaveChanges();
                    }
                    break;
            }
            MainWindow.UpdateTransactionsList();
            Close();
        }
    }
}
