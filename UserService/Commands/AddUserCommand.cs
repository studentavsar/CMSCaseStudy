using MediatR;
using UserService.Models;

namespace UserService.Commands
{
    public class AddUserCommand : IRequest<User>
    {
        public User User { get; }

        public AddUserCommand(User user)
        {
            User = user;
        }
    }
}
