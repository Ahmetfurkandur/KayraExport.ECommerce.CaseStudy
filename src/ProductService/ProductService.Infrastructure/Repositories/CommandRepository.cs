using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ProductService.Application.Interfaces.Repositories;
using ProductService.Domain.Entities;
using ProductService.Infrastructure.Contexts;

namespace ProductService.Infrastructure.Repositories
{
    /// <summary>
    /// CQRS gereği Repository sınıflarını/interfacelerini Command ve Query olarak ayırmayı tercih ediyorum.
    /// Bu sayede CQRS'in temelini oluşturan SoC (Seperation of Concerns) prensibine de sadık kalmış oluyoruz.
    /// Bu sınıf Command'ların repository sınıfıdır.
    /// </summary>
    /// <typeparam name="T">BaseEntity'i miras alan entityleri kabul eder</typeparam>
    public class CommandRepository<T> : ICommandRepository<T> where T : BaseEntity, new()
    {
        private readonly ProductDbContext context;

        public CommandRepository(ProductDbContext context)
        {
            this.context = context;
        }
        public DbSet<T> Table => context.Set<T>();

        public async Task<T> AddAsync(T entity)
        {
            EntityEntry<T> entry = await Table.AddAsync(entity);
            if (entry.State == EntityState.Added)
                return entry.Entity;

            return null;
        }

        public async Task<bool> AddRangeAsync(List<T> entities)
        {

            await Table.AddRangeAsync(entities);
            return true;
        }

        public bool Remove(T entity)
        {
            EntityEntry<T> entry = Table.Remove(entity);
            return entry.State == EntityState.Deleted;
        }

        public async Task<bool> RemoveAsync(Guid id)
        {
            T? model = await Table.FirstOrDefaultAsync(t => t.Id == id);
            return Remove(model);
        }

        public bool RemoveRange(List<T> entities)
        {
            Table.RemoveRange(entities);
            return true;
        }

        public async Task<int> SaveAsync(CancellationToken cancellationToken = default)
            => await context.SaveChangesAsync(cancellationToken);


        public bool Update(T entity)
        {
            EntityEntry<T> entityEntry = Table.Update(entity);
            return entityEntry.State == EntityState.Modified;
        }
    }
}
