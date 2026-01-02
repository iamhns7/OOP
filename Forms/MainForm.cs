using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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
        private Label lblKullaniciBilgi;
        private GroupBox grpIstatistikler;
        private Label lblToplamProje;
        private Label lblToplamGorev;
        private Label lblBekleyenGorev;
        private Label lblDevamEdenGorev;
        private Label lblTamamlananGorev;
        private GroupBox grpHizliIslemler;
        private Button btnYeniProje;
        private Button btnYeniGorev;
        private Button btnTumGorevler;
        private GroupBox grpSonGorevler;
        private ListBox lstSonGorevler;
        private Button btnGorevDetay;
        private GroupBox grpKullaniciPerformans;
        private Label lblKullaniciPerformans;
        private ProgressBar prgKullaniciBasari;

        public MainForm(User currentUser, UserService userService, 
            ProjectService projectService, TaskService taskService)
        {
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _projectService = projectService ?? throw new ArgumentNullException(nameof(projectService));
            _taskService = taskService ?? throw new ArgumentNullException(nameof(taskService));
            
            InitializeComponent();
            ConfigureRoleBasedUI(); // Rol bazlı arayüz ayarları
            LoadStatistics();
        }

        /// <summary>
        /// Rol bazlı arayüz ayarlaması (SOLID: Single Responsibility)
        /// </summary>
        private void ConfigureRoleBasedUI()
        {
            if (_currentUser.Rol == UserRole.Calisan)
            {
                // Çalışanlar için menü ve buton kısıtlamaları
                menuProjeler.Visible = false; // Çalışanlar proje yönetimine erişemez
                menuKullanicilar.Visible = false; // Kullanıcı yönetimi admin'e özel
                btnYeniProje.Visible = false; // Proje oluşturma butonu gizle
                
                // Hoşgeldin mesajını güncelle
                lblHosgeldin.Text = $"Hoşgeldin, {_currentUser.AdSoyad} (Çalışan) 👤";
            }
            else if (_currentUser.Rol == UserRole.Admin)
            {
                lblHosgeldin.Text = $"Hoşgeldin, {_currentUser.AdSoyad} (Admin) 👨‍💼";
            }
        }

        private void InitializeComponent()
        {
            this.menuStrip = new MenuStrip();
            this.menuProjeler = new ToolStripMenuItem();
            this.menuGorevler = new ToolStripMenuItem();
            this.menuKullanicilar = new ToolStripMenuItem();
            this.menuCikis = new ToolStripMenuItem();
            this.lblHosgeldin = new Label();
            this.lblKullaniciBilgi = new Label();
            this.grpIstatistikler = new GroupBox();
            this.lblToplamProje = new Label();
            this.lblToplamGorev = new Label();
            this.lblBekleyenGorev = new Label();
            this.lblDevamEdenGorev = new Label();
            this.lblTamamlananGorev = new Label();
            this.grpHizliIslemler = new GroupBox();
            this.btnYeniProje = new Button();
            this.btnYeniGorev = new Button();
            this.btnTumGorevler = new Button();
            this.grpSonGorevler = new GroupBox();
            this.lstSonGorevler = new ListBox();
            this.btnGorevDetay = new Button();
            this.grpKullaniciPerformans = new GroupBox();
            this.lblKullaniciPerformans = new Label();
            this.prgKullaniciBasari = new ProgressBar();
            
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

            // lblKullaniciBilgi
            this.lblKullaniciBilgi.AutoSize = true;
            this.lblKullaniciBilgi.Font = new Font("Segoe UI", 10F);
            this.lblKullaniciBilgi.ForeColor = Color.FromArgb(100, 100, 100);
            this.lblKullaniciBilgi.Location = new Point(20, 90);
            this.lblKullaniciBilgi.Name = "lblKullaniciBilgi";
            this.lblKullaniciBilgi.Text = "Yükleniyor...";

            // grpIstatistikler
            this.grpIstatistikler.BackColor = Color.White;
            this.grpIstatistikler.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.grpIstatistikler.ForeColor = Color.FromArgb(0, 120, 215);
            this.grpIstatistikler.Location = new Point(20, 130);
            this.grpIstatistikler.Name = "grpIstatistikler";
            this.grpIstatistikler.Size = new Size(360, 200);
            this.grpIstatistikler.Text = "📊 Görev Durumu";
            this.grpIstatistikler.Controls.Add(this.lblToplamProje);
            this.grpIstatistikler.Controls.Add(this.lblToplamGorev);
            this.grpIstatistikler.Controls.Add(this.lblBekleyenGorev);
            this.grpIstatistikler.Controls.Add(this.lblDevamEdenGorev);
            this.grpIstatistikler.Controls.Add(this.lblTamamlananGorev);

            // lblToplamProje
            this.lblToplamProje.AutoSize = true;
            this.lblToplamProje.Font = new Font("Segoe UI", 11F);
            this.lblToplamProje.ForeColor = Color.FromArgb(80, 80, 80);
            this.lblToplamProje.Location = new Point(20, 30);
            this.lblToplamProje.Name = "lblToplamProje";
            this.lblToplamProje.Text = "📁 Toplam Proje: 0";

            // lblToplamGorev
            this.lblToplamGorev.AutoSize = true;
            this.lblToplamGorev.Font = new Font("Segoe UI", 11F);
            this.lblToplamGorev.ForeColor = Color.FromArgb(80, 80, 80);
            this.lblToplamGorev.Location = new Point(20, 60);
            this.lblToplamGorev.Name = "lblToplamGorev";
            this.lblToplamGorev.Text = "📋 Toplam Görev: 0";

            // lblBekleyenGorev
            this.lblBekleyenGorev.AutoSize = true;
            this.lblBekleyenGorev.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            this.lblBekleyenGorev.ForeColor = Color.FromArgb(220, 53, 69);
            this.lblBekleyenGorev.Location = new Point(20, 95);
            this.lblBekleyenGorev.Name = "lblBekleyenGorev";
            this.lblBekleyenGorev.Text = "🔴 Bekleyen: 0";

            // lblDevamEdenGorev
            this.lblDevamEdenGorev.AutoSize = true;
            this.lblDevamEdenGorev.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            this.lblDevamEdenGorev.ForeColor = Color.FromArgb(255, 140, 0);
            this.lblDevamEdenGorev.Location = new Point(20, 125);
            this.lblDevamEdenGorev.Name = "lblDevamEdenGorev";
            this.lblDevamEdenGorev.Text = "🟠 Devam Eden: 0";

            // lblTamamlananGorev
            this.lblTamamlananGorev.AutoSize = true;
            this.lblTamamlananGorev.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            this.lblTamamlananGorev.ForeColor = Color.FromArgb(40, 167, 69);
            this.lblTamamlananGorev.Location = new Point(20, 155);
            this.lblTamamlananGorev.Name = "lblTamamlananGorev";
            this.lblTamamlananGorev.Text = "🟢 Tamamlanan: 0";

            // grpHizliIslemler
            this.grpHizliIslemler.BackColor = Color.White;
            this.grpHizliIslemler.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.grpHizliIslemler.ForeColor = Color.FromArgb(0, 120, 215);
            this.grpHizliIslemler.Location = new Point(400, 130);
            this.grpHizliIslemler.Name = "grpHizliIslemler";
            this.grpHizliIslemler.Size = new Size(380, 200);
            this.grpHizliIslemler.Text = "⚡ Hızlı İşlemler";
            this.grpHizliIslemler.Controls.Add(this.btnYeniProje);
            this.grpHizliIslemler.Controls.Add(this.btnYeniGorev);
            this.grpHizliIslemler.Controls.Add(this.btnTumGorevler);

            // btnYeniProje
            this.btnYeniProje.BackColor = Color.FromArgb(40, 167, 69);
            this.btnYeniProje.FlatStyle = FlatStyle.Flat;
            this.btnYeniProje.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            this.btnYeniProje.ForeColor = Color.White;
            this.btnYeniProje.Location = new Point(20, 35);
            this.btnYeniProje.Name = "btnYeniProje";
            this.btnYeniProje.Size = new Size(340, 40);
            this.btnYeniProje.Text = "➕ Yeni Proje Oluştur";
            this.btnYeniProje.UseVisualStyleBackColor = false;
            this.btnYeniProje.Cursor = Cursors.Hand;
            this.btnYeniProje.Click += BtnYeniProje_Click;

            // btnYeniGorev
            this.btnYeniGorev.BackColor = Color.FromArgb(0, 120, 215);
            this.btnYeniGorev.FlatStyle = FlatStyle.Flat;
            this.btnYeniGorev.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            this.btnYeniGorev.ForeColor = Color.White;
            this.btnYeniGorev.Location = new Point(20, 85);
            this.btnYeniGorev.Name = "btnYeniGorev";
            this.btnYeniGorev.Size = new Size(340, 40);
            this.btnYeniGorev.Text = "➕ Yeni Görev Ekle";
            this.btnYeniGorev.UseVisualStyleBackColor = false;
            this.btnYeniGorev.Cursor = Cursors.Hand;
            this.btnYeniGorev.Click += BtnYeniGorev_Click;

            // btnTumGorevler
            this.btnTumGorevler.BackColor = Color.FromArgb(108, 117, 125);
            this.btnTumGorevler.FlatStyle = FlatStyle.Flat;
            this.btnTumGorevler.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            this.btnTumGorevler.ForeColor = Color.White;
            this.btnTumGorevler.Location = new Point(20, 135);
            this.btnTumGorevler.Name = "btnTumGorevler";
            this.btnTumGorevler.Size = new Size(340, 40);
            this.btnTumGorevler.Text = "📋 Tüm Görevleri Görüntüle";
            this.btnTumGorevler.UseVisualStyleBackColor = false;
            this.btnTumGorevler.Cursor = Cursors.Hand;
            this.btnTumGorevler.Click += BtnTumGorevler_Click;

            // grpSonGorevler
            this.grpSonGorevler.BackColor = Color.White;
            this.grpSonGorevler.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.grpSonGorevler.ForeColor = Color.FromArgb(0, 120, 215);
            this.grpSonGorevler.Location = new Point(400, 350);
            this.grpSonGorevler.Name = "grpSonGorevler";
            this.grpSonGorevler.Size = new Size(380, 230);
            this.grpSonGorevler.Text = "🕒 Son Eklenen Görevler";
            this.grpSonGorevler.Controls.Add(this.lstSonGorevler);

            // lstSonGorevler
            this.lstSonGorevler.BackColor = Color.FromArgb(250, 250, 250);
            this.lstSonGorevler.BorderStyle = BorderStyle.None;
            this.lstSonGorevler.Font = new Font("Segoe UI", 10F);
            this.lstSonGorevler.ForeColor = Color.FromArgb(60, 60, 60);
            this.lstSonGorevler.Location = new Point(15, 30);
            this.lstSonGorevler.Name = "lstSonGorevler";
            this.lstSonGorevler.Size = new Size(350, 145);
            this.lstSonGorevler.Cursor = Cursors.Hand;
            this.lstSonGorevler.DoubleClick += LstSonGorevler_DoubleClick;

            // btnGorevDetay
            this.btnGorevDetay.BackColor = Color.FromArgb(0, 120, 215);
            this.btnGorevDetay.FlatStyle = FlatStyle.Flat;
            this.btnGorevDetay.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnGorevDetay.ForeColor = Color.White;
            this.btnGorevDetay.Location = new Point(15, 185);
            this.btnGorevDetay.Name = "btnGorevDetay";
            this.btnGorevDetay.Size = new Size(350, 35);
            this.btnGorevDetay.Text = "📋 Detayları Görüntüle";
            this.btnGorevDetay.Cursor = Cursors.Hand;
            this.btnGorevDetay.Click += BtnGorevDetay_Click;
            this.grpSonGorevler.Controls.Add(this.btnGorevDetay);

            // grpKullaniciPerformans
            this.grpKullaniciPerformans.BackColor = Color.White;
            this.grpKullaniciPerformans.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.grpKullaniciPerformans.ForeColor = Color.FromArgb(0, 120, 215);
            this.grpKullaniciPerformans.Location = new Point(20, 350);
            this.grpKullaniciPerformans.Name = "grpKullaniciPerformans";
            this.grpKullaniciPerformans.Size = new Size(360, 230);
            this.grpKullaniciPerformans.Text = "📊 Performansım";
            this.grpKullaniciPerformans.Controls.Add(this.lblKullaniciPerformans);
            this.grpKullaniciPerformans.Controls.Add(this.prgKullaniciBasari);

            // lblKullaniciPerformans
            this.lblKullaniciPerformans.AutoSize = false;
            this.lblKullaniciPerformans.Font = new Font("Segoe UI", 9F);
            this.lblKullaniciPerformans.ForeColor = Color.FromArgb(80, 80, 80);
            this.lblKullaniciPerformans.Location = new Point(15, 30);
            this.lblKullaniciPerformans.Size = new Size(330, 115);
            this.lblKullaniciPerformans.Text = "Yükleniyor...";

            // prgKullaniciBasari
            this.prgKullaniciBasari.Location = new Point(15, 155);
            this.prgKullaniciBasari.Name = "prgKullaniciBasari";
            this.prgKullaniciBasari.Size = new Size(330, 30);
            this.prgKullaniciBasari.Style = ProgressBarStyle.Continuous;
            this.prgKullaniciBasari.ForeColor = Color.FromArgb(40, 167, 69);

            // MainForm
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.FromArgb(245, 247, 250);
            this.ClientSize = new Size(800, 600);
            this.Controls.Add(this.lblHosgeldin);
            this.Controls.Add(this.lblKullaniciBilgi);
            this.Controls.Add(this.grpIstatistikler);
            this.Controls.Add(this.grpHizliIslemler);
            this.Controls.Add(this.grpKullaniciPerformans);
            this.Controls.Add(this.grpSonGorevler);
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
                var allProjects = _projectService.GetAllProjects();
                var allTasks = _taskService.GetAllTasks();
                var stats = _taskService.GetTaskStatistics();

                int toplamGorev = 0;

                // Rol bazlı veri gösterimi
                if (_currentUser.Rol == UserRole.Calisan)
                {
                    // Çalışanlar sadece kendilerine atanmış görevleri görür
                    var myTasks = allTasks.Where(t => t.AtananKullaniciId == _currentUser.Id).ToList();
                    toplamGorev = myTasks.Count;
                    
                    // Çalışanın üzerinde çalıştığı proje sayısı
                    var myProjects = myTasks.Select(t => t.ProjeId).Distinct().Count();
                    
                    // Sadece görev istatistikleri
                    lblToplamProje.Text = $"📁 Üzerinde Çalıştığım Proje: {myProjects}";
                    lblToplamGorev.Text = $"📋 Bana Atanan Görev: {toplamGorev}";
                    lblBekleyenGorev.Text = $"🔴 Bekleyen: {myTasks.Count(t => t.Durum == TaskStatus.Beklemede)}";
                    lblDevamEdenGorev.Text = $"🟠 Devam Eden: {myTasks.Count(t => t.Durum == TaskStatus.DevamEdiyor)}";
                    lblTamamlananGorev.Text = $"🟢 Tamamlanan: {myTasks.Count(t => t.Durum == TaskStatus.Tamamlandi)}";
                }
                else
                {
                    // Admin tüm verileri görür
                    foreach (var t in allTasks) toplamGorev++;
                    
                    // Sadece görev içeren projeleri say
                    var activeProjects = allProjects.Where(p => allTasks.Any(t => t.ProjeId == p.Id)).Count();

                    lblToplamProje.Text = $"📁 Üzerinde Çalışılan Proje: {activeProjects}";
                    lblToplamGorev.Text = $"📋 Toplam Görev: {toplamGorev}";
                    lblBekleyenGorev.Text = $"🔴 Bekleyen: {stats[TaskStatus.Beklemede]}";
                    lblDevamEdenGorev.Text = $"🟠 Devam Eden: {stats[TaskStatus.DevamEdiyor]}";
                    lblTamamlananGorev.Text = $"🟢 Tamamlanan: {stats[TaskStatus.Tamamlandi]}";
                }

                // Kullanıcıya özel görev sayısı
                var myAssignedTasks = allTasks.Where(t => t.AtananKullaniciId == _currentUser.Id).ToList();
                int kullaniciGorevSayisi = myAssignedTasks.Count(t => t.Durum == TaskStatus.Beklemede);
                int tamamlanan = myAssignedTasks.Count(t => t.Durum == TaskStatus.Tamamlandi);
                int toplamAtanan = myAssignedTasks.Count;

                // Kullanıcı bilgilendirme mesajı
                if (toplamAtanan == 0)
                {
                    lblKullaniciBilgi.Text = "ℹ️ Size henüz görev atanmamış.";
                    lblKullaniciBilgi.ForeColor = Color.FromArgb(108, 117, 125);
                }
                else if (kullaniciGorevSayisi > 0)
                {
                    lblKullaniciBilgi.Text = $"⚠️ Size atanmış {kullaniciGorevSayisi} adet bekleyen görev var!";
                    lblKullaniciBilgi.ForeColor = Color.FromArgb(220, 53, 69);
                }
                else if (toplamAtanan > 0 && tamamlanan == toplamAtanan)
                {
                    lblKullaniciBilgi.Text = "✅ Tüm görevleriniz tamamlanmış durumda.";
                    lblKullaniciBilgi.ForeColor = Color.FromArgb(40, 167, 69);
                }
                else
                {
                    lblKullaniciBilgi.Text = $"🔄 {toplamAtanan - tamamlanan} göreviniz devam ediyor.";
                    lblKullaniciBilgi.ForeColor = Color.FromArgb(255, 140, 0);
                }

                // Son 5 görevi yükle
                LoadRecentTasks();
                
                // Kullanıcı performansını yükle
                LoadUserPerformance();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"İstatistikler yüklenirken hata: {ex.Message}", "Hata",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Kullanıcı performans istatistiklerini yükler
        /// </summary>
        private void LoadUserPerformance()
        {
            try
            {
                var stats = _taskService.GetUserStatistics(_currentUser.Id);
                
                int totalAssigned = stats["TotalAssigned"];
                int completed = stats["Completed"];
                int inProgress = stats["InProgress"];
                int pending = stats["Pending"];
                int overdue = stats["Overdue"];
                int highPriority = stats["HighPriority"];

                // Performans metni
                var performanceText = $"📊 Toplam Atanan: {totalAssigned}\n" +
                                     $"✅ Tamamlanan: {completed}\n" +
                                     $"🔄 Devam Eden: {inProgress}\n" +
                                     $"⏳ Bekleyen: {pending}\n" +
                                     $"⚠️ Gecikmiş: {overdue}\n" +
                                     $"🔥 Yüksek Öncelik: {highPriority}";

                lblKullaniciPerformans.Text = performanceText;

                // Başarı yüzdesi hesapla
                int successPercentage = totalAssigned > 0 ? (completed * 100 / totalAssigned) : 0;
                prgKullaniciBasari.Value = Math.Min(successPercentage, 100);

                // ProgressBar rengini başarıya göre ayarla
                if (successPercentage >= 75)
                {
                    prgKullaniciBasari.ForeColor = Color.FromArgb(40, 167, 69); // Yeşil
                }
                else if (successPercentage >= 50)
                {
                    prgKullaniciBasari.ForeColor = Color.FromArgb(255, 193, 7); // Sarı
                }
                else
                {
                    prgKullaniciBasari.ForeColor = Color.FromArgb(220, 53, 69); // Kırmızı
                }
            }
            catch (Exception ex)
            {
                lblKullaniciPerformans.Text = $"Performans bilgisi yüklenemedi:\n{ex.Message}";
            }
        }

        private void LoadRecentTasks()
        {
            try
            {
                lstSonGorevler.Items.Clear();
                var tasks = _taskService.GetAllTasks().ToList();
                var users = _userService.GetAllUsers().ToList();

                // Son 5 görevi al (ID'ye göre azalan sırada)
                int count = 0;
                for (int i = tasks.Count - 1; i >= 0 && count < 5; i--)
                {
                    var task = tasks[i];
                    var user = users.FirstOrDefault(u => u.Id == task.AtananKullaniciId);
                    
                    string userName = user != null ? user.AdSoyad : "Atanmamış";
                    
                    string durum = task.Durum == TaskStatus.Beklemede ? "🔴" :
                                   task.Durum == TaskStatus.DevamEdiyor ? "🟠" : "🟢";
                    
                    // Task objesini tag olarak sakla
                    lstSonGorevler.Items.Add(new TaskListItem
                    {
                        Task = task,
                        DisplayText = $"{durum} {task.Baslik} - {userName}"
                    });
                    count++;
                }

                if (count == 0)
                {
                    lstSonGorevler.Items.Add("Henüz görev eklenmemiş.");
                }
            }
            catch (Exception ex)
            {
                lstSonGorevler.Items.Add($"Görevler yüklenirken hata: {ex.Message}");
            }
        }

        private void MenuProjeler_Click(object sender, EventArgs e)
        {
            // Rol kontrolü: Sadece admin projeleri yönetebilir
            if (_currentUser.Rol == UserRole.Calisan)
            {
                MessageBox.Show("Proje yönetimi sadece yöneticiler için erişilebilir!", "Yetki Hatası",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var projectForm = new ProjectForm(_projectService, _currentUser);
            projectForm.ShowDialog();
            LoadStatistics();
        }

        private void MenuGorevler_Click(object sender, EventArgs e)
        {
            var taskForm = new TaskForm(_taskService, _projectService, _userService, _currentUser);
            taskForm.ShowDialog();
            LoadStatistics();
        }

        private void MenuKullanicilar_Click(object sender, EventArgs e)
        {
            // Rol kontrolü: Sadece admin kullanıcı yönetimi yapabilir
            if (_currentUser.Rol == UserRole.Calisan)
            {
                MessageBox.Show("Kullanıcı yönetimi sadece yöneticiler için erişilebilir!", "Yetki Hatası",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var userForm = new UserForm(_userService, _currentUser);
            userForm.ShowDialog();
            LoadStatistics();
        }

        private void MenuCikis_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Çıkmak istediğinizden emin misiniz?", "Çıkış",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void BtnYeniProje_Click(object sender, EventArgs e)
        {
            // Rol kontrolü: Sadece admin yeni proje oluşturabilir
            if (_currentUser.Rol == UserRole.Calisan)
            {
                MessageBox.Show("Yeni proje oluşturma sadece yöneticiler için erişilebilir!", "Yetki Hatası",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var projectForm = new ProjectForm(_projectService, _currentUser);
            projectForm.ShowDialog();
            LoadStatistics();
        }

        private void BtnYeniGorev_Click(object sender, EventArgs e)
        {
            var taskForm = new TaskForm(_taskService, _projectService, _userService, _currentUser);
            taskForm.ShowDialog();
            LoadStatistics();
        }

        private void BtnTumGorevler_Click(object sender, EventArgs e)
        {
            var taskForm = new TaskForm(_taskService, _projectService, _userService, _currentUser);
            taskForm.ShowDialog();
            LoadStatistics();
        }

        /// <summary>
        /// Son eklenen görevler listesinde çift tıklama ile detay göster
        /// </summary>
        private void LstSonGorevler_DoubleClick(object sender, EventArgs e)
        {
            ShowTaskDetails();
        }

        /// <summary>
        /// Detayları Görüntüle butonuna basıldığında görev detayını göster
        /// </summary>
        private void BtnGorevDetay_Click(object sender, EventArgs e)
        {
            ShowTaskDetails();
        }

        /// <summary>
        /// Seçili görevin detaylarını göster
        /// </summary>
        private void ShowTaskDetails()
        {
            if (lstSonGorevler.SelectedItem == null)
            {
                MessageBox.Show("Lütfen detaylarını görmek için bir görev seçin!", "Uyarı",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (lstSonGorevler.SelectedItem is TaskListItem taskItem)
            {
                var task = taskItem.Task;
                
                // Proje bilgisini al
                var project = _projectService.GetById(task.ProjeId);
                string projeAdi = project != null ? project.ProjeAdi : "Bilinmeyen Proje";
                
                // Kullanıcı bilgisini al
                var user = task.AtananKullaniciId.HasValue ? _userService.GetById(task.AtananKullaniciId.Value) : null;
                string atananKullanici = user != null ? user.AdSoyad : "Atanmamış";
                
                // Durum bilgisi
                string durum = task.Durum == TaskStatus.Beklemede ? "Beklemede 🔴" :
                              task.Durum == TaskStatus.DevamEdiyor ? "Devam Ediyor 🟠" : "Tamamlandı 🟢";
                
                // Öncelik bilgisi
                string oncelik = task.Oncelik == TaskPriority.Dusuk ? "Düşük" :
                                task.Oncelik == TaskPriority.Orta ? "Orta" : "Yüksek 🔥";
                
                // Son tarih bilgisi
                string sonTarih = task.SonTarih.HasValue 
                    ? task.SonTarih.Value.ToString("dd.MM.yyyy") 
                    : "Belirlenmemiş";
                
                // Gecikme durumu
                string gecikme = task.IsOverdue() ? "\n⚠️ GECİKMİŞ GÖREV!" : "";
                
                // Oluşturulma tarihi
                string olusturma = task.OlusturmaTarihi.ToString("dd.MM.yyyy HH:mm");
                
                // Detay mesajı
                string detayMesaji = $"📋 GÖREV DETAYLARI\n" +
                                    $"━━━━━━━━━━━━━━━━━━━━━━━\n\n" +
                                    $"📌 Başlık: {task.Baslik}\n\n" +
                                    $"📝 Açıklama:\n{(string.IsNullOrWhiteSpace(task.Aciklama) ? "Açıklama yok" : task.Aciklama)}\n\n" +
                                    $"📁 Proje: {projeAdi}\n\n" +
                                    $"👤 Atanan Kullanıcı: {atananKullanici}\n\n" +
                                    $"🔖 Durum: {durum}\n\n" +
                                    $"⚡ Öncelik: {oncelik}\n\n" +
                                    $"📅 Son Tarih: {sonTarih}\n\n" +
                                    $"🕐 Oluşturulma: {olusturma}\n\n" +
                                    $"🆔 Görev ID: {task.Id}" +
                                    gecikme;
                
                MessageBox.Show(detayMesaji, "Görev Detayları", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }

    /// <summary>
    /// ListBox için görev item wrapper class
    /// </summary>
    internal class TaskListItem
    {
        public Task Task { get; set; }
        public string DisplayText { get; set; }

        public override string ToString()
        {
            return DisplayText;
        }
    }
}
