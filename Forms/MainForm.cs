using System;
using System.Drawing;
using System.Windows.Forms;
using CompanyTaskProjectManagement.Entities;
using CompanyTaskProjectManagement.Services;

namespace CompanyTaskProjectManagement.Forms
{
    /// <summary>
    /// Ana Form - Dashboard
    /// </summary>
    public partial class MainForm : Form
    {
        private readonly UserService _userService;
        private readonly ProjectService _projectService;
        private readonly TaskService _taskService;
        private readonly User _currentUser;

        private MenuStrip menuStrip;
        private ToolStripMenuItem menuProjeler;
        private ToolStripMenuItem menuGorevler;
        private ToolStripMenuItem menuKullanicilar;
        private ToolStripMenuItem menuCikis;

        private Label lblHosgeldin;
        private GroupBox grpIstatistikler;
        private Label lblToplamProje;
        private Label lblToplamGorev;
        private Label lblBekleyenGorev;
        private Label lblTamamlananGorev;

        public MainForm(User currentUser, UserService userService, 
            ProjectService projectService, TaskService taskService)
        {
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _projectService = projectService ?? throw new ArgumentNullException(nameof(projectService));
            _taskService = taskService ?? throw new ArgumentNullException(nameof(taskService));
            
            InitializeComponent();
            LoadStatistics();
        }

