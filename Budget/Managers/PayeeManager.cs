using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Budget.Entities;

namespace Budget.Managers
{
    static class PayeeManager
    {
        public static IQueryable<Payee> GetPayees(Boolean? active = true)
        {
            var db = new BudgetEntities();
            var query = from payee in db.Payees
                        select payee;
            if (active != null)
                query = query.Where(p => p.Active == active);
            query = query.OrderBy(p => p.Name);
            return query;
        }

        public static void AddPayee(String name, Boolean active = true)
        {
            var db = new BudgetEntities();
            var payee = new Payee();
            payee.Name = name;
            payee.Active = active;
            payee.Guid = Guid.NewGuid();
            db.Payees.Add(payee);
            db.SaveChanges();
        }
    }
}
