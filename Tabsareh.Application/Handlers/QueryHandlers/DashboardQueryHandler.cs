using Tabsareh.Application.Contracts.Queries.Dashboard;
using Tabsareh.Application.Contracts.QueryResult.Dashboard;
using Tabsareh.Domain;
using Tabsareh.Framework.Application;

namespace Tabsareh.Application.Handlers.QueryHandlers
{
    public class DashboardQueryHandler : IQueryHandler<GetDashboardStatsQuery, DashboardStatsResult>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DashboardQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<DashboardStatsResult> Handle(GetDashboardStatsQuery query)
        {
            var total = await _unitOfWork.OrderRepository.GetTotalSalesAmountAsync();
            var today = await _unitOfWork.OrderRepository.GetTodaySalesAmountAsync();
            var pending = await _unitOfWork.OrderRepository.GetPendingCardToCardCountAsync();

            return new DashboardStatsResult
            {
                TotalSalesAmount = total,
                TodaySalesAmount = today,
                PendingCardToCardCount = pending,
            };
        }
    }
}
