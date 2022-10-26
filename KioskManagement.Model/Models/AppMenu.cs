using KioskManagement.Model.Abtracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KioskManagement.Model.Models
{
    public class AppMenu : Auditable
    {
        /// <summary>
        /// Tên menu
        /// </summary>
        [StringLength(100)]
        public string MenuName { get; set; }

        /// <summary>
        /// Id menu cha
        /// </summary>
        public int? ParentId { get; set; }

        /// <summary>
        /// Icon chức năng
        /// </summary>
        [StringLength(100)]
        public string Icon { get; set; }

        /// <summary>
        /// Đường dẫn tới chức năng
        /// </summary>
        [StringLength(100)]
        public string Link { get; set; }

        [StringLength(100)]
        public string ActiveLink { get; set; }
    }
}
