using Tabsareh.Domain.Models.Admins;
using Tabsareh.Domain.Models.Categories;
using Tabsareh.Domain.Models.ContentOwners;
using Tabsareh.Domain.Models.Roles;
using Tabsareh.Domain.Models.Teachers;
using Tabsareh.Domain.Models.Users;

namespace Tabsareh.Domain
{
    public interface IUnitOfWork
    {
        IAdminRepository AdminRepository { get; set; }
        IRoleRepository RoleRepository { get; set; }
        ITeacherRepository TeacherRepository { get; set; }
        IContentOwnerRepository ContentOwnerRepository { get; set; }
        ICategoryRepository CategoryRepository { get; set; }
        IUserRepository UserRepository { get; set; }
    }
}
