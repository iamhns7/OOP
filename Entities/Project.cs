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

        public override string ToString()
        {
            return $"{ProjeAdi} ({BaslangicTarihi:dd.MM.yyyy})";
        }
    }
}
