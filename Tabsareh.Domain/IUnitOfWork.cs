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

namespace Tabsareh.Domain
{
    public interface IUnitOfWork
    {
        IAdminRepository AdminRepository { get; set; }
        ISmsOtpRepository SmsOtpRepository { get; set; }
        IRoleRepository RoleRepository { get; set; }
        ITeacherRepository TeacherRepository { get; set; }
        IContentOwnerRepository ContentOwnerRepository { get; set; }
        IContentOwnerPaymentRepository ContentOwnerPaymentRepository { get; set; }
        ICategoryRepository CategoryRepository { get; set; }
        IUserRepository UserRepository { get; set; }
        IBlogRepository BlogRepository { get; set; }
        IDynamicPageRepository DynamicPageRepository { get; set; }
        IDiscountCodeRepository DiscountCodeRepository { get; set; }
        IOrderRepository OrderRepository { get; set; }
        ICourseRepository CourseRepository { get; set; }
        ICourseChapterRepository CourseChapterRepository { get; set; }
        ICourseCommentRepository CourseCommentRepository { get; set; }
    }
}
