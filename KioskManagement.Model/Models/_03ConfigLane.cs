using System;
using System.Collections.Generic;

namespace KioskManagement.Model.Models
{
    public partial class _03ConfigLane
    {
        public int Id { get; set; }
        public int? GateId { get; set; }
        public string Name { get; set; }
        public string Lanetype { get; set; }
        public string Vehicletype { get; set; }
        public string Camview { get; set; }
        public string Camplate1 { get; set; }
        public string Camplate2 { get; set; }
        public string MaxCharHeight { get; set; }
        public string MinCharHeight { get; set; }
        public string DeviationAngle { get; set; }
        public string RotateAngle { get; set; }
        public int? AutoRecognition { get; set; }
        public string Roi { get; set; }
        public string Roicar { get; set; }
        public int? Orderby { get; set; }
        public int? MovePosition { get; set; }
        public string Recapture { get; set; }
        public string Editplate { get; set; }
        public string Printer { get; set; }
        public string Viewimg { get; set; }
        public string Opengate { get; set; }
    }
}
