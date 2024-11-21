using MediatR;
using ContentService.Models;

namespace ContentService.Commands
{
    public class UpdateContentCommand : IRequest<Unit>
    {
        public int Id { get; }
        public Content Content { get; }

        public UpdateContentCommand(int id, Content content)
        {
            Id = id;
            Content = content;
        }
    }
}
