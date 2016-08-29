using TundraCore;

using System.Linq;
using System.Windows;
using System.Collections.ObjectModel;

namespace TundraGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public bool LoggedIn = false;
        public User ActiveUser = null;
        public LoginWindow LoginWindow;

        public ObservableCollection<Account> Accounts;
        public ObservableCollection<Goal> Goals;
        public ObservableCollection<Transaction> Transactions;

        public MainWindow()
        {
            InitializeComponent();

            Accounts = new ObservableCollection<Account>();
            Goals = new ObservableCollection<Goal>();
            Transactions = new ObservableCollection<Transaction>();

            accountsListView.ItemsSource = Accounts;
            goalsListView.ItemsSource = Goals;
            transactionsListView.ItemsSource = Transactions;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            using (var context = new TundraContext())
            {
                context.SaveChanges();
            }
            Application.Current.Shutdown();
        }

        private bool GiveFocusToLoginWindow()
        {
            if (LoginWindow != null)
            {
                LoginWindow.Focus();
                return true;
            }
            return false;
        }

        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AboutMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Copyright 2016 Anthony Haddox");
        }

        private void NewAccountMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (!LoggedIn)
                new LoginWindow(this).ShowDialog();
            else
                new CreateFinancialAccountWindow(this).ShowDialog();
        }

        private void NewGoalMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (!LoggedIn)
                new LoginWindow(this).ShowDialog();
            else
                new CreateGoalWindow(this).ShowDialog();
        }

        private void NewTransactionMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (!LoggedIn)
                new LoginWindow(this).ShowDialog();

            new CreateTransactionWindow(this).ShowDialog();
        }

        public void UpdateAccountsList()
        {
            using (var context = new TundraContext())
            {
                Accounts.Clear();
                var accountQuery = (from a in context.Accounts
                                   where a.UserId == ActiveUser.Id
                                   select a).ToList();

                foreach (Account a in accountQuery)
                {
                    Accounts.Add(a);
                }
            }
        }

        public void UpdateGoalsList()
        {
            using (var context = new TundraContext())
            {
                Goals.Clear();
                var goalQuery = (from g in context.Goals
                                where g.UserId == ActiveUser.Id
                                select g).ToList();

                foreach (Goal g in goalQuery)
                {
                    Goals.Add(g);
                }
            }
        }

        public void UpdateTransactionsList()
        {
            using (var context = new TundraContext())
            {
                Transactions.Clear();
                var transactionQuery = (from t in context.Transactions
                                       where t.UserId == ActiveUser.Id
                                       select t).ToList();

                foreach (Transaction t in transactionQuery)
                {
                    Transactions.Add(t);
                }
            }
        }

        private void mainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoginWindow = new LoginWindow(this);
            LoginWindow.ShowDialog();

            if (LoggedIn)
            {
                UpdateLists();
            }
        }

        private void UpdateLists()
        {
            UpdateAccountsList();
            UpdateGoalsList();
            UpdateTransactionsList();
        }

        private void goalsListView_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (goalsListView.SelectedIndex < 0)
                return;

            new EditGoalWindow((Goal) goalsListView.Items.GetItemAt(goalsListView.SelectedIndex)).ShowDialog();
            UpdateGoalsList();
        }

        private void accountsListView_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (accountsListView.SelectedIndex < 0)
                return;

            new EditAccountWindow((Account) accountsListView.Items.GetItemAt(accountsListView.SelectedIndex)).ShowDialog();
            UpdateAccountsList();
        }

        private void transactionsListView_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (transactionsListView.SelectedIndex < 0)
                return;

            new EditTransactionWindow((Transaction) transactionsListView.Items.GetItemAt(transactionsListView.SelectedIndex)).ShowDialog();
            UpdateTransactionsList();
        }
    }
}
