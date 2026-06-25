using Tabsareh.Application.Contracts.QueryResult.Orders;
using Tabsareh.Domain;
using Tabsareh.Domain.Models.Courses;
using Tabsareh.Domain.Models.Discounts;
using Tabsareh.Framework.Application.Exceptions;

namespace Tabsareh.Application.Services
{
    internal static class OrderInvoiceBuilder
    {
        public static async Task<(OrderInvoiceResult Invoice, List<Course> Courses, DiscountCode? DiscountCode)> BuildAsync(
            IUnitOfWork unitOfWork,
            List<string> courseIds,
            string? discountCode)
        {
            var distinctCourseIds = courseIds
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x.Trim())
                .Distinct()
                .ToList();

            if (distinctCourseIds.Count == 0)
                throw new UserAccessException("At least one course is required.");

            var courses = new List<Course>();
            foreach (var courseId in distinctCourseIds)
            {
                var course = await unitOfWork.CourseRepository.GetByIdAsync(courseId);
                if (course is null || course.IsDeleted || !course.IsActive)
                    throw new NotFoundException("Course not found.");
                courses.Add(course);
            }

            DiscountCode? discount = null;
            var discountPercent = 0m;
            var normalizedCode = discountCode?.Trim().ToUpperInvariant();
            if (!string.IsNullOrWhiteSpace(normalizedCode))
            {
                discount = await unitOfWork.DiscountCodeRepository.GetByCodeAsync(normalizedCode);
                if (discount is null || !discount.CanUse(DateTime.Now))
                    throw new UserAccessException("Discount code is not valid.");
                discountPercent = discount.DiscountPercent;
            }

            var invoice = new OrderInvoiceResult
            {
                DiscountCode = normalizedCode,
                DiscountCodePercent = discountPercent
            };

            foreach (var course in courses)
            {
                var courseDiscountPercent = GetActiveDiscountPercent(course);
                var courseDiscountAmount = Round(course.Price * courseDiscountPercent / 100);
                var afterCourseDiscount = Math.Max(0, course.Price - courseDiscountAmount);
                var codeDiscountAmount = Round(afterCourseDiscount * discountPercent / 100);
                var finalAmount = Math.Max(0, afterCourseDiscount - codeDiscountAmount);

                invoice.Items.Add(new OrderInvoiceItemResult
                {
                    CourseId = course.Id,
                    CourseTitle = course.Title,
                    CoursePrice = course.Price,
                    CourseDiscountPercent = courseDiscountPercent,
                    CourseDiscountAmount = courseDiscountAmount,
                    DiscountCodePercent = discountPercent,
                    DiscountCodeAmount = codeDiscountAmount,
                    FinalAmount = finalAmount
                });
            }

            invoice.SubtotalAmount = invoice.Items.Sum(x => x.CoursePrice);
            invoice.CourseDiscountAmount = invoice.Items.Sum(x => x.CourseDiscountAmount);
            invoice.DiscountCodeAmount = invoice.Items.Sum(x => x.DiscountCodeAmount);
            invoice.PayableAmount = invoice.Items.Sum(x => x.FinalAmount);
            return (invoice, courses, discount);
        }

        private static decimal GetActiveDiscountPercent(Course course)
        {
            if (!course.DiscountPercent.HasValue || course.DiscountPercent <= 0) return 0;
            var today = DateTime.Now.Date;
            if (course.DiscountStartDate.HasValue && today < course.DiscountStartDate.Value.Date) return 0;
            if (course.DiscountEndDate.HasValue && today > course.DiscountEndDate.Value.Date) return 0;
            return course.DiscountPercent.Value;
        }

        private static decimal Round(decimal value) => Math.Round(value, 0);
    }
}
