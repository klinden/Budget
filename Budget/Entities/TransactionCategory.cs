//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Budget.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class TransactionCategory
    {
        public System.Guid Guid { get; set; }
        public System.Guid Transaction { get; set; }
        public System.Guid Category { get; set; }
        public Nullable<double> Amount { get; set; }
    
        public virtual Category Category1 { get; set; }
        public virtual Transaction Transaction1 { get; set; }
    }
}
