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
    /// Interaction logic for PayeeEntryWindow.xaml
    /// </summary>
    public partial class PayeeEntryWindow : Window, BudgetWindow
    {
        private BudgetWindow _callingWindow;
        public PayeeEntryWindow()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            PayeeManager.AddPayee(tbName.Text);
            this.Close();
        }

        public BudgetWindow CallingWindow
        {
            get { return _callingWindow; }
            set { _callingWindow = value; }
        }

        public void RefreshWindow()
        {
            // nothing to do
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_callingWindow != null)
                _callingWindow.RefreshWindow();
        }
    }
}
