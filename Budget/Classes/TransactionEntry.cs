using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Budget.Entities;
using System.Collections;
using Budget.Managers;
using System.Text.RegularExpressions;
using System.Globalization;

namespace Budget.Classes
{
    public class TransactionEntry
    {
        public TransactionEntryWindow.ModeValues Mode;
        public Guid? TransationID { get; set; }
        public DateTime? Date { get; set; }
        public Payee Payee { get; set; }
        public string Amount { get; set; }
        public Account Account { get; set; }
        public string Notes { get; set; }
        public string CheckNumber { get; set; }
        public readonly List<DictionaryEntry> Budget = new List<DictionaryEntry>();

        private void Validate(out List<string> errors, out List<string> warnings)
        {
            errors = new List<string>();
            warnings = new List<string>();

            // ERRORS
            // make sure a date is specified
            if (Date == null)
                errors.Add("Date must be specified.");

            // make sure result can be cast to a decimal
            try
            {
                Double.Parse(Amount, NumberStyles.Currency);
            }
            catch (Exception)
            {
                errors.Add("Amount must be numeric.");
            }

            // make sure Account is specified
            if (Account == null)
            {
                errors.Add("No account specified");
            }
            // make sure the Account is in the table (this probably won't happen)
            else if (!DBManager.Get(Account))
                errors.Add("Invalid Account: " + Account.Name);

            // do the same for all the budget categories
            //foreach (DictionaryEntry item in Budget)
            for(int i=0; i < Budget.Count; i++)
            {
                DictionaryEntry item = Budget[i];

                try
                {
                    Double.Parse(item.Value.ToString(), NumberStyles.Currency);
                }
                catch (Exception)
                {
                    errors.Add("Budget amount #" + (i + 1) + " must be numeric.");
                }

                if (item.Key == null || !DBManager.Get((Category)item.Key))
                {
                    errors.Add("Invalid Budget Category: " + ((Category)item.Key).Name);
                }
            }

            if (errors.Count == 0)
            {
                // WARNINGS
                // budgeted amount doesn't match transaction amount
                Double totalBudgetedAmount = 0;
                foreach (DictionaryEntry item in Budget)
                {
                    totalBudgetedAmount += Double.Parse(item.Value.ToString(), NumberStyles.Currency);
                }
                if (totalBudgetedAmount != Double.Parse(Amount, NumberStyles.Currency))
                {
                    warnings.Add("Total transaction amount does not equal total budgeted amount.");
                }
            }
        }

        public Boolean Save(out List<string> errors, out List<string> warnings, Boolean ignoreWarnings = false)
        {
            Boolean result = false;
            Validate(out errors, out warnings);
            if (errors.Count == 0 && (ignoreWarnings || warnings.Count == 0))
            {
                try
                {
                    var db = new BudgetEntities();

                    if (Mode == TransactionEntryWindow.ModeValues.Add)
                    {
                        // add the transaction
                        var transaction = new Transaction();
                        transaction.Guid = Guid.NewGuid();
                        transaction = PopulateObject(transaction, db);
                        db.Transactions.Add(transaction);
                        db.SaveChanges();
                        result = true;
                    }
                    else if (Mode == TransactionEntryWindow.ModeValues.Edit)
                    {
                        // update the transaction
                        var transaction = (from t in db.Transactions
                                           where t.Guid == TransationID
                                           select t).Single();
                        transaction = PopulateObject(transaction, db);
                        db.SaveChanges();
                        result = true;
                    }
                    else
                    {
                        throw new InvalidOperationException("Invalid mode specified.");
                    }
                }
                catch (Exception ex)
                {
                    errors.Add(ex.Message);
                }
            }
            return result;
        }

        private Transaction PopulateObject(Transaction transaction, BudgetEntities db)
        {
            transaction.Date = Date.Value;
            transaction.Amount = Double.Parse(Amount, NumberStyles.Currency);
            transaction.Payee = Payee.Guid;
            transaction.Account = Account.Guid;
            transaction.Notes = Notes;
            transaction.CheckNumber = CheckNumber;

            // clear transaction categories
            foreach (var transactionCategory in transaction.TransactionCategories.ToList())
            {
                db.TransactionCategories.Remove(transactionCategory);
            }

            // add the transaction categories
            foreach (DictionaryEntry item in Budget)
            {
                var transactionCategory = new TransactionCategory();
                transactionCategory.Guid = Guid.NewGuid();
                transactionCategory.Amount = Double.Parse(item.Value.ToString(), NumberStyles.Currency);
                transactionCategory.Category = ((Category)item.Key).Guid;
                transaction.TransactionCategories.Add(transactionCategory);
            }
            return transaction;
        }
    }
}
