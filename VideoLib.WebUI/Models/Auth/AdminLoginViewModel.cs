using System.ComponentModel.DataAnnotations;

namespace VideoLib.WebUI.Models.Auth
{
    public class AdminLoginViewModel
    {
            [Required]
            public string UserName { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
    }
}