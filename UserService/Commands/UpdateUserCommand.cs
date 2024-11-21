using MediatR;
using UserService.Models;

namespace UserService.Commands
{
    public class UpdateUserCommand : IRequest<Unit>
    {
        public int Id { get; }
        public User User { get; }

        public UpdateUserCommand(int id, User user)
        {
            Id = id;
            User = user;
        }
    }
}
