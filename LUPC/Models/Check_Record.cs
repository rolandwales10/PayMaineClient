//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LUPC.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Check_Record
    {
        public int Check_Record_ID { get; set; }
        public Nullable<System.DateTime> Date_Deposit { get; set; }
        public Nullable<System.DateTime> Date_Received { get; set; }
        public string Check_Number { get; set; }
        public Nullable<System.DateTime> Check_Date { get; set; }
        public string Check_Name { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<int> Action_ID { get; set; }
        public Nullable<int> LU_Fee_ID { get; set; }
        public string Comment { get; set; }
        public Nullable<int> Staff_ID { get; set; }
        public Nullable<System.DateTime> Sent_To_Augusta { get; set; }
        public string Payment_Type { get; set; }
        public string Status { get; set; }
        public Nullable<decimal> Application_Transaction_Fee { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Address { get; set; }
        public string Zip_Code { get; set; }
        public string Email { get; set; }
        public Nullable<System.DateTime> Enter_Date { get; set; }
        public string Entered_By { get; set; }
        public Nullable<System.DateTime> Edit_Date { get; set; }
        public string Edited_By { get; set; }
        public byte[] timestamp { get; set; }
    
        public virtual Action Action { get; set; }
        public virtual LU_Fee LU_Fee { get; set; }
        public virtual Staff Staff { get; set; }
    }
}