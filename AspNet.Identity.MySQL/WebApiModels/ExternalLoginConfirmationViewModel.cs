using System.ComponentModel.DataAnnotations;

namespace AspNet.Identity.MySQL.WebApiModels
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}