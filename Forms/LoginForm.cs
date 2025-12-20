using System;
using System.Windows.Forms;
using CompanyTaskProjectManagement.Entities;
using CompanyTaskProjectManagement.Services;

namespace CompanyTaskProjectManagement.Forms
{
    /// <summary>
    /// Kullanıcı Giriş Formu
    /// </summary>
    public partial class LoginForm : Form
    {
        private readonly UserService _userService;
        private TextBox txtKullaniciAdi;
        private TextBox txtSifre;
        private Button btnGiris;
        private Button btnGeri;
        private Label lblKullaniciAdi;
        private Label lblSifre;
        private Label lblBaslik;
        private Label lblAltBaslik;
        private LinkLabel lnkKayitOl;

        public User AuthenticatedUser { get; private set; }

        public LoginForm(UserService userService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.lblBaslik = new Label();
            this.lblAltBaslik = new Label();
            this.lblKullaniciAdi = new Label();
            this.txtKullaniciAdi = new TextBox();
            this.lblSifre = new Label();
            this.txtSifre = new TextBox();
            this.btnGiris = new Button();
            this.btnGeri = new Button();
            this.lnkKayitOl = new LinkLabel();
            this.SuspendLayout();

            // lblBaslik
            this.lblBaslik.AutoSize = true;
            this.lblBaslik.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblBaslik.ForeColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.lblBaslik.Location = new System.Drawing.Point(60, 25);
            this.lblBaslik.Name = "lblBaslik";
            this.lblBaslik.Size = new System.Drawing.Size(280, 30);
            this.lblBaslik.Text = "👤 Kullanıcı Girişi";

            // lblAltBaslik
            this.lblAltBaslik.AutoSize = true;
            this.lblAltBaslik.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic);
            this.lblAltBaslik.ForeColor = System.Drawing.Color.FromArgb(120, 120, 120);
            this.lblAltBaslik.Location = new System.Drawing.Point(65, 60);
            this.lblAltBaslik.Name = "lblAltBaslik";
            this.lblAltBaslik.Size = new System.Drawing.Size(270, 15);
            this.lblAltBaslik.Text = "Lütfen kullanıcı bilgileriniz ile giriş yapınız";

            // lblKullaniciAdi
            this.lblKullaniciAdi.AutoSize = true;
            this.lblKullaniciAdi.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblKullaniciAdi.Location = new System.Drawing.Point(50, 100);
            this.lblKullaniciAdi.Name = "lblKullaniciAdi";
            this.lblKullaniciAdi.Size = new System.Drawing.Size(95, 19);
            this.lblKullaniciAdi.Text = "Kullanıcı Adı:";

            // txtKullaniciAdi
            this.txtKullaniciAdi.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtKullaniciAdi.Location = new System.Drawing.Point(50, 125);
            this.txtKullaniciAdi.Name = "txtKullaniciAdi";
            this.txtKullaniciAdi.Size = new System.Drawing.Size(300, 27);
            this.txtKullaniciAdi.TabIndex = 0;

            // lblSifre
            this.lblSifre.AutoSize = true;
            this.lblSifre.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblSifre.Location = new System.Drawing.Point(50, 165);
            this.lblSifre.Name = "lblSifre";
            this.lblSifre.Size = new System.Drawing.Size(42, 19);
            this.lblSifre.Text = "Şifre:";

            // txtSifre
            this.txtSifre.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtSifre.Location = new System.Drawing.Point(50, 190);
            this.txtSifre.Name = "txtSifre";
            this.txtSifre.PasswordChar = '●';
            this.txtSifre.Size = new System.Drawing.Size(300, 27);
            this.txtSifre.TabIndex = 1;
            this.txtSifre.KeyPress += TxtSifre_KeyPress;

            // btnGiris
            this.btnGiris.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnGiris.FlatStyle = FlatStyle.Flat;
            this.btnGiris.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnGiris.ForeColor = System.Drawing.Color.White;
            this.btnGiris.Location = new System.Drawing.Point(50, 240);
            this.btnGiris.Name = "btnGiris";
            this.btnGiris.Size = new System.Drawing.Size(300, 40);
            this.btnGiris.TabIndex = 2;
            this.btnGiris.Text = "Giriş Yap";
            this.btnGiris.UseVisualStyleBackColor = false;
            this.btnGiris.Cursor = Cursors.Hand;
            this.btnGiris.Click += new EventHandler(this.BtnGiris_Click);

            // btnGeri
            this.btnGeri.BackColor = System.Drawing.Color.FromArgb(108, 117, 125);
            this.btnGeri.FlatStyle = FlatStyle.Flat;
            this.btnGeri.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnGeri.ForeColor = System.Drawing.Color.White;
            this.btnGeri.Location = new System.Drawing.Point(50, 290);
            this.btnGeri.Name = "btnGeri";
            this.btnGeri.Size = new System.Drawing.Size(300, 35);
            this.btnGeri.TabIndex = 3;
            this.btnGeri.Text = "← Geri Dön";
            this.btnGeri.UseVisualStyleBackColor = false;
            this.btnGeri.Cursor = Cursors.Hand;
            this.btnGeri.Click += BtnGeri_Click;

            // lnkKayitOl
            this.lnkKayitOl.AutoSize = true;
            this.lnkKayitOl.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lnkKayitOl.Location = new System.Drawing.Point(105, 340);
            this.lnkKayitOl.Name = "lnkKayitOl";
            this.lnkKayitOl.Size = new System.Drawing.Size(190, 15);
            this.lnkKayitOl.TabIndex = 4;
            this.lnkKayitOl.TabStop = true;
            this.lnkKayitOl.Text = "Hesabınız yok mu? Kayıt Olun";
            this.lnkKayitOl.LinkColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.lnkKayitOl.LinkClicked += new LinkLabelLinkClickedEventHandler(this.LnkKayitOl_LinkClicked);

            // LoginForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(240, 244, 248);
            this.ClientSize = new System.Drawing.Size(400, 380);
            this.Controls.Add(this.lblBaslik);
            this.Controls.Add(this.lblAltBaslik);
            this.Controls.Add(this.lblKullaniciAdi);
            this.Controls.Add(this.txtKullaniciAdi);
            this.Controls.Add(this.lblSifre);
            this.Controls.Add(this.txtSifre);
            this.Controls.Add(this.btnGiris);
            this.Controls.Add(this.btnGeri);
            this.Controls.Add(this.lnkKayitOl);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LoginForm";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Kullanıcı Girişi";
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

                if (string.IsNullOrWhiteSpace(kullaniciAdi))
                {
                    MessageBox.Show("Kullanıcı adı boş olamaz!", "Uyarı", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtKullaniciAdi.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(sifre))
                {
                    MessageBox.Show("Şifre boş olamaz!", "Uyarı", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtSifre.Focus();
                    return;
                }

                var user = _userService.Login(kullaniciAdi, sifre);

                if (user != null)
                {
                    // Normal kullanıcı girişinde admin olamaz kontrolü
                    if (user.Rol == UserRole.Admin)
                    {
                        MessageBox.Show("Admin kullanıcıları bu alandan giriş yapamaz!\n\n" +
                                      "Lütfen 'Admin Girişi' seçeneğini kullanınız.", "Erişim Reddedildi",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                    MessageBox.Show("Kullanıcı adı veya şifre hatalı!", "Hata", 
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

        private void LnkKayitOl_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var registerForm = new RegisterForm(_userService);
            registerForm.ShowDialog();
            
            if (registerForm.IsRegistered)
            {
                MessageBox.Show("Kayıt başarılı! Şimdi giriş yapabilirsiniz.", "Bilgi",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnGeri_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
