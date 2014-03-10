using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Budget.Entities;
using Budget.Classes;
using System.Configuration;

namespace Budget.Managers
{
    public static class CategoryManager
    {
        public static IQueryable<Category> GetCategories(int year, Boolean? active = true)
        {
            var db = new BudgetEntities();
            var query = from category in db.Categories
                        where category.Year == year
                        select category;
            if (active != null)
                query = query.Where(p => p.Active == active);
            query = query.OrderBy(p => p.Name);
            return query;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="year">The year to pull records from</param>
        /// <param name="startMonth">Integer value corresponding to the starting month</param>
        /// <param name="endMonth">Integer value corresponding to the starting month</param>
        /// <param name="active">Optional value specifying whether or not to only return active categories (null implies both active and inactive)</param>
        /// <returns></returns>
        public static IEnumerable<CategoryStatus> GetBudgetStatus(int year, int startMonth, int endMonth, Boolean? active = true)
        {
            DateTime startDate = new DateTime(year, startMonth, 1);
            DateTime endDate = new DateTime(year, endMonth, DateTime.DaysInMonth(year, endMonth));
            var db = new BudgetEntities();
            var query = from category in db.Categories
                        where category.Year == year
                        select new CategoryStatus 
                        { 
                            RelatedCategory = category,
                            AdjustedBudgetedAmount = (category.Budgeted / 12) * (endMonth - startMonth + 1),
                            NetGainLoss = category.TransactionCategories.Where(p => p.Transaction1.Date >= startDate && p.Transaction1.Date <= endDate).Sum(p => p.Amount).HasValue ? category.TransactionCategories.Where(p => p.Transaction1.Date >= startDate && p.Transaction1.Date <= endDate).Sum(p => p.Amount).Value : 0
                        };
            if (active != null)
                query = query.Where(p => p.RelatedCategory.Active == active);
            query = query.OrderBy(p => p.RelatedCategory.Name);
            return query;
        }
    }
}
