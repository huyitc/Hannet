using Hannet.Model.Abtracts;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;

namespace Hannet.Model.Models
{
    [Table("AppUsers")]
    public class AppUser : IdentityUser, IAuditable
    {
        /// <summary>
        /// Họ tên
        /// </summary>
        [StringLength(50)]
        public string FullName { get; set; }

        /// <summary>
        /// Ảnh đại diện
        /// </summary>
        public byte[] Image { get; set; }

        [StringLength(450)]
        public string CreatedBy { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }

        [StringLength(450)]
        public string UpdatedBy { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? UpdatedDate { get; set; }
        public bool Status { get; set; }
        public bool IsDeleted { get; set; }
        int IAuditable.Id { get; set; }

        [StringLength(450)]
        public string DeletedBy { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? DeletedDate { get; set; }
        public int? EM_ID { get; set; }

        public async Task<IdentityResult> GenerateUserIdentityAsync(UserManager<AppUser> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
    }
}
