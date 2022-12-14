using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hannet.Model.ViewModels
{
    public class TEmployeeViewModel
    {
        public int EmId { get; set; }
        public int? EmTypeId { get; set; }
        public int? RegId { get; set; }
        public int? DepId { get; set; }
        public string EmCode { get; set; }
        public string EmName { get; set; }
        public string EmGender { get; set; }
        public DateTime? EmBirthdate { get; set; }
        public string EmIdentityNumber { get; set; }
        public string EmAddress { get; set; }
        public string Description { get; set; }
        public string EmPhone { get; set; }
        public string EmEmail { get; set; }
        public byte[] EmImage { get; set; }
        public bool? EmStatus { get; set; }
        public int? PlaceId { get; set; }
        public int? ZonId { get; set; }
    }
}
