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
    
    public partial class Calendar
    {
        public int id { get; set; }
        public string userID { get; set; }
        public System.DateTime startTime { get; set; }
        public System.DateTime endTime { get; set; }
        public bool repeat { get; set; }
    
        public virtual AspNetUser AspNetUser { get; set; }
    }
}
