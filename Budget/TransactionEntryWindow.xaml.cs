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
using Budget.Classes;
using System.Collections;
using Budget.Interfaces;

namespace Budget
{
    /// <summary>
    /// Interaction logic for TransactionWindow.xaml
    /// </summary>
    public partial class TransactionEntryWindow : Window, BudgetWindow
    {
        public enum ModeValues { Add, Edit }
        private ModeValues Mode;
        private Guid? TransactionID;
        private BudgetWindow _callingWindow;

        public TransactionEntryWindow()
        {
            InitializeComponent();
            Mode = ModeValues.Add;
        }
        public TransactionEntryWindow(Guid transactionID) : this()
        {
            TransactionID = transactionID;
            this.Title = "Edit Transaction";
            Mode = ModeValues.Edit;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshWindow();
        }

        private void sbmtBtn_Click(object sender, RoutedEventArgs e)
        {
            var transactionEntry = new TransactionEntry();
            transactionEntry.Mode = Mode;
            transactionEntry.TransationID = TransactionID;
            transactionEntry.Account = (Account)cbAccount.SelectedItem;
            transactionEntry.Amount = tbAmount.Text;
            transactionEntry.CheckNumber = tbCheckNumber.Text;
            transactionEntry.Notes = tbNotes.Text;
            if(cbBudget1.SelectedValue != null)
                transactionEntry.Budget.Add(new DictionaryEntry(cbBudget1.SelectedItem, tbBudget1Amount.Text));
            if (cbBudget2.SelectedValue != null)
                transactionEntry.Budget.Add(new DictionaryEntry(cbBudget2.SelectedItem, tbBudget2Amount.Text));
            if (cbBudget3.SelectedValue != null)
                transactionEntry.Budget.Add(new DictionaryEntry(cbBudget3.SelectedItem, tbBudget3Amount.Text));
            if (cbBudget4.SelectedValue != null)
                transactionEntry.Budget.Add(new DictionaryEntry(cbBudget4.SelectedItem, tbBudget4Amount.Text));
            if (cbBudget5.SelectedValue != null)
                transactionEntry.Budget.Add(new DictionaryEntry(cbBudget5.SelectedItem, tbBudget5Amount.Text));
            if (cbBudget6.SelectedValue != null)
                transactionEntry.Budget.Add(new DictionaryEntry(cbBudget6.SelectedItem, tbBudget6Amount.Text));
            transactionEntry.Date = dpDate.SelectedDate;
            transactionEntry.Payee = (Payee)cbPayee.SelectedItem;
            List<string> errors;
            List<string> warnings;
            SaveTransaction(transactionEntry, out errors, out warnings);
        }

        private void SaveTransaction(TransactionEntry transactionEntry, out List<string> errors, out List<string> warnings, Boolean ignoreWarnings = false)
        {
            if (transactionEntry.Save(out errors, out warnings, ignoreWarnings))
            {
                if (Mode == ModeValues.Add)
                {
                    // add was successful
                    MessageBox.Show("Transaction successfully added.");
                    ClearWindow();
                }
                else if (Mode == ModeValues.Edit)
                {
                    // update was successful
                    MessageBox.Show("Transaction successfully updated.");
                    this.Close();
                }
            }
            else
            {
                // there was a problem
                var sb = new StringBuilder();

                // show the errors first
                if (errors.Count > 0)
                {
                    foreach (string error in errors)
                    {
                        sb.Append(error).Append("\n");
                    }
                    MessageBox.Show(sb.ToString(), "Error");
                }
                // if no errors, then show the warnings
                else if (warnings.Count > 0)
                {
                    sb.Append("The following warnings occurred:\n\n");
                    foreach (string warning in warnings)
                    {
                        sb.Append(warning).Append("\n");
                    }
                    sb.Append("\nContinue?");
                    var result = MessageBox.Show(sb.ToString(), "Warning", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.Yes)
                    {
                        // if user selects continue, re-attempt the save, but ignore warnings
                        SaveTransaction(transactionEntry, out errors, out warnings, true);
                    }
                }
            }
        }

