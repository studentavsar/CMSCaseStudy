using Microsoft.Extensions.Logging;

namespace UserService.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly UserDbContext _context;
        private readonly ILoggerFactory _loggerFactory;
        private IUserRepository _userRepository;

        public UnitOfWork(UserDbContext context, ILoggerFactory loggerFactory)
        {
            _context = context;
            _loggerFactory = loggerFactory;
        }

        public IUserRepository Users
        {
            get
            {
                return _userRepository ??= new UserRepository(_context, _loggerFactory.CreateLogger<UserRepository>());
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            try
            {
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                var logger = _loggerFactory.CreateLogger<UnitOfWork>();
                logger.LogError(ex, "An error occurred while saving changes.");
                throw;
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
