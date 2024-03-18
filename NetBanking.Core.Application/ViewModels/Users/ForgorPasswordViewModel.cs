using System.ComponentModel.DataAnnotations;

namespace NetBanking.Core.Application.ViewModels.Users
{
    public class ForgorPasswordViewModel
    {
        [Required(ErrorMessage = "Debe colocar el correo del usuario")]
        [DataType(DataType.Text)]
        public string Email { get; set; }
        public bool HasError { get; set; }
        public string Error { get; set; }
    }
}
