using System;
using System.Collections.Generic;
using System.Linq;
using CompanyTaskProjectManagement.Entities;
using CompanyTaskProjectManagement.Repositories;

namespace CompanyTaskProjectManagement.Services
{
    /// <summary>
    /// Görev Service - İş mantığı katmanı
    /// Single Responsibility Principle: Sadece görev işlemleriyle ilgilenir
    /// </summary>
    public class TaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IUserRepository _userRepository;

        // Dependency Injection (Constructor Injection)
        public TaskService(
            ITaskRepository taskRepository, 
            IProjectRepository projectRepository,
            IUserRepository userRepository)
        {
            _taskRepository = taskRepository ?? throw new ArgumentNullException(nameof(taskRepository));
            _projectRepository = projectRepository ?? throw new ArgumentNullException(nameof(projectRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public Task GetById(int id)
        {
            return _taskRepository.GetById(id);
        }

        public IEnumerable<Task> GetAllTasks()
        {
            return _taskRepository.GetAll();
        }

        public IEnumerable<Task> GetTasksByProject(int projeId)
        {
            return _taskRepository.GetByProjectId(projeId);
        }

        public IEnumerable<Task> GetTasksByUser(int kullaniciId)
        {
            return _taskRepository.GetByUserId(kullaniciId);
        }

        public IEnumerable<Task> GetTasksByStatus(TaskStatus durum)
        {
            return _taskRepository.GetByStatus(durum);
        }

        /// <summary>
        /// Belirli bir kullanıcıya atanmış bekleyen görevleri getirir (Filtreleme)
        /// </summary>
        public IEnumerable<Task> GetPendingTasksByUser(int kullaniciId)
        {
            return _taskRepository.GetByUserId(kullaniciId)
                .Where(t => t.Durum == TaskStatus.Beklemede);
        }

        /// <summary>
        /// Sadece bekleyen görevleri getirir (Filtreleme)
        /// </summary>
        public IEnumerable<Task> GetPendingTasks()
        {
            return _taskRepository.GetByStatus(TaskStatus.Beklemede);
        }

        /// <summary>
        /// Son tarihi geçmiş görevleri getirir
        /// </summary>
        public IEnumerable<Task> GetOverdueTasks()
        {
            return _taskRepository.GetAll()
                .Where(t => t.IsOverdue());
        }

        /// <summary>
        /// Belirli önceliğe göre görevleri getirir
        /// </summary>
        public IEnumerable<Task> GetTasksByPriority(TaskPriority oncelik)
        {
            return _taskRepository.GetAll()
                .Where(t => t.Oncelik == oncelik);
        }

        public void AddTask(Task task)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));

            ValidateTask(task);
            _taskRepository.Add(task);
        }

        public void UpdateTask(Task task)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));

            ValidateTask(task);

            var existingTask = _taskRepository.GetById(task.Id);
            if (existingTask == null)
                throw new InvalidOperationException("Görev bulunamadı!");

            _taskRepository.Update(task);
        }

        public void DeleteTask(int id)
        {
            var task = _taskRepository.GetById(id);
            if (task == null)
                throw new InvalidOperationException("Görev bulunamadı!");

            _taskRepository.Delete(id);
        }

        public void ChangeTaskStatus(int taskId, TaskStatus yeniDurum)
        {
            var task = _taskRepository.GetById(taskId);
            if (task == null)
                throw new InvalidOperationException("Görev bulunamadı!");

            task.Durum = yeniDurum;
            _taskRepository.Update(task);
        }

        public void AssignTaskToUser(int taskId, int kullaniciId)
        {
            var task = _taskRepository.GetById(taskId);
            if (task == null)
                throw new InvalidOperationException("Görev bulunamadı!");

            var user = _userRepository.GetById(kullaniciId);
            if (user == null)
                throw new InvalidOperationException("Kullanıcı bulunamadı!");

            task.AtananKullaniciId = kullaniciId;
            _taskRepository.Update(task);
        }

        public Dictionary<TaskStatus, int> GetTaskStatistics()
        {
            var allTasks = _taskRepository.GetAll();
            return new Dictionary<TaskStatus, int>
            {
                { TaskStatus.Beklemede, allTasks.Count(t => t.Durum == TaskStatus.Beklemede) },
                { TaskStatus.DevamEdiyor, allTasks.Count(t => t.Durum == TaskStatus.DevamEdiyor) },
                { TaskStatus.Tamamlandi, allTasks.Count(t => t.Durum == TaskStatus.Tamamlandi) }
            };
        }

        private void ValidateTask(Task task)
        {
            if (string.IsNullOrWhiteSpace(task.Baslik))
                throw new ArgumentException("Görev başlığı boş olamaz!");

            // Proje kontrolü
            var project = _projectRepository.GetById(task.ProjeId);
            if (project == null)
                throw new ArgumentException("Geçersiz proje ID!");

            // Kullanıcı kontrolü (eğer atanmışsa)
            if (task.AtananKullaniciId.HasValue)
            {
                var user = _userRepository.GetById(task.AtananKullaniciId.Value);
                if (user == null)
                    throw new ArgumentException("Geçersiz kullanıcı ID!");
            }
        }
    }
}
