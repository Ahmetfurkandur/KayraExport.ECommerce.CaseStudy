using Microsoft.EntityFrameworkCore;
using ProductService.Domain.Entities;

namespace ProductService.Application.Interfaces.Repositories
{
    /// <summary>
    /// Temel repository interface'i. Repository Design Pattern'i tekrar eden kodları merkezileştirmeyi hedefler. 
    /// Bu sebepten ötürü ORM (EF Core) kodlarını merkezileştirip tekrar tekrar yazmamak için reepository pattern tercih edilir.
    /// Ayrıca soyutlama ve testing gibi konularda da büyük avantaj sağlar.
    /// </summary>
    /// <typeparam name="T">BaseEntity'i miras alan entityleri kabul eder</typeparam>
    public interface IRepository<T> where T : BaseEntity, new()
    {
        DbSet<T> Table { get; }
    }
}
