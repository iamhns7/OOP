using System;
using System.Collections.Generic;
using System.Linq;
using CompanyTaskProjectManagement.Entities;

namespace CompanyTaskProjectManagement.Repositories
{
    /// <summary>
    /// In-Memory Generic Repository Implementation
    /// IRepository<T> interface'ini implemente eder (Polymorphism)
    /// </summary>
    /// <typeparam name="T">BaseEntity'den türeyen herhangi bir varlık</typeparam>
    public class InMemoryRepository<T> : IRepository<T> where T : BaseEntity
    {
        // In-Memory veri depolama (List<T>)
        protected List<T> _dataStore;
        protected int _nextId;

        public InMemoryRepository()
        {
            _dataStore = new List<T>();
            _nextId = 1;
        }

        public virtual T GetById(int id)
        {
            return _dataStore.FirstOrDefault(x => x.Id == id);
        }

        public virtual IEnumerable<T> GetAll()
        {
            return _dataStore.ToList();
        }

        public virtual void Add(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            entity.Id = _nextId++;
            _dataStore.Add(entity);
        }

        public virtual void Update(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var existing = GetById(entity.Id);
            if (existing != null)
            {
                var index = _dataStore.IndexOf(existing);
                _dataStore[index] = entity;
            }
        }

        public virtual void Delete(int id)
        {
            var entity = GetById(id);
            if (entity != null)
            {
                _dataStore.Remove(entity);
            }
        }
    }
}
