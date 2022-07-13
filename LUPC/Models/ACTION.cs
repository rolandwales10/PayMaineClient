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
    
    public partial class Action
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Action()
        {
            this.Action_Name = new HashSet<Action_Name>();
            this.Action_Town = new HashSet<Action_Town>();
            this.Check_Record = new HashSet<Check_Record>();
            this.COC = new HashSet<COC>();
        }
    
        public int Action_ID { get; set; }
        public string Action_By { get; set; }
        public Nullable<int> Staff_ID { get; set; }
        public Nullable<int> Region_ID { get; set; }
        public Nullable<System.DateTime> Date_Received { get; set; }
        public Nullable<int> LU_Form_ID { get; set; }
        public Nullable<int> LU_Action_Category_ID { get; set; }
        public Nullable<int> LU_Action_Number_Type_ID { get; set; }
        public string Action_Number_Sequence { get; set; }
        public string Permit_Amendment { get; set; }
        public string Permit_Year { get; set; }
        public string Description { get; set; }
        public string Cross_Reference_Note { get; set; }
        public Nullable<decimal> Action_Fee { get; set; }
        public string Permitted_New_Dwelling { get; set; }
        public Nullable<int> Dwelling_Count { get; set; }
        public Nullable<int> Subdivision_Lot_Count { get; set; }
        public string Sealed { get; set; }
        public string Deleted { get; set; }
        public Nullable<System.DateTime> Enter_Date { get; set; }
        public string Entered_By { get; set; }
        public Nullable<System.DateTime> Edit_Date { get; set; }
        public string Edited_By { get; set; }
        public byte[] timestamp { get; set; }
    
        public virtual LU_Action_Number_Type LU_Action_Number_Type { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Action_Name> Action_Name { get; set; }
        public virtual Staff Staff { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Action_Town> Action_Town { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Check_Record> Check_Record { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<COC> COC { get; set; }
    }
}
