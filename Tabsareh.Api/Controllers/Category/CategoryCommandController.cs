using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tabsareh.Application.Contracts.Commands.Categories;
using Tabsareh.Framework.Api;
using Tabsareh.Framework.Application;

namespace Tabsareh.Api.Controllers.Category
{
    [Authorize(Policy = "manage_categories")]
    public class CategoryCommandController : BaseCommandController
    {
        public CategoryCommandController(ICommandBus bus) : base(bus) { }

        [HttpPost("AddCategory")]
        public async Task<ActionResult<CommandResult>> AddCategory([FromBody] AddCategoryCommand command)
        {
            return Ok(await Bus.Dispatch(command));
        }

        [HttpPut("UpdateCategory")]
        public async Task<ActionResult<CommandResult>> UpdateCategory([FromBody] UpdateCategoryCommand command)
        {
            return Ok(await Bus.Dispatch(command));
        }

        [HttpDelete("DeleteCategory")]
        public async Task<ActionResult<CommandResult>> DeleteCategory([FromBody] DeleteCategoryCommand command)
        {
            return Ok(await Bus.Dispatch(command));
        }
    }
}
