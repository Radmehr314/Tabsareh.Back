using Tabsareh.Domain;
using Tabsareh.Domain.Models.Admins;
using Tabsareh.Domain.Models.ContentOwners;
using Tabsareh.Domain.Models.Roles;
using Tabsareh.Domain.Models.Teachers;

namespace Tabsareh.Infrastructure.Persistance
{
    public class UnitOfWork : IUnitOfWork
    {
        public IAdminRepository AdminRepository { get; set; }
        public IRoleRepository RoleRepository { get; set; }
        public ITeacherRepository TeacherRepository { get; set; }
        public IContentOwnerRepository ContentOwnerRepository { get; set; }

        public UnitOfWork(
            IAdminRepository adminRepository,
            IRoleRepository roleRepository,
            ITeacherRepository teacherRepository,
            IContentOwnerRepository contentOwnerRepository)
        {
            AdminRepository = adminRepository;
            RoleRepository = roleRepository;
            TeacherRepository = teacherRepository;
            ContentOwnerRepository = contentOwnerRepository;
        }
    }
}
