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
using Budget.Interfaces;
using Budget.Managers;
using System.Collections;
using Budget.Classes;
using Budget.Entities;
using System.Configuration;

namespace Budget
{
    /// <summary>
    /// Interaction logic for BudgetStatusWindow.xaml
    /// </summary>
    public partial class BudgetStatusWindow : Window, BudgetWindow
    {
        private BudgetWindow _callingWindow;
        private static readonly int curYear = Convert.ToInt32(ConfigurationManager.AppSettings["curYear"]);

        public BudgetStatusWindow()
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

        private void InitializeControls()
        {
            // initialize lists
            var months = new List<MonthItem>();
            months.Add(new MonthItem { DisplayValue = "January", NumericValue = 1 });
            months.Add(new MonthItem { DisplayValue = "February", NumericValue = 2 });
            months.Add(new MonthItem { DisplayValue = "March", NumericValue = 3 });
            months.Add(new MonthItem { DisplayValue = "April", NumericValue = 4 });
            months.Add(new MonthItem { DisplayValue = "May", NumericValue = 5 });
            months.Add(new MonthItem { DisplayValue = "June", NumericValue = 6 });
            months.Add(new MonthItem { DisplayValue = "July", NumericValue = 7 });
            months.Add(new MonthItem { DisplayValue = "August", NumericValue = 8 });
            months.Add(new MonthItem { DisplayValue = "September", NumericValue = 9 });
            months.Add(new MonthItem { DisplayValue = "October", NumericValue = 10 });
            months.Add(new MonthItem { DisplayValue = "November", NumericValue = 11 });
            months.Add(new MonthItem { DisplayValue = "December", NumericValue = 12 });
            cbStartMonth.ItemsSource = months;
            cbStartMonth.DisplayMemberPath = "DisplayValue";
            cbStartMonth.SelectedValuePath = "NumericValue";
            cbEndMonth.ItemsSource = months;
            cbEndMonth.DisplayMemberPath = "DisplayValue";
            cbEndMonth.SelectedValuePath = "NumericValue";

            var years = TransactionManager.GetTransactions().Select(p => p.Date.Year).Distinct().ToList();
            if (!years.Contains(curYear))
                years.Add(curYear);
            cbYear.ItemsSource = years.ToList();

            // set default values
            cbStartMonth.SelectedValue = 1;
            cbEndMonth.SelectedValue = DateTime.Now.Month;
            cbYear.SelectedIndex = years.IndexOf(curYear);
        }

        private void dgBudgetStatus_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dgBudgetStatus.SelectedItem != null && dgBudgetStatus.SelectedItem is CategoryStatus)
            {
                Category selectedCategory = ((CategoryStatus)dgBudgetStatus.SelectedItem).RelatedCategory;
                var transactionLogWindow = new TransactionLogWindow(selectedCategory);
                transactionLogWindow.CallingWindow = this;
                transactionLogWindow.Show();
            }
        }

        public void RefreshWindow()
        {
            var budgetStatusQuery = CategoryManager.GetBudgetStatus((int)cbYear.SelectedValue, (int)cbStartMonth.SelectedValue, (int)cbEndMonth.SelectedValue);
            dgBudgetStatus.ItemsSource = budgetStatusQuery.ToList();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeControls();
            RefreshWindow();
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            RefreshWindow();
        }
    }

    public class MonthItem
    {
        public int NumericValue { get; set; }
        public String DisplayValue { get; set; }
    }
}
