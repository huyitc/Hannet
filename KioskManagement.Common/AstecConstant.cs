using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KioskManagement.Common
{
    public static class AstecConstant
    {
        // Nhân viên
        public const int emTypeIDEmployee = 1;
        // Khách
        public const int emTypeIDCustomer = 3;
        // Cư dân
        public const int emTypeIDResident = 2;

        // TIME_ATTENDANCY_CHECK
        public const int FINGERPRINT_TIME_ATTENDANCY_CHECK = 1;
        public const int CARD_TIME_ATTENDANCY_CHECK = 3;
        public const int FINGERPRINT_AND_CARD_TIME_ATTENDANCY_CHECK = 2;

        // Const From Date
        public static DateTime CONST_FROM_DATE = new DateTime(2022, 8, 1);
        public static int DayToScheCode(string day)
        {
            int scheCode = 0;
            switch (day)
            {
                case "SUNDAY":
                    scheCode = 0;
                    break;
                case "MONDAY":
                    scheCode = 1;
                    break;
                case "TUESDAY":
                    scheCode = 2;
                    break;
                case "WEDNESDAY":
                    scheCode = 3;
                    break;
                case "THURSDAY":
                    scheCode = 4;
                    break;
                case "FRIDAY":
                    scheCode = 5;
                    break;
                case "SATURDAY":
                    scheCode = 6;
                    break;
            }
            return scheCode;
        }
        public static string ScheCodeToDay(int sche_Code)
        {
            string day = "";
            switch (sche_Code)
            {
                case 0:
                    day = "Chủ nhật";
                    break;
                case 1:
                    day = "Thứ 2";
                    break;
                case 2:
                    day = "Thứ 3";
                    break;
                case 3:
                    day = "Thứ 4";
                    break;
                case 4:
                    day = "Thứ 5";
                    break;
                case 5:
                    day = "Thứ 6";
                    break;
                case 6:
                    day = "Thứ 7";
                    break;
            }
            return day;
        }
        
        //RESULT_CHECK
        public const int RESULT_CHECK_NGHI = 0;
        public const int RESULT_CHECK_DI_LAM_BINH_THUONG = 1;
        public const int RESULT_CHECK_KHAC = 2;
        public const int RESULT_CHECK_NGHI_PHEP = 3;
        public const int RESULT_CHECK_NGHI_PHEP_KHONG_LUONG = 4;

        //TimeAttendance RESULT
        public const string RESULT_CHAM_DUNG_GIO = "OK";
        public const string RESULT_KHONG_CHAM_CONG = "NO";
        public const string RESULT_NGHI_PHEP = "NP";
        public const string RESULT_NGHI_PHEP_KHONG_LUONG = "NPKL";

        //TimeAttendance RESULT_ALL
        public const string RESULT_ALL_DI_LAM_BINH_THUONG = "Đi làm bình thường";
        public const string RESULT_ALL_NGHI = "Nghỉ";
        public const string RESULT_ALL_KHONG_CHAM_DU_CONG = "Không chấm đủ công";
        public const string RESULT_ALL_NGHI_PHEP = "NP";
        public const string RESULT_ALL_NGHI_PHEP_KHONG_LUONG = "NPKL";
        public const string RESULT_ALL_CONG_TAC_NGOAI_SANG = "Công tác ngoài sáng";
        public const string RESULT_ALL_CONG_TAC_NGOAI_CHIEU = " Công tác ngoài chiều";
        public const string RESULT_ALL_CONG_TAC_NGOAI = "Công tác ngoài";

        public const string SANG = "Sáng";
        public const string CHIEU = "Chiều";
        public const string CANGAY = "Cả ngày";
    }
}
