using MediatR;
using UserService.Models;

namespace UserService.Queries
{
    public class GetUserByIdQuery : IRequest<User>
    {
        public int Id { get; }

        public GetUserByIdQuery(int id)
        {
            Id = id;
        }
    }
}
