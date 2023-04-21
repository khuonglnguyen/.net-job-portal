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
    
    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            this.AppliedJobs = new HashSet<AppliedJob>();
            this.FavouriteJobs = new HashSet<FavouriteJob>();
        }
    
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string TenthGrade { get; set; }
        public string TwelfthGrade { get; set; }
        public string GraduationGrade { get; set; }
        public string PostGraduationGrade { get; set; }
        public string Phd { get; set; }
        public string WorksOn { get; set; }
        public string Experience { get; set; }
        public string Resume { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public Nullable<int> RoleId { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AppliedJob> AppliedJobs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FavouriteJob> FavouriteJobs { get; set; }
        public virtual Role Role { get; set; }
    }
}
