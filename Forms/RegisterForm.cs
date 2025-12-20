using System;
using System.Windows.Forms;
using CompanyTaskProjectManagement.Entities;
using CompanyTaskProjectManagement.Services;

namespace CompanyTaskProjectManagement.Forms
{
    /// <summary>
    /// Kullanıcı Kayıt Formu
    /// </summary>
    public partial class RegisterForm : Form
    {
        private readonly UserService _userService;
        private TextBox txtAdSoyad;
        private TextBox txtKullaniciAdi;
        private TextBox txtSifre;
        private TextBox txtSifreTekrar;
        private ComboBox cmbRol;
        private Button btnKayitOl;
        private Button btnIptal;
        private Label lblBaslik;
        private Label lblAdSoyad;
        private Label lblKullaniciAdi;
        private Label lblSifre;
        private Label lblSifreTekrar;
        private Label lblRol;

        public bool IsRegistered { get; private set; }

        public RegisterForm(UserService userService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.lblBaslik = new Label();
            this.lblAdSoyad = new Label();
            this.txtAdSoyad = new TextBox();
            this.lblKullaniciAdi = new Label();
            this.txtKullaniciAdi = new TextBox();
            this.lblSifre = new Label();
            this.txtSifre = new TextBox();
            this.lblSifreTekrar = new Label();
            this.txtSifreTekrar = new TextBox();
            this.lblRol = new Label();
            this.cmbRol = new ComboBox();
            this.btnKayitOl = new Button();
            this.btnIptal = new Button();
            this.SuspendLayout();

            // lblBaslik
            this.lblBaslik.AutoSize = true;
            this.lblBaslik.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblBaslik.ForeColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.lblBaslik.Location = new System.Drawing.Point(130, 20);
            this.lblBaslik.Name = "lblBaslik";
            this.lblBaslik.Size = new System.Drawing.Size(200, 30);
            this.lblBaslik.Text = "Yeni Hesap Oluştur";

            // lblAdSoyad
            this.lblAdSoyad.AutoSize = true;
            this.lblAdSoyad.Location = new System.Drawing.Point(50, 70);
            this.lblAdSoyad.Name = "lblAdSoyad";
            this.lblAdSoyad.Size = new System.Drawing.Size(70, 15);
            this.lblAdSoyad.Text = "Ad Soyad:";

            // txtAdSoyad
            this.txtAdSoyad.Location = new System.Drawing.Point(50, 90);
            this.txtAdSoyad.Name = "txtAdSoyad";
            this.txtAdSoyad.Size = new System.Drawing.Size(350, 23);
            this.txtAdSoyad.TabIndex = 0;

            // lblKullaniciAdi
            this.lblKullaniciAdi.AutoSize = true;
            this.lblKullaniciAdi.Location = new System.Drawing.Point(50, 125);
            this.lblKullaniciAdi.Name = "lblKullaniciAdi";
            this.lblKullaniciAdi.Size = new System.Drawing.Size(80, 15);
            this.lblKullaniciAdi.Text = "Kullanıcı Adı:";

            // txtKullaniciAdi
            this.txtKullaniciAdi.Location = new System.Drawing.Point(50, 145);
            this.txtKullaniciAdi.Name = "txtKullaniciAdi";
            this.txtKullaniciAdi.Size = new System.Drawing.Size(350, 23);
            this.txtKullaniciAdi.TabIndex = 1;

            // lblSifre
            this.lblSifre.AutoSize = true;
            this.lblSifre.Location = new System.Drawing.Point(50, 180);
            this.lblSifre.Name = "lblSifre";
            this.lblSifre.Size = new System.Drawing.Size(36, 15);
            this.lblSifre.Text = "Şifre:";

            // txtSifre
            this.txtSifre.Location = new System.Drawing.Point(50, 200);
            this.txtSifre.Name = "txtSifre";
            this.txtSifre.PasswordChar = '●';
            this.txtSifre.Size = new System.Drawing.Size(350, 23);
            this.txtSifre.TabIndex = 2;

            // lblSifreTekrar
            this.lblSifreTekrar.AutoSize = true;
            this.lblSifreTekrar.Location = new System.Drawing.Point(50, 235);
            this.lblSifreTekrar.Name = "lblSifreTekrar";
            this.lblSifreTekrar.Size = new System.Drawing.Size(80, 15);
            this.lblSifreTekrar.Text = "Şifre Tekrar:";

            // txtSifreTekrar
            this.txtSifreTekrar.Location = new System.Drawing.Point(50, 255);
            this.txtSifreTekrar.Name = "txtSifreTekrar";
            this.txtSifreTekrar.PasswordChar = '●';
            this.txtSifreTekrar.Size = new System.Drawing.Size(350, 23);
            this.txtSifreTekrar.TabIndex = 3;

            // lblRol
            this.lblRol.AutoSize = true;
            this.lblRol.Location = new System.Drawing.Point(50, 290);
            this.lblRol.Name = "lblRol";
            this.lblRol.Size = new System.Drawing.Size(30, 15);
            this.lblRol.Text = "Rol:";

            // cmbRol
            this.cmbRol.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbRol.Location = new System.Drawing.Point(50, 310);
            this.cmbRol.Name = "cmbRol";
            this.cmbRol.Size = new System.Drawing.Size(350, 23);
            this.cmbRol.TabIndex = 4;
            this.cmbRol.Items.AddRange(new object[] { "Çalışan", "Admin" });
            this.cmbRol.SelectedIndex = 0;

            // btnKayitOl
            this.btnKayitOl.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnKayitOl.ForeColor = System.Drawing.Color.White;
            this.btnKayitOl.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnKayitOl.Location = new System.Drawing.Point(50, 360);
            this.btnKayitOl.Name = "btnKayitOl";
            this.btnKayitOl.Size = new System.Drawing.Size(170, 35);
            this.btnKayitOl.TabIndex = 5;
            this.btnKayitOl.Text = "Kayıt Ol";
            this.btnKayitOl.UseVisualStyleBackColor = false;
            this.btnKayitOl.Click += new EventHandler(this.BtnKayitOl_Click);

            // btnIptal
            this.btnIptal.BackColor = System.Drawing.Color.FromArgb(100, 100, 100);
            this.btnIptal.ForeColor = System.Drawing.Color.White;
            this.btnIptal.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular);
            this.btnIptal.Location = new System.Drawing.Point(230, 360);
            this.btnIptal.Name = "btnIptal";
            this.btnIptal.Size = new System.Drawing.Size(170, 35);
            this.btnIptal.TabIndex = 6;
            this.btnIptal.Text = "İptal";
            this.btnIptal.UseVisualStyleBackColor = false;
            this.btnIptal.Click += new EventHandler(this.BtnIptal_Click);

            // RegisterForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(240, 244, 248);
            this.ClientSize = new System.Drawing.Size(450, 420);
            this.Controls.Add(this.lblBaslik);
            this.Controls.Add(this.lblAdSoyad);
            this.Controls.Add(this.txtAdSoyad);
            this.Controls.Add(this.lblKullaniciAdi);
            this.Controls.Add(this.txtKullaniciAdi);
            this.Controls.Add(this.lblSifre);
            this.Controls.Add(this.txtSifre);
            this.Controls.Add(this.lblSifreTekrar);
            this.Controls.Add(this.txtSifreTekrar);
            this.Controls.Add(this.lblRol);
            this.Controls.Add(this.cmbRol);
            this.Controls.Add(this.btnKayitOl);
            this.Controls.Add(this.btnIptal);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RegisterForm";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Kayıt Ol";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void BtnKayitOl_Click(object sender, EventArgs e)
        {
            try
            {
                // Validasyon
                if (string.IsNullOrWhiteSpace(txtAdSoyad.Text))
                {
                    MessageBox.Show("Ad Soyad boş olamaz!", "Uyarı",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtAdSoyad.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtKullaniciAdi.Text))
                {
                    MessageBox.Show("Kullanıcı adı boş olamaz!", "Uyarı",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtKullaniciAdi.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtSifre.Text))
                {
                    MessageBox.Show("Şifre boş olamaz!", "Uyarı",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtSifre.Focus();
                    return;
                }

                if (txtSifre.Text.Length < 3)
                {
                    MessageBox.Show("Şifre en az 3 karakter olmalıdır!", "Uyarı",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtSifre.Focus();
                    return;
                }

                if (txtSifre.Text != txtSifreTekrar.Text)
                {
                    MessageBox.Show("Şifreler eşleşmiyor!", "Uyarı",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtSifreTekrar.Focus();
                    return;
                }

                // Yeni kullanıcı oluştur
                UserRole rol = cmbRol.SelectedIndex == 1 ? UserRole.Admin : UserRole.Calisan;
                var newUser = new User(
                    txtAdSoyad.Text.Trim(),
                    txtKullaniciAdi.Text.Trim(),
                    txtSifre.Text,
                    rol
                );

                _userService.AddUser(newUser);

                MessageBox.Show("Kayıt başarılı! Şimdi giriş yapabilirsiniz.", "Başarılı",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                IsRegistered = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Kayıt sırasında hata oluştu: {ex.Message}", "Hata",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnIptal_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
