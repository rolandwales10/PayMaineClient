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
    
    public partial class Action_Town
    {
        public int Action_Town_ID { get; set; }
        public int Action_ID { get; set; }
        public int Town_ID { get; set; }
        public Nullable<System.DateTime> Enter_Date { get; set; }
        public string Entered_By { get; set; }
        public Nullable<System.DateTime> Edit_Date { get; set; }
        public string Edited_By { get; set; }
        public byte[] timestamp { get; set; }
    
        public virtual Action Action { get; set; }
        public virtual Town Town { get; set; }
    }
}
