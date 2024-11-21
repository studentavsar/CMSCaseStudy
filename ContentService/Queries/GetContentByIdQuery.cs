using MediatR;
using ContentService.Models;

namespace ContentService.Queries
{
    public class GetContentByIdQuery : IRequest<Content>
    {
        public int Id { get; }

        public GetContentByIdQuery(int id)
        {
            Id = id;
        }
    }
}
