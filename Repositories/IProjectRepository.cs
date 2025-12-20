using System.Collections.Generic;
using CompanyTaskProjectManagement.Entities;

namespace CompanyTaskProjectManagement.Repositories
{
    /// <summary>
    /// Proje Repository Interface
    /// </summary>
    public interface IProjectRepository : IRepository<Project>
    {
        IEnumerable<Project> GetActiveProjects();
    }
}
