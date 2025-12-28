# SQL Server Entegrasyonu - Kullanım Kılavuzu

## 📋 Yapılan Değişiklikler

SQL Server Express entegrasyonu başarıyla projeye eklenmiştir. Mevcut In-Memory yapısı bozulmadan, yeni bir SQL tabanlı repository oluşturulmuştur.

### Eklenen Dosyalar

1. **App.config** - Veritabanı bağlantı ayarları
2. **Repositories/Sql/UserSqlRepository.cs** - SQL Server repository implementasyonu
3. **DatabaseSetup.sql** - Veritabanı ve tablo oluşturma scripti

### Güncellenen Dosyalar

- **CompanyTaskProjectManagement.csproj** - NuGet paketleri eklendi:
  - Microsoft.Data.SqlClient (v5.1.1)
  - System.Configuration.ConfigurationManager (v7.0.0)

## 🗄️ Veritabanı Kurulumu

### Adım 1: SQL Server Express Kurulumu
Eğer SQL Server Express yüklü değilse, [Microsoft'tan indirip](https://www.microsoft.com/sql-server/sql-server-downloads) yükleyin.

### Adım 2: Veritabanı Oluşturma

1. **SQL Server Management Studio (SSMS)** veya **Azure Data Studio** açın
2. `localhost\SQLEXPRESS` instance'ına bağlanın (Windows Authentication)
3. `DatabaseSetup.sql` dosyasını açın ve çalıştırın

Script şunları yapar:
- `CompanyTaskDB` veritabanını oluşturur
- `Users` tablosunu oluşturur
- Örnek kullanıcılar ekler (admin, ahmet, ayse, mehmet)

### Adım 3: Bağlantıyı Test Edin

```sql
USE CompanyTaskDB;
SELECT * FROM Users;
```

## 💻 Kod Kullanımı

### Program.cs'de Repository Seçimi

**Eski Yöntem (In-Memory):**
```csharp
IUserRepository userRepository = new InMemoryUserRepository();
```

**Yeni Yöntem (SQL Server):**
```csharp
IUserRepository userRepository = new UserSqlRepository();
```

### Örnek Kullanım

```csharp
using CompanyTaskProjectManagement.Repositories.Sql;

// SQL Repository oluştur
IUserRepository sqlRepo = new UserSqlRepository();

// Tüm kullanıcıları getir
var users = sqlRepo.GetAll();

// Kullanıcı adına göre ara
var user = sqlRepo.GetByUsername("admin");

// Yeni kullanıcı ekle
var newUser = new User("Ali Veli", "ali", "123456", UserRole.Calisan);
sqlRepo.Add(newUser);

// Kullanıcı doğrulama
var authenticatedUser = sqlRepo.Authenticate("admin", "admin123");
```

## 🔧 Teknik Detaylar

### UserSqlRepository Özellikleri

- **Interface**: `IUserRepository` interface'ini tamamen implemente eder
- **ADO.NET**: `SqlConnection`, `SqlCommand`, `SqlDataReader` kullanır
- **Parametreli Sorgular**: SQL Injection koruması için `@param` kullanır
- **Exception Handling**: Tüm SQL işlemlerinde try-catch bloğu
- **Connection Management**: `using` statement ile otomatik kaynak yönetimi

### Implement Edilen Metodlar

✅ **GetAll()** - Tüm kullanıcıları getir  
✅ **GetByUsername(string)** - Kullanıcı adına göre ara  
✅ **Add(User)** - Yeni kullanıcı ekle  
✅ **Authenticate(string, string)** - Kullanıcı doğrulama  
✅ **GetById(int)** - ID'ye göre kullanıcı getir  
✅ **Update(User)** - Kullanıcı güncelle  
✅ **Delete(int)** - Kullanıcı sil

## 📊 Veritabanı Şeması

### Users Tablosu

| Kolon | Tip | Açıklama |
|-------|-----|----------|
| Id | INT (PK, Identity) | Otomatik artan ID |
| AdSoyad | NVARCHAR(100) | Kullanıcının adı soyadı |
| KullaniciAdi | NVARCHAR(50) UNIQUE | Benzersiz kullanıcı adı |
| Sifre | NVARCHAR(100) | Şifre (plain text - demo amaçlı) |
| Rol | INT | 0=Admin, 1=Çalışan |

## 🔐 Örnek Giriş Bilgileri

| Kullanıcı Adı | Şifre | Rol |
|----------------|-------|-----|
| admin | admin123 | Admin |
| ahmet | 123456 | Çalışan |
| ayse | 123456 | Çalışan |
| mehmet | 123456 | Çalışan |

## ⚠️ Önemli Notlar

1. **Connection String**: `App.config` dosyasında tanımlıdır
2. **Windows Authentication**: Trusted_Connection=True kullanır
3. **Server Name**: `localhost\SQLEXPRESS` (varsayılan)
4. **Şifre Güvenliği**: Demo amaçlı plain text kullanılmıştır. Üretim ortamında şifreler hash'lenmelidir (BCrypt, PBKDF2, vb.)
5. **Dependency Injection**: İleride DI container (Autofac, Unity) ile kullanılabilir

## 🚀 İleri Seviye Kullanım

### Dependency Injection ile Kullanım (Örnek)

```csharp
// Startup veya Program.cs
public static IUserRepository CreateUserRepository(bool useSqlServer)
{
    if (useSqlServer)
        return new UserSqlRepository();
    else
        return new InMemoryUserRepository();
}

// Kullanım
var repo = CreateUserRepository(useSqlServer: true);
var userService = new UserService(repo);
```

### Async/Await Desteği (Gelecek Geliştirme)

Repository'ye async metodlar eklenebilir:
```csharp
Task<IEnumerable<User>> GetAllAsync();
Task<User> GetByUsernameAsync(string username);
Task AddAsync(User user);
```

## 📝 Lisans ve Katkı

Bu kod, OOP prensiplerini göstermek ve SQL Server entegrasyonunu öğretmek amacıyla oluşturulmuştur.
