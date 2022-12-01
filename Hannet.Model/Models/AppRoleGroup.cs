using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hannet.Model.Models
{
    /// <summary>
    /// Quyền trong nhóm
    /// </summary>
    [Table("AppRoleGroups")]
    public class AppRoleGroup
    {
        [Key]
        [Column(Order = 1)]
        public int GroupId { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [Column(Order = 2)]
        [StringLength(450)]
        public string RoleId { set; get; }

        [ForeignKey("RoleId")]
        public virtual AppRole AppRole { set; get; }

        [ForeignKey("GroupId")]
        public virtual AppGroup AppGroup { set; get; }
    }
}