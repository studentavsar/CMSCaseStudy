namespace ContentService.Data
{
    public interface IUnitOfWork : IDisposable
    {
        IContentRepository Contents { get; }
        Task<int> SaveChangesAsync();
    }
}
