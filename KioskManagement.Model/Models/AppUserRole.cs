using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace KioskManagement.Model.Models
{
    // <summary>
    /// Quyền được cấp cho tài khoản
    /// </summary>
    [Table("AppUserRoles")]
    public class AppUserRole : IdentityUserRole<string>
    {
        //[Key]
        //[Column(Order = 1)]
        //[StringLength(128)]
        //public string UserId { set; get; }

        //[Key]
        //[Column(Order = 2)]
        //[StringLength(128)]
        //public string RoleId { set; get; }

        //[ForeignKey("UserId")]
        //public virtual AppUser AppUser { set; get; }

        //[ForeignKey("RoleId")]
        //public virtual AppRole AppRole { set; get; }
    }
}
