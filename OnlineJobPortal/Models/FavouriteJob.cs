//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OnlineJobPortal.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class FavouriteJob
    {
        public int Id { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<int> JobId { get; set; }
    
        public virtual Job Job { get; set; }
        public virtual User User { get; set; }
    }
}