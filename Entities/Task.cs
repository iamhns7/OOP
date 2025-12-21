using System;

namespace CompanyTaskProjectManagement.Entities
{
    /// <summary>
    /// Görev varlığı - BaseEntity'den türetilmiştir (Inheritance)
    /// </summary>
    public class Task : BaseEntity
    {
        // Encapsulation: private field'lar
        private string _baslik;
        private string _aciklama;
        private TaskStatus _durum;
        private TaskPriority _oncelik;
        private DateTime? _sonTarih;
        private int? _atananKullaniciId;
        private int _projeId;

        // Public property'ler
        public string Baslik 
        { 
            get => _baslik; 
            set => _baslik = value; 
        }

        public string Aciklama 
        { 
            get => _aciklama; 
            set => _aciklama = value; 
        }

        public TaskStatus Durum 
        { 
            get => _durum; 
            set => _durum = value; 
        }

        public TaskPriority Oncelik 
        { 
            get => _oncelik; 
            set => _oncelik = value; 
        }

        public DateTime? SonTarih 
        { 
            get => _sonTarih; 
            set => _sonTarih = value; 
        }

        public int? AtananKullaniciId 
        { 
            get => _atananKullaniciId; 
            set => _atananKullaniciId = value; 
        }

        public int ProjeId 
        { 
            get => _projeId; 
            set => _projeId = value; 
        }

        public Task()
        {
            _baslik = string.Empty;
            _aciklama = string.Empty;
            _durum = TaskStatus.Beklemede;
            _oncelik = TaskPriority.Orta;
        }

        public Task(string baslik, string aciklama, int projeId, int? atananKullaniciId = null) : this()
        {
            _baslik = baslik;
            _aciklama = aciklama;
            _projeId = projeId;
            _atananKullaniciId = atananKullaniciId;
        }

        /// <summary>
        /// Son tarihi geçmiş mi kontrol eder
        /// </summary>
        public bool IsOverdue()
        {
            return SonTarih.HasValue && 
                   SonTarih.Value < DateTime.Now && 
                   Durum != TaskStatus.Tamamlandi;
        }

        public override string ToString()
        {
            return $"{Baslik} ({Durum})";
        }
    }

    /// <summary>
    /// Görev durumları
    /// </summary>
    public enum TaskStatus
    {
        Beklemede,
        DevamEdiyor,
        Tamamlandi
    }

    /// <summary>
    /// Görev öncelikleri
    /// </summary>
    public enum TaskPriority
    {
        Dusuk,
        Orta,
        Yuksek
    }
}
