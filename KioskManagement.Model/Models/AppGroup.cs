using KioskManagement.Model.Abtracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KioskManagement.Model.Models
{
        public class AppGroup : Auditable
        {
            [StringLength(50)]
            public string GroupCode { get; set; }
            [StringLength(50)]
            public string Name { get; set; }
        }
}
