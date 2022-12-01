using System;
using System.Collections.Generic;

namespace Hannet.Model.Models
{
    public partial class ATimeAdd
    {
        public int TiaId { get; set; }
        public int? EmId { get; set; }
        public int? ProId { get; set; }
        public DateTime? TiaFrom { get; set; }
        public DateTime? TiaTo { get; set; }

        public virtual TEmployee Em { get; set; }
        public virtual AProject Pro { get; set; }
    }
}
