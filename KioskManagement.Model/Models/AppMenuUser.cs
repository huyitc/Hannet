using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KioskManagement.Model.Models
{
    /// <summary>
    /// Menu được cấp cho tài khoản
    /// </summary>
    [Table("AppMenuUsers")]
    public class AppMenuUser
    {
        [Key]
        [Column(Order = 1)]
        [StringLength(450)]
        public string UserId { get; set; }
        [Column(Order = 2)]
        public int MenuId { get; set; }

        [ForeignKey("UserId")]
        public virtual AppUser AppUser { get; set; }

        [ForeignKey("MenuId")]
        public virtual AppMenu AppMenu { get; set; }
    }
}