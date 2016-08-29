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
    /// Interaction logic for EditTransactionWindow.xaml
    /// </summary>
    public partial class EditTransactionWindow : Window
    {
        private Transaction Transaction;
        public EditTransactionWindow(Transaction trans)
        {
            InitializeComponent();
            Transaction = trans;

            Title += Transaction.Name;
            nameTextBox.Text = Transaction.Name;
            descriptionTextBox.Text = Transaction.Description;
            amountTextBox.Text = Transaction.Amount.ToString();
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(nameTextBox.Text))
            {
                MessageBox.Show("Please enter a name.", "Edit Transaction - Name Error");
                return;
            }
            if (string.IsNullOrWhiteSpace(descriptionTextBox.Text))
            {
                MessageBox.Show("Please enter a description.", "Edit Transaction - Description Error");
                return;
            }
            if (string.IsNullOrWhiteSpace(amountTextBox.Text))
            {
                MessageBox.Show("Please enter an amount.", "Edit Transaction - Amount Error");
                return;
            }

            Transaction.Name = nameTextBox.Text;
            Transaction.Description = descriptionTextBox.Text;
            Transaction.Amount = Convert.ToDecimal(amountTextBox.Text);

            using (var context = new TundraContext())
            {
                context.Entry(Transaction).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }

            Close();
        }

        private void deletebutton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this transaction?", "Delete Transaction", MessageBoxButton.YesNoCancel);

            if (result == MessageBoxResult.Yes)
            {
                using (var context = new TundraContext())
                {
                    context.Entry(Transaction).State = System.Data.Entity.EntityState.Deleted;
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

        private void amountTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            ValidateDecimalInput(amountTextBox);
        }
    }
}
