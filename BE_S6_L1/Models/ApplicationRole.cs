using Microsoft.AspNetCore.Identity;

namespace BE_S6_L1.Models
{
    public class ApplicationRole
    {
        public ICollection<ApplicationUserRole> ApplicationUserRoles { get; set; }
    }
}
