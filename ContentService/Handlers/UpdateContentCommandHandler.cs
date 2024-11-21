using MediatR;
using ContentService.Models;
using ContentService.Commands;
using ContentService.Data;

namespace ContentService.Handlers
{
    public class UpdateContentCommandHandler : IRequestHandler<UpdateContentCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateContentCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(UpdateContentCommand request, CancellationToken cancellationToken)
        {
            _unitOfWork.Contents.UpdateContentAsync(request.Content);
            await _unitOfWork.SaveChangesAsync();
            return Unit.Value;
        }
    }
}
