using TundraCore;

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace TundraGUI
{
    /// <summary>
    /// Interaction logic for CreateGoalWindow.xaml
    /// </summary>
    public partial class CreateGoalWindow : Window
    {
        private MainWindow MainWindow;

        public CreateGoalWindow(MainWindow win)
        {
            InitializeComponent();
            MainWindow = win;
            UpdateDepositControls();
        }

        private void depoitCheckbox_Click(object sender, RoutedEventArgs e)
        {
            UpdateDepositControls();
        }

        private void UpdateDepositControls()
        {
            if((bool)depositCheckBox.IsChecked)
            {
                depositAmountLabel.Visibility = Visibility.Visible; 
                depositAmountTextBox.Visibility = Visibility.Visible;
            }
            else
            {
                depositAmountLabel.Visibility = Visibility.Hidden;
                depositAmountTextBox.Visibility = Visibility.Hidden;
            }
            
        }

        private void ValidateTextBoxInputIsDecimal(TextBox sender)
        {
            try
            {
                Convert.ToDecimal(sender.Text, new CultureInfo("en-US"));
            }
            catch (Exception)
            {
                if (sender.Text.Length == 0)
                {
                    sender.Text = string.Empty;
                    sender.CaretIndex = 1;
                }
                else
                {
                    sender.Text = sender.Text.Substring(0, sender.Text.Length - 1);
                    sender.CaretIndex = sender.Text.Length;
                }

            }
        }

        private void targetAmountTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            ValidateTextBoxInputIsDecimal(targetAmountTextBox);
        }

        private void depositAmountTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            ValidateTextBoxInputIsDecimal(depositAmountTextBox);
        }

        private void submitButton_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrWhiteSpace(nameTextBox.Text))
            {
                MessageBox.Show("Please enter a name.", "Create Goal - Name Error");
                return;
            }
            if (string.IsNullOrWhiteSpace(descriptionTextBox.Text))
            {
                MessageBox.Show("Please enter a description.", "Create Goal - Description Error");
                return;
            }
            if (string.IsNullOrWhiteSpace(targetAmountTextBox.Text))
            {
                MessageBox.Show("Please enter a target amount.", "Create Goal - Target Amount Error");
                return;
            }
            if (deadlineDatePicker.SelectedDate == null || deadlineDatePicker.SelectedDate < DateTime.Now)
            {
                MessageBox.Show("Please select a deadline", "Create Goal - Deadline Error");
                return;
            }
            if((bool) (depositCheckBox.IsChecked) && string.IsNullOrWhiteSpace(depositAmountTextBox.Text))
            {
                MessageBox.Show("Please enter a deposit amount.", "Create Goal - Deposit Error");
            }

            using (var context = new TundraContext())
            {
                Goal goal = new Goal
                {
                    UserId = MainWindow.ActiveUser.Id,
                    Name = nameTextBox.Text,
                    Description = descriptionTextBox.Text,
                    TargetAmount = Convert.ToDecimal(targetAmountTextBox.Text),
                    AcruedAmount = 0,
                    Accomplished = false,
                    Created = DateTime.Now,
                    Deadline = (DateTime)deadlineDatePicker.SelectedDate,
                    Modified = DateTime.Now
                };
                if ((bool)(depositCheckBox.IsChecked))
                    goal.AcruedAmount += Convert.ToDecimal(depositAmountTextBox.Text);

                context.Goals.Add(goal);
                context.SaveChanges();

                MainWindow.UpdateGoalsList();
                Close();
            }
        }
    }
}
