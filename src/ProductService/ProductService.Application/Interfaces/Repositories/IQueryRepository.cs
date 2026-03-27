using ProductService.Domain.Entities;
using System.Linq.Expressions;

namespace ProductService.Application.Interfaces.Repositories
{
    /// <summary>
    /// CQRS gereği Repository sınıflarını/interfacelerini Command ve Query olarak ayırmayı tercih ediyorum.
    /// Bu sayede CQRS'in temelini oluşturan SoC (Seperation of Concerns) prensibine de sadık kalmış oluyoruz.
    /// Bu interface Query'lerin repository interface'idir.
    /// </summary>
    /// <typeparam name="T">BaseEntity'i miras alan entityleri kabul eder</typeparam>
    public interface IQueryRepository<T> : IRepository<T> where T : BaseEntity, new()
    {
        IQueryable<T> GetAll(bool tracking = true);
        IQueryable<T> GetWhere(Expression<Func<T, bool>> filter, bool tracking = true);
        Task<T> GetSingleAsync(Expression<Func<T, bool>> filter, bool tracking = true);
        Task<T> GetByIdAsync(Guid id, bool tracking = true);
    }
}
