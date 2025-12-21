# Şirket İçin Görev ve Proje Yönetim Otomasyon Sistemi

## Proje Açıklaması
Bu proje, Nesne Yönelimli Programlama (OOP) dersi için geliştirilmiş bir Windows Forms (.NET 6) uygulamasıdır.

## Özellikler
- Kullanıcı yönetimi (Admin ve Çalışan rolleri)
- Rol bazlı yetkilendirme ve arayüz kontrolü
- Proje yönetimi
- Görev yönetimi ve takibi
- **Görev önceliklendirme (Düşük, Orta, Yüksek)**
- **Görev son tarihi belirleme**
- **Gelişmiş filtreleme özellikleri**
- **Son tarihi geçmiş görevlerin görsel vurgulanması**
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

## Yeni Özellikler (v2.0)

### 1. Görev Önceliği (Priority)
- `TaskPriority` enum: Düşük, Orta, Yüksek
- Her görev için öncelik seviyesi belirleme
- Yüksek öncelikli görevler görsel olarak vurgulanır

### 2. Görev Son Tarihi (Due Date)
- Görevlere opsiyonel son tarih ekleme
- Son tarihi geçmiş görevler otomatik olarak kırmızı renkle işaretlenir
- DateTimePicker ile kolay tarih seçimi

### 3. Gelişmiş Filtreleme
- Tüm görevler
- Sadece bekleyen görevler
- Sadece devam eden görevler
- Sadece tamamlanan görevler
- **Sadece bana atanan görevler** (Çalışanlar için otomatik)
- **Son tarihi geçen görevler**

### 4. Rol Bazlı Arayüz Kontrolü

**Admin Kullanıcılar:**
- Tüm projeleri görüntüleme ve yönetme
- Tüm görevleri görüntüleme ve yönetme
- Proje oluşturma, düzenleme, silme
- Görev oluşturma, düzenleme, silme
- Tüm istatistiklere erişim

**Çalışan Kullanıcılar:**
- Sadece kendilerine atanmış görevleri görüntüleme
- Kendi görevlerinin durumunu güncelleme
- Proje yönetimine erişim yok
- Salt okunur proje listesi görüntüleme

## Yeni Özellikler (v3.0) ⭐

### 5. Akıllı Görev Arama 🔍
- **Gerçek zamanlı arama**: Başlık ve açıklamada anında arama
- **LINQ kullanımı**: Performanslı veri filtreleme
- **Rol bazlı arama**: Çalışanlar sadece kendi görevlerinde arama yapabilir

### 6. Gelişmiş Sıralama Sistemi 📊
- **Akıllı Sıralama**: Gecikmiş → Yüksek Öncelik → Son Tarih sıralaması
- **Öncelik Sıralaması**: Yüksek → Orta → Düşük
- **Son Tarih Sıralaması**: Yakın → Uzak
- **Durum Sıralaması**: Beklemede → Devam Ediyor → Tamamlandı

### 7. Kullanıcı Performans İstatistikleri 📈
- **Kişisel Dashboard**: Her kullanıcı için özelleştirilmiş istatistikler
- **Görsel ProgressBar**: Başarı yüzdesini gösteren ilerleme çubuğu
- **Detaylı Metrikler**:
  - Toplam atanan görev sayısı
  - Tamamlanan görev sayısı
  - Devam eden görevler
  - Bekleyen görevler
  - Gecikmiş görevler
  - Yüksek öncelikli görevler

### 8. Proje İlerleme Hesaplama (Calculated Properties) 🎯
- **Project.GetCompletionPercentage()**: Proje tamamlanma yüzdesi
- **Project.IsActive()**: Projenin aktif olup olmadığını kontrol
- **Project.GetTotalDays()**: Proje toplam gün sayısı
- **OOP Örneği**: Encapsulation - İş mantığı entity içinde

### 9. Görsel İyileştirmeler 🎨
- **Renkli Görev Vurgulama**:
  - 🔴 Kırmızı: Son tarihi geçmiş görevler
  - 🟠 Turuncu: Yüksek öncelikli görevler
- **Dashboard Yenileme**: Daha modern ve bilgilendirici arayüz
- **ProgressBar**: Kullanıcı başarı oranı göstergesi


- 3 örnek proje
- 7 örnek görev
- 4 kullanıcı

## Geliştirme Notları

- Veritabanı kullanımı için tasarlanmıştır ancak şu anda In-Memory veri yönetimi kullanmaktadır
- SQL Server Express entegrasyonu için `IRepository<T>` implementasyonu kolayca değiştirilebilir
- Tüm iş mantığı validasyonları Service katmanında yapılmaktadır

## Lisans
Bu proje akademik amaçlar için geliştirilmiştir.
