using System.ComponentModel.DataAnnotations;

namespace KioskManagement.Model.ViewModels
{
    public class LoginRequestModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}