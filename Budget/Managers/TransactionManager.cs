using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Budget.Entities;
using System.Collections;
using System.Configuration;

namespace Budget.Managers
{
    public static class TransactionManager
    {
        public static Transaction GetTransaction(Guid id)
        {
            var db = new BudgetEntities();
            var query = (from transaction in db.Transactions
                        where transaction.Guid == id
                        select transaction).Single();
            return query;
        }
        public static IEnumerable<Transaction> GetTransactions(Category selectedCategory = null, int year = default(int))
        {
            var db = new BudgetEntities();
            var query = from transaction in db.Transactions
                        select transaction;
            
            // if a year is specified just get transactions for that year
            if (year != default(int))
                query = query.Where(p => p.Date.Year == year);

            // if a category is specified, only get transactions for that category
            if (selectedCategory != null)
                query = query.Where(p => p.TransactionCategories.Any(q => q.Category == selectedCategory.Guid));

            // order by
            query = query.OrderBy(p => p.Date).ThenBy(q => q.Payee1.Name);

            return query;
        }
        public static void DeleteTransaction(IEnumerable<Transaction> transactions)
        {
            var db = new BudgetEntities();
            foreach (var curTransaction in transactions)
            {
                var query = (from transaction in db.Transactions
                             where transaction.Guid == curTransaction.Guid
                             select transaction).Single();
                db.Transactions.Remove(query);
            }
            db.SaveChanges();
        }
    }
}
