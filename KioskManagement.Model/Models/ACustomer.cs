using System;
using System.Collections.Generic;

namespace KioskManagement.Model.Models
{
    public partial class ACustomer
    {
        public int TtId { get; set; }
        public byte[] TtMatTruoc { get; set; }
        public byte[] TtMatSau { get; set; }
        public byte[] TtAnhChup { get; set; }
        public string TtSoGiayTo { get; set; }
        public string TtHoTen { get; set; }
        public string TtNgaySinh { get; set; }
        public string TtGioiTinh { get; set; }
        public string TtQuocTich { get; set; }
        public string TtQueQuan { get; set; }
        public string TtThuongTru { get; set; }
        public string TtNgayHetHan { get; set; }
        public int? EmIdCreated { get; set; }
        public string TtNgayCap { get; set; }
        public string TtNoiCap { get; set; }
        public int? TtLoaiGt { get; set; }
        public string TtDvCt { get; set; }
    }
}
