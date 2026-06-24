using Tabsareh.Framework.Application;
using Tabsareh.Framework.Application.Exceptions;
using Tabsareh.Application.Contracts.Commands.Teachers;
using Tabsareh.Domain;
using Tabsareh.Domain.Models.Teachers;

namespace Tabsareh.Application.Handlers.CommandHandlers
{
    public class TeacherCommandHandler :
        ICommandHandler<AddTeacherCommand>,
        ICommandHandler<UpdateTeacherCommand>,
        ICommandHandler<DeleteTeacherCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public TeacherCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CommandResult> Handle(AddTeacherCommand command)
        {
            var teacher = new Teacher(command.FirstName, command.LastName, command.ProfileImage, command.Description);
            var id = await _unitOfWork.TeacherRepository.AddAsync(teacher);
            return new CommandResult { Id = id };
        }

        public async Task<CommandResult> Handle(UpdateTeacherCommand command)
        {
            var teacher = await _unitOfWork.TeacherRepository.GetByIdAsync(command.Id);
            if (teacher is null || teacher.IsDeleted) throw new NotFoundException("استاد یافت نشد.");

            teacher.Update(command.FirstName, command.LastName, command.ProfileImage, command.Description);
            var result = await _unitOfWork.TeacherRepository.UpdateAsync(teacher);
            return new CommandResult { Id = result.Id };
        }

        public async Task<CommandResult> Handle(DeleteTeacherCommand command)
        {
            var teacher = await _unitOfWork.TeacherRepository.GetByIdAsync(command.Id);
            if (teacher is null || teacher.IsDeleted) throw new NotFoundException("استاد یافت نشد.");

            teacher.Delete();
            var result = await _unitOfWork.TeacherRepository.UpdateAsync(teacher);
            return new CommandResult { Id = result.Id };
        }
    }
}
