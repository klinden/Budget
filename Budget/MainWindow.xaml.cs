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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Windows.Markup;
using Budget.Interfaces;

namespace Budget
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, BudgetWindow
    {
        private BudgetWindow _callingWindow;
        public MainWindow()
        {
            InitializeComponent();
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
            // do nothing
        }

        private void TransactionLogButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new TransactionLogWindow();
            window.CallingWindow = this;
            window.Show();
        }

        private void NewTransactionButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new TransactionEntryWindow();
            window.CallingWindow = this;
            window.Show();
        }

        private void BudgetStatusButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new BudgetStatusWindow();
            window.CallingWindow = this;
            window.Show();
        }

        private void AccountBalancesButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new AccountBalancesWindow();
            window.CallingWindow = this;
            window.Show();
        }
    }
}
