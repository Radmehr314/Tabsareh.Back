using Tabsareh.Application.Contracts.Queries.CourseComments;
using Tabsareh.Application.Contracts.QueryResult.CourseComments;
using Tabsareh.Domain;
using Tabsareh.Domain.Models.CourseComments;
using Tabsareh.Framework.Application;

namespace Tabsareh.Application.Handlers.QueryHandlers
{
    public class CourseCommentQueryHandler :
        IQueryHandler<GetCourseCommentsPagedQuery, GetCourseCommentsPagedQueryResult>,
        IQueryHandler<GetPublicCourseCommentsQuery, GetPublicCourseCommentsQueryResult>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CourseCommentQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GetCourseCommentsPagedQueryResult> Handle(GetCourseCommentsPagedQuery query)
        {
            var paged = await _unitOfWork.CourseCommentRepository.GetPagedAsync(query.Options, new CourseCommentFilter
            {
                CourseId = query.CourseId,
                IsApproved = query.IsApproved,
            });

            return new GetCourseCommentsPagedQueryResult
            {
                Items = paged.Items.Select(ToResult).ToList(),
                TotalCount = paged.TotalCount,
                Skip = paged.Skip,
                Limit = paged.Limit,
            };
        }

        public async Task<GetPublicCourseCommentsQueryResult> Handle(GetPublicCourseCommentsQuery query)
        {
            var comments = await _unitOfWork.CourseCommentRepository.GetApprovedByCourseIdAsync(query.CourseId);
            var avg = await _unitOfWork.CourseCommentRepository.GetAverageRatingByCourseIdAsync(query.CourseId);

            return new GetPublicCourseCommentsQueryResult
            {
                Comments = comments.Select(c => new PublicCourseCommentResult
                {
                    Id = c.Id,
                    AuthorName = c.AuthorName,
                    Content = c.Content,
                    Rating = c.Rating,
                    CreatedAt = c.CreatedAt.ToString("yyyy/MM/dd"),
                }).ToList(),
                AverageRating = avg.HasValue ? Math.Round(avg.Value, 1) : null,
                CommentCount = comments.Count,
            };
        }

        private static CourseCommentResult ToResult(CourseCommentListItem item) => new()
        {
            Id = item.Comment.Id,
            CourseId = item.Comment.CourseId,
            CourseTitle = item.CourseTitle,
            AuthorName = item.Comment.AuthorName,
            AuthorPhone = item.Comment.AuthorPhone,
            Content = item.Comment.Content,
            Rating = item.Comment.Rating,
            IsApproved = item.Comment.IsApproved,
            CreatedAt = item.Comment.CreatedAt.ToString("yyyy/MM/dd HH:mm"),
        };
    }
}
