using System;
using System.Windows.Forms;
using CompanyTaskProjectManagement.Entities;
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

            // Giriş türü seçim formu
            while (true)
            {
                using (var roleSelectionForm = new RoleSelectionForm())
                {
                    if (roleSelectionForm.ShowDialog() != DialogResult.OK)
                    {
                        // Kullanıcı çıkış yaptı
                        return;
                    }

                    bool isAdminLogin = roleSelectionForm.IsAdminLogin;
                    User authenticatedUser = null;

                    if (isAdminLogin)
                    {
                        // Admin girişi
                        using (var adminLoginForm = new AdminLoginForm(userService))
                        {
                            if (adminLoginForm.ShowDialog() == DialogResult.OK)
                            {
                                authenticatedUser = adminLoginForm.AuthenticatedUser;
                            }
                            else
                            {
                                // Admin giriş iptal - tekrar rol seçimine dön
                                continue;
                            }
                        }
                    }
                    else
                    {
                        // Normal kullanıcı girişi
                        using (var loginForm = new LoginForm(userService))
                        {
                            if (loginForm.ShowDialog() == DialogResult.OK)
                            {
                                authenticatedUser = loginForm.AuthenticatedUser;
                            }
                            else
                            {
                                // Normal giriş iptal - tekrar rol seçimine dön
                                continue;
                            }
                        }
                    }

                    // Başarılı giriş - Ana formu aç
                    if (authenticatedUser != null)
                    {
                        Application.Run(new MainForm(authenticatedUser, userService, projectService, taskService));
                        break;
                    }
                }
            }
        }
    }
}
