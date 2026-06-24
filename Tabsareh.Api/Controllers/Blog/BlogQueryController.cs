using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tabsareh.Application.Contracts.Queries.Blog;
using Tabsareh.Application.Contracts.QueryResult.Blog;
using Tabsareh.Framework.Api;
using Tabsareh.Framework.Application;

namespace Tabsareh.Api.Controllers.Blog
{
    [Authorize(Policy = "manage_blogs")]
    public class BlogQueryController : BaseQueryController
    {
        public BlogQueryController(IQueryBus bus) : base(bus) { }

        [HttpGet("Blogs")]
        public async Task<ActionResult<GetBlogsPagedQueryResult>> GetBlogsPaged([FromQuery] GetBlogsPagedQuery query)
        {
            return Ok(await Bus.Dispatch<GetBlogsPagedQuery, GetBlogsPagedQueryResult>(query));
        }

        [HttpGet("AllBlogs")]
        public async Task<ActionResult<List<BlogItemResult>>> GetAllBlogs()
        {
            return Ok(await Bus.Dispatch<GetAllBlogsQuery, List<BlogItemResult>>(new GetAllBlogsQuery()));
        }

        [HttpGet("GetBlog")]
        public async Task<ActionResult<BlogItemResult>> GetBlog([FromQuery] GetBlogByIdQuery query)
        {
            return Ok(await Bus.Dispatch<GetBlogByIdQuery, BlogItemResult>(query));
        }

        [HttpGet("GetBlogBySlug")]
        public async Task<ActionResult<BlogItemResult>> GetBlogBySlug([FromQuery] GetBlogBySlugQuery query)
        {
            return Ok(await Bus.Dispatch<GetBlogBySlugQuery, BlogItemResult>(query));
        }
    }
}
