using System;
using System.Drawing;
using System.Windows.Forms;

namespace CompanyTaskProjectManagement.Forms
{
    /// <summary>
    /// Giriş türü seçim formu (Normal Kullanıcı / Admin)
    /// </summary>
    public partial class RoleSelectionForm : Form
    {
        private Button btnKullaniciGirisi;
        private Button btnAdminGirisi;
        private Button btnCikis;
        private Label lblBaslik;
        private Label lblAciklama;
        private Label lblAltBaslik;
        private Panel pnlHeader;

        public bool IsAdminLogin { get; private set; }

        public RoleSelectionForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.pnlHeader = new Panel();
            this.lblBaslik = new Label();
            this.lblAltBaslik = new Label();
            this.lblAciklama = new Label();
            this.btnKullaniciGirisi = new Button();
            this.btnAdminGirisi = new Button();
            this.btnCikis = new Button();
            this.pnlHeader.SuspendLayout();
            this.SuspendLayout();

            // pnlHeader
            this.pnlHeader.BackColor = Color.FromArgb(0, 120, 215);
            this.pnlHeader.Controls.Add(this.lblBaslik);
            this.pnlHeader.Controls.Add(this.lblAltBaslik);
            this.pnlHeader.Dock = DockStyle.Top;
            this.pnlHeader.Location = new Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new Size(550, 100);

            // lblBaslik
            this.lblBaslik.AutoSize = true;
            this.lblBaslik.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            this.lblBaslik.ForeColor = Color.White;
            this.lblBaslik.Location = new Point(20, 20);
            this.lblBaslik.Name = "lblBaslik";
            this.lblBaslik.Size = new Size(400, 32);
            this.lblBaslik.Text = "Görev ve Proje Yönetim Sistemi";

            // lblAltBaslik
            this.lblAltBaslik.AutoSize = true;
            this.lblAltBaslik.Font = new Font("Segoe UI", 10F);
            this.lblAltBaslik.ForeColor = Color.FromArgb(230, 240, 255);
            this.lblAltBaslik.Location = new Point(20, 58);
            this.lblAltBaslik.Name = "lblAltBaslik";
            this.lblAltBaslik.Size = new Size(300, 19);
            this.lblAltBaslik.Text = "Şirket İçi Görev Takip ve Organizasyon Platformu";

            // lblAciklama
            this.lblAciklama.AutoSize = true;
            this.lblAciklama.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            this.lblAciklama.ForeColor = Color.FromArgb(60, 60, 60);
            this.lblAciklama.Location = new Point(160, 130);
            this.lblAciklama.Name = "lblAciklama";
            this.lblAciklama.Size = new Size(230, 21);
            this.lblAciklama.Text = "Giriş Türünüzü Seçiniz";

            // btnKullaniciGirisi
            this.btnKullaniciGirisi.BackColor = Color.White;
            this.btnKullaniciGirisi.FlatStyle = FlatStyle.Flat;
            this.btnKullaniciGirisi.FlatAppearance.BorderColor = Color.FromArgb(0, 120, 215);
            this.btnKullaniciGirisi.FlatAppearance.BorderSize = 2;
            this.btnKullaniciGirisi.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            this.btnKullaniciGirisi.ForeColor = Color.FromArgb(0, 120, 215);
            this.btnKullaniciGirisi.Location = new Point(75, 180);
            this.btnKullaniciGirisi.Name = "btnKullaniciGirisi";
            this.btnKullaniciGirisi.Size = new Size(400, 70);
            this.btnKullaniciGirisi.Text = "👤 Normal Kullanıcı Girişi";
            this.btnKullaniciGirisi.TextAlign = ContentAlignment.MiddleLeft;
            this.btnKullaniciGirisi.Padding = new Padding(20, 0, 0, 0);
            this.btnKullaniciGirisi.UseVisualStyleBackColor = false;
            this.btnKullaniciGirisi.Cursor = Cursors.Hand;
            this.btnKullaniciGirisi.Click += BtnKullaniciGirisi_Click;

            // btnAdminGirisi
            this.btnAdminGirisi.BackColor = Color.White;
            this.btnAdminGirisi.FlatStyle = FlatStyle.Flat;
            this.btnAdminGirisi.FlatAppearance.BorderColor = Color.FromArgb(220, 53, 69);
            this.btnAdminGirisi.FlatAppearance.BorderSize = 2;
            this.btnAdminGirisi.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            this.btnAdminGirisi.ForeColor = Color.FromArgb(220, 53, 69);
            this.btnAdminGirisi.Location = new Point(75, 270);
            this.btnAdminGirisi.Name = "btnAdminGirisi";
            this.btnAdminGirisi.Size = new Size(400, 70);
            this.btnAdminGirisi.Text = "🔐 Yönetici Girişi";
            this.btnAdminGirisi.TextAlign = ContentAlignment.MiddleLeft;
            this.btnAdminGirisi.Padding = new Padding(20, 0, 0, 0);
            this.btnAdminGirisi.UseVisualStyleBackColor = false;
            this.btnAdminGirisi.Cursor = Cursors.Hand;
            this.btnAdminGirisi.Click += BtnAdminGirisi_Click;

            // btnCikis
            this.btnCikis.BackColor = Color.FromArgb(108, 117, 125);
            this.btnCikis.FlatStyle = FlatStyle.Flat;
            this.btnCikis.Font = new Font("Segoe UI", 10F);
            this.btnCikis.ForeColor = Color.White;
            this.btnCikis.Location = new Point(75, 360);
            this.btnCikis.Name = "btnCikis";
            this.btnCikis.Size = new Size(400, 40);
            this.btnCikis.Text = "❌ Çıkış";
            this.btnCikis.UseVisualStyleBackColor = false;
            this.btnCikis.Cursor = Cursors.Hand;
            this.btnCikis.Click += BtnCikis_Click;

            // RoleSelectionForm
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.FromArgb(245, 247, 250);
            this.ClientSize = new Size(550, 430);
            this.Controls.Add(this.pnlHeader);
            this.Controls.Add(this.lblAciklama);
            this.Controls.Add(this.btnKullaniciGirisi);
            this.Controls.Add(this.btnAdminGirisi);
            this.Controls.Add(this.btnCikis);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "RoleSelectionForm";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Görev Yönetim Sistemi - Giriş";
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void BtnCikis_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Uygulamadan çıkmak istediğinizden emin misiniz?", "Çıkış",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void BtnKullaniciGirisi_Click(object sender, EventArgs e)
        {
            IsAdminLogin = false;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void BtnAdminGirisi_Click(object sender, EventArgs e)
        {
            IsAdminLogin = true;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
