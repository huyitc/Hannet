using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hannet.Model.Models
{
    /// <summary>
    /// Nhóm người dùng được cấp cho tài khoản
    /// </summary>
    [Table("AppUserGroups")]
    public class AppUserGroup
    {
        [StringLength(450)]
        [Key]
        [Column(Order = 1)]
        public string UserId { set; get; }
        [Column(Order = 2)]
        public int GroupId { set; get; }

        [ForeignKey("UserId")]
        public virtual AppUser AppUser { set; get; }

        [ForeignKey("GroupId")]
        public virtual AppGroup AppGroup { set; get; }
    }
}