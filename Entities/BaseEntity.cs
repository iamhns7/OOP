using System;

namespace CompanyTaskProjectManagement.Entities
{
    /// <summary>
    /// Tüm varlıklar için temel sınıf (Inheritance örneği)
    /// </summary>
    public abstract class BaseEntity
    {
        // Encapsulation: private field, public property
        private int _id;
        
        public int Id 
        { 
            get => _id; 
            set => _id = value; 
        }

        public DateTime OlusturmaTarihi { get; set; }

        protected BaseEntity()
        {
            OlusturmaTarihi = DateTime.Now;
        }
    }
}
