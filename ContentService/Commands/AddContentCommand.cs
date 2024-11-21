using MediatR;
using ContentService.Models;

namespace ContentService.Commands
{
    public class AddContentCommand : IRequest<Content>
    {
        public Content Content { get; }

        public AddContentCommand(Content content)
        {
            Content = content;
        }
    }
}
