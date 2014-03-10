using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Budget.Entities;

namespace Budget.Managers
{
    public static class DBManager
    {
        public static Boolean Get(Account acct)
        {
            var db = new BudgetEntities();
            var query = from account in db.Accounts
                        where account.Guid == acct.Guid
                        select account;
            return (query.Count() > 0);
        }

        public static Boolean Get(Category cat)
        {
            var db = new BudgetEntities();
            var query = from category in db.Categories
                        where category.Guid == cat.Guid
                        select category;
            return (query.Count() > 0);
        }
    }
}
