using Infoline.OmixEntegrationApp.FtpEntegration.Abstract;
using Infoline.OmixEntegrationApp.FtpEntegration.Entities;
using Infoline.OmixEntegrationApp.FtpEntegration.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Infoline.OmixEntegrationApp.FtpEntegration.Concrete.Mobiltel
{
    public class FtpWorkerForMobiltel : IFtpWorker
    {
        public FtpConfiguration Configuration { get; }
        public FtpWorkerForMobiltel()
        {
            var url = ConfigurationManager.AppSettings["MobiltelUrl"].ToString();
            var userName = ConfigurationManager.AppSettings["MobiltelUserName"].ToString();
            var password = ConfigurationManager.AppSettings["MobiltelPassword"].ToString();
            var directory = ConfigurationManager.AppSettings["MobiltelDirectory"].ToString();
            var readAllDirectory = ConfigurationManager.AppSettings["MobiltelReadAllDirectory"].ToString();
            var fileExtension = ConfigurationManager.AppSettings["MobiltelFileExtension"].ToString();
            var fileDateTimeFormat = ConfigurationManager.AppSettings["MobiltelFileDateTimeFormat"].ToString();

            if (url == null) Log.Error("Mobiltel Url Not Found!");
            if (userName == null) Log.Error("Mobiltel UserName Not Found!");
            if (password == null) Log.Error("Mobiltel Password Not Found!");
            if (directory == null) Log.Error("Mobiltel Directory Not Found!");
            if (readAllDirectory == null) Log.Error("Mobiltel ReadAllDirectory Property Not Found");
            if (readAllDirectory == null) Log.Error("Mobiltel ReadAllDirectory Property Not Found");
            if (fileExtension == null) Log.Error("Mobiltel File Extension Property Not Found");
            if (fileDateTimeFormat == null) Log.Error("Mobiltel File Date Format Property Not Found");

            this.Configuration = new FtpConfiguration
            {
                Directory = directory ?? "",
                Password = password ?? "",
                SearchAllDirectory = readAllDirectory == "true",
                Url = url ?? "",
                UserName = userName ?? "",
                FileExtension = fileExtension ?? "",
                FileDateTimeFormat = fileDateTimeFormat ?? ""
            };
        }


       

        public IEnumerable<DirectoryItem> GetAllFilesNames(FtpConfiguration configuration)
        {
            Log.Info(string.Format("Getting File Names On Mobitel : {0}", configuration.Url));
            List<DirectoryItem> directoryItems = new List<DirectoryItem>();
            try
            {

                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(configuration.Url);
                request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                request.Credentials = new NetworkCredential(configuration.UserName, configuration.Password);
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
                    item.BaseUri = configuration.Url;
                    item.IsDirectory = Tools.IsDir(line);
                    item.Name = Tools.GetItemName(line);
                    item.Items = item.IsDirectory ? GetAllFilesNames(new FtpConfiguration { Url = item.AbsolutePath, UserName = configuration.UserName, Password = config.Password }).ToList() : null;
                    directoryItems.Add(item);
                }
            }
            catch (Exception e)
            {

                Log.Error(configuration.Url + " failed! : " + e.Message);
            }
            return directoryItems;
        }

        public IEnumerable<string[]> GetRawFile(DirectoryItem directoryItem)
        {
            Log.Info(string.Format("Getting File  {0} on KVK Server {1}", directoryItem.Name, directoryItem.BaseUri));
            var liststringArray = new List<string[]>();
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(directoryItem.AbsolutePath);
            request.Method = WebRequestMethods.Ftp.DownloadFile;
            request.Credentials = new NetworkCredential(directoryItem.UserName, directoryItem.Password);
            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(responseStream);
            string line;
            try
            {
                while ((line = reader.ReadLine()) != null)
                {
                    List<string> splitedLines = line.Split(';').ToList();
                    liststringArray.Add(splitedLines.ToArray());
                }
                response.Close();
                reader.Close();
            }
            catch (Exception e)
            {
                Log.Warning(e.Message);
            }
            return liststringArray;




        }

    }
}
