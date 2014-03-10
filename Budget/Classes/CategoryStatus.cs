using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Budget.Entities;

namespace Budget.Classes
{
    public class CategoryStatus
    {
        public Category RelatedCategory { get; set; }
        public double AdjustedBudgetedAmount { get; set; }
        public double NetGainLoss { get; set; }
        public double AmountLeft { get { return AdjustedBudgetedAmount + NetGainLoss; } }
    }
}
