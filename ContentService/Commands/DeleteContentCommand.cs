using MediatR;

namespace ContentService.Commands
{
    public class DeleteContentCommand : IRequest<Unit>
    {
        public int Id { get; }

        public DeleteContentCommand(int id)
        {
            Id = id;
        }
    }
}
