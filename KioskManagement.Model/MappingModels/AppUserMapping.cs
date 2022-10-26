using Microsoft.AspNetCore.Identity;
using KioskManagement.Model.Models;
using System;
using System.Collections.Generic;

namespace KioskManagement.Model.MappingModels
{
    public class AppUserMapping : IdentityUser
    {
        public string FullName { get; set; }
        public byte[] Image { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { set; get; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public int? EmId { get; set; }
        public string EmName { get; set; }

        public virtual List<AppGroup> Groups { get; set; }
    }
}