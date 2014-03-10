using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Budget.Entities;
using Budget.Classes;
using System.Configuration;

namespace Budget.Managers
{
    public static class AccountManager
    {
        public static IQueryable<Account> GetAccounts(Boolean? active = true)
        {
            var db = new BudgetEntities();
            var query = from account in db.Accounts
                        select account;
            if (active != null)
                query = query.Where(p => p.Active == active);
            query = query.OrderBy(p => p.Name);
            return query;
        }

        public static IQueryable<AccountBalance> GetAccountBalances(Boolean? active = true)
        {
            var db = new BudgetEntities();
            var query = from account in db.Accounts
                        select new AccountBalance { Account = account, Balance = account.Transactions.Sum(p => p.Amount.Value) };
            if (active != null)
                query = query.Where(p => p.Account.Active == active);
            query = query.OrderBy(p => p.Account.Name);
            return query;
        }
    }
}
