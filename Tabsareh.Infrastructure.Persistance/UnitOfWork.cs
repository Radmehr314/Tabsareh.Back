using Tabsareh.Domain;
using Tabsareh.Domain.Models.Admins;
using Tabsareh.Domain.Models.Auth;
using Tabsareh.Domain.Models.Blogs;
using Tabsareh.Domain.Models.Categories;
using Tabsareh.Domain.Models.ContentOwners;
using Tabsareh.Domain.Models.CourseComments;
using Tabsareh.Domain.Models.Courses;
using Tabsareh.Domain.Models.DynamicPages;
using Tabsareh.Domain.Models.Discounts;
using Tabsareh.Domain.Models.Orders;
using Tabsareh.Domain.Models.Roles;
using Tabsareh.Domain.Models.Teachers;
using Tabsareh.Domain.Models.Users;

namespace Tabsareh.Infrastructure.Persistance
{
    public class UnitOfWork : IUnitOfWork
    {
        public IAdminRepository AdminRepository { get; set; }
        public ISmsOtpRepository SmsOtpRepository { get; set; }
        public IRoleRepository RoleRepository { get; set; }
        public ITeacherRepository TeacherRepository { get; set; }
        public IContentOwnerRepository ContentOwnerRepository { get; set; }
        public IContentOwnerPaymentRepository ContentOwnerPaymentRepository { get; set; }
        public ICategoryRepository CategoryRepository { get; set; }
        public IUserRepository UserRepository { get; set; }
        public IBlogRepository BlogRepository { get; set; }
        public IDynamicPageRepository DynamicPageRepository { get; set; }
        public IDiscountCodeRepository DiscountCodeRepository { get; set; }
        public IOrderRepository OrderRepository { get; set; }
        public ICourseRepository CourseRepository { get; set; }
        public ICourseChapterRepository CourseChapterRepository { get; set; }
        public ICourseCommentRepository CourseCommentRepository { get; set; }

        public UnitOfWork(
            IAdminRepository adminRepository,
            ISmsOtpRepository smsOtpRepository,
            IRoleRepository roleRepository,
            ITeacherRepository teacherRepository,
            IContentOwnerRepository contentOwnerRepository,
            IContentOwnerPaymentRepository contentOwnerPaymentRepository,
            ICategoryRepository categoryRepository,
            IUserRepository userRepository,
            IBlogRepository blogRepository,
            IDynamicPageRepository dynamicPageRepository,
            IDiscountCodeRepository discountCodeRepository,
            IOrderRepository orderRepository,
            ICourseRepository courseRepository,
            ICourseChapterRepository courseChapterRepository,
            ICourseCommentRepository courseCommentRepository)
        {
            AdminRepository = adminRepository;
            SmsOtpRepository = smsOtpRepository;
            RoleRepository = roleRepository;
            TeacherRepository = teacherRepository;
            ContentOwnerRepository = contentOwnerRepository;
            ContentOwnerPaymentRepository = contentOwnerPaymentRepository;
            CategoryRepository = categoryRepository;
            UserRepository = userRepository;
            BlogRepository = blogRepository;
            DynamicPageRepository = dynamicPageRepository;
            DiscountCodeRepository = discountCodeRepository;
            OrderRepository = orderRepository;
            CourseRepository = courseRepository;
            CourseChapterRepository = courseChapterRepository;
            CourseCommentRepository = courseCommentRepository;
        }
    }
}
