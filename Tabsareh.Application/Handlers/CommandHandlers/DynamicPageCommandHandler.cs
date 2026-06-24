using Tabsareh.Application.Contracts.Commands.DynamicPages;
using Tabsareh.Domain;
using Tabsareh.Domain.Models.DynamicPages;
using Tabsareh.Framework.Application;
using Tabsareh.Framework.Application.Exceptions;

namespace Tabsareh.Application.Handlers.CommandHandlers
{
    public class DynamicPageCommandHandler :
        ICommandHandler<AddDynamicPageCommand>,
        ICommandHandler<UpdateDynamicPageCommand>,
        ICommandHandler<DeleteDynamicPageCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DynamicPageCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CommandResult> Handle(AddDynamicPageCommand command)
        {
            ValidateRequired(command.Title, command.Body);

            var slug = MakeSlug(string.IsNullOrWhiteSpace(command.Slug) ? command.Title : command.Slug);
            if (await _unitOfWork.DynamicPageRepository.ExistsBySlugAsync(slug))
                throw new UserAccessException("اسلاگ صفحه تکراری است.");

            var page = new DynamicPage(
                command.Title,
                slug,
                command.Body,
                command.MetaTitle,
                command.MetaDescription,
                command.MetaKeywords,
                command.DisplayOrder,
                command.IsPublished);

            var id = await _unitOfWork.DynamicPageRepository.AddAsync(page);
            return new CommandResult { Id = id };
        }

        public async Task<CommandResult> Handle(UpdateDynamicPageCommand command)
        {
            ValidateRequired(command.Title, command.Body);

            var page = await _unitOfWork.DynamicPageRepository.GetByIdAsync(command.Id);
            if (page is null || page.IsDeleted) throw new NotFoundException("صفحه یافت نشد.");

            var slug = MakeSlug(string.IsNullOrWhiteSpace(command.Slug) ? command.Title : command.Slug);
            if (await _unitOfWork.DynamicPageRepository.ExistsBySlugAsync(slug, command.Id))
                throw new UserAccessException("اسلاگ صفحه تکراری است.");

            page.Update(
                command.Title,
                slug,
                command.Body,
                command.MetaTitle,
                command.MetaDescription,
                command.MetaKeywords,
                command.DisplayOrder,
                command.IsPublished);

            var result = await _unitOfWork.DynamicPageRepository.UpdateAsync(page);
            return new CommandResult { Id = result.Id };
        }

        public async Task<CommandResult> Handle(DeleteDynamicPageCommand command)
        {
            var page = await _unitOfWork.DynamicPageRepository.GetByIdAsync(command.Id);
            if (page is null || page.IsDeleted) throw new NotFoundException("صفحه یافت نشد.");

            page.Delete();
            var result = await _unitOfWork.DynamicPageRepository.UpdateAsync(page);
            return new CommandResult { Id = result.Id };
        }

        private static void ValidateRequired(string title, string body)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new UserAccessException("عنوان صفحه الزامی است.");
            if (string.IsNullOrWhiteSpace(body))
                throw new UserAccessException("بدنه صفحه الزامی است.");
        }

        private static string MakeSlug(string? value)
        {
            var input = (value ?? string.Empty).Trim().ToLowerInvariant();
            var chars = new List<char>();
            var lastWasDash = false;

            foreach (var ch in input)
            {
                if (char.IsLetterOrDigit(ch))
                {
                    chars.Add(ch);
                    lastWasDash = false;
                }
                else if (!lastWasDash)
                {
                    chars.Add('-');
                    lastWasDash = true;
                }
            }

            var slug = new string(chars.ToArray()).Trim('-');
            return string.IsNullOrWhiteSpace(slug) ? Guid.NewGuid().ToString("N") : slug;
        }
    }
}
