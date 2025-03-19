using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace BE_S6_L1.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Nome { get; set; }
        public string Cognome { get; set; }
    }
}
