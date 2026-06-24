using Tabsareh.Application.Contracts.Commands.Blogs;
using Tabsareh.Domain;
using Tabsareh.Domain.Models.Blogs;
using Tabsareh.Framework.Application;
using Tabsareh.Framework.Application.Exceptions;

namespace Tabsareh.Application.Handlers.CommandHandlers
{
    public class BlogCommandHandler :
        ICommandHandler<AddBlogCommand>,
        ICommandHandler<UpdateBlogCommand>,
        ICommandHandler<DeleteBlogCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public BlogCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CommandResult> Handle(AddBlogCommand command)
        {
            ValidateRequired(command.Title, command.Body);
            await ValidateCategory(command.CategoryId);

            var slug = MakeSlug(string.IsNullOrWhiteSpace(command.Slug) ? command.Title : command.Slug);
            if (await _unitOfWork.BlogRepository.ExistsBySlugAsync(slug))
                throw new UserAccessException("اسلاگ بلاگ تکراری است.");

            var blog = new Blog(
                command.Title,
                slug,
                command.TitleImage,
                command.Body,
                command.Excerpt,
                command.CategoryId,
                command.MetaTitle,
                command.MetaDescription,
                command.MetaKeywords,
                command.IsPublished);

            var id = await _unitOfWork.BlogRepository.AddAsync(blog);
            return new CommandResult { Id = id };
        }

        public async Task<CommandResult> Handle(UpdateBlogCommand command)
        {
            ValidateRequired(command.Title, command.Body);

            var blog = await _unitOfWork.BlogRepository.GetByIdAsync(command.Id);
            if (blog is null || blog.IsDeleted) throw new NotFoundException("بلاگ یافت نشد.");

            await ValidateCategory(command.CategoryId);

            var slug = MakeSlug(string.IsNullOrWhiteSpace(command.Slug) ? command.Title : command.Slug);
            if (await _unitOfWork.BlogRepository.ExistsBySlugAsync(slug, command.Id))
                throw new UserAccessException("اسلاگ بلاگ تکراری است.");

            blog.Update(
                command.Title,
                slug,
                command.TitleImage,
                command.Body,
                command.Excerpt,
                command.CategoryId,
                command.MetaTitle,
                command.MetaDescription,
                command.MetaKeywords,
                command.IsPublished);

            var result = await _unitOfWork.BlogRepository.UpdateAsync(blog);
            return new CommandResult { Id = result.Id };
        }

        public async Task<CommandResult> Handle(DeleteBlogCommand command)
        {
            var blog = await _unitOfWork.BlogRepository.GetByIdAsync(command.Id);
            if (blog is null || blog.IsDeleted) throw new NotFoundException("بلاگ یافت نشد.");

            blog.Delete();
            var result = await _unitOfWork.BlogRepository.UpdateAsync(blog);
            return new CommandResult { Id = result.Id };
        }

        private static void ValidateRequired(string title, string body)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new UserAccessException("عنوان بلاگ الزامی است.");
            if (string.IsNullOrWhiteSpace(body))
                throw new UserAccessException("بدنه بلاگ الزامی است.");
        }

        private async Task ValidateCategory(string? categoryId)
        {
            if (string.IsNullOrWhiteSpace(categoryId)) return;

            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(categoryId);
            if (category is null || category.IsDeleted)
                throw new NotFoundException("دسته بندی بلاگ یافت نشد.");
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
