using System.Collections.Generic;
using CompanyTaskProjectManagement.Entities;

namespace CompanyTaskProjectManagement.Repositories
{
    /// <summary>
    /// Generic Repository Interface (Interface örneği)
    /// SOLID - Interface Segregation Principle
    /// </summary>
    /// <typeparam name="T">BaseEntity'den türeyen herhangi bir varlık</typeparam>
    public interface IRepository<T> where T : BaseEntity
    {
        // CRUD operasyonları
        T GetById(int id);
        IEnumerable<T> GetAll();
        void Add(T entity);
        void Update(T entity);
        void Delete(int id);
    }
}
