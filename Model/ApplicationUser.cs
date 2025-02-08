using Microsoft.AspNetCore.Identity;
using TalStorage.Models;

namespace TalStorage.Models
{
    public class ApplicationUser : IdentityUser, IBaseEntity
    {
        public string FullName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
