using Tabsareh.Application.Contracts.Commands.CourseComments;
using Tabsareh.Domain;
using Tabsareh.Domain.Models.CourseComments;
using Tabsareh.Framework.Application;
using Tabsareh.Framework.Application.Exceptions;

namespace Tabsareh.Application.Handlers.CommandHandlers
{
    public class CourseCommentCommandHandler :
        ICommandHandler<AddCourseCommentCommand>,
        ICommandHandler<ApproveCourseCommentCommand>,
        ICommandHandler<RejectCourseCommentCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CourseCommentCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CommandResult> Handle(AddCourseCommentCommand command)
        {
            if (command.Rating < 1 || command.Rating > 5)
                throw new ValidationException("امتیاز باید بین ۱ تا ۵ باشد.");

            var comment = new CourseComment(
                command.CourseId,
                command.AuthorName,
                command.AuthorPhone,
                command.Content,
                command.Rating);

            await _unitOfWork.CourseCommentRepository.AddAsync(comment);
            return new CommandResult();
        }

        public async Task<CommandResult> Handle(ApproveCourseCommentCommand command)
        {
            var comment = await _unitOfWork.CourseCommentRepository.GetByIdAsync(command.Id)
                ?? throw new NotFoundException("کامنت یافت نشد.");

            comment.Approve();
            await _unitOfWork.CourseCommentRepository.UpdateAsync(comment);
            await UpdateCourseRatingStats(comment.CourseId);
            return new CommandResult();
        }

        public async Task<CommandResult> Handle(RejectCourseCommentCommand command)
        {
            var comment = await _unitOfWork.CourseCommentRepository.GetByIdAsync(command.Id)
                ?? throw new NotFoundException("کامنت یافت نشد.");

            comment.Reject();
            await _unitOfWork.CourseCommentRepository.UpdateAsync(comment);
            await UpdateCourseRatingStats(comment.CourseId);
            return new CommandResult();
        }

        private async Task UpdateCourseRatingStats(string courseId)
        {
            var course = await _unitOfWork.CourseRepository.GetByIdAsync(courseId);
            if (course is null) return;

            var avg = await _unitOfWork.CourseCommentRepository.GetAverageRatingByCourseIdAsync(courseId);
            var approved = await _unitOfWork.CourseCommentRepository.GetApprovedByCourseIdAsync(courseId);
            course.UpdateRatingStats(avg, approved.Count);
            await _unitOfWork.CourseRepository.UpdateAsync(course);
        }
    }
}
