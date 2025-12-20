# Şirket İçin Görev ve Proje Yönetim Otomasyon Sistemi

## Proje Açıklaması
Bu proje, Nesne Yönelimli Programlama (OOP) dersi için geliştirilmiş bir Windows Forms (.NET 6) uygulamasıdır.

## Özellikler
- Kullanıcı yönetimi (Admin ve Çalışan rolleri)
- Proje yönetimi
- Görev yönetimi ve takibi
- Kullanıcı girişi ve yetkilendirme

## Kullanılan Teknolojiler
- C# (.NET 6)
- Windows Forms
- In-Memory veri yönetimi (List<T>)

## OOP Prensipleri

### 1. Encapsulation (Kapsülleme)
- Tüm entity sınıflarında private field'lar ve public property'ler kullanılmıştır
- Örnek: `User`, `Project`, `Task` sınıfları

### 2. Inheritance (Kalıtım)
- `BaseEntity` soyut sınıfından türetilmiş varlıklar
- `InMemoryRepository<T>` generic sınıfından türetilmiş repository'ler

### 3. Polymorphism (Çok Biçimlilik)
- Interface implementasyonları (`IRepository<T>`, `IUserRepository`, vb.)
- Method overriding

### 4. Abstraction (Soyutlama)
- Interface'ler ile soyutlama
- Repository pattern

### 5. SOLID Prensipleri
- **Single Responsibility**: Her sınıf tek bir sorumluluk taşır
- **Dependency Injection**: Constructor injection kullanımı

## Proje Yapısı

```
CompanyTaskProjectManagement/
├── Entities/                    # Varlık sınıfları
│   ├── BaseEntity.cs           # Temel entity sınıfı
│   ├── User.cs                 # Kullanıcı entity
│   ├── Project.cs              # Proje entity
│   └── Task.cs                 # Görev entity
│
├── Repositories/               # Veri erişim katmanı
│   ├── IRepository.cs          # Generic repository interface
│   ├── InMemoryRepository.cs   # Generic in-memory implementation
│   ├── IUserRepository.cs      # Kullanıcı repository interface
│   ├── UserRepository.cs       # Kullanıcı repository implementation
│   ├── IProjectRepository.cs   # Proje repository interface
│   ├── ProjectRepository.cs    # Proje repository implementation
│   ├── ITaskRepository.cs      # Görev repository interface
│   └── TaskRepository.cs       # Görev repository implementation
│
├── Services/                   # İş mantığı katmanı
│   ├── UserService.cs          # Kullanıcı servisi
│   ├── ProjectService.cs       # Proje servisi
│   └── TaskService.cs          # Görev servisi
│
├── Forms/                      # Kullanıcı arayüzü
│   ├── LoginForm.cs            # Giriş formu
│   ├── MainForm.cs             # Ana panel
│   ├── ProjectForm.cs          # Proje yönetim formu
│   └── TaskForm.cs             # Görev yönetim formu
│
└── Program.cs                  # Uygulama giriş noktası
```

## Test Kullanıcıları

Uygulama başlatıldığında aşağıdaki kullanıcılarla giriş yapabilirsiniz:

**Admin Kullanıcı:**
- Kullanıcı Adı: `admin`
- Şifre: `admin123`

**Çalışan Kullanıcılar:**
- Kullanıcı Adı: `ahmet`, Şifre: `123`
- Kullanıcı Adı: `ayse`, Şifre: `123`
- Kullanıcı Adı: `mehmet`, Şifre: `123`

## Çalıştırma

### Visual Studio ile:
1. `CompanyTaskProjectManagement.sln` dosyasını açın
2. F5 tuşuna basarak projeyi çalıştırın

### Komut satırından:
```bash
cd CompanyTaskProjectManagement
dotnet run
```

## Örnek Veriler

Uygulama başlatıldığında otomatik olarak örnek projeler ve görevler yüklenir:
- 3 örnek proje
- 7 örnek görev
- 4 kullanıcı

## Geliştirme Notları

- Veritabanı kullanımı için tasarlanmıştır ancak şu anda In-Memory veri yönetimi kullanmaktadır
- SQL Server Express entegrasyonu için `IRepository<T>` implementasyonu kolayca değiştirilebilir
- Tüm iş mantığı validasyonları Service katmanında yapılmaktadır

## Lisans
Bu proje akademik amaçlar için geliştirilmiştir.
