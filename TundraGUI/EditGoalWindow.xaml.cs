using TundraCore;

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Globalization;

namespace TundraGUI
{
    /// <summary>
    /// Interaction logic for EditGoalWindow.xaml
    /// </summary>
    public partial class EditGoalWindow : Window
    {
        private Goal Goal;
        public EditGoalWindow(Goal g)
        {
            InitializeComponent();
            Goal = g;

            this.Title += Goal.Name;

            nameTextBox.Text = Goal.Name;
            descriptionTextBox.Text = Goal.Description;
            targetAmountTextBox.Text = Goal.TargetAmount.ToString();
            acruedAmountTextBox.Text = Goal.AcruedAmount.ToString();
            accomplishedCheckBox.IsChecked = Goal.Accomplished;
            deadlineDatePicker.SelectedDate = Goal.Deadline;

            SetTextBoxEnabled();
        }

        private void SetTextBoxEnabled()
        {
            nameTextBox.IsEnabled = (bool) !accomplishedCheckBox.IsChecked;
            descriptionTextBox.IsEnabled = (bool)!accomplishedCheckBox.IsChecked;
            targetAmountTextBox.IsEnabled = (bool)!accomplishedCheckBox.IsChecked;
            acruedAmountTextBox.IsEnabled = (bool)!accomplishedCheckBox.IsChecked;
            deadlineDatePicker.IsEnabled = (bool)!accomplishedCheckBox.IsChecked;
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

        private void acruedAmountTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            ValidateDecimalInput(acruedAmountTextBox);
        }

        private void targetAmountTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            ValidateDecimalInput(targetAmountTextBox);
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(nameTextBox.Text))
            {
                MessageBox.Show("Please enter a name.", "Edit Goal - Name Error");
                return;
            }
            if (string.IsNullOrWhiteSpace(descriptionTextBox.Text))
            {
                MessageBox.Show("Please enter a description.", "Edit Goal - Description Error");
                return;
            }
            if (string.IsNullOrWhiteSpace(targetAmountTextBox.Text))
            {
                MessageBox.Show("Please enter a target amount.", "Edit Goal - Target Amount Error");
                return;
            }
            if (deadlineDatePicker.SelectedDate == null || deadlineDatePicker.SelectedDate < DateTime.Now)
            {
                MessageBox.Show("Please select a deadline", "Edit Goal - Deadline Error");
                return;
            }

            Goal.Name = nameTextBox.Text;
            Goal.Description = descriptionTextBox.Text;
            Goal.TargetAmount = Convert.ToDecimal(targetAmountTextBox.Text);
            Goal.AcruedAmount = Convert.ToDecimal(acruedAmountTextBox.Text);
            Goal.Accomplished = (bool) accomplishedCheckBox.IsChecked;
            Goal.Deadline = (DateTime) deadlineDatePicker.SelectedDate;

            using (var context = new TundraContext())
            {
                context.Entry(Goal).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }

            Close();
        }

        private void accomplishedCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            SetTextBoxEnabled();
        }

        private void accomplishedCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            SetTextBoxEnabled();
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this goal?", "Delete Goal", MessageBoxButton.YesNoCancel);
            
            if(result == MessageBoxResult.Yes)
            {
                using (var context = new TundraContext())
                {
                    context.Entry(Goal).State = System.Data.Entity.EntityState.Deleted;
                    context.SaveChanges();
                    Close();
                    return;
                }
            }
            if(result == MessageBoxResult.No || result == MessageBoxResult.Cancel)
            {
                return;
            }
        }
    }
}
