using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KioskManagement.Model.ViewModels
{
    public class TEmployeeViewModel
    {
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
    }
}
