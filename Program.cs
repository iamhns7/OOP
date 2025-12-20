using System;
using System.Windows.Forms;
using CompanyTaskProjectManagement.Forms;
using CompanyTaskProjectManagement.Repositories;
using CompanyTaskProjectManagement.Services;

namespace CompanyTaskProjectManagement
{
    static class Program
    {
        /// <summary>
        /// Uygulamanın ana giriş noktası.
        /// Dependency Injection Pattern kullanılarak servisler oluşturulur.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Repository'leri oluştur (In-Memory)
            var userRepository = new UserRepository();
            var projectRepository = new ProjectRepository();
            var taskRepository = new TaskRepository();

            // Service'leri oluştur (Dependency Injection)
            var userService = new UserService(userRepository);
            var projectService = new ProjectService(projectRepository);
            var taskService = new TaskService(taskRepository, projectRepository, userRepository);

            // Login formunu göster
            using (var loginForm = new LoginForm(userService))
            {
                if (loginForm.ShowDialog() == DialogResult.OK)
                {
                    // Başarılı giriş - Ana formu aç
                    var authenticatedUser = loginForm.AuthenticatedUser;
                    Application.Run(new MainForm(authenticatedUser, userService, projectService, taskService));
                }
            }
        }
    }
}
