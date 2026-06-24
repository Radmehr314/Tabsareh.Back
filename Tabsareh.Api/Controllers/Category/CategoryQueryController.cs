using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tabsareh.Application.Contracts.Queries.Category;
using Tabsareh.Application.Contracts.QueryResult.Category;
using Tabsareh.Framework.Api;
using Tabsareh.Framework.Application;

namespace Tabsareh.Api.Controllers.Category
{
    [Authorize(Policy = "manage_categories")]
    public class CategoryQueryController : BaseQueryController
    {
        public CategoryQueryController(IQueryBus bus) : base(bus) { }

        [HttpGet("Categories")]
        public async Task<ActionResult<GetCategoriesPagedQueryResult>> GetCategoriesPaged([FromQuery] GetCategoriesPagedQuery query)
        {
            return Ok(await Bus.Dispatch<GetCategoriesPagedQuery, GetCategoriesPagedQueryResult>(query));
        }

        [HttpGet("AllCategories")]
        public async Task<ActionResult<List<CategoryItemResult>>> GetAllCategories()
        {
            return Ok(await Bus.Dispatch<GetAllCategoriesQuery, List<CategoryItemResult>>(new GetAllCategoriesQuery()));
        }

        [HttpGet("GetCategory")]
        public async Task<ActionResult<CategoryItemResult>> GetCategory([FromQuery] GetCategoryByIdQuery query)
        {
            return Ok(await Bus.Dispatch<GetCategoryByIdQuery, CategoryItemResult>(query));
        }
    }
}