        public void ClearWindow()
        {
            dpDate.SelectedDate = null;
            cbPayee.SelectedValue = null;
            tbAmount.Text = "";
            tbNotes.Text = "";
            tbCheckNumber.Text = "";
            cbAccount.SelectedValue = null;
            cbBudget1.SelectedValue = null;
            cbBudget2.SelectedValue = null;
            cbBudget3.SelectedValue = null;
            cbBudget4.SelectedValue = null;
            cbBudget5.SelectedValue = null;
            cbBudget6.SelectedValue = null;
            tbBudget1Amount.Text = "";
            tbBudget2Amount.Text = "";
            tbBudget3Amount.Text = "";
            tbBudget4Amount.Text = "";
            tbBudget5Amount.Text = "";
            tbBudget6Amount.Text = "";
        }

        private void cnclBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
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
            List<Payee> payees = PayeeManager.GetPayees().ToList();
            payees.Insert(0, new Payee { Name = "<Add New>", Guid = Guid.Empty });
            cbPayee.ItemsSource = payees.ToList();
            cbAccount.ItemsSource = AccountManager.GetAccounts().ToList();

            // load the item to be edited (if specified)
            if (TransactionID != null)
            {
                var transaction = TransactionManager.GetTransaction(TransactionID.Value);
                if (transaction != null)
                {
                    dpDate.SelectedDate = transaction.Date;
                    cbPayee.SelectedValue = transaction.Payee;

                    tbAmount.Text = String.Format("{0:c}", transaction.Amount);
                    cbAccount.SelectedValue = transaction.Account;

                    tbCheckNumber.Text = transaction.CheckNumber;
                    tbNotes.Text = transaction.Notes;

                    PopulateCategoriesList();

                    if (transaction.TransactionCategories.Count > 0)
                    {
                        cbBudget1.SelectedValue = transaction.TransactionCategories.ElementAt(0).Category;
                        tbBudget1Amount.Text = String.Format("{0:c}", transaction.TransactionCategories.ElementAt(0).Amount);
                    }
                    if (transaction.TransactionCategories.Count > 1)
                    {
                        cbBudget2.SelectedValue = transaction.TransactionCategories.ElementAt(1).Category;
                        tbBudget2Amount.Text = String.Format("{0:c}", transaction.TransactionCategories.ElementAt(1).Amount);
                    }
                    if (transaction.TransactionCategories.Count > 2)
                    {
                        cbBudget3.SelectedValue = transaction.TransactionCategories.ElementAt(2).Category;
                        tbBudget3Amount.Text = String.Format("{0:c}", transaction.TransactionCategories.ElementAt(2).Amount);
                    }
                    if (transaction.TransactionCategories.Count > 3)
                    {
                        cbBudget4.SelectedValue = transaction.TransactionCategories.ElementAt(3).Category;
                        tbBudget4Amount.Text = String.Format("{0:c}", transaction.TransactionCategories.ElementAt(3).Amount);
                    }
                    if (transaction.TransactionCategories.Count > 4)
                    {
                        cbBudget5.SelectedValue = transaction.TransactionCategories.ElementAt(4).Category;
                        tbBudget5Amount.Text = String.Format("{0:c}", transaction.TransactionCategories.ElementAt(4).Amount);
                    }
                    if (transaction.TransactionCategories.Count > 5)
                    {
                        cbBudget6.SelectedValue = transaction.TransactionCategories.ElementAt(5).Category;
                        tbBudget6Amount.Text = String.Format("{0:c}", transaction.TransactionCategories.ElementAt(5).Amount);
                    }
                }
                else
                {
                    // transaction does not exist
                    MessageBox.Show("There was a problem loading the transaction.");
                    this.Close();
                }
            }
        }

        private void PopulateCategoriesList()
        {
            if (dpDate.SelectedDate.HasValue)
            {
                var yearCategories = CategoryManager.GetCategories(dpDate.SelectedDate.Value.Year).ToList();
                cbBudget1.ItemsSource = yearCategories;
                cbBudget2.ItemsSource = yearCategories;
                cbBudget3.ItemsSource = yearCategories;
                cbBudget4.ItemsSource = yearCategories;
                cbBudget5.ItemsSource = yearCategories;
                cbBudget6.ItemsSource = yearCategories;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(_callingWindow != null)
                _callingWindow.RefreshWindow();
        }

        private void cbPayee_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // if the user selected "Add New" open the PayeeEntryWindow
            if (cbPayee.SelectedValue != null && (Guid)cbPayee.SelectedValue == Guid.Empty)
            {
                var window = new PayeeEntryWindow();
                window.CallingWindow = this;
                window.Show();
            }
        }

        private void dpDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            PopulateCategoriesList();
        }
    }
}
