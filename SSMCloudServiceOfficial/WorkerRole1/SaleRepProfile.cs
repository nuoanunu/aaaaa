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
    
    public partial class SaleRepProfile
    {
        public int id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Avatar { get; set; }
        public string Suffix { get; set; }
        public Nullable<System.DateTime> EmployedScine { get; set; }
        public Nullable<int> Prospecting { get; set; }
        public Nullable<int> NeedsAnalysisQuestions { get; set; }
        public Nullable<int> ReferenceStories { get; set; }
        public Nullable<int> Gatekeepers { get; set; }
        public Nullable<int> Opening { get; set; }
        public Nullable<int> NeedsAnalysis { get; set; }
        public Nullable<int> Budget { get; set; }
        public Nullable<int> BuyingCommittee { get; set; }
        public Nullable<int> Risk { get; set; }
        public Nullable<int> Timeline { get; set; }
        public Nullable<int> Objections { get; set; }
        public Nullable<int> Presentation { get; set; }
        public Nullable<int> NextSteClosing { get; set; }
        public Nullable<int> CrosssellUpsell { get; set; }
        public Nullable<int> Integrity { get; set; }
        public Nullable<int> Competency { get; set; }
        public Nullable<int> Recognition { get; set; }
        public Nullable<int> ProActivity { get; set; }
        public Nullable<int> Chemistry { get; set; }
        public Nullable<int> Savvy { get; set; }
        public Nullable<int> LOUs { get; set; }
        public Nullable<int> ProposalCreation { get; set; }
        public Nullable<int> AdministrativeCRM { get; set; }
        public Nullable<int> Territoryplanning { get; set; }
        public string userID { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public Nullable<System.DateTime> dateOfBirth { get; set; }
        public Nullable<double> VolumeOfSales { get; set; }
        public Nullable<int> newContactmade { get; set; }
        public Nullable<double> winrate { get; set; }
        public Nullable<double> SaleCycle { get; set; }
        public Nullable<double> avarageActivitiesPerdeal { get; set; }
        public Nullable<double> AvarageKPI { get; set; }
    
        public virtual AspNetUser AspNetUser { get; set; }
    }
}
