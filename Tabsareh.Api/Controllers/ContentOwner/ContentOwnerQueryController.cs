using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tabsareh.Framework.Api;
using Tabsareh.Framework.Application;
using Tabsareh.Application.Contracts.Queries.ContentOwner;
using Tabsareh.Application.Contracts.QueryResult.ContentOwner;

namespace Tabsareh.Api.Controllers.ContentOwner
{
    public class ContentOwnerQueryController : BaseQueryController
    {
        public ContentOwnerQueryController(IQueryBus bus) : base(bus) { }

        [HttpGet("ContentOwners")]
        [Authorize(Policy = "manage_content_owners")]
        public async Task<ActionResult<GetContentOwnersPagedQueryResult>> GetContentOwners([FromQuery] GetContentOwnersPagedQuery query)
        {
            return Ok(await Bus.Dispatch<GetContentOwnersPagedQuery, GetContentOwnersPagedQueryResult>(query));
        }

        [HttpGet("AllContentOwnersWithoutPagination")]
        [Authorize(Policy = "manage_content_owners")]
        public async Task<ActionResult<List<ContentOwnerItemResult>>> GetAllContentOwners([FromQuery] GetAllContentOwnersWithoutPaginationQuery query)
        {
            return Ok(await Bus.Dispatch<GetAllContentOwnersWithoutPaginationQuery, List<ContentOwnerItemResult>>(query));
        }

        [HttpGet("GetContentOwner")]
        [Authorize(Policy = "manage_content_owners")]
        public async Task<ActionResult<ContentOwnerItemResult>> GetContentOwner([FromQuery] GetContentOwnerByIdQuery query)
        {
            return Ok(await Bus.Dispatch<GetContentOwnerByIdQuery, ContentOwnerItemResult>(query));
        }

        [HttpGet("ContentOwnerPayments")]
        [Authorize(Policy = "manage_content_owners")]
        public async Task<ActionResult<List<ContentOwnerPaymentItemResult>>> GetContentOwnerPayments([FromQuery] GetContentOwnerPaymentsQuery query)
        {
            return Ok(await Bus.Dispatch<GetContentOwnerPaymentsQuery, List<ContentOwnerPaymentItemResult>>(query));
        }

        [HttpGet("GetLoginContentOwnerInfo")]
        [Authorize(Roles = "content_owner")]
        public async Task<ActionResult<GetContentOwnerInfoByTokenQueryResult>> GetLoginContentOwnerInfo([FromQuery] GetContentOwnerInfoByTokenQuery query)
        {
            return Ok(await Bus.Dispatch<GetContentOwnerInfoByTokenQuery, GetContentOwnerInfoByTokenQueryResult>(query));
        }

        [HttpGet("MyContentOwnerPayments")]
        [Authorize(Roles = "content_owner")]
        public async Task<ActionResult<List<ContentOwnerPaymentItemResult>>> GetMyContentOwnerPayments()
        {
            return Ok(await Bus.Dispatch<GetMyContentOwnerPaymentsQuery, List<ContentOwnerPaymentItemResult>>(new GetMyContentOwnerPaymentsQuery()));
        }
    }
}
