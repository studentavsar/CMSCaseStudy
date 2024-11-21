using MediatR;
using UserService.Models;
using UserService.Commands;
using UserService.Data;

namespace UserService.Handlers
{
    public class AddUserCommandHandler : IRequestHandler<AddUserCommand, User>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddUserCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<User> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.Users.AddUserAsync(request.User);
            await _unitOfWork.SaveChangesAsync();
            return request.User;
        }
    }
}
