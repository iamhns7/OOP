using System;
using System.Collections.Generic;
using System.Linq;
using CompanyTaskProjectManagement.Entities;

namespace CompanyTaskProjectManagement.Repositories
{
    /// <summary>
    /// Proje Repository Implementation
    /// InMemoryRepository'den türetilmiştir (Inheritance)
    /// IProjectRepository'i implemente eder (Polymorphism)
    /// </summary>
    public class ProjectRepository : InMemoryRepository<Project>, IProjectRepository
    {
        public ProjectRepository()
        {
            // Test verileri
            SeedData();
        }

        public IEnumerable<Project> GetActiveProjects()
        {
            return _dataStore.Where(p => p.BitisTarihi == null || p.BitisTarihi > DateTime.Now);
        }

        private void SeedData()
        {
            // Örnek projeler
            Add(new Project(
                "E-Ticaret Platformu", 
                "Yeni nesil e-ticaret web sitesi geliştirme projesi", 
                new DateTime(2024, 1, 15),
                new DateTime(2024, 6, 30)
            ));

            Add(new Project(
                "Mobil Uygulama", 
                "Android ve iOS için mobil uygulama geliştirme", 
                new DateTime(2024, 2, 1),
                null
            ));

            Add(new Project(
                "Kurumsal CRM Sistemi", 
                "Müşteri ilişkileri yönetim sistemi", 
                new DateTime(2024, 3, 1),
                null
            ));
        }
    }
}
