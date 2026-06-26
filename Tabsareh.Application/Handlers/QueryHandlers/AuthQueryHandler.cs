using Microsoft.Extensions.Configuration;
using Tabsareh.Application.Contracts.Contracts;
using Tabsareh.Application.Contracts.Queries.Auth;
using Tabsareh.Application.Contracts.QueryResult.Auth;
using Tabsareh.Domain;
using Tabsareh.Domain.Models.Auth;
using Tabsareh.Domain.Models.Users;
using Tabsareh.Framework.Application;
using Tabsareh.Framework.Application.Exceptions;
using Tabsareh.Framework.Application.Security;

namespace Tabsareh.Application.Handlers.QueryHandlers
{
    public class AuthQueryHandler :
        IQueryHandler<LoginAdminQuery, LoginDto>,
        IQueryHandler<LoginQuery, LoginResultDto>,
        IQueryHandler<RequestUserOtpQuery, RequestUserOtpResultDto>,
        IQueryHandler<VerifyUserOtpQuery, LoginResultDto>,
        IQueryHandler<GetCurrentUserQuery, CurrentUserDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly ITokenService _tokenService;
        private readonly IUserInfoService _userInfoService;

        public AuthQueryHandler(IUnitOfWork unitOfWork, IConfiguration configuration, ITokenService tokenService, IUserInfoService userInfoService)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _tokenService = tokenService;
            _userInfoService = userInfoService;
        }

        public async Task<LoginDto> Handle(LoginAdminQuery query)
        {
            var pepper = _configuration["Security:Pepper"]
                ?? throw new InvalidOperationException("Missing Security:Pepper");

            var admin = await _unitOfWork.AdminRepository.GetByUserNameAsync(query.UserName);
            if (admin is null) throw new NotFoundException("نام کاربری یا رمز عبور اشتباه است.");

            var isValidUser = HashMaker.Verify(query.Password, pepper, admin.Salt, admin.Password);
            if (!isValidUser) throw new NotFoundException("نام کاربری یا رمز عبور اشتباه است.");
            if (admin.IsBan) throw new UserAccessException("حساب کاربری مسدود شده است.");

            var token = _tokenService.Generate(
                userId: admin.Id,
                tokenVersion: 1,
                deviceId: "web",
                role: "admin",
                permissions: await GetAdminPermissions(admin.RoleId));

            return new LoginDto
            {
                AccessToken = token.Value,
                TokenType = "Bearer",
                ExpiresIn = (int)token.ExpiresIn.TotalSeconds
            };
        }

        public async Task<LoginResultDto> Handle(LoginQuery query)
        {
            var pepper = _configuration["Security:Pepper"]
                ?? throw new InvalidOperationException("Missing Security:Pepper");

            var admin = await _unitOfWork.AdminRepository.GetByUserNameAsync(query.UserName);
            if (admin is not null && HashMaker.Verify(query.Password, pepper, admin.Salt, admin.Password))
            {
                if (admin.IsBan) throw new UserAccessException("حساب کاربری مسدود شده است.");

                var adminToken = _tokenService.Generate(
                    userId: admin.Id,
                    tokenVersion: 1,
                    deviceId: "web",
                    role: "admin",
                    permissions: await GetAdminPermissions(admin.RoleId));

                return new LoginResultDto
                {
                    AccessToken = adminToken.Value,
                    TokenType = "Bearer",
                    ExpiresIn = (int)adminToken.ExpiresIn.TotalSeconds,
                    Role = "admin"
                };
            }

            var owner = await _unitOfWork.ContentOwnerRepository.GetByUserNameAsync(query.UserName);
            if (owner is not null && HashMaker.Verify(query.Password, pepper, owner.Salt, owner.Password))
            {
                if (owner.IsBan) throw new UserAccessException("حساب کاربری مسدود شده است.");

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

            throw new NotFoundException("نام کاربری یا رمز عبور اشتباه است.");
        }

        public async Task<RequestUserOtpResultDto> Handle(RequestUserOtpQuery query)
        {
            var phone = NormalizePhone(query.Phone);
            if (string.IsNullOrWhiteSpace(phone))
                throw new UserAccessException("شماره موبایل الزامی است.");

            var code = Random.Shared.Next(100000, 999999).ToString();
            var otp = new SmsOtp(phone, code, DateTime.UtcNow.AddMinutes(2));
            var id = await _unitOfWork.SmsOtpRepository.AddAsync(otp);

            return new RequestUserOtpResultDto
            {
                Id = id,
                Phone = phone,
                ExpiresInSeconds = 120,
                Code = code
            };
        }

        public async Task<LoginResultDto> Handle(VerifyUserOtpQuery query)
        {
            var phone = NormalizePhone(query.Phone);
            var code = query.Code?.Trim();
            if (string.IsNullOrWhiteSpace(phone) || string.IsNullOrWhiteSpace(code))
                throw new UserAccessException("شماره موبایل و کد تأیید الزامی هستند.");

            var otp = await _unitOfWork.SmsOtpRepository.GetLatestValidAsync(phone, code);
            if (otp is null) throw new UserAccessException("کد وارد شده نامعتبر یا منقضی شده است.");

            var user = await _unitOfWork.UserRepository.GetByPhoneAsync(phone);
            if (user is null)
            {
                user = new User("User", string.Empty, phone, phone);
                await _unitOfWork.UserRepository.AddAsync(user);
            }

            otp.MarkUsed();
            await _unitOfWork.SmsOtpRepository.UpdateAsync(otp);

            var token = _tokenService.Generate(
                userId: user.Id,
                tokenVersion: 1,
                deviceId: "mobile",
                role: "user",
                permissions: new List<string>());

            return new LoginResultDto
            {
                AccessToken = token.Value,
                TokenType = "Bearer",
                ExpiresIn = (int)token.ExpiresIn.TotalSeconds,
                Role = "user"
            };
        }

        public async Task<CurrentUserDto> Handle(GetCurrentUserQuery query)
        {
            var userId = _userInfoService.GetUserIdByToken();
            var role = _userInfoService.GetRoleByToken();

            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(role))
                throw new UserAccessException("توکن نامعتبر است.");

            if (role == "admin")
            {
                var admin = await _unitOfWork.AdminRepository.GetByIdAsync(userId);
                if (admin is null || admin.IsDeleted) throw new NotFoundException("کاربر یافت نشد.");

                var adminRole = !string.IsNullOrWhiteSpace(admin.RoleId)
                    ? await _unitOfWork.RoleRepository.GetByIdAsync(admin.RoleId)
                    : null;

                return new CurrentUserDto
                {
                    Id = admin.Id,
                    Role = "admin",
                    FullName = $"{admin.FirstName} {admin.LastName}".Trim(),
                    UserName = admin.UserName,
                    Phone = admin.Phone,
                    RoleId = admin.RoleId,
                    RoleName = adminRole?.Name,
                    Permissions = adminRole?.Permissions ?? new List<string>()
                };
            }

            if (role == "content_owner")
            {
                var owner = await _unitOfWork.ContentOwnerRepository.GetByIdAsync(userId);
                if (owner is null || owner.IsDeleted) throw new NotFoundException("کاربر یافت نشد.");

                return new CurrentUserDto
                {
                    Id = owner.Id,
                    Role = "content_owner",
                    FullName = owner.Name,
                    UserName = owner.UserName
                };
            }

            if (role == "user")
            {
                var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
                if (user is null || user.IsDeleted) throw new NotFoundException("کاربر یافت نشد.");

                return new CurrentUserDto
                {
                    Id = user.Id,
                    Role = "user",
                    FullName = $"{user.FirstName} {user.LastName}".Trim(),
                    UserName = user.UserName,
                    Phone = user.Phone
                };
            }

            throw new UserAccessException("نوع کاربری پشتیبانی نمی‌شود.");
        }

        private async Task<List<string>> GetAdminPermissions(string? roleId)
        {
            if (string.IsNullOrWhiteSpace(roleId))
                return new List<string>();

            var role = await _unitOfWork.RoleRepository.GetByIdAsync(roleId);
            return role?.Permissions ?? new List<string>();
        }

        private static string NormalizePhone(string? phone)
            => (phone ?? string.Empty).Trim().Replace(" ", string.Empty).Replace("-", string.Empty);
    }
}
