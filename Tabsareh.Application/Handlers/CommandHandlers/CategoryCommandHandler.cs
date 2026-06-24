using Tabsareh.Application.Contracts.Commands.Categories;
using Tabsareh.Domain;
using Tabsareh.Domain.Models.Categories;
using Tabsareh.Framework.Application;
using Tabsareh.Framework.Application.Exceptions;

namespace Tabsareh.Application.Handlers.CommandHandlers
{
    public class CategoryCommandHandler :
        ICommandHandler<AddCategoryCommand>,
        ICommandHandler<UpdateCategoryCommand>,
        ICommandHandler<DeleteCategoryCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CommandResult> Handle(AddCategoryCommand command)
        {
            await ValidateParent(command.ParentId);

            var exists = await _unitOfWork.CategoryRepository.ExistsByNameAsync(command.Name, command.ParentId);
            if (exists) throw new UserAccessException("دسته بندی با این نام در این سطح قبلا ثبت شده است.");

            var category = new Category(command.Name, command.ParentId);
            var id = await _unitOfWork.CategoryRepository.AddAsync(category);
            return new CommandResult { Id = id };
        }

        public async Task<CommandResult> Handle(UpdateCategoryCommand command)
        {
            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(command.Id);
            if (category is null || category.IsDeleted) throw new NotFoundException("دسته بندی یافت نشد.");

            if (command.ParentId == command.Id)
                throw new UserAccessException("دسته بندی نمی تواند والد خودش باشد.");

            await ValidateParent(command.ParentId);
            await ValidateNoCycle(command.Id, command.ParentId);

            var exists = await _unitOfWork.CategoryRepository.ExistsByNameAsync(command.Name, command.ParentId, command.Id);
            if (exists) throw new UserAccessException("دسته بندی با این نام در این سطح قبلا ثبت شده است.");

            category.Update(command.Name, command.ParentId);
            var result = await _unitOfWork.CategoryRepository.UpdateAsync(category);
            return new CommandResult { Id = result.Id };
        }

        public async Task<CommandResult> Handle(DeleteCategoryCommand command)
        {
            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(command.Id);
            if (category is null || category.IsDeleted) throw new NotFoundException("دسته بندی یافت نشد.");

            var hasChildren = await _unitOfWork.CategoryRepository.HasChildrenAsync(command.Id);
            if (hasChildren) throw new UserAccessException("ابتدا زیر دسته های این دسته بندی را حذف کنید.");

            category.Delete();
            var result = await _unitOfWork.CategoryRepository.UpdateAsync(category);
            return new CommandResult { Id = result.Id };
        }

        private async Task ValidateParent(string? parentId)
        {
            if (string.IsNullOrWhiteSpace(parentId)) return;

            var parent = await _unitOfWork.CategoryRepository.GetByIdAsync(parentId);
            if (parent is null || parent.IsDeleted) throw new NotFoundException("دسته بندی والد یافت نشد.");
        }

        private async Task ValidateNoCycle(string id, string? parentId)
        {
            var currentParentId = parentId;
            var visited = new HashSet<string> { id };

            while (!string.IsNullOrWhiteSpace(currentParentId))
            {
                if (!visited.Add(currentParentId))
                    throw new UserAccessException("ساختار دسته بندی چرخه ای است.");

                var parent = await _unitOfWork.CategoryRepository.GetByIdAsync(currentParentId);
                if (parent is null || parent.IsDeleted) return;
                currentParentId = parent.ParentId;
            }
        }
    }
}