        private void InitializeComponent()
        {
            this.menuStrip = new MenuStrip();
            this.menuProjeler = new ToolStripMenuItem();
            this.menuGorevler = new ToolStripMenuItem();
            this.menuKullanicilar = new ToolStripMenuItem();
            this.menuCikis = new ToolStripMenuItem();
            this.lblHosgeldin = new Label();
            this.grpIstatistikler = new GroupBox();
            this.lblToplamProje = new Label();
            this.lblToplamGorev = new Label();
            this.lblBekleyenGorev = new Label();
            this.lblTamamlananGorev = new Label();
            
            this.SuspendLayout();

            // menuStrip
            this.menuStrip.Items.AddRange(new ToolStripItem[] {
                this.menuProjeler,
                this.menuGorevler,
                this.menuKullanicilar,
                this.menuCikis
            });
            this.menuStrip.BackColor = Color.FromArgb(0, 120, 215);
            this.menuStrip.Font = new Font("Segoe UI", 11F, FontStyle.Regular);
            this.menuStrip.Location = new Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Padding = new Padding(10, 8, 0, 8);
            this.menuStrip.Size = new Size(800, 40);

            // menuProjeler
            this.menuProjeler.ForeColor = Color.White;
            this.menuProjeler.Name = "menuProjeler";
            this.menuProjeler.Padding = new Padding(15, 5, 15, 5);
            this.menuProjeler.Size = new Size(90, 24);
            this.menuProjeler.Text = "📁 Projeler";
            this.menuProjeler.Click += MenuProjeler_Click;

            // menuGorevler
            this.menuGorevler.ForeColor = Color.White;
            this.menuGorevler.Name = "menuGorevler";
            this.menuGorevler.Padding = new Padding(15, 5, 15, 5);
            this.menuGorevler.Size = new Size(95, 24);
            this.menuGorevler.Text = "✓ Görevler";
            this.menuGorevler.Click += MenuGorevler_Click;

            // menuKullanicilar
            this.menuKullanicilar.ForeColor = Color.White;
            this.menuKullanicilar.Name = "menuKullanicilar";
            this.menuKullanicilar.Padding = new Padding(15, 5, 15, 5);
            this.menuKullanicilar.Size = new Size(115, 24);
            this.menuKullanicilar.Text = "👥 Kullanıcılar";
            this.menuKullanicilar.Visible = (_currentUser.Rol == UserRole.Admin);
            this.menuKullanicilar.Click += MenuKullanicilar_Click;

            // menuCikis
            this.menuCikis.ForeColor = Color.White;
            this.menuCikis.Name = "menuCikis";
            this.menuCikis.Padding = new Padding(15, 5, 15, 5);
            this.menuCikis.Size = new Size(70, 24);
            this.menuCikis.Text = "🚪 Çıkış";
            this.menuCikis.Click += MenuCikis_Click;

            // lblHosgeldin
            this.lblHosgeldin.AutoSize = true;
            this.lblHosgeldin.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            this.lblHosgeldin.ForeColor = Color.FromArgb(0, 120, 215);
            this.lblHosgeldin.Location = new Point(20, 60);
            this.lblHosgeldin.Name = "lblHosgeldin";
            this.lblHosgeldin.Text = $"Hoşgeldiniz, {_currentUser.AdSoyad}";

            // grpIstatistikler
            this.grpIstatistikler.BackColor = Color.White;
            this.grpIstatistikler.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.grpIstatistikler.ForeColor = Color.FromArgb(0, 120, 215);
            this.grpIstatistikler.Location = new Point(20, 100);
            this.grpIstatistikler.Name = "grpIstatistikler";
            this.grpIstatistikler.Size = new Size(760, 200);
            this.grpIstatistikler.Text = "İstatistikler";
            this.grpIstatistikler.Controls.Add(this.lblToplamProje);
            this.grpIstatistikler.Controls.Add(this.lblToplamGorev);
            this.grpIstatistikler.Controls.Add(this.lblBekleyenGorev);
            this.grpIstatistikler.Controls.Add(this.lblTamamlananGorev);

            // lblToplamProje
            this.lblToplamProje.AutoSize = true;
            this.lblToplamProje.Font = new Font("Segoe UI", 12F);
            this.lblToplamProje.ForeColor = Color.FromArgb(16, 124, 16);
            this.lblToplamProje.Location = new Point(20, 30);
            this.lblToplamProje.Name = "lblToplamProje";
            this.lblToplamProje.Text = "Toplam Proje: 0";

            // lblToplamGorev
            this.lblToplamGorev.AutoSize = true;
            this.lblToplamGorev.Font = new Font("Segoe UI", 12F);
            this.lblToplamGorev.ForeColor = Color.FromArgb(0, 120, 215);
            this.lblToplamGorev.Location = new Point(20, 60);
            this.lblToplamGorev.Name = "lblToplamGorev";
            this.lblToplamGorev.Text = "Toplam Görev: 0";

            // lblBekleyenGorev
            this.lblBekleyenGorev.AutoSize = true;
            this.lblBekleyenGorev.Font = new Font("Segoe UI", 12F);
            this.lblBekleyenGorev.ForeColor = Color.FromArgb(255, 140, 0);
            this.lblBekleyenGorev.Location = new Point(20, 90);
            this.lblBekleyenGorev.Name = "lblBekleyenGorev";
            this.lblBekleyenGorev.Text = "Bekleyen Görev: 0";

            // lblTamamlananGorev
            this.lblTamamlananGorev.AutoSize = true;
            this.lblTamamlananGorev.Font = new Font("Segoe UI", 12F);
            this.lblTamamlananGorev.ForeColor = Color.FromArgb(16, 124, 16);
            this.lblTamamlananGorev.Location = new Point(20, 120);
            this.lblTamamlananGorev.Name = "lblTamamlananGorev";
            this.lblTamamlananGorev.Text = "Tamamlanan Görev: 0";

            // MainForm
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.FromArgb(245, 247, 250);
            this.ClientSize = new Size(800, 500);
            this.Controls.Add(this.lblHosgeldin);
            this.Controls.Add(this.grpIstatistikler);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.Name = "MainForm";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Görev ve Proje Yönetim Sistemi";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void LoadStatistics()
        {
            try
            {
                var projects = _projectService.GetAllProjects();
                var tasks = _taskService.GetAllTasks();
                var stats = _taskService.GetTaskStatistics();

                int toplamProje = 0;
                int toplamGorev = 0;
                foreach (var p in projects) toplamProje++;
                foreach (var t in tasks) toplamGorev++;

                lblToplamProje.Text = $"Toplam Proje: {toplamProje}";
                lblToplamGorev.Text = $"Toplam Görev: {toplamGorev}";
                lblBekleyenGorev.Text = $"Bekleyen Görev: {stats[TaskStatus.Beklemede]}";
                lblTamamlananGorev.Text = $"Tamamlanan Görev: {stats[TaskStatus.Tamamlandi]}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"İstatistikler yüklenirken hata: {ex.Message}", "Hata",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MenuProjeler_Click(object sender, EventArgs e)
        {
            var projectForm = new ProjectForm(_projectService);
            projectForm.ShowDialog();
            LoadStatistics(); // İstatistikleri yenile
        }

        private void MenuGorevler_Click(object sender, EventArgs e)
        {
            var taskForm = new TaskForm(_taskService, _projectService, _userService, _currentUser);
            taskForm.ShowDialog();
            LoadStatistics(); // İstatistikleri yenile
        }

        private void MenuKullanicilar_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Kullanıcı yönetimi formu henüz eklenmedi.", "Bilgi",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void MenuCikis_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Çıkmak istediğinizden emin misiniz?", "Çıkış",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
    }
}
