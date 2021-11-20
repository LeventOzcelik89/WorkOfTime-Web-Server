using Infoline.Framework.Database;
using Infoline.OmixEntegrationApp.FtpEntegrations.Model;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;


namespace Infoline.OmixEntegrationApp.FtpEntegrations.Business
{
    public class FtpGenpa : IFtpDistributorEntegration
    {
        public FtpConfiguration ftpConfiguration { get; private set; }
        private string Token { get; set; }

        public string DistributorName { get { return "Genpa"; } }
        public Guid DistributorId { get { return new Guid("da14f7f9-2a41-48b9-acd0-fd62602c8bcf"); } }
        private List<string> FileList = new List<string>();
        private string DirUrl { get; set; }
        private string LoginUrl { get; set; }

        public FtpGenpa()
        {
            SetFtpConfiguration();
            Login();
        }
        public ResultStatus ExportFilesToDatabase()
        {
            throw new NotImplementedException();
        }

        public string FileTypeName(string fileName)
        {
            throw new NotImplementedException();
        }

        public PRD_EntegrationFiles[] GetFilesInFtp(DateTime processDate)
        {
            Log.Info("Logging On Genpa Wing Ftp Server");
            try
            {
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                var httpRequest = (HttpWebRequest)WebRequest.Create(LoginUrl);
                httpRequest.Method = "POST";
                httpRequest.ContentType = "multipart/form-data";
                var formData = "username=" + this.ftpConfiguration.UserName + "&password=" + this.ftpConfiguration.Password + "&username_val=" + this.ftpConfiguration.UserName + "&password_val=" + this.ftpConfiguration.Password + "&submit_btn=Oturum%20A%C3%A7";
                using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
                {
                    streamWriter.Write(formData);
                }
                var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var cookie = httpResponse.Headers["Set-Cookie"];
                    int indexofComma = cookie.IndexOf(';');
                    Token = cookie.Substring(0, indexofComma);
                }
            }
            catch (Exception e)
            {
                Log.Error("GENPA : " + e.ToString());
            }
            return null;
        }

        public PRD_EntegrationAction[] GetSellInFilesInFtp(string fileName, Guid entegrationFilesId)
        {
            throw new NotImplementedException();
        }

        public void SetFtpConfiguration()
        {
            var userName = ConfigurationManager.AppSettings["GenpaUserName"].ToString();
            var password = ConfigurationManager.AppSettings["GenpaPassword"].ToString();
            var dirUrl = ConfigurationManager.AppSettings["GenpaDirUrl"].ToString();
            if (dirUrl == null)
            {
                Log.Error("GenpaDir Url Not Found!");
            }
            else
            {
                this.DirUrl = dirUrl;
            }
            var loginUrl = ConfigurationManager.AppSettings["GenpaLoginUrl"].ToString();
            if (loginUrl == null)
            {
                Log.Error("LoginUrl Not Found!");
            }
            else
            {
                this.LoginUrl = loginUrl;
            }

            this.ftpConfiguration = new FtpConfiguration
            {
                Password = password,
                UserName = userName,
            };
        }
        private void Login()
        {

            Log.Info("Logging On Genpa Wing Ftp Server");
            try
            {
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                var httpRequest = (HttpWebRequest)WebRequest.Create(LoginUrl);
                httpRequest.Method = "POST";
                httpRequest.ContentType = "multipart/form-data";
                var formData = "username=" + this.ftpConfiguration.UserName + "&password=" + ftpConfiguration.Password + "&username_val=" + ftpConfiguration.UserName + "&password_val=" + ftpConfiguration.Password + "&submit_btn=Oturum%20A%C3%A7";
                using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
                {
                    streamWriter.Write(formData);
                }
                var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var cookie = httpResponse.Headers["Set-Cookie"];
                    int indexofComma = cookie.IndexOf(';');
                    Token = cookie.Substring(0, indexofComma);
                }
            }
            catch (Exception e)
            {
                Log.Error("GENPA : " + e.ToString());
            }


        }
    }
}
