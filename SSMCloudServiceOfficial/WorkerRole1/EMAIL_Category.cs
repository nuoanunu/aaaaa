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
    
    public partial class EMAIL_Category
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public EMAIL_Category()
        {
            this.Email_Template = new HashSet<Email_Template>();
        }
    
        public int id { get; set; }
        public string Name { get; set; }
        public string description { get; set; }
        public Nullable<System.DateTime> lastupdate { get; set; }
        public Nullable<System.DateTime> createddate { get; set; }
        public string creator { get; set; }
        public string color { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Email_Template> Email_Template { get; set; }
    }
}
