using Infoline.OmixEntegrationApp.DistFtpEntegration.Model;
using Infoline.OmixEntegrationApp.DistFtpEntegration.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;

namespace Infoline.OmixEntegrationApp.DistFtpEntegration.Concrete
{
    public class FtpWorkerForMobiltel
    {
        public FtpConfiguration FtpConfiguration { get; }
        public List<FileNameWithUrl> FptUrl = new List<FileNameWithUrl>();
        public FtpWorkerForMobiltel()
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
                
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(config.Url);
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
                        FptUrl.Add(new FileNameWithUrl { Password = FtpConfiguration.Password, UserName = FtpConfiguration.UserName, FileName = item.Name, Url = item.BaseUri ,FileCreatedDate=item.DateCreated});
                    }
                }
            }
            catch (Exception e)
            {

                Log.Error(config.Url + " failed! : " + e.Message);
            }
            return directoryItems;
        }
        public IEnumerable<string[]> GetRawFile(FileNameWithUrl file) {
            Log.Info(string.Format("Getting File  {0} on KVK Server {1}", file.FileName, file.Url));
            var liststringArray = new List<string[]>();
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(file.Url + "/" + file.FileName);
            request.Method = WebRequestMethods.Ftp.DownloadFile;
            request.Credentials = new NetworkCredential(file.UserName, file.Password);
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
        public IEnumerable<SellIn> GetSellInObjectForToday()
        {
            List<SellIn> sellIns = new List<SellIn>();
            GetFileNames(this.FtpConfiguration);
            var datetimeNow = DateTime.Now;
            var day = datetimeNow.Day.ToString();
            if (datetimeNow.Day < 10)
            {
                day = "0" + datetimeNow.Day;
            }
            var kvkDate = datetimeNow.Year + "" + datetimeNow.Month + "" + day;
            var fileNames = FptUrl.Where(x => x.FileName.Contains("SELLIN")&&x.FileName.Contains(FtpConfiguration.FileExtension)).ToList();
            Log.Info(string.Format("{0} Sellin File Found On Kvk Ftp Server", fileNames.Count));
            foreach (var file in fileNames)
            {
                try
                {
                    List<PropertyIndex> Index = new List<PropertyIndex>();
                    var getRawFile = GetRawFile(file).ToList();
                    var getHeaders = getRawFile[0];
                    
                    getRawFile.RemoveAt(0);
                    for (int i = 0; i < getHeaders.Length; i++)
                    {
                        Index.Add(new PropertyIndex { Index = i, Name = getHeaders[i].Replace("\\","").Replace("\"","") });
                    }
                    foreach (var rawFile in getRawFile)
                    {
                        try
                        {
                            var item = new SellIn();
                            for (int i = 0; i < rawFile.Length; i++)
                            {
                                try
                                {
                                    var getIndexName = Index.Where(x => x.Index == i).Select(x => x.Name).FirstOrDefault();
                                    var prop = item.GetType().GetProperty(getIndexName.Replace(" ", ""));
                                    if (prop.PropertyType.IsAssignableFrom(typeof(int)))
                                    {
                                        prop.SetValue(item, Convert.ToInt32(rawFile[i].Replace("\\", "").Replace("\"", "")));
                                    }
                                    else
                                    {
                                        prop.SetValue(item, rawFile[i].Replace("\\", "").Replace("\"", ""));
                                    }
                                }
                                catch (Exception e)
                                {

                                    Log.Error(e.ToString());
                                }

                            }
                            sellIns.Add(item);
                        }
                        catch (Exception e)
                        {
                            Log.Error(e.ToString());
                        }
                    }
                    Thread.Sleep(new TimeSpan(0, 0, 10));
                }
                catch (Exception e)
                {
                    Log.Error(e.ToString());
                }
            }
            return sellIns;
        }
        public IEnumerable<SellThr> GetSellThrObjectForToday()
        {
            List<SellThr> sellThrs = new List<SellThr>();
            GetFileNames(this.FtpConfiguration);
            var datetimeNow = DateTime.Now;
            var day = datetimeNow.Day.ToString();
            if (datetimeNow.Day < 10)
            {
                day = "0" + datetimeNow.Day;
            }
            var kvkDate = datetimeNow.Year + "" + datetimeNow.Month + "" + day;
            var fileNames = FptUrl.Where(x => x.FileName.Contains("SELLTHR") && x.FileName.Contains(FtpConfiguration.FileExtension)).ToList();
            Log.Info(string.Format("{0} SellThr File Found On Kvk Ftp Server", fileNames.Count));
            foreach (var file in fileNames)
            {
                try
                {
                    List<PropertyIndex> Index = new List<PropertyIndex>();
                    var getRawFile = GetRawFile(file).ToList();
                    var getHeaders = getRawFile[0];
                    getRawFile.RemoveAt(0);
                    for (int i = 0; i < getHeaders.Length; i++)
                    {
                        Index.Add(new PropertyIndex { Index = i, Name = getHeaders[i] });
                    }
                    foreach (var rawFile in getRawFile)
                    {
                        try
                        {
                            var item = new SellThr();
                            for (int i = 0; i < rawFile.Length; i++)
                            {
                                try
                                {
                                    var getIndexName = Index.Where(x => x.Index == i).Select(x => x.Name).FirstOrDefault();
                                    if (getIndexName == "CustomerGenpaCode" || getIndexName == "CustomerKVKCode" || getIndexName == "CustomerMobitelCode")
                                    {
                                        getIndexName = "CustomerCode";
                                    }
                                    var prop = item.GetType().GetProperty(getIndexName.Replace(" ", ""));
                                    if (prop.PropertyType.IsAssignableFrom(typeof(int)))
                                    {
                                        prop.SetValue(item, Convert.ToInt32(rawFile[i]));
                                    }
                                    else
                                    {
                                        prop.SetValue(item, rawFile[i]);
                                    }
                                }
                                catch (Exception e)
                                {

                                    Log.Error(e.ToString());
                                }

                            }
                            sellThrs.Add(item);
                        }
                        catch (Exception e)
                        {
                            Log.Error(e.ToString());
                        }
                    }
                    Thread.Sleep(new TimeSpan(0, 0, 10));
                }
                catch (Exception e)
                {
                    Log.Error(e.ToString());
                }
            }
            return sellThrs;
        }

    }
}
