using Microsoft.Extensions.Configuration;
using Tabsareh.Framework.Application;
using Tabsareh.Framework.Application.Exceptions;
using Tabsareh.Framework.Application.Security;
using Tabsareh.Application.Contracts.Commands.ContentOwners;
using Tabsareh.Domain;
using Tabsareh.Domain.Models.ContentOwners;

namespace Tabsareh.Application.Handlers.CommandHandlers
{
    public class ContentOwnerCommandHandler :
        ICommandHandler<AddContentOwnerCommand>,
        ICommandHandler<UpdateContentOwnerCommand>,
        ICommandHandler<BanContentOwnerCommand>,
        ICommandHandler<UnBanContentOwnerCommand>,
        ICommandHandler<DeleteContentOwnerCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;

        public ContentOwnerCommandHandler(IUnitOfWork unitOfWork, IConfiguration config)
        {
            _unitOfWork = unitOfWork;
            _config = config;
        }

        public async Task<CommandResult> Handle(AddContentOwnerCommand command)
        {
            if (command.Password != command.ConfirmPassword)
                throw new UserAccessException("رمز عبور با تایید آن همخوانی ندارد");

            var existUsername = await _unitOfWork.ContentOwnerRepository.ExistsByUserNameAsync(command.UserName);
            if (existUsername) throw new UserAccessException("نام کاربری تکراری است.");

            await EnsureTeachersExist(command.TeacherIds);

            var pepper = _config["Security:Pepper"]
                ?? throw new InvalidOperationException("Missing Security:Pepper");

            var (hashPassword, salt) = HashMaker.HashPassword(command.Password, pepper);
            var owner = new ContentOwner(command.Name, command.UserName, hashPassword, salt, command.IsBan, command.TeacherIds);
            var id = await _unitOfWork.ContentOwnerRepository.AddAsync(owner);
            return new CommandResult { Id = id };
        }

        public async Task<CommandResult> Handle(UpdateContentOwnerCommand command)
        {
            var owner = await _unitOfWork.ContentOwnerRepository.GetByIdAsync(command.Id);
            if (owner is null || owner.IsDeleted) throw new NotFoundException("صاحب اثر یافت نشد.");

            await EnsureTeachersExist(command.TeacherIds);

            if (command.Password != null)
            {
                if (command.Password != command.ConfirmPassword)
                    throw new UserAccessException("رمز عبور با تایید آن همخوانی ندارد");

                var pepper = _config["Security:Pepper"]
                    ?? throw new InvalidOperationException("Missing Security:Pepper");

                var (hashPassword, salt) = HashMaker.HashPassword(command.Password, pepper);
                owner.Update(command.Name, command.UserName, hashPassword, salt, command.TeacherIds);
            }
            else
            {
                owner.Update(command.Name, command.UserName, null, null, command.TeacherIds);
            }

            var result = await _unitOfWork.ContentOwnerRepository.UpdateAsync(owner);
            return new CommandResult { Id = result.Id };
        }

        public async Task<CommandResult> Handle(BanContentOwnerCommand command)
        {
            var existUsername = await _unitOfWork.ContentOwnerRepository.ExistsByUserNameAsync(command.Username);
            if (!existUsername) throw new NotFoundException("نام کاربری یافت نشد.");
            var id = await _unitOfWork.ContentOwnerRepository.BanUserAsync(command.Username);
            return new CommandResult { Id = id };
        }

        public async Task<CommandResult> Handle(UnBanContentOwnerCommand command)
        {
            var existUsername = await _unitOfWork.ContentOwnerRepository.ExistsByUserNameAsync(command.Username);
            if (!existUsername) throw new NotFoundException("نام کاربری یافت نشد.");
            var id = await _unitOfWork.ContentOwnerRepository.UnbanUserAsync(command.Username);
            return new CommandResult { Id = id };
        }

        public async Task<CommandResult> Handle(DeleteContentOwnerCommand command)
        {
            var owner = await _unitOfWork.ContentOwnerRepository.GetByIdAsync(command.Id);
            if (owner is null || owner.IsDeleted) throw new NotFoundException("صاحب اثر یافت نشد.");
            owner.Delete();
            var result = await _unitOfWork.ContentOwnerRepository.UpdateAsync(owner);
            return new CommandResult { Id = result.Id };
        }

        private async Task EnsureTeachersExist(List<string> teacherIds)
        {
            if (teacherIds == null || teacherIds.Count == 0) return;

            var distinctIds = teacherIds.Distinct().ToList();
            var found = await _unitOfWork.TeacherRepository.GetByIdsAsync(distinctIds);
            if (found.Count != distinctIds.Count)
                throw new NotFoundException("یک یا چند استاد انتخاب‌شده یافت نشد.");
        }
    }
}
