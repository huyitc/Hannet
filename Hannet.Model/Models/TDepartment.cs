using System;
using System.Collections.Generic;

namespace Hannet.Model.Models
{
    public partial class TDepartment
    {
        public TDepartment()
        {
            TEmployees = new HashSet<TEmployee>();
        }

        public int DepId { get; set; }
        public string DepName { get; set; }
        public string DepDescription { get; set; }
        public bool? DepStatus { get; set; }

        public virtual ICollection<TEmployee> TEmployees { get; set; }
    }
}
