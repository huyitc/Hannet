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
            TCardNos = new HashSet<TCardNo>();
            TEmployeeFaces = new HashSet<TEmployeeFace>();
            TEmployeeFingers = new HashSet<TEmployeeFinger>();
        }

        public int EmId { get; set; }
        public int? EmTypeId { get; set; }
        public int? RegId { get; set; }
        public int? DepId { get; set; }
        public int? EmIdCreated { get; set; }
        public string EmCode { get; set; }
        public string EmName { get; set; }
        public string EmGender { get; set; }
        public DateTime? EmBirthdate { get; set; }
        public string EmIdentityNumber { get; set; }
        public string EmAddress { get; set; }
        public string EmPhone { get; set; }
        public string EmEmail { get; set; }
        public byte[] EmImage { get; set; }
        public bool? EmTimeCheck { get; set; }
        public bool? EmStatus { get; set; }
        public bool? EditStatus { get; set; }
        public string Pin { get; set; }
        public int? GaId { get; set; }
        public bool? FaceExist { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? SchDevId { get; set; }
        public string DevIdSynchronized { get; set; }

        public virtual TDepartment Dep { get; set; }
        public virtual TEmployeeType EmType { get; set; }
        public virtual TGroupAccess Ga { get; set; }
        public virtual TRegency Reg { get; set; }
        public virtual ICollection<ATimeAdd> ATimeAdds { get; set; }
        public virtual ICollection<TAccount> TAccounts { get; set; }
        public virtual ICollection<TCardNo> TCardNos { get; set; }
        public virtual ICollection<TEmployeeFace> TEmployeeFaces { get; set; }
        public virtual ICollection<TEmployeeFinger> TEmployeeFingers { get; set; }
    }
}
