using System;
using System.Collections.Generic;

namespace KioskManagement.Model.Models
{
    public partial class TEmployee
    {
        public TEmployee()
        {
            ATimeAdds = new HashSet<ATimeAdd>();
            TAccounts = new HashSet<TAccount>();
            TEmployeeFaces = new HashSet<TEmployeeFace>();
        }

        public int EmId { get; set; }
        public int? EmTypeId { get; set; }
        public int? RegId { get; set; }
        public int? DepId { get; set; }
        public string EmCode { get; set; }
        public string EmName { get; set; }
        public string EmGender { get; set; }
        public DateTime? EmBirthdate { get; set; }
        public string EmIdentityNumber { get; set; }
        public string Description { get; set; }
        public string EmPhone { get; set; }
        public string EmEmail { get; set; }
        public byte[] EmImage { get; set; }
        public bool? EmStatus { get; set; }
        public bool? FaceExist { get; set; }
        public int PlaceId { get; set; }
        public int ZonId { get; set; }

        public virtual TDepartment Dep { get; set; }
        public virtual TEmployeeType EmType { get; set; }
        public virtual TGroupAccess Ga { get; set; }
        public virtual TRegency Reg { get; set; }
        public virtual AZone Zon { get; set; }
        public virtual ICollection<ATimeAdd> ATimeAdds { get; set; }
        public virtual ICollection<TAccount> TAccounts { get; set; }
        public virtual ICollection<TEmployeeFace> TEmployeeFaces { get; set; }
    }
}
