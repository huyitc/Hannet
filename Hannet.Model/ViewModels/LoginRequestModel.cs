using System.ComponentModel.DataAnnotations;

namespace Hannet.Model.ViewModels
{
    public class LoginRequestModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}