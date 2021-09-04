
namespace Infoline.DownloadFilesApp
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
            this.txtTenantCode = new System.Windows.Forms.TextBox();
            this.btnDownloadFiles = new System.Windows.Forms.Button();
            this.lblTenant = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtTenantCode
            // 
            this.txtTenantCode.Location = new System.Drawing.Point(57, 10);
            this.txtTenantCode.Name = "txtTenantCode";
            this.txtTenantCode.Size = new System.Drawing.Size(161, 20);
            this.txtTenantCode.TabIndex = 0;
            this.txtTenantCode.Text = "1101";
            // 
            // btnDownloadFiles
            // 
            this.btnDownloadFiles.Location = new System.Drawing.Point(15, 36);
            this.btnDownloadFiles.Name = "btnDownloadFiles";
            this.btnDownloadFiles.Size = new System.Drawing.Size(203, 23);
            this.btnDownloadFiles.TabIndex = 1;
            this.btnDownloadFiles.Text = "<< Download >>";
            this.btnDownloadFiles.UseVisualStyleBackColor = true;
            this.btnDownloadFiles.Click += new System.EventHandler(this.btnDownloadFiles_Click);
            // 
            // lblTenant
            // 
            this.lblTenant.AutoSize = true;
            this.lblTenant.Location = new System.Drawing.Point(12, 13);
            this.lblTenant.Name = "lblTenant";
            this.lblTenant.Size = new System.Drawing.Size(41, 13);
            this.lblTenant.TabIndex = 2;
            this.lblTenant.Text = "Tenant";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(354, 202);
            this.Controls.Add(this.lblTenant);
            this.Controls.Add(this.btnDownloadFiles);
            this.Controls.Add(this.txtTenantCode);
            this.Name = "Form1";
            this.Text = "DownloadFiles";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtTenantCode;
        private System.Windows.Forms.Button btnDownloadFiles;
        private System.Windows.Forms.Label lblTenant;
    }
}

