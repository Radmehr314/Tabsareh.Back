using Tabsareh.Domain;
using Tabsareh.Domain.Models.Admins;
using Tabsareh.Domain.Models.Blogs;
using Tabsareh.Domain.Models.Categories;
using Tabsareh.Domain.Models.ContentOwners;
using Tabsareh.Domain.Models.Roles;
using Tabsareh.Domain.Models.Teachers;
using Tabsareh.Domain.Models.Users;

namespace Tabsareh.Infrastructure.Persistance
{
    public class UnitOfWork : IUnitOfWork
    {
        public IAdminRepository AdminRepository { get; set; }
        public IRoleRepository RoleRepository { get; set; }
        public ITeacherRepository TeacherRepository { get; set; }
        public IContentOwnerRepository ContentOwnerRepository { get; set; }
        public ICategoryRepository CategoryRepository { get; set; }
        public IUserRepository UserRepository { get; set; }
        public IBlogRepository BlogRepository { get; set; }

        public UnitOfWork(
            IAdminRepository adminRepository,
            IRoleRepository roleRepository,
            ITeacherRepository teacherRepository,
            IContentOwnerRepository contentOwnerRepository,
            ICategoryRepository categoryRepository,
            IUserRepository userRepository,
            IBlogRepository blogRepository)
        {
            AdminRepository = adminRepository;
            RoleRepository = roleRepository;
            TeacherRepository = teacherRepository;
            ContentOwnerRepository = contentOwnerRepository;
            CategoryRepository = categoryRepository;
            UserRepository = userRepository;
            BlogRepository = blogRepository;
        }
    }
}
