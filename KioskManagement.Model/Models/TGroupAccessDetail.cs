using System;
using System.Collections.Generic;

namespace KioskManagement.Model.Models
{
    public partial class TGroupAccessDetail
    {
        public int GadId { get; set; }
        public int? GaId { get; set; }
        public int? DevId { get; set; }
        public bool? GadStatus { get; set; }

        public virtual TDevice Dev { get; set; }
        public virtual TGroupAccess Ga { get; set; }
    }
}
