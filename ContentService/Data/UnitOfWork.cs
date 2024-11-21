
namespace ContentService.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ContentDbContext _context;
        private readonly ILoggerFactory _loggerFactory;
        private IContentRepository _contentRepository;

        public UnitOfWork(ContentDbContext context, ILoggerFactory loggerFactory)
        {
            _context = context;
            _loggerFactory = loggerFactory;
        }

        public IContentRepository Contents
        {
            get
            {
                return _contentRepository ??= new ContentRepository(_context, _loggerFactory.CreateLogger<ContentRepository>());
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
