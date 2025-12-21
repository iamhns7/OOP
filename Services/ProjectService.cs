using System;
using System.Collections.Generic;
using System.Linq;
using CompanyTaskProjectManagement.Entities;
using CompanyTaskProjectManagement.Repositories;

namespace CompanyTaskProjectManagement.Services
{
    /// <summary>
    /// Proje Service - İş mantığı katmanı
    /// Single Responsibility Principle: Sadece proje işlemleriyle ilgilenir
    /// </summary>
    public class ProjectService
    {
        private readonly IProjectRepository _projectRepository;

        // Dependency Injection (Constructor Injection)
        public ProjectService(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository ?? throw new ArgumentNullException(nameof(projectRepository));
        }

        public Project GetById(int id)
        {
            return _projectRepository.GetById(id);
        }

        public IEnumerable<Project> GetAllProjects()
        {
            return _projectRepository.GetAll();
        }

        public IEnumerable<Project> GetActiveProjects()
        {
            return _projectRepository.GetActiveProjects();
        }

        public void AddProject(Project project)
        {
            if (project == null)
                throw new ArgumentNullException(nameof(project));

            ValidateProject(project);
            _projectRepository.Add(project);
        }

        public void UpdateProject(Project project)
        {
            if (project == null)
                throw new ArgumentNullException(nameof(project));

            ValidateProject(project);

            var existingProject = _projectRepository.GetById(project.Id);
            if (existingProject == null)
                throw new InvalidOperationException("Proje bulunamadı!");

            _projectRepository.Update(project);
        }

        public void DeleteProject(int id)
        {
            var project = _projectRepository.GetById(id);
            if (project == null)
                throw new InvalidOperationException("Proje bulunamadı!");

            _projectRepository.Delete(id);
        }

        /// <summary>
        /// Proje istatistiklerini döndürür (Aggregation örneği)
        /// </summary>
        public Dictionary<string, object> GetProjectStatistics(int projectId, IEnumerable<Task> projectTasks)
        {
            var tasks = projectTasks.ToList();
            var totalTasks = tasks.Count;
            var completedTasks = tasks.Count(t => t.Durum == TaskStatus.Tamamlandi);
            var pendingTasks = tasks.Count(t => t.Durum == TaskStatus.Beklemede);
            var inProgressTasks = tasks.Count(t => t.Durum == TaskStatus.DevamEdiyor);
            var overdueTasks = tasks.Count(t => t.IsOverdue());

            return new Dictionary<string, object>
            {
                { "TotalTasks", totalTasks },
                { "CompletedTasks", completedTasks },
                { "PendingTasks", pendingTasks },
                { "InProgressTasks", inProgressTasks },
                { "OverdueTasks", overdueTasks },
                { "CompletionPercentage", totalTasks > 0 ? (completedTasks * 100 / totalTasks) : 0 }
            };
        }

        private void ValidateProject(Project project)
        {
            if (string.IsNullOrWhiteSpace(project.ProjeAdi))
                throw new ArgumentException("Proje adı boş olamaz!");

            if (project.BaslangicTarihi == default)
                throw new ArgumentException("Başlangıç tarihi geçersiz!");

            if (project.BitisTarihi.HasValue && project.BitisTarihi < project.BaslangicTarihi)
                throw new ArgumentException("Bitiş tarihi, başlangıç tarihinden önce olamaz!");
        }
    }
}
