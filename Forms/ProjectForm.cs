using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CompanyTaskProjectManagement.Entities;
using CompanyTaskProjectManagement.Services;

namespace CompanyTaskProjectManagement.Forms
{
    /// <summary>
    /// Proje Yönetim Formu
    /// </summary>
    public partial class ProjectForm : Form
    {
        private readonly ProjectService _projectService;
        private readonly User _currentUser;
        private DataGridView dgvProjects;
        private Button btnYeni;
        private Button btnDuzenle;
        private Button btnSil;
        private Button btnKapat;
        private GroupBox grpDetay;
        private TextBox txtProjeAdi;
        private TextBox txtAciklama;
        private DateTimePicker dtpBaslangic;
        private DateTimePicker dtpBitis;
        private CheckBox chkBitisVarMi;
        private Button btnKaydet;
        private Button btnIptal;
        private Label lblProjeAdi;
        private Label lblAciklama;
        private Label lblBaslangic;
        private Label lblBitis;

        private Project _selectedProject;

        public ProjectForm(ProjectService projectService, User currentUser)
        {
            _projectService = projectService ?? throw new ArgumentNullException(nameof(projectService));
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
            InitializeComponent();
            ConfigureRoleBasedUI();
            LoadProjects();
        }

        /// <summary>
        /// Rol bazlı arayüz konfigürasyonu
        /// </summary>
        private void ConfigureRoleBasedUI()
        {
            if (_currentUser.Rol == UserRole.Calisan)
            {
                // Çalışanlar için CRUD işlemleri kısıtlı
                btnYeni.Enabled = false;
                btnDuzenle.Enabled = false;
                btnSil.Enabled = false;
                
                this.Text = "Proje Görüntüleme (Salt Okunur)";
                
                MessageBox.Show("Çalışanlar proje ekleyemez, düzenleyemez veya silemez.\nSadece görüntüleme yetkisine sahipsiniz.", 
                    "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void InitializeComponent()
        {
            this.dgvProjects = new DataGridView();
            this.btnYeni = new Button();
            this.btnDuzenle = new Button();
            this.btnSil = new Button();
            this.btnKapat = new Button();
            this.grpDetay = new GroupBox();
            this.lblProjeAdi = new Label();
            this.txtProjeAdi = new TextBox();
            this.lblAciklama = new Label();
            this.txtAciklama = new TextBox();
            this.lblBaslangic = new Label();
            this.dtpBaslangic = new DateTimePicker();
            this.chkBitisVarMi = new CheckBox();
            this.lblBitis = new Label();
            this.dtpBitis = new DateTimePicker();
            this.btnKaydet = new Button();
            this.btnIptal = new Button();

            ((System.ComponentModel.ISupportInitialize)(this.dgvProjects)).BeginInit();
            this.grpDetay.SuspendLayout();
            this.SuspendLayout();

            // dgvProjects
            this.dgvProjects.AllowUserToAddRows = false;
            this.dgvProjects.AllowUserToDeleteRows = false;
            this.dgvProjects.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvProjects.Location = new Point(12, 12);
            this.dgvProjects.MultiSelect = false;
            this.dgvProjects.Name = "dgvProjects";
            this.dgvProjects.ReadOnly = true;
            this.dgvProjects.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvProjects.Size = new Size(600, 350);
            this.dgvProjects.TabIndex = 0;

            // btnYeni
            this.btnYeni.BackColor = Color.FromArgb(16, 124, 16);
            this.btnYeni.ForeColor = Color.White;
            this.btnYeni.FlatStyle = FlatStyle.Flat;
            this.btnYeni.Location = new Point(12, 370);
            this.btnYeni.Name = "btnYeni";
            this.btnYeni.Size = new Size(100, 30);
            this.btnYeni.Text = "Yeni";
            this.btnYeni.Click += BtnYeni_Click;

            // btnDuzenle
            this.btnDuzenle.BackColor = Color.FromArgb(0, 120, 215);
            this.btnDuzenle.ForeColor = Color.White;
            this.btnDuzenle.FlatStyle = FlatStyle.Flat;
            this.btnDuzenle.Location = new Point(120, 370);
            this.btnDuzenle.Name = "btnDuzenle";
            this.btnDuzenle.Size = new Size(100, 30);
            this.btnDuzenle.Text = "Düzenle";
            this.btnDuzenle.Click += BtnDuzenle_Click;

            // btnSil
            this.btnSil.BackColor = Color.FromArgb(196, 43, 28);
            this.btnSil.ForeColor = Color.White;
            this.btnSil.FlatStyle = FlatStyle.Flat;
            this.btnSil.Location = new Point(228, 370);
            this.btnSil.Name = "btnSil";
            this.btnSil.Size = new Size(100, 30);
            this.btnSil.Text = "Sil";
            this.btnSil.Click += BtnSil_Click;

            // btnKapat
            this.btnKapat.Location = new Point(512, 370);
            this.btnKapat.Name = "btnKapat";
            this.btnKapat.Size = new Size(100, 30);
            this.btnKapat.Text = "Kapat";
            this.btnKapat.Click += (s, e) => this.Close();

            // grpDetay
            this.grpDetay.BackColor = Color.White;
            this.grpDetay.Location = new Point(630, 12);
            this.grpDetay.Name = "grpDetay";
            this.grpDetay.Size = new Size(350, 390);
            this.grpDetay.Text = "Proje Detayları";
            this.grpDetay.Visible = false;

            // lblProjeAdi
            this.lblProjeAdi.AutoSize = true;
            this.lblProjeAdi.Location = new Point(15, 30);
            this.lblProjeAdi.Text = "Proje Adı:";
            this.grpDetay.Controls.Add(this.lblProjeAdi);

            // txtProjeAdi
            this.txtProjeAdi.Location = new Point(15, 50);
            this.txtProjeAdi.Size = new Size(320, 23);
            this.grpDetay.Controls.Add(this.txtProjeAdi);

            // lblAciklama
            this.lblAciklama.AutoSize = true;
            this.lblAciklama.Location = new Point(15, 85);
            this.lblAciklama.Text = "Açıklama:";
            this.grpDetay.Controls.Add(this.lblAciklama);

            // txtAciklama
            this.txtAciklama.Location = new Point(15, 105);
            this.txtAciklama.Multiline = true;
            this.txtAciklama.Size = new Size(320, 80);
            this.grpDetay.Controls.Add(this.txtAciklama);

            // lblBaslangic
            this.lblBaslangic.AutoSize = true;
            this.lblBaslangic.Location = new Point(15, 200);
            this.lblBaslangic.Text = "Başlangıç Tarihi:";
            this.grpDetay.Controls.Add(this.lblBaslangic);

            // dtpBaslangic
            this.dtpBaslangic.Location = new Point(15, 220);
            this.dtpBaslangic.Size = new Size(320, 23);
            this.grpDetay.Controls.Add(this.dtpBaslangic);

            // chkBitisVarMi
            this.chkBitisVarMi.AutoSize = true;
            this.chkBitisVarMi.Location = new Point(15, 255);
            this.chkBitisVarMi.Text = "Bitiş tarihi belirlensin";
            this.chkBitisVarMi.CheckedChanged += (s, e) => dtpBitis.Enabled = chkBitisVarMi.Checked;
            this.grpDetay.Controls.Add(this.chkBitisVarMi);

            // lblBitis
            this.lblBitis.AutoSize = true;
            this.lblBitis.Location = new Point(15, 280);
            this.lblBitis.Text = "Bitiş Tarihi:";
            this.grpDetay.Controls.Add(this.lblBitis);

            // dtpBitis
            this.dtpBitis.Location = new Point(15, 300);
            this.dtpBitis.Size = new Size(320, 23);
            this.dtpBitis.Enabled = false;
            this.grpDetay.Controls.Add(this.dtpBitis);

            // btnKaydet
            this.btnKaydet.BackColor = Color.FromArgb(0, 120, 215);
            this.btnKaydet.ForeColor = Color.White;
            this.btnKaydet.Location = new Point(15, 340);
            this.btnKaydet.Size = new Size(150, 35);
            this.btnKaydet.Text = "Kaydet";
            this.btnKaydet.Click += BtnKaydet_Click;
            this.grpDetay.Controls.Add(this.btnKaydet);

            // btnIptal
            this.btnIptal.Location = new Point(185, 340);
            this.btnIptal.Size = new Size(150, 35);
            this.btnIptal.Text = "İptal";
            this.btnIptal.Click += BtnIptal_Click;
            this.grpDetay.Controls.Add(this.btnIptal);

            // ProjectForm
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.FromArgb(245, 247, 250);
            this.ClientSize = new Size(1000, 420);
            this.Controls.Add(this.dgvProjects);
            this.Controls.Add(this.btnYeni);
            this.Controls.Add(this.btnDuzenle);
            this.Controls.Add(this.btnSil);
            this.Controls.Add(this.btnKapat);
            this.Controls.Add(this.grpDetay);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "ProjectForm";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Proje Yönetimi";
            
            ((System.ComponentModel.ISupportInitialize)(this.dgvProjects)).EndInit();
            this.grpDetay.ResumeLayout(false);
            this.grpDetay.PerformLayout();
            this.ResumeLayout(false);
        }

        private void LoadProjects()
        {
            try
            {
                var projects = _projectService.GetAllProjects().ToList();
                dgvProjects.DataSource = projects;
                
                if (dgvProjects.Columns.Count > 0)
                {
                    dgvProjects.Columns["Id"].HeaderText = "ID";
                    dgvProjects.Columns["ProjeAdi"].HeaderText = "Proje Adı";
                    dgvProjects.Columns["Aciklama"].HeaderText = "Açıklama";
                    dgvProjects.Columns["BaslangicTarihi"].HeaderText = "Başlangıç";
                    dgvProjects.Columns["BitisTarihi"].HeaderText = "Bitiş";
                    dgvProjects.Columns["OlusturmaTarihi"].Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Projeler yüklenirken hata: {ex.Message}", "Hata",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnYeni_Click(object sender, EventArgs e)
        {
            _selectedProject = null;
            txtProjeAdi.Clear();
            txtAciklama.Clear();
            dtpBaslangic.Value = DateTime.Now;
            dtpBitis.Value = DateTime.Now.AddMonths(1);
            chkBitisVarMi.Checked = false;
            grpDetay.Visible = true;
        }

        private void BtnDuzenle_Click(object sender, EventArgs e)
        {
            if (dgvProjects.SelectedRows.Count == 0)
            {
                MessageBox.Show("Lütfen düzenlemek için bir proje seçin!", "Uyarı",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _selectedProject = (Project)dgvProjects.SelectedRows[0].DataBoundItem;
            txtProjeAdi.Text = _selectedProject.ProjeAdi;
            txtAciklama.Text = _selectedProject.Aciklama;
            dtpBaslangic.Value = _selectedProject.BaslangicTarihi;
            
            if (_selectedProject.BitisTarihi.HasValue)
            {
                chkBitisVarMi.Checked = true;
                dtpBitis.Value = _selectedProject.BitisTarihi.Value;
            }
            else
            {
                chkBitisVarMi.Checked = false;
            }
            
            grpDetay.Visible = true;
        }

        private void BtnSil_Click(object sender, EventArgs e)
        {
            if (dgvProjects.SelectedRows.Count == 0)
            {
                MessageBox.Show("Lütfen silmek için bir proje seçin!", "Uyarı",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var project = (Project)dgvProjects.SelectedRows[0].DataBoundItem;
            
            if (MessageBox.Show($"'{project.ProjeAdi}' projesini silmek istediğinizden emin misiniz?",
                "Silme Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    _projectService.DeleteProject(project.Id);
                    MessageBox.Show("Proje başarıyla silindi!", "Başarılı",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadProjects();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Proje silinirken hata: {ex.Message}", "Hata",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtProjeAdi.Text))
                {
                    MessageBox.Show("Proje adı boş olamaz!", "Uyarı",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (_selectedProject == null)
                {
                    // Yeni proje
                    var newProject = new Project
                    {
                        ProjeAdi = txtProjeAdi.Text.Trim(),
                        Aciklama = txtAciklama.Text.Trim(),
                        BaslangicTarihi = dtpBaslangic.Value,
                        BitisTarihi = chkBitisVarMi.Checked ? (DateTime?)dtpBitis.Value : null
                    };
                    _projectService.AddProject(newProject);
                    MessageBox.Show("Proje başarıyla eklendi!", "Başarılı",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // Güncelleme
                    _selectedProject.ProjeAdi = txtProjeAdi.Text.Trim();
                    _selectedProject.Aciklama = txtAciklama.Text.Trim();
                    _selectedProject.BaslangicTarihi = dtpBaslangic.Value;
                    _selectedProject.BitisTarihi = chkBitisVarMi.Checked ? (DateTime?)dtpBitis.Value : null;
                    
                    _projectService.UpdateProject(_selectedProject);
                    MessageBox.Show("Proje başarıyla güncellendi!", "Başarılı",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                grpDetay.Visible = false;
                LoadProjects();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Proje kaydedilirken hata: {ex.Message}", "Hata",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnIptal_Click(object sender, EventArgs e)
        {
            grpDetay.Visible = false;
        }
    }
}
