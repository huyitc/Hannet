using Hannet.Model.Abtracts;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hannet.Model.Models
{
    public class AppRole : IdentityRole, IAuditable
    {
        [StringLength(450)]
        public string CreatedBy { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }

        [StringLength(450)]
        public string UpdatedBy { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? UpdatedDate { get; set; }

        public bool IsDeleted { get; set; }
        int IAuditable.Id { get; set; }

        [StringLength(450)]
        public string DeletedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedDate { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        [StringLength(50)]
        public string Description { get; set; }

        /// <summary>
        /// Id quyền cha
        /// </summary>
        [StringLength(450)]
        public string ParentId { get; set; }

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

        public int? Order_By { get; set; }
    }
}
