//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WorkerRole1
{
    using System;
    using System.Collections.Generic;
    
    public partial class TrialAccount
    {
        public int AccountID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int PlanID { get; set; }
        public int Status { get; set; }
        public Nullable<int> contactID { get; set; }
    
        public virtual contact contact { get; set; }
        public virtual productMarketPlan productMarketPlan { get; set; }
    }
}
