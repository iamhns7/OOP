using System.Collections.Generic;
using CompanyTaskProjectManagement.Entities;

namespace CompanyTaskProjectManagement.Repositories
{
    /// <summary>
    /// Görev Repository Interface
    /// </summary>
    public interface ITaskRepository : IRepository<Task>
    {
        IEnumerable<Task> GetByProjectId(int projeId);
        IEnumerable<Task> GetByUserId(int kullaniciId);
        IEnumerable<Task> GetByStatus(TaskStatus durum);
    }
}
