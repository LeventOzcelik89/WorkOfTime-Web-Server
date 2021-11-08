using Infoline.OmixEntegrationApp.DistFtpEntegration.Model;
using Infoline.OmixEntegrationApp.DistFtpEntegration.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace Infoline.OmixEntegrationApp.DistFtpEntegration.Concrete
{
    public class FtpWorkerForMobitel
    {
        public FtpConfiguration FtpConfiguration { get; }
        private List<FileNameWithUrl> FptUrl = new List<FileNameWithUrl>();
        public FtpWorkerForMobitel()
        {
            var url = ConfigurationManager.AppSettings["MobitelUrl"].ToString();
            var userName = ConfigurationManager.AppSettings["MobitelUserName"].ToString();
            var password = ConfigurationManager.AppSettings["MobitelPassword"].ToString();
            var directory = ConfigurationManager.AppSettings["MobitelDirectory"].ToString();
            var readAllDirectory = ConfigurationManager.AppSettings["ReadAllDirectory"].ToString();
            var fileExtension = ConfigurationManager.AppSettings["MobitelFileExtension"].ToString();

            if (url == null) Log.Error("Mobitel Url Not Found!");
            if (userName == null) Log.Error("Mobitel UserName Not Found!");
            if (password == null) Log.Error("Mobitel Password Not Found!");
            if (directory == null) Log.Error("Mobitel Directory Not Found!");
            if (readAllDirectory == null) Log.Error("Mobitel ReadAllDirectory Property Not Found");
            if (readAllDirectory == null) Log.Error("Mobitel ReadAllDirectory Property Not Found");
            if (fileExtension == null) Log.Error("Mobitel File Extension Property Not Found");

            this.FtpConfiguration = new FtpConfiguration
            {
                Directory = directory ?? "",
                Password = password ?? "",
                SearchAllDirectory = readAllDirectory == "true",
                Url = url ?? "",
                UserName = userName ?? "",
                FileExtension = fileExtension ?? ""
            };


        }

        public IEnumerable<DirectoryItem> GetFileNames(FtpConfiguration config)
        {
            Log.Info(string.Format("Getting File Names On Mobitel : {0}", config.Url));
            List<DirectoryItem> directoryItems = new List<DirectoryItem>();
            try
            {
                string url = config.Url;
                if (config.SearchAllDirectory)
                {
                    url += config.Directory;
                }
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(url);
                request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                request.Credentials = new NetworkCredential(config.UserName, config.Password);
                string[] list = null;
                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    list = reader.ReadToEnd().Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                }
                foreach (string line in list)
                {
                    var item = new DirectoryItem();

                    item.DateCreated = Tools.GetDate(line);
                    item.BaseUri = config.Url;
                    item.IsDirectory = Tools.IsDir(line);
                    item.Name = Tools.GetItemName(line);
                    item.Items = item.IsDirectory ? GetFileNames(new FtpConfiguration { Url = item.AbsolutePath, UserName = config.UserName, Password = config.Password }).ToList() : null;
                    directoryItems.Add(item);
                    if (!item.IsDirectory)
                    {
                        FptUrl.Add(new FileNameWithUrl { Password = FtpConfiguration.Password, UserName = FtpConfiguration.UserName, FileName = item.Name, Url = item.BaseUri });
                    }





                }






            }
            catch (Exception e)
            {

                Log.Error(config.Url + " failed! : " + e.Message);
            }




            return null;
        }



    }
}
