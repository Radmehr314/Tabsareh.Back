using Tabsareh.Framework.Application;
using Tabsareh.Framework.Application.Exceptions;
using Tabsareh.Application.Contracts.Queries.Teacher;
using Tabsareh.Application.Contracts.QueryResult.Teacher;
using Tabsareh.Application.Mapper;
using Tabsareh.Domain;

namespace Tabsareh.Application.Handlers.QueryHandlers
{
    public class TeacherQueryHandler :
        IQueryHandler<GetTeachersPagedQuery, GetTeachersPagedQueryResult>,
        IQueryHandler<GetAllTeachersWithoutPaginationQuery, List<TeacherItemResult>>,
        IQueryHandler<GetTeacherByIdQuery, TeacherItemResult>
    {
        private readonly IUnitOfWork _unitOfWork;

        public TeacherQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GetTeachersPagedQueryResult> Handle(GetTeachersPagedQuery query)
        {
            var paged = await _unitOfWork.TeacherRepository.GetPagedAsync(query.Options);
            return new GetTeachersPagedQueryResult
            {
                Items = paged.Items.ToItemList(),
                TotalCount = (int)paged.TotalCount,
                Skip = paged.Skip,
                Limit = paged.Limit
            };
        }

        public async Task<List<TeacherItemResult>> Handle(GetAllTeachersWithoutPaginationQuery query)
        {
            var teachers = await _unitOfWork.TeacherRepository.GetAllAsync();
            return teachers.ToItemList();
        }

        public async Task<TeacherItemResult> Handle(GetTeacherByIdQuery query)
        {
            var teacher = await _unitOfWork.TeacherRepository.GetByIdAsync(query.Id);
            if (teacher is null || teacher.IsDeleted) throw new NotFoundException("استاد یافت نشد.");
            return teacher.ToItem();
        }
    }
}
