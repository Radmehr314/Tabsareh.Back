using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tabsareh.Application.Contracts.Commands.Blogs;
using Tabsareh.Framework.Api;
using Tabsareh.Framework.Application;

namespace Tabsareh.Api.Controllers.Blog
{
    [Authorize(Policy = "manage_blogs")]
    public class BlogCommandController : BaseCommandController
    {
        public BlogCommandController(ICommandBus bus) : base(bus) { }

        [HttpPost("AddBlog")]
        public async Task<ActionResult<CommandResult>> AddBlog([FromBody] AddBlogCommand command)
        {
            return Ok(await Bus.Dispatch(command));
        }

        [HttpPut("UpdateBlog")]
        public async Task<ActionResult<CommandResult>> UpdateBlog([FromBody] UpdateBlogCommand command)
        {
            return Ok(await Bus.Dispatch(command));
        }

        [HttpDelete("DeleteBlog")]
        public async Task<ActionResult<CommandResult>> DeleteBlog([FromBody] DeleteBlogCommand command)
        {
            return Ok(await Bus.Dispatch(command));
        }
    }
}
