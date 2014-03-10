using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Budget.Managers;
using Budget.Entities;
using Budget.Interfaces;
using System.Configuration;

namespace Budget
{
    /// <summary>
    /// Interaction logic for TransactionLogWindow.xaml
    /// </summary>
    public partial class TransactionLogWindow : Window, BudgetWindow
    {
        private static readonly int curYear = Convert.ToInt32(ConfigurationManager.AppSettings["curYear"]);
        private BudgetWindow _callingWindow;
        private Category _selectedCategory;
        public TransactionLogWindow()
        {
            InitializeComponent();
        }
        public TransactionLogWindow(Category cat)
            : this()
        {
            _selectedCategory = cat;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeControls();
            RefreshWindow();
        }

        private void InitializeControls()
        {
            var years = TransactionManager.GetTransactions().Select(p => p.Date.Year).Distinct().ToList();
            if (!years.Contains(curYear))
                years.Add(curYear);
            cbYear.ItemsSource = years.ToList();
            cbYear.SelectedIndex = years.IndexOf(curYear);
        }

        private void dgTransactionLog_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dgTransactionLog.SelectedItem != null && dgTransactionLog.SelectedItem is Transaction)
            {
                var transactionEntryWindow = new TransactionEntryWindow(((Transaction)dgTransactionLog.SelectedItem).Guid);
                transactionEntryWindow.CallingWindow = this;
                transactionEntryWindow.Show();
            }
        }

        public BudgetWindow CallingWindow
        {
            get
            {
                return _callingWindow;
            }
            set
            {
                _callingWindow = value;
            }
        }

        public void RefreshWindow()
        {
            var transactions = TransactionManager.GetTransactions(_selectedCategory, (int)cbYear.SelectedValue);
            dgTransactionLog.ItemsSource = transactions.ToList();
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            RefreshWindow();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (dgTransactionLog.SelectedItems.Count > 0)
            {
                var result = MessageBox.Show("Are you sure you want to remove this transaction?", "Confirm Deletion", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        TransactionManager.DeleteTransaction(dgTransactionLog.SelectedItems.Cast<Transaction>());
                    }
                    catch
                    {
                        MessageBox.Show("There was a problem removing the selected transaction(s).");
                    }
                    RefreshWindow();
                }
            }
            else
            {
                MessageBox.Show("No transactions selected.");
            }
        }

        private void cbYear_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshWindow();
        }
    }
}
