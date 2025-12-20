using System;

namespace CompanyTaskProjectManagement.Entities
{
    /// <summary>
    /// Kullanıcı varlığı - BaseEntity'den türetilmiştir (Inheritance)
    /// </summary>
    public class User : BaseEntity
    {
        // Encapsulation: private field'lar
        private string _adSoyad;
        private string _kullaniciAdi;
        private string _sifre;
        private UserRole _rol;

        // Public property'ler
        public string AdSoyad 
        { 
            get => _adSoyad; 
            set => _adSoyad = value; 
        }

        public string KullaniciAdi 
        { 
            get => _kullaniciAdi; 
            set => _kullaniciAdi = value; 
        }

        public string Sifre 
        { 
            get => _sifre; 
            set => _sifre = value; 
        }

        public UserRole Rol 
        { 
            get => _rol; 
            set => _rol = value; 
        }

        public User()
        {
            _adSoyad = string.Empty;
            _kullaniciAdi = string.Empty;
            _sifre = string.Empty;
            _rol = UserRole.Calisan;
        }

        public User(string adSoyad, string kullaniciAdi, string sifre, UserRole rol) : this()
        {
            _adSoyad = adSoyad;
            _kullaniciAdi = kullaniciAdi;
            _sifre = sifre;
            _rol = rol;
        }

        public override string ToString()
        {
            return $"{AdSoyad} ({Rol})";
        }
    }

    /// <summary>
    /// Kullanıcı rolleri
    /// </summary>
    public enum UserRole
    {
        Admin,
        Calisan
    }
}
