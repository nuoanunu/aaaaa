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
    
    public partial class PlanOption
    {
        public int id { get; set; }
        public int orderItemID { get; set; }
        public int optionID { get; set; }
    
        public virtual attributeOption attributeOption { get; set; }
        public virtual productMarketPlan productMarketPlan { get; set; }
    }
}
