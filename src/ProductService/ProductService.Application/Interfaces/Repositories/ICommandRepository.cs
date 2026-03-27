using ProductService.Domain.Entities;

namespace ProductService.Application.Interfaces.Repositories
{
    /// <summary>
    /// CQRS gereği Repository sınıflarını/interfacelerini Command ve Query olarak ayırmayı tercih ediyorum.
    /// Bu sayede CQRS'in temelini oluşturan SoC (Seperation of Concerns) prensibine de sadık kalmış oluyoruz.
    /// Bu interface Command'ların repository interface'idir.
    /// </summary>
    /// <typeparam name="T">BaseEntity'i miras alan entityleri kabul eder</typeparam>
    public interface ICommandRepository<T> : IRepository<T> where T : BaseEntity, new()
    {
        Task<T> AddAsync(T entity);
        Task<bool> AddRangeAsync(List<T> entities);
        bool Remove(T entity);
        Task<bool> RemoveAsync(Guid id);
        bool RemoveRange(List<T> entities);
        bool Update(T entity);
        Task<int> SaveAsync(CancellationToken cancellationToken = default);
    }
}
