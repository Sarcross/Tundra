using TundraCore;

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
using System.Globalization;

namespace TundraGUI
{
    /// <summary>
    /// Interaction logic for EditAccountWindow.xaml
    /// </summary>
    public partial class EditAccountWindow : Window
    {
        private Account Account;
        public EditAccountWindow(Account acc)
        {
            InitializeComponent();
            Account = acc;

            Title += Account.Name;
            nameTextBox.Text = Account.Name;
            balanceTextBox.Text = Account.Balance.ToString();
        }

        private void ValidateDecimalInput(TextBox box)
        {
            try
            {
                Convert.ToDecimal(box.Text, new CultureInfo("en-US"));
            }
            catch (Exception)
            {
                if (box.Text.Length == 0)
                {
                    box.Text = string.Empty;
                    box.CaretIndex = 1;
                }
                else
                {
                    box.Text = box.Text.Substring(0, box.Text.Length - 1);
                    box.CaretIndex = box.Text.Length;
                }

            }
        }

        private void balanceTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            ValidateDecimalInput(balanceTextBox);
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(nameTextBox.Text))
            {
                MessageBox.Show("Please enter a name.", "Edit Account - Name Error");
                return;
            }
            if (string.IsNullOrWhiteSpace(balanceTextBox.Text))
            {
                MessageBox.Show("Please enter a balance.", "Edit Account - Balance Error");
                return;
            }

            Account.Name = nameTextBox.Text;
            Account.Balance = Convert.ToDecimal(balanceTextBox.Text);

            using (var context = new TundraContext())
            {
                context.Entry(Account).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }

            Close();
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this account?", "Delete Account", MessageBoxButton.YesNoCancel);

            if (result == MessageBoxResult.Yes)
            {
                using (var context = new TundraContext())
                {
                    context.Entry(Account).State = System.Data.Entity.EntityState.Deleted;
                    context.SaveChanges();
                    Close();
                    return;
                }
            }
            if (result == MessageBoxResult.No || result == MessageBoxResult.Cancel)
            {
                return;
            }
        }
    }
}
