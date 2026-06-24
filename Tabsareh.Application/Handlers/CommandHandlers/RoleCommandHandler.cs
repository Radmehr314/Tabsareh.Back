using Tabsareh.Framework.Application;
using Tabsareh.Framework.Application.Exceptions;
using Tabsareh.Application.Contracts.Commands.Roles;
using Tabsareh.Domain;
using Tabsareh.Domain.Models.Roles;

namespace Tabsareh.Application.Handlers.CommandHandlers
{
    public class RoleCommandHandler :
        ICommandHandler<AddRoleCommand>,
        ICommandHandler<UpdateRoleCommand>,
        ICommandHandler<DeleteRoleCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RoleCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CommandResult> Handle(AddRoleCommand command)
        {
            var exists = await _unitOfWork.RoleRepository.ExistsByNameAsync(command.Name);
            if (exists) throw new UserAccessException("نقشی با این نام قبلاً ثبت شده است.");

            var role = new Role(command.Name, command.Permissions);
            var id = await _unitOfWork.RoleRepository.AddAsync(role);
            return new CommandResult { Id = id };
        }

        public async Task<CommandResult> Handle(UpdateRoleCommand command)
        {
            var role = await _unitOfWork.RoleRepository.GetByIdAsync(command.Id);
            if (role is null) throw new NotFoundException("نقش مورد نظر یافت نشد.");

            role.Update(command.Name, command.Permissions);
            var result = await _unitOfWork.RoleRepository.UpdateAsync(role);
            return new CommandResult { Id = result.Id };
        }

        public async Task<CommandResult> Handle(DeleteRoleCommand command)
        {
            var role = await _unitOfWork.RoleRepository.GetByIdAsync(command.Id);
            if (role is null) throw new NotFoundException("نقش مورد نظر یافت نشد.");

            await _unitOfWork.RoleRepository.DeleteAsync(command.Id);
            return new CommandResult { Id = command.Id };
        }
    }
}
