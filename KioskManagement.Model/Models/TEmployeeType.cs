using System;
using System.Collections.Generic;

namespace KioskManagement.Model.Models
{
    public partial class TEmployeeType
    {
        public TEmployeeType()
        {
            TEmployees = new HashSet<TEmployee>();
        }

        public int EmTypeId { get; set; }
        public string EmType { get; set; }
        /// <summary>
        /// 1 là nhân viên, 0 là cư dân, 2 khách hàng ...
        /// </summary>
        public int? EmCheck { get; set; }

        public virtual ICollection<TEmployee> TEmployees { get; set; }
    }
}
