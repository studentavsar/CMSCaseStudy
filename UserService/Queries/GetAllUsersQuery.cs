using MediatR;
using UserService.Models;

namespace UserService.Queries
{
    public class GetAllUsersQuery : IRequest<IEnumerable<User>> { }
}
