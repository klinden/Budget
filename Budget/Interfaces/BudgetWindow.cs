using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Budget.Interfaces
{
    public interface BudgetWindow
    {
        BudgetWindow CallingWindow {get; set;}
        void RefreshWindow();
    }
}
