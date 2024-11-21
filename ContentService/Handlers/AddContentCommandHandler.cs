using MediatR;
using ContentService.Models;
using ContentService.Commands;
using ContentService.Data;
using ContentService.Models;

namespace ContentService.Handlers
{
    public class AddContentCommandHandler : IRequestHandler<AddContentCommand, Content>
    {
        private readonly IUnitOfWork _unitOfWork;
        public AddContentCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Content> Handle(AddContentCommand request, CancellationToken cancellationToken)
        {
            //var user = await _userServiceCommunicator.GetUserByIdAsync(request.Content.UserId);
            await _unitOfWork.Contents.AddContentAsync(request.Content);
            await _unitOfWork.SaveChangesAsync();
            return request.Content;
        }
    }
}
