using Infoline.WorkOfTime.BusinessAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Infoline.DownloadFilesApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnDownloadFiles_Click(object sender, EventArgs e)
        {
            if (txtTenantCode == null)
            {
                MessageBox.Show("TenanCode Giriniz...");
                return;
            }

            var tenantCode = Convert.ToInt32(txtTenantCode.Text);
            var tenant = TenantConfig.GetTenants().Where(a => a.TenantCode == tenantCode).FirstOrDefault();

            var db = new WorkOfTimeDatabase(tenant.GetConnectionString());
            db = tenant.GetDatabase();

            var files = db.GetSYS_Files();

            foreach (var item in files)
            {
                var dataString = "";
                var fileName = "";

                if (item.DataTable == "CMP_Company")
                {
                    var data = db.GetVWCMP_CompanyById(item.DataId.Value);
                    if (data == null) continue;
                    dataString = data.code + "_" + data.name + "_" + data.type_Title;
                    fileName = "Cariler";
                }
                else if (item.DataTable == "CRM_Contact")
                {
                    var data = db.GetVWCRM_ContactById(item.DataId.Value);
                    if (data == null) continue;
                    dataString = data.CustomerCompanyId_Title + "_" + data.id;
                    fileName = "CRM_Toplantı_Dosyaları";
                }
                else if (item.DataTable == "CRM_Presentation")
                {
                    var data = db.GetVWCRM_PresentationById(item.DataId.Value);
                    if (data == null) continue;
                    dataString = data.CustomerCompany_Title + "_" + data.id;
                    fileName = "CRM_Potansiyel_Dosyaları";
                }
                else if (item.DataTable == "INV_CommissionsPersons")
                {
                    var data = db.GetVWINV_CommissionsPersonsById(item.DataId.Value);
                    if (data == null) continue;
                    dataString = data.CommissionCode + "_" + data.Person_Title;
                    fileName = "Gorevlendirme_Dosyaları";
                }
                else if (item.DataTable == "INV_CompanyPersonAssessment")
                {
                    var data = db.GetVWINV_CompanyPersonAssessmentById(item.DataId.Value);
                    if (data == null) continue;
                    dataString = data.AssessmentCode + "_" + data.AssessmentType + "AylıkDeğerlendirme_" + data.Person_Title;
                    fileName = "Personel_Değerlendirme";
                }
                else if (item.DataTable == "INV_Permit")
                {
                    var data = db.GetVWINV_PermitById(item.DataId.Value);
                    if (data == null) continue;
                    dataString = data.PermitCode + "_" + data.Person_Title;
                    fileName = "İzin_Dosyaları";
                }
                else if (item.DataTable == "PRD_Product")
                {
                    var data = db.GetVWPRD_ProductById(item.DataId.Value);
                    if (data == null) continue;
                    dataString = data.code + "_" + data.name;
                    fileName = "Ürün_Dosyaları";
                }
                else if (item.DataTable == "PRJ_Project")
                {
                    var data = db.GetVWPRJ_ProjectById(item.DataId.Value);
                    if (data == null) continue;
                    dataString = data.ProjectCode + "_" + data.ProjectName + "_" + data.ProjectScope;
                    fileName = "Proje_Dosyaları";
                }
                else if (item.DataTable == "SH_PersonCertificate")
                {
                    var data = db.GetVWSH_PersonCertificateById(item.DataId.Value);
                    if (data == null) continue;
                    dataString = data.User_Title + "_" + data.CertificateType_Title + "_" + data.CertificateName;
                    fileName = "Personel_Sertifikaları";
                }
                else if (item.DataTable == "SH_PersonSchools")
                {
                    var data = db.GetVWSH_PersonSchoolsById(item.DataId.Value);
                    if (data == null) continue;
                    dataString = data.User_Title + "_" + data.Level_Title + "_" + data.School_Title;
                    fileName = "Personel_Okul_Dosyaları";
                }
                else if (item.DataTable == "SH_User")
                {
                    var data = db.GetVWSH_UserById(item.DataId.Value);
                    if (data == null) continue;
                    dataString = data.code + "_" + data.loginname + "_" + data.FullName + "_" + data.Company_Title + "_" + data.Type_Title;
                    fileName = "Personel_Kullanıcı_Dosyaları";
                }
                else
                    continue;

                DownloadFile(tenant.WebDomain.Split(',')[1], item.FilePath, fileName, dataString, item.FileExtension);
            }
            MessageBox.Show("Bitti");
        }
        

        private void DownloadFile(string baseUrl, string fileUrl, string fileName, string dataString, string filetype)
        {
            try
            {
                var destinationFile = "Files\\" + fileName;
                DestinationCheck(destinationFile);

                destinationFile = destinationFile + "\\" + dataString + "." + filetype;

                var uri = baseUrl + "/" + fileUrl;
                using (WebClient client = new WebClient())
                {
                    client.DownloadFile(new Uri(uri), destinationFile);
                    client.DownloadFileAsync(new Uri(uri), destinationFile);
                }
            }
            catch (Exception)
            {

            }
        }
        
        private static void DestinationCheck(string destinationFiles)
        {
            if (!Directory.Exists(destinationFiles))
                Directory.CreateDirectory(destinationFiles);
        }
    }
}
