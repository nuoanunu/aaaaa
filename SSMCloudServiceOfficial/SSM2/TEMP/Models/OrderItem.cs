//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SSM.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class OrderItem
    {
        public int id { get; set; }
        public int orderID { get; set; }
        public int planID { get; set; }
        public double SoldPrice { get; set; }
        public string SaleRepBenefit { get; set; }
        public int LicenseID { get; set; }
    
        public virtual AspNetUser AspNetUser { get; set; }
        public virtual License License { get; set; }
        public virtual order order { get; set; }
        public virtual productMarketPlan productMarketPlan { get; set; }
    }
}
