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
    
    public partial class softwareProduct
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public softwareProduct()
        {
            this.Customer_Request = new HashSet<Customer_Request>();
            this.Deals = new HashSet<Deal>();
            this.PrePurchase_FollowUp_Plan = new HashSet<PrePurchase_FollowUp_Plan>();
            this.Product_responsible = new HashSet<Product_responsible>();
            this.productAttributes = new HashSet<productAttribute>();
            this.productMarketPlans = new HashSet<productMarketPlan>();
        }
    
        public int id { get; set; }
        public string SKU { get; set; }
        public string name { get; set; }
        public string keywords { get; set; }
        public string shortDescription { get; set; }
        public string fullDescription { get; set; }
        public bool isActive { get; set; }
        public double unitPrice { get; set; }
        public Nullable<double> altPrice1 { get; set; }
        public Nullable<double> altPrice2 { get; set; }
        public string screenShots { get; set; }
        public string icon { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Customer_Request> Customer_Request { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Deal> Deals { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PrePurchase_FollowUp_Plan> PrePurchase_FollowUp_Plan { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Product_responsible> Product_responsible { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<productAttribute> productAttributes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<productMarketPlan> productMarketPlans { get; set; }
    }
}