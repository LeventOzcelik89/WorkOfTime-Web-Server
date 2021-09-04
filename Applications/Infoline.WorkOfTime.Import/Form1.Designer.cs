namespace Infoline.WorkOfTime.Import
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label17 = new System.Windows.Forms.Label();
            this.txtPass = new System.Windows.Forms.TextBox();
            this.btnBağlan = new System.Windows.Forms.Button();
            this.txtDBName = new System.Windows.Forms.TextBox();
            this.txtUser = new System.Windows.Forms.TextBox();
            this.txtServer = new System.Windows.Forms.TextBox();
            this.label22 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.lblCon = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.txtFileName = new System.Windows.Forms.TextBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnDosyaAc = new System.Windows.Forms.Button();
            this.btnAktar = new System.Windows.Forms.Button();
            this.dataGridViewColumn = new System.Windows.Forms.DataGridView();
            this.FirmaKaydi = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colKullanıcıKaydi = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colAd = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSoyad = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DoğumTarihi = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDepartman = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUnvan = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEmail = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSabitTel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCepTel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAdres = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colYasadigiIl = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colYasadigiIlce = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTCKimlikNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSirketCepTel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCalistigiSirket = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSigortaUnvanı = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colKademe = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colIseBaşlangicTarihi = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colProfilFoto_Vesikalik = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewColumn)).BeginInit();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label17);
            this.panel2.Controls.Add(this.txtPass);
            this.panel2.Controls.Add(this.btnBağlan);
            this.panel2.Controls.Add(this.txtDBName);
            this.panel2.Controls.Add(this.txtUser);
            this.panel2.Controls.Add(this.txtServer);
            this.panel2.Controls.Add(this.label22);
            this.panel2.Controls.Add(this.label21);
            this.panel2.Controls.Add(this.label20);
            this.panel2.Controls.Add(this.lblCon);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Font = new System.Drawing.Font("Cambria", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1103, 51);
            this.panel2.TabIndex = 7;
            // 
            // label17
            // 
            this.label17.BackColor = System.Drawing.SystemColors.HotTrack;
            this.label17.Dock = System.Windows.Forms.DockStyle.Top;
            this.label17.Font = new System.Drawing.Font("Cambria", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label17.ForeColor = System.Drawing.Color.White;
            this.label17.Location = new System.Drawing.Point(0, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(1103, 21);
            this.label17.TabIndex = 15;
            this.label17.Text = "SQL Conection";
            this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtPass
            // 
            this.txtPass.Location = new System.Drawing.Point(705, 23);
            this.txtPass.Margin = new System.Windows.Forms.Padding(4);
            this.txtPass.Name = "txtPass";
            this.txtPass.Size = new System.Drawing.Size(184, 23);
            this.txtPass.TabIndex = 1;
            this.txtPass.Text = "c2SSsCM9Y3HZVU";
            // 
            // btnBağlan
            // 
            this.btnBağlan.BackColor = System.Drawing.Color.Red;
            this.btnBağlan.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBağlan.Location = new System.Drawing.Point(894, 22);
            this.btnBağlan.Name = "btnBağlan";
            this.btnBağlan.Size = new System.Drawing.Size(106, 24);
            this.btnBağlan.TabIndex = 11;
            this.btnBağlan.Text = "&Bağlan";
            this.btnBağlan.UseVisualStyleBackColor = false;
            this.btnBağlan.Click += new System.EventHandler(this.btnBağlan_Click);
            // 
            // txtDBName
            // 
            this.txtDBName.Location = new System.Drawing.Point(244, 23);
            this.txtDBName.Margin = new System.Windows.Forms.Padding(4);
            this.txtDBName.Name = "txtDBName";
            this.txtDBName.Size = new System.Drawing.Size(184, 23);
            this.txtDBName.TabIndex = 1;
            this.txtDBName.Text = "Intranet1001Test";
            // 
            // txtUser
            // 
            this.txtUser.Location = new System.Drawing.Point(483, 23);
            this.txtUser.Margin = new System.Windows.Forms.Padding(4);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(184, 23);
            this.txtUser.TabIndex = 1;
            this.txtUser.Text = "sa";
            // 
            // txtServer
            // 
            this.txtServer.Location = new System.Drawing.Point(70, 23);
            this.txtServer.Margin = new System.Windows.Forms.Padding(4);
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(105, 23);
            this.txtServer.TabIndex = 1;
            this.txtServer.Text = "10.100.0.222";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(670, 26);
            this.label22.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(32, 15);
            this.label22.TabIndex = 6;
            this.label22.Text = "Pass";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(443, 26);
            this.label21.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(32, 15);
            this.label21.TabIndex = 6;
            this.label21.Text = "User";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(182, 26);
            this.label20.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(56, 15);
            this.label20.TabIndex = 6;
            this.label20.Text = "DBName";
            // 
            // lblCon
            // 
            this.lblCon.AutoSize = true;
            this.lblCon.Location = new System.Drawing.Point(8, 26);
            this.lblCon.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCon.Name = "lblCon";
            this.lblCon.Size = new System.Drawing.Size(42, 15);
            this.lblCon.TabIndex = 6;
            this.lblCon.Text = "Server";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnDosyaAc);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.txtFileName);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Enabled = false;
            this.panel1.Font = new System.Drawing.Font("Cambria", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.panel1.Location = new System.Drawing.Point(0, 51);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1103, 54);
            this.panel1.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.SystemColors.HotTrack;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("Cambria", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(1103, 20);
            this.label1.TabIndex = 15;
            this.label1.Text = "Excel Yolu";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtFileName
            // 
            this.txtFileName.Location = new System.Drawing.Point(2, 24);
            this.txtFileName.Margin = new System.Windows.Forms.Padding(4);
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.Size = new System.Drawing.Size(890, 23);
            this.txtFileName.TabIndex = 1;
            this.txtFileName.Text = ".";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 567);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1103, 22);
            this.statusStrip1.TabIndex = 10;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnAktar);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Enabled = false;
            this.panel3.Font = new System.Drawing.Font("Cambria", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.panel3.Location = new System.Drawing.Point(0, 530);
            this.panel3.Margin = new System.Windows.Forms.Padding(4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1103, 37);
            this.panel3.TabIndex = 9;
            // 
            // btnDosyaAc
            // 
            this.btnDosyaAc.Location = new System.Drawing.Point(899, 23);
            this.btnDosyaAc.Name = "btnDosyaAc";
            this.btnDosyaAc.Size = new System.Drawing.Size(101, 25);
            this.btnDosyaAc.TabIndex = 16;
            this.btnDosyaAc.Text = "&Dosya Aç";
            this.btnDosyaAc.UseVisualStyleBackColor = true;
            this.btnDosyaAc.Click += new System.EventHandler(this.btnDosyaAc_Click);
            // 
            // btnAktar
            // 
            this.btnAktar.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnAktar.Location = new System.Drawing.Point(0, 0);
            this.btnAktar.Name = "btnAktar";
            this.btnAktar.Size = new System.Drawing.Size(101, 37);
            this.btnAktar.TabIndex = 17;
            this.btnAktar.Text = "&Aktar";
            this.btnAktar.UseVisualStyleBackColor = true;
            this.btnAktar.Click += new System.EventHandler(this.btnAktar_Click);
            // 
            // dataGridViewColumn
            // 
            this.dataGridViewColumn.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dataGridViewColumn.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dataGridViewColumn.ColumnHeadersHeight = 50;
            this.dataGridViewColumn.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.FirmaKaydi,
            this.colKullanıcıKaydi,
            this.colAd,
            this.colSoyad,
            this.DoğumTarihi,
            this.colDepartman,
            this.colUnvan,
            this.colEmail,
            this.colSabitTel,
            this.colCepTel,
            this.colAdres,
            this.colYasadigiIl,
            this.colYasadigiIlce,
            this.colTCKimlikNo,
            this.colSirketCepTel,
            this.colCalistigiSirket,
            this.colSigortaUnvanı,
            this.colKademe,
            this.colIseBaşlangicTarihi,
            this.colProfilFoto_Vesikalik});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Cambria", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewColumn.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewColumn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewColumn.Location = new System.Drawing.Point(0, 105);
            this.dataGridViewColumn.Name = "dataGridViewColumn";
            this.dataGridViewColumn.RowTemplate.Height = 25;
            this.dataGridViewColumn.Size = new System.Drawing.Size(1103, 425);
            this.dataGridViewColumn.TabIndex = 11;
            // 
            // FirmaKaydi
            // 
            this.FirmaKaydi.DataPropertyName = "FirmaKaydi";
            this.FirmaKaydi.HeaderText = "FirmaKaydi";
            this.FirmaKaydi.Name = "FirmaKaydi";
            // 
            // colKullanıcıKaydi
            // 
            this.colKullanıcıKaydi.DataPropertyName = "KullanıcıKaydi";
            this.colKullanıcıKaydi.HeaderText = "KullanıcıKaydi";
            this.colKullanıcıKaydi.Name = "colKullanıcıKaydi";
            // 
            // colAd
            // 
            this.colAd.DataPropertyName = "Ad";
            this.colAd.HeaderText = "Ad";
            this.colAd.Name = "colAd";
            this.colAd.Width = 200;
            // 
            // colSoyad
            // 
            this.colSoyad.DataPropertyName = "Soyad";
            this.colSoyad.HeaderText = "Soyad";
            this.colSoyad.Name = "colSoyad";
            // 
            // DoğumTarihi
            // 
            this.DoğumTarihi.DataPropertyName = "DogumTarihi";
            this.DoğumTarihi.HeaderText = "DoğumTarihi";
            this.DoğumTarihi.Name = "DoğumTarihi";
            // 
            // colDepartman
            // 
            this.colDepartman.DataPropertyName = "Departman";
            this.colDepartman.HeaderText = "Departman";
            this.colDepartman.Name = "colDepartman";
            // 
            // colUnvan
            // 
            this.colUnvan.DataPropertyName = "Unvan";
            this.colUnvan.HeaderText = "Unvan";
            this.colUnvan.Name = "colUnvan";
            // 
            // colEmail
            // 
            this.colEmail.DataPropertyName = "Email";
            this.colEmail.HeaderText = "Email";
            this.colEmail.Name = "colEmail";
            // 
            // colSabitTel
            // 
            this.colSabitTel.DataPropertyName = "SabitTel";
            this.colSabitTel.HeaderText = "SabitTel";
            this.colSabitTel.Name = "colSabitTel";
            // 
            // colCepTel
            // 
            this.colCepTel.DataPropertyName = "CepTel";
            this.colCepTel.HeaderText = "CepTel";
            this.colCepTel.Name = "colCepTel";
            // 
            // colAdres
            // 
            this.colAdres.DataPropertyName = "Adres";
            this.colAdres.HeaderText = "Adres";
            this.colAdres.Name = "colAdres";
            // 
            // colYasadigiIl
            // 
            this.colYasadigiIl.DataPropertyName = "YasadigiIl";
            this.colYasadigiIl.HeaderText = "Yaşadığı İl";
            this.colYasadigiIl.Name = "colYasadigiIl";
            // 
            // colYasadigiIlce
            // 
            this.colYasadigiIlce.DataPropertyName = "YasadigiIlce";
            this.colYasadigiIlce.HeaderText = "Yaşadığı İlçe";
            this.colYasadigiIlce.Name = "colYasadigiIlce";
            // 
            // colTCKimlikNo
            // 
            this.colTCKimlikNo.DataPropertyName = "TCKimlikNo";
            this.colTCKimlikNo.HeaderText = "TCKimlikNo";
            this.colTCKimlikNo.Name = "colTCKimlikNo";
            // 
            // colSirketCepTel
            // 
            this.colSirketCepTel.DataPropertyName = "SirketCepTel";
            this.colSirketCepTel.HeaderText = "ŞirketCepTel";
            this.colSirketCepTel.Name = "colSirketCepTel";
            // 
            // colCalistigiSirket
            // 
            this.colCalistigiSirket.DataPropertyName = "CalistigiSirket";
            this.colCalistigiSirket.HeaderText = "ÇalıştığıŞirket";
            this.colCalistigiSirket.Name = "colCalistigiSirket";
            // 
            // colSigortaUnvanı
            // 
            this.colSigortaUnvanı.DataPropertyName = "SigortaUnvanı";
            this.colSigortaUnvanı.HeaderText = "SigortaÜnvanı";
            this.colSigortaUnvanı.Name = "colSigortaUnvanı";
            // 
            // colKademe
            // 
            this.colKademe.DataPropertyName = "Kademe";
            this.colKademe.HeaderText = "Kademe";
            this.colKademe.Name = "colKademe";
            // 
            // colIseBaşlangicTarihi
            // 
            this.colIseBaşlangicTarihi.DataPropertyName = "IseBaslangicTarihi";
            this.colIseBaşlangicTarihi.HeaderText = "İşeBaşlangıçTarihi";
            this.colIseBaşlangicTarihi.Name = "colIseBaşlangicTarihi";
            // 
            // colProfilFoto_Vesikalik
            // 
            this.colProfilFoto_Vesikalik.DataPropertyName = "ProfilFoto_Vesikalik";
            this.colProfilFoto_Vesikalik.HeaderText = "ProfilFoto_Vesikalık";
            this.colProfilFoto_Vesikalik.Name = "colProfilFoto_Vesikalik";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1103, 589);
            this.Controls.Add(this.dataGridViewColumn);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.statusStrip1);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewColumn)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox txtPass;
        private System.Windows.Forms.Button btnBağlan;
        private System.Windows.Forms.TextBox txtDBName;
        private System.Windows.Forms.TextBox txtUser;
        private System.Windows.Forms.TextBox txtServer;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label lblCon;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtFileName;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnDosyaAc;
        private System.Windows.Forms.Button btnAktar;
        private System.Windows.Forms.DataGridView dataGridViewColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn FirmaKaydi;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colKullanıcıKaydi;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAd;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSoyad;
        private System.Windows.Forms.DataGridViewTextBoxColumn DoğumTarihi;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDepartman;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUnvan;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEmail;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSabitTel;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCepTel;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAdres;
        private System.Windows.Forms.DataGridViewTextBoxColumn colYasadigiIl;
        private System.Windows.Forms.DataGridViewTextBoxColumn colYasadigiIlce;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTCKimlikNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSirketCepTel;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCalistigiSirket;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSigortaUnvanı;
        private System.Windows.Forms.DataGridViewTextBoxColumn colKademe;
        private System.Windows.Forms.DataGridViewTextBoxColumn colIseBaşlangicTarihi;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProfilFoto_Vesikalik;
    }
}

