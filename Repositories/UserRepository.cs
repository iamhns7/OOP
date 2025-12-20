using System.Linq;
using CompanyTaskProjectManagement.Entities;

namespace CompanyTaskProjectManagement.Repositories
{
    /// <summary>
    /// Kullanıcı Repository Implementation
    /// InMemoryRepository'den türetilmiştir (Inheritance)
    /// IUserRepository'i implemente eder (Polymorphism)
    /// </summary>
    public class UserRepository : InMemoryRepository<User>, IUserRepository
    {
        public UserRepository()
        {
            // Test verileri
            SeedData();
        }

        public User GetByUsername(string kullaniciAdi)
        {
            return _dataStore.FirstOrDefault(u => u.KullaniciAdi == kullaniciAdi);
        }

        public User Authenticate(string kullaniciAdi, string sifre)
        {
            return _dataStore.FirstOrDefault(u => 
                u.KullaniciAdi == kullaniciAdi && u.Sifre == sifre);
        }

        private void SeedData()
        {
            // Örnek admin kullanıcı
            Add(new User("Admin Kullanıcı", "admin", "admin123", UserRole.Admin));
            
            // Örnek çalışanlar
            Add(new User("Ahmet Yılmaz", "ahmet", "123", UserRole.Calisan));
            Add(new User("Ayşe Demir", "ayse", "123", UserRole.Calisan));
            Add(new User("Mehmet Kaya", "mehmet", "123", UserRole.Calisan));
        }
    }
}
