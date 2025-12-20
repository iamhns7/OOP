using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CompanyTaskProjectManagement.Entities;
using CompanyTaskProjectManagement.Services;

namespace CompanyTaskProjectManagement.Forms
{
    /// <summary>
    /// Görev Yönetim Formu
    /// </summary>
    public partial class TaskForm : Form
    {
        private readonly TaskService _taskService;
        private readonly ProjectService _projectService;
        private readonly UserService _userService;
        private readonly User _currentUser;

        private DataGridView dgvTasks;
        private Button btnYeni;
        private Button btnDuzenle;
        private Button btnSil;
        private Button btnKapat;
        private ComboBox cmbFiltre;
        private Label lblFiltre;
        private GroupBox grpDetay;
        private TextBox txtBaslik;
        private TextBox txtAciklama;
        private ComboBox cmbProje;
        private ComboBox cmbKullanici;
        private ComboBox cmbDurum;
        private Button btnKaydet;
        private Button btnIptal;
        private Label lblBaslik;
        private Label lblAciklama;
        private Label lblProje;
        private Label lblKullanici;
        private Label lblDurum;

        private Task _selectedTask;

        public TaskForm(TaskService taskService, ProjectService projectService, 
            UserService userService, User currentUser)
        {
            _taskService = taskService ?? throw new ArgumentNullException(nameof(taskService));
            _projectService = projectService ?? throw new ArgumentNullException(nameof(projectService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
            
            InitializeComponent();
            LoadProjects();
            LoadUsers();
            LoadTasks();
        }

        private void InitializeComponent()
        {
            this.dgvTasks = new DataGridView();
            this.btnYeni = new Button();
            this.btnDuzenle = new Button();
            this.btnSil = new Button();
            this.btnKapat = new Button();
            this.lblFiltre = new Label();
            this.cmbFiltre = new ComboBox();
            this.grpDetay = new GroupBox();
            this.lblBaslik = new Label();
            this.txtBaslik = new TextBox();
            this.lblAciklama = new Label();
            this.txtAciklama = new TextBox();
            this.lblProje = new Label();
            this.cmbProje = new ComboBox();
            this.lblKullanici = new Label();
            this.cmbKullanici = new ComboBox();
            this.lblDurum = new Label();
            this.cmbDurum = new ComboBox();
            this.btnKaydet = new Button();
            this.btnIptal = new Button();

            ((System.ComponentModel.ISupportInitialize)(this.dgvTasks)).BeginInit();
            this.grpDetay.SuspendLayout();
            this.SuspendLayout();

            // dgvTasks
            this.dgvTasks.AllowUserToAddRows = false;
            this.dgvTasks.AllowUserToDeleteRows = false;
            this.dgvTasks.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTasks.Location = new Point(12, 50);
            this.dgvTasks.MultiSelect = false;
            this.dgvTasks.Name = "dgvTasks";
            this.dgvTasks.ReadOnly = true;
            this.dgvTasks.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvTasks.Size = new Size(600, 310);
            this.dgvTasks.TabIndex = 0;

            // lblFiltre
            this.lblFiltre.AutoSize = true;
            this.lblFiltre.Location = new Point(12, 15);
            this.lblFiltre.Text = "Filtre:";

            // cmbFiltre
            this.cmbFiltre.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbFiltre.Location = new Point(60, 12);
            this.cmbFiltre.Size = new Size(200, 23);
            this.cmbFiltre.Items.AddRange(new object[] { "Tümü", "Beklemede", "Devam Ediyor", "Tamamlandı" });
            this.cmbFiltre.SelectedIndex = 0;
            this.cmbFiltre.SelectedIndexChanged += CmbFiltre_SelectedIndexChanged;

            // btnYeni
            this.btnYeni.Location = new Point(12, 370);
            this.btnYeni.Size = new Size(100, 30);
            this.btnYeni.Text = "Yeni";
            this.btnYeni.Click += BtnYeni_Click;

            // btnDuzenle
            this.btnDuzenle.Location = new Point(120, 370);
            this.btnDuzenle.Size = new Size(100, 30);
            this.btnDuzenle.Text = "Düzenle";
            this.btnDuzenle.Click += BtnDuzenle_Click;

            // btnSil
            this.btnSil.Location = new Point(228, 370);
            this.btnSil.Size = new Size(100, 30);
            this.btnSil.Text = "Sil";
            this.btnSil.Click += BtnSil_Click;

            // btnKapat
            this.btnKapat.Location = new Point(512, 370);
            this.btnKapat.Size = new Size(100, 30);
            this.btnKapat.Text = "Kapat";
            this.btnKapat.Click += (s, e) => this.Close();

            // grpDetay
            this.grpDetay.Location = new Point(630, 12);
            this.grpDetay.Size = new Size(350, 390);
            this.grpDetay.Text = "Görev Detayları";
            this.grpDetay.Visible = false;

            // lblBaslik
            this.lblBaslik.AutoSize = true;
            this.lblBaslik.Location = new Point(15, 30);
            this.lblBaslik.Text = "Başlık:";
            this.grpDetay.Controls.Add(this.lblBaslik);

            // txtBaslik
            this.txtBaslik.Location = new Point(15, 50);
            this.txtBaslik.Size = new Size(320, 23);
            this.grpDetay.Controls.Add(this.txtBaslik);

            // lblAciklama
            this.lblAciklama.AutoSize = true;
            this.lblAciklama.Location = new Point(15, 85);
            this.lblAciklama.Text = "Açıklama:";
            this.grpDetay.Controls.Add(this.lblAciklama);

            // txtAciklama
            this.txtAciklama.Location = new Point(15, 105);
            this.txtAciklama.Multiline = true;
            this.txtAciklama.Size = new Size(320, 60);
            this.grpDetay.Controls.Add(this.txtAciklama);

            // lblProje
            this.lblProje.AutoSize = true;
            this.lblProje.Location = new Point(15, 180);
            this.lblProje.Text = "Proje:";
            this.grpDetay.Controls.Add(this.lblProje);

            // cmbProje
            this.cmbProje.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbProje.Location = new Point(15, 200);
            this.cmbProje.Size = new Size(320, 23);
            this.grpDetay.Controls.Add(this.cmbProje);

            // lblKullanici
            this.lblKullanici.AutoSize = true;
            this.lblKullanici.Location = new Point(15, 235);
            this.lblKullanici.Text = "Atanan Kullanıcı:";
            this.grpDetay.Controls.Add(this.lblKullanici);

            // cmbKullanici
            this.cmbKullanici.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbKullanici.Location = new Point(15, 255);
            this.cmbKullanici.Size = new Size(320, 23);
            this.grpDetay.Controls.Add(this.cmbKullanici);

            // lblDurum
            this.lblDurum.AutoSize = true;
            this.lblDurum.Location = new Point(15, 290);
            this.lblDurum.Text = "Durum:";
            this.grpDetay.Controls.Add(this.lblDurum);

            // cmbDurum
            this.cmbDurum.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbDurum.Location = new Point(15, 310);
            this.cmbDurum.Size = new Size(320, 23);
            this.cmbDurum.Items.AddRange(new object[] { "Beklemede", "Devam Ediyor", "Tamamlandı" });
            this.grpDetay.Controls.Add(this.cmbDurum);

            // btnKaydet
            this.btnKaydet.BackColor = Color.FromArgb(0, 120, 215);
            this.btnKaydet.ForeColor = Color.White;
            this.btnKaydet.Location = new Point(15, 345);
            this.btnKaydet.Size = new Size(150, 35);
            this.btnKaydet.Text = "Kaydet";
            this.btnKaydet.Click += BtnKaydet_Click;
            this.grpDetay.Controls.Add(this.btnKaydet);

            // btnIptal
            this.btnIptal.Location = new Point(185, 345);
            this.btnIptal.Size = new Size(150, 35);
            this.btnIptal.Text = "İptal";
            this.btnIptal.Click += BtnIptal_Click;
            this.grpDetay.Controls.Add(this.btnIptal);

            // TaskForm
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(1000, 420);
            this.Controls.Add(this.lblFiltre);
            this.Controls.Add(this.cmbFiltre);
            this.Controls.Add(this.dgvTasks);
            this.Controls.Add(this.btnYeni);
            this.Controls.Add(this.btnDuzenle);
            this.Controls.Add(this.btnSil);
            this.Controls.Add(this.btnKapat);
            this.Controls.Add(this.grpDetay);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "TaskForm";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Görev Yönetimi";

            ((System.ComponentModel.ISupportInitialize)(this.dgvTasks)).EndInit();
            this.grpDetay.ResumeLayout(false);
            this.grpDetay.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void LoadProjects()
        {
            var projects = _projectService.GetAllProjects().ToList();
            cmbProje.DataSource = projects;
            cmbProje.DisplayMember = "ProjeAdi";
            cmbProje.ValueMember = "Id";
        }

        private void LoadUsers()
        {
            var users = _userService.GetAllUsers().ToList();
            users.Insert(0, new User { Id = 0, AdSoyad = "-- Atanmamış --" });
            cmbKullanici.DataSource = users;
            cmbKullanici.DisplayMember = "AdSoyad";
            cmbKullanici.ValueMember = "Id";
        }

        private void LoadTasks(TaskStatus? durum = null)
        {
            try
            {
                var tasks = durum.HasValue 
                    ? _taskService.GetTasksByStatus(durum.Value).ToList()
                    : _taskService.GetAllTasks().ToList();

                dgvTasks.DataSource = tasks;

                if (dgvTasks.Columns.Count > 0)
                {
                    dgvTasks.Columns["Id"].HeaderText = "ID";
                    dgvTasks.Columns["Baslik"].HeaderText = "Başlık";
                    dgvTasks.Columns["Aciklama"].HeaderText = "Açıklama";
                    dgvTasks.Columns["Durum"].HeaderText = "Durum";
                    dgvTasks.Columns["ProjeId"].HeaderText = "Proje ID";
                    dgvTasks.Columns["AtananKullaniciId"].HeaderText = "Kullanıcı ID";
                    dgvTasks.Columns["OlusturmaTarihi"].Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Görevler yüklenirken hata: {ex.Message}", "Hata",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CmbFiltre_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cmbFiltre.SelectedIndex)
            {
                case 0: LoadTasks(); break;
                case 1: LoadTasks(TaskStatus.Beklemede); break;
                case 2: LoadTasks(TaskStatus.DevamEdiyor); break;
                case 3: LoadTasks(TaskStatus.Tamamlandi); break;
            }
        }

        private void BtnYeni_Click(object sender, EventArgs e)
        {
            _selectedTask = null;
            txtBaslik.Clear();
            txtAciklama.Clear();
            cmbProje.SelectedIndex = 0;
            cmbKullanici.SelectedIndex = 0;
            cmbDurum.SelectedIndex = 0;
            grpDetay.Visible = true;
        }

        private void BtnDuzenle_Click(object sender, EventArgs e)
        {
            if (dgvTasks.SelectedRows.Count == 0)
            {
                MessageBox.Show("Lütfen düzenlemek için bir görev seçin!", "Uyarı",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _selectedTask = (Task)dgvTasks.SelectedRows[0].DataBoundItem;
            txtBaslik.Text = _selectedTask.Baslik;
            txtAciklama.Text = _selectedTask.Aciklama;
            cmbProje.SelectedValue = _selectedTask.ProjeId;
            cmbKullanici.SelectedValue = _selectedTask.AtananKullaniciId ?? 0;
            cmbDurum.SelectedIndex = (int)_selectedTask.Durum;
            grpDetay.Visible = true;
        }

        private void BtnSil_Click(object sender, EventArgs e)
        {
            if (dgvTasks.SelectedRows.Count == 0)
            {
                MessageBox.Show("Lütfen silmek için bir görev seçin!", "Uyarı",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var task = (Task)dgvTasks.SelectedRows[0].DataBoundItem;

            if (MessageBox.Show($"'{task.Baslik}' görevini silmek istediğinizden emin misiniz?",
                "Silme Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    _taskService.DeleteTask(task.Id);
                    MessageBox.Show("Görev başarıyla silindi!", "Başarılı",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadTasks();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Görev silinirken hata: {ex.Message}", "Hata",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtBaslik.Text))
                {
                    MessageBox.Show("Görev başlığı boş olamaz!", "Uyarı",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (cmbProje.SelectedValue == null)
                {
                    MessageBox.Show("Lütfen bir proje seçin!", "Uyarı",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int projeId = (int)cmbProje.SelectedValue;
                int? kullaniciId = (int)cmbKullanici.SelectedValue;
                if (kullaniciId == 0) kullaniciId = null;

                if (_selectedTask == null)
                {
                    // Yeni görev
                    var newTask = new Task
                    {
                        Baslik = txtBaslik.Text.Trim(),
                        Aciklama = txtAciklama.Text.Trim(),
                        ProjeId = projeId,
                        AtananKullaniciId = kullaniciId,
                        Durum = (TaskStatus)cmbDurum.SelectedIndex
                    };
                    _taskService.AddTask(newTask);
                    MessageBox.Show("Görev başarıyla eklendi!", "Başarılı",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // Güncelleme
                    _selectedTask.Baslik = txtBaslik.Text.Trim();
                    _selectedTask.Aciklama = txtAciklama.Text.Trim();
                    _selectedTask.ProjeId = projeId;
                    _selectedTask.AtananKullaniciId = kullaniciId;
                    _selectedTask.Durum = (TaskStatus)cmbDurum.SelectedIndex;

                    _taskService.UpdateTask(_selectedTask);
                    MessageBox.Show("Görev başarıyla güncellendi!", "Başarılı",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                grpDetay.Visible = false;
                LoadTasks();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Görev kaydedilirken hata: {ex.Message}", "Hata",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnIptal_Click(object sender, EventArgs e)
        {
            grpDetay.Visible = false;
        }
    }
}
