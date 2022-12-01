using System;
using System.Collections.Generic;

namespace Hannet.Model.Models
{
    public partial class TRegency
    {
        public TRegency()
        {
            TEmployees = new HashSet<TEmployee>();
        }

        public int RegId { get; set; }
        public string RegName { get; set; }
        public string RegDescription { get; set; }
        public bool? RegStatus { get; set; }

        public virtual ICollection<TEmployee> TEmployees { get; set; }
    }
}
