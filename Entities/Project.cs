using System;

namespace CompanyTaskProjectManagement.Entities
{
    /// <summary>
    /// Proje varlığı - BaseEntity'den türetilmiştir (Inheritance)
    /// </summary>
    public class Project : BaseEntity
    {
        // Encapsulation: private field'lar
        private string _projeAdi;
        private string _aciklama;
        private DateTime _baslangicTarihi;
        private DateTime? _bitisTarihi;

        // Public property'ler
        public string ProjeAdi 
        { 
            get => _projeAdi; 
            set => _projeAdi = value; 
        }

        public string Aciklama 
        { 
            get => _aciklama; 
            set => _aciklama = value; 
        }

        public DateTime BaslangicTarihi 
        { 
            get => _baslangicTarihi; 
            set => _baslangicTarihi = value; 
        }

        public DateTime? BitisTarihi 
        { 
            get => _bitisTarihi; 
            set => _bitisTarihi = value; 
        }
        public string Ad { get; internal set; }

        public Project()
        {
            _projeAdi = string.Empty;
            _aciklama = string.Empty;
            _baslangicTarihi = DateTime.Now;
        }

        public Project(string projeAdi, string aciklama, DateTime baslangicTarihi, DateTime? bitisTarihi = null) : this()
        {
            _projeAdi = projeAdi;
            _aciklama = aciklama;
            _baslangicTarihi = baslangicTarihi;
            _bitisTarihi = bitisTarihi;
        }

        /// <summary>
        /// Projenin tamamlanma yüzdesini hesaplar (Calculated Property)
        /// OOP: Encapsulation - İş mantığı entity içinde
        /// </summary>
        public int GetCompletionPercentage(int totalTasks, int completedTasks)
        {
            if (totalTasks == 0) return 0;
            return (int)((double)completedTasks / totalTasks * 100);
        }

        /// <summary>
        /// Projenin aktif olup olmadığını kontrol eder
        /// </summary>
        public bool IsActive()
        {
            if (!BitisTarihi.HasValue) return true;
            return BitisTarihi.Value >= DateTime.Now;
        }

        /// <summary>
        /// Projenin toplam gün sayısını hesaplar
        /// </summary>
        public int GetTotalDays()
        {
            var endDate = BitisTarihi ?? DateTime.Now;
            return (endDate - BaslangicTarihi).Days;
        }

        public override string ToString()
        {
            return $"{ProjeAdi} ({BaslangicTarihi:dd.MM.yyyy})";
        }
    }
}
