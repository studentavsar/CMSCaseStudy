using MediatR;
using ContentService.Models;

namespace ContentService.Queries
{
    public class GetAllContentsQuery : IRequest<IEnumerable<Content>> { }
}
