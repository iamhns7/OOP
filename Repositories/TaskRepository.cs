using System.Collections.Generic;
using System.Linq;
using CompanyTaskProjectManagement.Entities;

namespace CompanyTaskProjectManagement.Repositories
{
    /// <summary>
    /// Görev Repository Implementation
    /// InMemoryRepository'den türetilmiştir (Inheritance)
    /// ITaskRepository'i implemente eder (Polymorphism)
    /// </summary>
    public class TaskRepository : InMemoryRepository<Task>, ITaskRepository
    {
        public TaskRepository()
        {
            // Test verileri
            SeedData();
        }

        public IEnumerable<Task> GetByProjectId(int projeId)
        {
            return _dataStore.Where(t => t.ProjeId == projeId);
        }

        public IEnumerable<Task> GetByUserId(int kullaniciId)
        {
            return _dataStore.Where(t => t.AtananKullaniciId == kullaniciId);
        }

        public IEnumerable<Task> GetByStatus(TaskStatus durum)
        {
            return _dataStore.Where(t => t.Durum == durum);
        }

        private void SeedData()
        {
            // E-Ticaret Platformu (Proje Id: 1) görevleri
            Add(new Task("Veritabanı Tasarımı", "E-ticaret için veritabanı şeması oluşturma", 1, 2));
            Add(new Task("Ürün Katalog Modülü", "Ürün listeleme ve filtreleme modülü", 1, 2));
            Add(new Task("Sepet İşlemleri", "Alışveriş sepeti fonksiyonları", 1, 3));

            // Mobil Uygulama (Proje Id: 2) görevleri
            Add(new Task("UI/UX Tasarımı", "Mobil uygulama arayüz tasarımı", 2, 3));
            Add(new Task("API Entegrasyonu", "Backend API ile entegrasyon", 2, 4));

            // Kurumsal CRM Sistemi (Proje Id: 3) görevleri
            Add(new Task("Müşteri Modülü", "Müşteri bilgileri yönetim modülü", 3, 4));
            Add(new Task("Raporlama Sistemi", "Satış raporlama ve analiz sistemi", 3, 2));

            // Bazı görevlerin durumlarını güncelle
            var tasks = _dataStore.ToList();
            if (tasks.Count > 0)
            {
                tasks[0].Durum = TaskStatus.Tamamlandi;
                tasks[1].Durum = TaskStatus.DevamEdiyor;
                tasks[2].Durum = TaskStatus.DevamEdiyor;
            }
        }
    }
}
