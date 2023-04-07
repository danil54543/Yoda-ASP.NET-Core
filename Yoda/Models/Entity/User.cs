using Microsoft.AspNetCore.Identity;

namespace Yoda.Models.Entity
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
        public List<Note>? Notes { get; set; }
    }
}
