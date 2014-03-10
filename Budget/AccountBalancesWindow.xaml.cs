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
using Budget.Interfaces;

namespace Budget
{
    /// <summary>
    /// Interaction logic for AccountBalancesWindow.xaml
    /// </summary>
    public partial class AccountBalancesWindow : Window, BudgetWindow
    {
        private BudgetWindow _callingWindow;

        public AccountBalancesWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshWindow();
        }

        public void RefreshWindow()
        {
            var accounts = AccountManager.GetAccountBalances();
            dgAccountBalances.ItemsSource = accounts.ToList();
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
    }
}
