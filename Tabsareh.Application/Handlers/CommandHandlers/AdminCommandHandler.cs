using Microsoft.Extensions.Configuration;
using Tabsareh.Framework.Application;
using Tabsareh.Framework.Application.Exceptions;
using Tabsareh.Framework.Application.Security;
using Tabsareh.Application.Contracts.Commands.Admins;
using Tabsareh.Domain;
using Tabsareh.Domain.Models.Admins;

namespace Tabsareh.Application.Handlers.CommandHandlers
{
    public class AdminCommandHandler :
        ICommandHandler<AddAdminCommand>,
        ICommandHandler<BanAdminCommand>,
        ICommandHandler<UnBanAdminCommand>,
        ICommandHandler<UpdateAdminCommand>,
        ICommandHandler<DeleteAdminCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;

        public AdminCommandHandler(IUnitOfWork unitOfWork, IConfiguration config)
        {
            _unitOfWork = unitOfWork;
            _config = config;
        }

        public async Task<CommandResult> Handle(AddAdminCommand command)
        {
            if (command.Password != command.ConfirmPassword)
                throw new UserAccessException("رمز عبور با تایید آن همخوانی ندارد");

            var existUsername = await _unitOfWork.AdminRepository.ExistsByUserNameAsync(command.UserName);
            if (existUsername) throw new UserAccessException("نام کاربری تکراری است.");

            if (!string.IsNullOrWhiteSpace(command.RoleId))
            {
                var role = await _unitOfWork.RoleRepository.GetByIdAsync(command.RoleId);
                if (role is null) throw new NotFoundException("نقش مورد نظر یافت نشد.");
            }

            var pepper = _config["Security:Pepper"]
                ?? throw new InvalidOperationException("کلید امنیتی پیکربندی نشده است.");

            var (hashPassword, salt) = HashMaker.HashPassword(command.Password, pepper);
            var admin = new Admin(command.UserName, command.FirstName, command.LastName, command.Phone, hashPassword, salt, command.IsBan, command.RoleId);
            var id = await _unitOfWork.AdminRepository.AddAsync(admin);
            return new CommandResult { Id = id };
        }

        public async Task<CommandResult> Handle(BanAdminCommand command)
        {
            var existUsername = await _unitOfWork.AdminRepository.ExistsByUserNameAsync(command.Username);
            if (!existUsername) throw new NotFoundException("نام کاربری یافت نشد.");
            var admin = await _unitOfWork.AdminRepository.BanUserAsync(command.Username);
            return new CommandResult { Id = admin };
        }

        public async Task<CommandResult> Handle(UnBanAdminCommand command)
        {
            var existUsername = await _unitOfWork.AdminRepository.ExistsByUserNameAsync(command.Username);
            if (!existUsername) throw new NotFoundException("نام کاربری یافت نشد.");
            var admin = await _unitOfWork.AdminRepository.UnbanUserAsync(command.Username);
            return new CommandResult { Id = admin };
        }

        public async Task<CommandResult> Handle(UpdateAdminCommand command)
        {
            var admin = await _unitOfWork.AdminRepository.GetByIdAsync(command.Id);
            if (admin is null) throw new NotFoundException("ادمین یافت نشد.");

            if (!string.IsNullOrWhiteSpace(command.RoleId))
            {
                var role = await _unitOfWork.RoleRepository.GetByIdAsync(command.RoleId);
                if (role is null) throw new NotFoundException("نقش مورد نظر یافت نشد.");
            }

            if (command.Password != null)
            {
                if (command.Password != command.ConfirmPassword)
                    throw new UserAccessException("رمز عبور با تایید آن همخوانی ندارد");

                var pepper = _config["Security:Pepper"]
                    ?? throw new InvalidOperationException("کلید امنیتی پیکربندی نشده است.");

                var (hashPassword, salt) = HashMaker.HashPassword(command.Password, pepper);
                admin.Update(command.UserName, command.FirstName, command.LastName, command.Phone, hashPassword, salt, command.RoleId);
            }
            else
            {
                admin.Update(command.UserName, command.FirstName, command.LastName, command.Phone, null, null, command.RoleId);
            }

            var result = await _unitOfWork.AdminRepository.UpdateAsync(admin);
            return new CommandResult { Id = result.Id };
        }

        public async Task<CommandResult> Handle(DeleteAdminCommand command)
        {
            var admin = await _unitOfWork.AdminRepository.GetByIdAsync(command.Id);
            if (admin is null) throw new NotFoundException("ادمین یافت نشد.");
            admin.Delete();
            var result = await _unitOfWork.AdminRepository.UpdateAsync(admin);
            return new CommandResult { Id = result.Id };
        }
    }
}
