using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hannet.Model.ViewModels
{
    public class AZoneViewModel
    {
        public int ZonId { get; set; }
        public string ZonName { get; set; }
        public string ZonDescription { get; set; }
        public bool? ZonStatus { get; set; }
        public int? PlaceId { get; set; }
    }

    public class AZoneHanet
    {
        public string name { get; set; }
        public string address { get; set; }
        public int? id { get; set; }
        public int? placeID { get; set; }
    }

    public class EmployeeHanet
    {
        public string name { get; set; }
        public byte[] file { get; set; }
        public string aliasID { get; set; }
        public string placeID { get; set; }
        public string title { get; set; }
        public int? type { get; set; }
    }

    public class UpdateFaceImage
    {
        public byte[] file { get; set; }
        public string aliasID { get; set; }
        public string placeID { get; set; }

    }

    public class DeleteEmployeeHannet
    {
       public int emId { get; set; }
       public string aliasID { get; set; }
    }

    public class PlaceModel
    {
        public string address { get; set; }
        public string name { get; set; }
        public int id { get; set; }
        public long userID { get; set; }
    }

    public class Place
    {
        public int returnCode { get; set; }
        public string returnMessage { get; set; }
        public PlaceModel data { get; set; }
    }

    public class DeleteZone
    {
        public int id { get; set; }
        public int placeId { get; set; }
    }

    public class GetCheckin
    {
        public string placeID { get; set; }
        public string date { get; set; }
    }
}
