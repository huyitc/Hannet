using Microsoft.AspNetCore.Identity;
using System;

namespace Hannet.Model.ViewModels
{
    public class AppRoleViewModel : IdentityRole
    {
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public string Description { get; set; }
        public string ParentId { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string Icon { get; set; }
        public string Link { get; set; }
        public string ActiveLink { get; set; }
        public int? Order_By { get; set; }

    }
}