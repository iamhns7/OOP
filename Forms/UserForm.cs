using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CompanyTaskProjectManagement.Entities;
using CompanyTaskProjectManagement.Services;

namespace CompanyTaskProjectManagement.Forms
{
    /// <summary>
    /// Kullanıcı Yönetim Formu
    /// </summary>
    public partial class UserForm : Form
    {
        private readonly UserService _userService;
        private readonly User _currentUser;
        private DataGridView dgvUsers;
        private Button btnYeni;
        private Button btnDuzenle;
        private Button btnSil;
        private Button btnKapat;
        private GroupBox grpDetay;
        private TextBox txtAdSoyad;
        private TextBox txtKullaniciAdi;
        private TextBox txtSifre;
        private ComboBox cmbRol;
        private Button btnKaydet;
        private Button btnIptal;
        private Label lblAdSoyad;
        private Label lblKullaniciAdi;
        private Label lblSifre;
        private Label lblRol;

        private User _selectedUser;

        public UserForm(UserService userService, User currentUser)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
            InitializeComponent();
            ConfigureRoleBasedUI();
            LoadUsers();
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
                
                this.Text = "Kullanıcı Görüntüleme (Salt Okunur)";
                
                MessageBox.Show("Çalışanlar kullanıcı ekleyemez, düzenleyemez veya silemez.\nSadece görüntüleme yetkisine sahipsiniz.", 
                    "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void InitializeComponent()
        {
            this.dgvUsers = new DataGridView();
            this.btnYeni = new Button();
            this.btnDuzenle = new Button();
            this.btnSil = new Button();
            this.btnKapat = new Button();
            this.grpDetay = new GroupBox();
            this.lblAdSoyad = new Label();
            this.txtAdSoyad = new TextBox();
            this.lblKullaniciAdi = new Label();
            this.txtKullaniciAdi = new TextBox();
            this.lblSifre = new Label();
            this.txtSifre = new TextBox();
            this.lblRol = new Label();
            this.cmbRol = new ComboBox();
            this.btnKaydet = new Button();
            this.btnIptal = new Button();

            ((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).BeginInit();
            this.grpDetay.SuspendLayout();
            this.SuspendLayout();

            // dgvUsers
            this.dgvUsers.AllowUserToAddRows = false;
            this.dgvUsers.AllowUserToDeleteRows = false;
            this.dgvUsers.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvUsers.Location = new Point(12, 12);
            this.dgvUsers.MultiSelect = false;
            this.dgvUsers.Name = "dgvUsers";
            this.dgvUsers.ReadOnly = true;
            this.dgvUsers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvUsers.Size = new Size(600, 350);
            this.dgvUsers.TabIndex = 0;

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
            this.grpDetay.Text = "Kullanıcı Detayları";
            this.grpDetay.Visible = false;

            // lblAdSoyad
            this.lblAdSoyad.AutoSize = true;
            this.lblAdSoyad.Location = new Point(15, 30);
            this.lblAdSoyad.Text = "Ad Soyad:";
            this.grpDetay.Controls.Add(this.lblAdSoyad);

            // txtAdSoyad
            this.txtAdSoyad.Location = new Point(15, 50);
            this.txtAdSoyad.Size = new Size(320, 23);
            this.grpDetay.Controls.Add(this.txtAdSoyad);

            // lblKullaniciAdi
            this.lblKullaniciAdi.AutoSize = true;
            this.lblKullaniciAdi.Location = new Point(15, 85);
            this.lblKullaniciAdi.Text = "Kullanıcı Adı:";
            this.grpDetay.Controls.Add(this.lblKullaniciAdi);

            // txtKullaniciAdi
            this.txtKullaniciAdi.Location = new Point(15, 105);
            this.txtKullaniciAdi.Size = new Size(320, 23);
            this.grpDetay.Controls.Add(this.txtKullaniciAdi);

            // lblSifre
            this.lblSifre.AutoSize = true;
            this.lblSifre.Location = new Point(15, 140);
            this.lblSifre.Text = "Şifre:";
            this.grpDetay.Controls.Add(this.lblSifre);

            // txtSifre
            this.txtSifre.Location = new Point(15, 160);
            this.txtSifre.Size = new Size(320, 23);
            this.txtSifre.PasswordChar = '*';
            this.grpDetay.Controls.Add(this.txtSifre);

            // lblRol
            this.lblRol.AutoSize = true;
            this.lblRol.Location = new Point(15, 195);
            this.lblRol.Text = "Rol:";
            this.grpDetay.Controls.Add(this.lblRol);

            // cmbRol
            this.cmbRol.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbRol.Location = new Point(15, 215);
            this.cmbRol.Size = new Size(320, 23);
            this.cmbRol.Items.AddRange(new object[] { UserRole.Admin, UserRole.Calisan });
            this.grpDetay.Controls.Add(this.cmbRol);

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

            // UserForm
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.FromArgb(245, 247, 250);
            this.ClientSize = new Size(1000, 420);
            this.Controls.Add(this.dgvUsers);
            this.Controls.Add(this.btnYeni);
            this.Controls.Add(this.btnDuzenle);
            this.Controls.Add(this.btnSil);
            this.Controls.Add(this.btnKapat);
            this.Controls.Add(this.grpDetay);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "UserForm";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Kullanıcı Yönetimi";
            
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).EndInit();
            this.grpDetay.ResumeLayout(false);
            this.grpDetay.PerformLayout();
            this.ResumeLayout(false);
        }

        private void LoadUsers()
        {
            try
            {
                var users = _userService.GetAllUsers().ToList();
                dgvUsers.DataSource = users;
                
                if (dgvUsers.Columns.Count > 0)
                {
                    dgvUsers.Columns["Id"].HeaderText = "ID";
                    dgvUsers.Columns["AdSoyad"].HeaderText = "Ad Soyad";
                    dgvUsers.Columns["KullaniciAdi"].HeaderText = "Kullanıcı Adı";
                    dgvUsers.Columns["Sifre"].HeaderText = "Şifre";
                    dgvUsers.Columns["Rol"].HeaderText = "Rol";
                    dgvUsers.Columns["OlusturmaTarihi"].Visible = false;
                    
                    // Şifre sütununu gizle (güvenlik için)
                    dgvUsers.Columns["Sifre"].Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Kullanıcılar yüklenirken hata: {ex.Message}", "Hata",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnYeni_Click(object sender, EventArgs e)
        {
            _selectedUser = null;
            txtAdSoyad.Clear();
            txtKullaniciAdi.Clear();
            txtSifre.Clear();
            cmbRol.SelectedIndex = 1; // Default: Calisan
            grpDetay.Visible = true;
        }

        private void BtnDuzenle_Click(object sender, EventArgs e)
        {
            if (dgvUsers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Lütfen düzenlemek için bir kullanıcı seçin!", "Uyarı",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _selectedUser = (User)dgvUsers.SelectedRows[0].DataBoundItem;
            txtAdSoyad.Text = _selectedUser.AdSoyad;
            txtKullaniciAdi.Text = _selectedUser.KullaniciAdi;
            txtSifre.Text = _selectedUser.Sifre;
            cmbRol.SelectedItem = _selectedUser.Rol;
            
            grpDetay.Visible = true;
        }

        private void BtnSil_Click(object sender, EventArgs e)
        {
            if (dgvUsers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Lütfen silmek için bir kullanıcı seçin!", "Uyarı",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var user = (User)dgvUsers.SelectedRows[0].DataBoundItem;
            
            // Kendi hesabını silmeyi engelle
            if (user.Id == _currentUser.Id)
            {
                MessageBox.Show("Kendi hesabınızı silemezsiniz!", "Uyarı",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            if (MessageBox.Show($"'{user.AdSoyad}' kullanıcısını silmek istediğinizden emin misiniz?",
                "Silme Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    _userService.DeleteUser(user.Id);
                    MessageBox.Show("Kullanıcı başarıyla silindi!", "Başarılı",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadUsers();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Kullanıcı silinirken hata: {ex.Message}", "Hata",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtAdSoyad.Text))
                {
                    MessageBox.Show("Ad Soyad boş olamaz!", "Uyarı",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtKullaniciAdi.Text))
                {
                    MessageBox.Show("Kullanıcı adı boş olamaz!", "Uyarı",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtSifre.Text))
                {
                    MessageBox.Show("Şifre boş olamaz!", "Uyarı",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (cmbRol.SelectedItem == null)
                {
                    MessageBox.Show("Lütfen bir rol seçin!", "Uyarı",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (_selectedUser == null)
                {
                    // Yeni kullanıcı
                    var newUser = new User
                    {
                        AdSoyad = txtAdSoyad.Text.Trim(),
                        KullaniciAdi = txtKullaniciAdi.Text.Trim(),
                        Sifre = txtSifre.Text.Trim(),
                        Rol = (UserRole)cmbRol.SelectedItem
                    };
                    _userService.AddUser(newUser);
                    MessageBox.Show("Kullanıcı başarıyla eklendi!", "Başarılı",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // Güncelleme
                    _selectedUser.AdSoyad = txtAdSoyad.Text.Trim();
                    _selectedUser.KullaniciAdi = txtKullaniciAdi.Text.Trim();
                    _selectedUser.Sifre = txtSifre.Text.Trim();
                    _selectedUser.Rol = (UserRole)cmbRol.SelectedItem;
                    
                    _userService.UpdateUser(_selectedUser);
                    MessageBox.Show("Kullanıcı başarıyla güncellendi!", "Başarılı",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                grpDetay.Visible = false;
                LoadUsers();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Kullanıcı kaydedilirken hata: {ex.Message}", "Hata",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnIptal_Click(object sender, EventArgs e)
        {
            grpDetay.Visible = false;
        }
    }
}
