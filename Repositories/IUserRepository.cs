using CompanyTaskProjectManagement.Entities;

namespace CompanyTaskProjectManagement.Repositories
{
    /// <summary>
    /// Kullanıcı Repository Interface
    /// </summary>
    public interface IUserRepository : IRepository<User>
    {
        User GetByUsername(string kullaniciAdi);
        User Authenticate(string kullaniciAdi, string sifre);
    }
}
