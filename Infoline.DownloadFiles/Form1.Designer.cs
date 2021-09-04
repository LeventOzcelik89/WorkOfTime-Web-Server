
namespace Infoline.DownloadFiles
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnDownloadFiles = new System.Windows.Forms.Button();
            this.txtTenantCode = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnDownloadFiles
            // 
            this.btnDownloadFiles.Location = new System.Drawing.Point(13, 42);
            this.btnDownloadFiles.Name = "btnDownloadFiles";
            this.btnDownloadFiles.Size = new System.Drawing.Size(137, 23);
            this.btnDownloadFiles.TabIndex = 0;
            this.btnDownloadFiles.Text = "<< Dosyaları İndir >>";
            this.btnDownloadFiles.UseVisualStyleBackColor = true;
            this.btnDownloadFiles.Click += new System.EventHandler(this.btnDownloadFiles_Click);
            // 
            // txtTenantCode
            // 
            this.txtTenantCode.Location = new System.Drawing.Point(13, 13);
            this.txtTenantCode.Name = "txtTenantCode";
            this.txtTenantCode.PlaceholderText = "Tenant Code Giriniz...";
            this.txtTenantCode.Size = new System.Drawing.Size(136, 23);
            this.txtTenantCode.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 68);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "label1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(306, 150);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtTenantCode);
            this.Controls.Add(this.btnDownloadFiles);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnDownloadFiles;
        private System.Windows.Forms.TextBox txtTenantCode;
        private System.Windows.Forms.Label label1;
    }
}

