//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebRole1.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Plan_Step
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Plan_Step()
        {
            this.Plan_Step1 = new HashSet<Plan_Step>();
            this.Plan_Step11 = new HashSet<Plan_Step>();
        }
    
        public int id { get; set; }
        public int planID { get; set; }
        public Nullable<int> previousStep { get; set; }
        public Nullable<int> nextStep { get; set; }
        public int stepNo { get; set; }
        public string StepEmailContent { get; set; }
        public Nullable<int> TimeFromLastStep { get; set; }
        public Nullable<System.DateTime> sendDate { get; set; }
        public string subject { get; set; }
        public bool RequireMoreDetail { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Plan_Step> Plan_Step1 { get; set; }
        public virtual Plan_Step Plan_Step2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Plan_Step> Plan_Step11 { get; set; }
        public virtual Plan_Step Plan_Step3 { get; set; }
        public virtual PrePurchase_FollowUp_Plan PrePurchase_FollowUp_Plan { get; set; }
    }
}