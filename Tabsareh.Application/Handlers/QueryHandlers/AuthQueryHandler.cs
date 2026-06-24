using Microsoft.Extensions.Configuration;
using Tabsareh.Framework.Application;
using Tabsareh.Framework.Application.Exceptions;
using Tabsareh.Framework.Application.Security;
using Tabsareh.Application.Contracts.Contracts;
using Tabsareh.Application.Contracts.Queries.Auth;
using Tabsareh.Application.Contracts.QueryResult.Auth;
using Tabsareh.Domain;

namespace Tabsareh.Application.Handlers.QueryHandlers
{
    public class AuthQueryHandler :
        IQueryHandler<LoginAdminQuery, LoginDto>,
        IQueryHandler<LoginQuery, LoginResultDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly ITokenService _tokenService;

        public AuthQueryHandler(IUnitOfWork unitOfWork, IConfiguration configuration, ITokenService tokenService)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _tokenService = tokenService;
        }

        public async Task<LoginDto> Handle(LoginAdminQuery query)
        {
            var pepper = _configuration["Security:Pepper"]
                ?? throw new InvalidOperationException("Missing Security:Pepper");

            var admin = await _unitOfWork.AdminRepository.GetByUserNameAsync(query.UserName);
            if (admin is null) throw new NotFoundException("نام کاربری یا رمز عبور صحیح نمیباشد.");

            var isValidUser = HashMaker.Verify(query.Password, pepper, admin.Salt, admin.Password);
            if (!isValidUser)
                throw new NotFoundException("نام کاربری یا رمز عبور صحیح نمیباشد.");

            if (admin.IsBan) throw new UserAccessException("حساب کاربری شما محدود شده است لطفا با مجموعه تماس بگیرید.");

            List<string> permissions = new();
            if (!string.IsNullOrWhiteSpace(admin.RoleId))
            {
                var role = await _unitOfWork.RoleRepository.GetByIdAsync(admin.RoleId);
                if (role != null)
                    permissions = role.Permissions;
            }

            var token = _tokenService.Generate(
                userId: admin.Id,
                tokenVersion: 1,
                deviceId: "web",
                role: "admin",
                permissions: permissions);

            return new LoginDto
            {
                AccessToken = token.Value,
                TokenType = "Bearer",
                ExpiresIn = (int)token.ExpiresIn.TotalSeconds
            };
        }

        /// <summary>
        /// ورود یکپارچه؛ ابتدا میان ادمین‌ها و سپس صاحبان اثر جستجو می‌کند.
        /// </summary>
        public async Task<LoginResultDto> Handle(LoginQuery query)
        {
            var pepper = _configuration["Security:Pepper"]
                ?? throw new InvalidOperationException("Missing Security:Pepper");

            // ---- ابتدا تلاش برای ورود ادمین ----
            var admin = await _unitOfWork.AdminRepository.GetByUserNameAsync(query.UserName);
            if (admin is not null && HashMaker.Verify(query.Password, pepper, admin.Salt, admin.Password))
            {
                if (admin.IsBan)
                    throw new UserAccessException("حساب کاربری شما محدود شده است لطفا با مجموعه تماس بگیرید.");

                List<string> permissions = new();
                if (!string.IsNullOrWhiteSpace(admin.RoleId))
                {
                    var role = await _unitOfWork.RoleRepository.GetByIdAsync(admin.RoleId);
                    if (role != null) permissions = role.Permissions;
                }

                var adminToken = _tokenService.Generate(
                    userId: admin.Id,
                    tokenVersion: 1,
                    deviceId: "web",
                    role: "admin",
                    permissions: permissions);

                return new LoginResultDto
                {
                    AccessToken = adminToken.Value,
                    TokenType = "Bearer",
                    ExpiresIn = (int)adminToken.ExpiresIn.TotalSeconds,
                    Role = "admin"
                };
            }

            // ---- سپس تلاش برای ورود صاحب اثر ----
            var owner = await _unitOfWork.ContentOwnerRepository.GetByUserNameAsync(query.UserName);
            if (owner is not null && HashMaker.Verify(query.Password, pepper, owner.Salt, owner.Password))
            {
                if (owner.IsBan)
                    throw new UserAccessException("حساب کاربری شما محدود شده است لطفا با مجموعه تماس بگیرید.");

                var ownerToken = _tokenService.Generate(
                    userId: owner.Id,
                    tokenVersion: 1,
                    deviceId: "web",
                    role: "content_owner",
                    permissions: new List<string>());

                return new LoginResultDto
                {
                    AccessToken = ownerToken.Value,
                    TokenType = "Bearer",
                    ExpiresIn = (int)ownerToken.ExpiresIn.TotalSeconds,
                    Role = "content_owner"
                };
            }

            throw new NotFoundException("نام کاربری یا رمز عبور صحیح نمیباشد.");
        }
    }
}
