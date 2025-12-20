using System;
using System.Drawing;
using System.Windows.Forms;
using CompanyTaskProjectManagement.Entities;
using CompanyTaskProjectManagement.Services;

namespace CompanyTaskProjectManagement.Forms
{
    /// <summary>
    /// Admin Giriş Formu - Sadece Admin rolündeki kullanıcılar girebilir
    /// </summary>
    public partial class AdminLoginForm : Form
    {
        private readonly UserService _userService;
        private TextBox txtKullaniciAdi;
        private TextBox txtSifre;
        private Button btnGiris;
        private Button btnGeri;
        private Label lblKullaniciAdi;
        private Label lblSifre;
        private Label lblBaslik;
        private Label lblUyari;

        public User AuthenticatedUser { get; private set; }

        public AdminLoginForm(UserService userService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.lblBaslik = new Label();
            this.lblUyari = new Label();
            this.lblKullaniciAdi = new Label();
            this.txtKullaniciAdi = new TextBox();
            this.lblSifre = new Label();
            this.txtSifre = new TextBox();
            this.btnGiris = new Button();
            this.btnGeri = new Button();
            this.SuspendLayout();

            // lblBaslik
            this.lblBaslik.AutoSize = true;
            this.lblBaslik.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            this.lblBaslik.ForeColor = Color.FromArgb(220, 53, 69);
            this.lblBaslik.Location = new Point(110, 30);
            this.lblBaslik.Name = "lblBaslik";
            this.lblBaslik.Size = new Size(180, 30);
            this.lblBaslik.Text = "🔐 Admin Girişi";

            // lblUyari
            this.lblUyari.AutoSize = true;
            this.lblUyari.Font = new Font("Segoe UI", 9F, FontStyle.Italic);
            this.lblUyari.ForeColor = Color.FromArgb(150, 150, 150);
            this.lblUyari.Location = new Point(70, 70);
            this.lblUyari.Name = "lblUyari";
            this.lblUyari.Size = new Size(260, 15);
            this.lblUyari.Text = "Sadece yönetici yetkisine sahip kullanıcılar girebilir";

            // lblKullaniciAdi
            this.lblKullaniciAdi.AutoSize = true;
            this.lblKullaniciAdi.Font = new Font("Segoe UI", 10F);
            this.lblKullaniciAdi.Location = new Point(50, 110);
            this.lblKullaniciAdi.Name = "lblKullaniciAdi";
            this.lblKullaniciAdi.Size = new Size(95, 19);
            this.lblKullaniciAdi.Text = "Kullanıcı Adı:";

            // txtKullaniciAdi
            this.txtKullaniciAdi.Font = new Font("Segoe UI", 11F);
            this.txtKullaniciAdi.Location = new Point(50, 135);
            this.txtKullaniciAdi.Name = "txtKullaniciAdi";
            this.txtKullaniciAdi.Size = new Size(300, 27);
            this.txtKullaniciAdi.TabIndex = 0;

            // lblSifre
            this.lblSifre.AutoSize = true;
            this.lblSifre.Font = new Font("Segoe UI", 10F);
            this.lblSifre.Location = new Point(50, 175);
            this.lblSifre.Name = "lblSifre";
            this.lblSifre.Size = new Size(42, 19);
            this.lblSifre.Text = "Şifre:";

            // txtSifre
            this.txtSifre.Font = new Font("Segoe UI", 11F);
            this.txtSifre.Location = new Point(50, 200);
            this.txtSifre.Name = "txtSifre";
            this.txtSifre.PasswordChar = '●';
            this.txtSifre.Size = new Size(300, 27);
            this.txtSifre.TabIndex = 1;
            this.txtSifre.KeyPress += TxtSifre_KeyPress;

            // btnGiris
            this.btnGiris.BackColor = Color.FromArgb(220, 53, 69);
            this.btnGiris.FlatStyle = FlatStyle.Flat;
            this.btnGiris.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            this.btnGiris.ForeColor = Color.White;
            this.btnGiris.Location = new Point(50, 250);
            this.btnGiris.Name = "btnGiris";
            this.btnGiris.Size = new Size(300, 40);
            this.btnGiris.TabIndex = 2;
            this.btnGiris.Text = "Giriş Yap";
            this.btnGiris.UseVisualStyleBackColor = false;
            this.btnGiris.Cursor = Cursors.Hand;
            this.btnGiris.Click += BtnGiris_Click;

            // btnGeri
            this.btnGeri.BackColor = Color.FromArgb(108, 117, 125);
            this.btnGeri.FlatStyle = FlatStyle.Flat;
            this.btnGeri.Font = new Font("Segoe UI", 10F);
            this.btnGeri.ForeColor = Color.White;
            this.btnGeri.Location = new Point(50, 300);
            this.btnGeri.Name = "btnGeri";
            this.btnGeri.Size = new Size(300, 35);
            this.btnGeri.TabIndex = 3;
            this.btnGeri.Text = "← Geri Dön";
            this.btnGeri.UseVisualStyleBackColor = false;
            this.btnGeri.Cursor = Cursors.Hand;
            this.btnGeri.Click += BtnGeri_Click;

            // AdminLoginForm
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.FromArgb(245, 247, 250);
            this.ClientSize = new Size(400, 370);
            this.Controls.Add(this.lblBaslik);
            this.Controls.Add(this.lblUyari);
            this.Controls.Add(this.lblKullaniciAdi);
            this.Controls.Add(this.txtKullaniciAdi);
            this.Controls.Add(this.lblSifre);
            this.Controls.Add(this.txtSifre);
            this.Controls.Add(this.btnGiris);
            this.Controls.Add(this.btnGeri);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "AdminLoginForm";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Admin Girişi";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void TxtSifre_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                BtnGiris_Click(sender, e);
            }
        }

        private void BtnGiris_Click(object sender, EventArgs e)
        {
            try
            {
                string kullaniciAdi = txtKullaniciAdi.Text.Trim();
                string sifre = txtSifre.Text;

                if (string.IsNullOrEmpty(kullaniciAdi) || string.IsNullOrEmpty(sifre))
                {
                    MessageBox.Show("Kullanıcı adı ve şifre boş olamaz!", "Uyarı",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var user = _userService.Login(kullaniciAdi, sifre);

                if (user != null)
                {
                    // Admin kontrolü
                    if (user.Rol != UserRole.Admin)
                    {
                        MessageBox.Show("Bu alan sadece yönetici yetkisine sahip kullanıcılar içindir!\n\n" +
                                      "Lütfen normal kullanıcı girişini kullanınız.", "Erişim Reddedildi",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtKullaniciAdi.Clear();
                        txtSifre.Clear();
                        txtKullaniciAdi.Focus();
                        return;
                    }

                    AuthenticatedUser = user;
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Kullanıcı adı veya şifre hatalı!", "Giriş Başarısız",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtSifre.Clear();
                    txtKullaniciAdi.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Giriş sırasında hata oluştu: {ex.Message}", "Hata",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnGeri_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
