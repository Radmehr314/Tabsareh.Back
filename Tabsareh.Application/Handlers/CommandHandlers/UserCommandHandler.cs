using Tabsareh.Application.Contracts.Commands.Users;
using Tabsareh.Domain;
using Tabsareh.Domain.Models.Users;
using Tabsareh.Framework.Application;
using Tabsareh.Framework.Application.Exceptions;

namespace Tabsareh.Application.Handlers.CommandHandlers
{
    public class UserCommandHandler :
        ICommandHandler<AddUserCommand>,
        ICommandHandler<UpdateUserCommand>,
        ICommandHandler<DeleteUserCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CommandResult> Handle(AddUserCommand command)
        {
            var exists = await _unitOfWork.UserRepository.ExistsByUserNameAsync(command.UserName);
            if (exists) throw new UserAccessException("نام کاربری تکراری است.");

            var user = new User(command.FirstName, command.LastName, command.UserName, command.Phone);
            var id = await _unitOfWork.UserRepository.AddAsync(user);
            return new CommandResult { Id = id };
        }

        public async Task<CommandResult> Handle(UpdateUserCommand command)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(command.Id);
            if (user is null || user.IsDeleted) throw new NotFoundException("کاربر یافت نشد.");

            var exists = await _unitOfWork.UserRepository.ExistsByUserNameAsync(command.UserName, command.Id);
            if (exists) throw new UserAccessException("نام کاربری تکراری است.");

            user.Update(command.FirstName, command.LastName, command.UserName, command.Phone);
            var result = await _unitOfWork.UserRepository.UpdateAsync(user);
            return new CommandResult { Id = result.Id };
        }

        public async Task<CommandResult> Handle(DeleteUserCommand command)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(command.Id);
            if (user is null || user.IsDeleted) throw new NotFoundException("کاربر یافت نشد.");

            user.Delete();
            var result = await _unitOfWork.UserRepository.UpdateAsync(user);
            return new CommandResult { Id = result.Id };
        }
    }
}
