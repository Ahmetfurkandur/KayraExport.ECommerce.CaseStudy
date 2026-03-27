using Microsoft.EntityFrameworkCore;
using ProductService.Application.Interfaces.Repositories;
using ProductService.Domain.Entities;
using ProductService.Infrastructure.Contexts;
using System.Linq.Expressions;

namespace ProductService.Infrastructure.Repositories
{
    /// <summary>
    /// CQRS gereği Repository sınıflarını/interfacelerini Command ve Query olarak ayırmayı tercih ediyorum.
    /// Bu sayede CQRS'in temelini oluşturan SoC (Seperation of Concerns) prensibine de sadık kalmış oluyoruz.
    /// Bu sınıf Query'lerin repository sınıfıdır.
    /// </summary>
    /// <typeparam name="T">BaseEntity'i miras alan entityleri kabul eder</typeparam>
    public class QueryRepository<T> : IQueryRepository<T> where T : BaseEntity, new()
    {
        private readonly ProductDbContext context;

        public QueryRepository(ProductDbContext context)
        {
            this.context = context;
        }
        public DbSet<T> Table => context.Set<T>();

        /// <summary>
        /// Tüm verileri getirir
        /// </summary>
        /// <param name="tracking">ChangeTracker ile entity'leri izlemek için true, İzleme işlemini kapatıp okuma performansını arttırmak için false yapılmalıdır. Varsayılan: True</param>
        /// <returns></returns>
        public IQueryable<T> GetAll(bool tracking = true)
        {
            var table = Table.AsQueryable();

            if (!tracking)
                table = table.AsNoTracking();

            return table;
        }

        public async Task<T> GetByIdAsync(Guid id, bool tracking = true)
        {
            var query = Table.AsQueryable();

            if (!tracking)
                query = query.AsNoTracking();
            return await query
                    .FirstOrDefaultAsync(data => data.Id == id);
        }

        public async Task<T> GetSingleAsync(Expression<Func<T, bool>> filter, bool tracking = true)
        {
            var query = Table.AsQueryable();

            if (!tracking)
                query = query.AsNoTracking();
            return await query
                    .FirstOrDefaultAsync(filter);
        }

        public IQueryable<T> GetWhere(Expression<Func<T, bool>> filter, bool tracking = true)
        {
            var query = Table.Where(filter);

            if (!tracking)
                query = query.AsNoTracking();
            return query;
        }
    }
}
