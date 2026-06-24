using Tabsareh.Framework.Application;

namespace Tabsareh.Application.Contracts.Commands.Blogs
{
    public class DeleteBlogCommand : ICommand
    {
        public string Id { get; set; }
    }
}
