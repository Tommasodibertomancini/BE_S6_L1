using System.ComponentModel.DataAnnotations;

namespace BE_S6_L1.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Il campo Email è obbligatorio")]
        [EmailAddress(ErrorMessage = "Inserire un indirizzo email valido")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Il campo Nome è obbligatorio")]
        [Display(Name = "Nome")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Il campo Cognome è obbligatorio")]
        [Display(Name = "Cognome")]
        public string Cognome { get; set; }

        [Required(ErrorMessage = "Il campo Password è obbligatorio")]
        [StringLength(100, ErrorMessage = "La {0} deve essere lunga almeno {2} caratteri.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Conferma password")]
        [Compare("Password", ErrorMessage = "La password e la password di conferma non corrispondono.")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginViewModel
    {
        [Required(ErrorMessage = "Il campo Email è obbligatorio")]
        [EmailAddress(ErrorMessage = "Inserire un indirizzo email valido")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Il campo Password è obbligatorio")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Ricordami")]
        public bool RememberMe { get; set; }
    }
}
