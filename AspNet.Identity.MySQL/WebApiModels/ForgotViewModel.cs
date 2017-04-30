using System.ComponentModel.DataAnnotations;

namespace AspNet.Identity.MySQL.WebApiModels
{
    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}