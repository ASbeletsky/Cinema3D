using System.ComponentModel.DataAnnotations;

namespace VideoLib.WebUI.Models.Auth
{
    public class AdminLoginViewModel
    {
            [Required]
            [Display(Name = "Электонная почта")]
            public string UserName { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Пароль")]
            public string Password { get; set; }

            [Display(Name = "Запомнить?")]
            public bool RememberMe { get; set; }
    }
}