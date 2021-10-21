using Infoline.OmixEntegrationApp.DistFtpEntegration.Abstract;
using Infoline.OmixEntegrationApp.DistFtpEntegration.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
namespace Infoline.OmixEntegrationApp.DistFtpEntegration.Concrete
{
    public class FtpWorkerForKvk : IFtpWorker
    {
        private List<FileNameWithUrl> FptUrl = new List<FileNameWithUrl>();
        public FtpConfiguration FtpConfiguration { get; set; }
        public void SetConfiguration(FtpConfiguration ftpConfiguration)
        {
            this.FtpConfiguration = ftpConfiguration;
        }
        public FtpConfiguration GetConfiguration()
        {
            return this.FtpConfiguration;
        }
        public IEnumerable<SellIn> GetSellInObjectForToday()
        {
            List<SellIn> sellIns = new List<SellIn>();
            GetFileNames(this.FtpConfiguration);
            var datetimeNow = DateTime.Now;
            var kvkDate = datetimeNow.Year + "" + datetimeNow.Month + "" + datetimeNow.Day;
            var fileNames = FptUrl.Where(x => x.FileName.Contains("SELLIN") && x.FileName.Contains(kvkDate)).ToList();
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
                        Index.Add(new PropertyIndex { Index = i, Name = getHeaders[i] });
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
            var kvkDate = datetimeNow.Year + "" + datetimeNow.Month + "" + datetimeNow.Day;
            var fileNames = FptUrl.Where(x => x.FileName.Contains("SELLTHR") && x.FileName.Contains(kvkDate)).ToList();
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
        private IEnumerable<DirectoryItem> GetFileNames(FtpConfiguration config)
        {
            Log.Info("Getting All File Names On KVK Server");
            List<DirectoryItem> returnValue = new List<DirectoryItem>();
            Log.Info(string.Format("Getting All File Names From Kvk Server {0}", config.Url));
            try
            {
                FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(config.Url);
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
                    DirectoryItem item = new DirectoryItem();
                    string data = line;
                    bool isDirectory = data[0].ToString() == "d";
                    var name = data.Substring(56);
                    item.Name = name;
                    item.BaseUri = config.Url;
                    item.IsDirectory = isDirectory;
                    if (name == "." || name == "..")
                    {
                    }
                    else
                    {
                        item.Items = item.IsDirectory ? GetFileNames(new FtpConfiguration { Url = item.AbsolutePath, UserName = config.UserName, Password = config.Password }).ToList() : null;
                        returnValue.Add(item);
                        if (!isDirectory)
                        {
                            FptUrl.Add(new FileNameWithUrl { Password = GetConfiguration().Password, UserName = GetConfiguration().UserName, FileName = name, Url = item.BaseUri });
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error(config.Url + " failed! : " + e.Message);
            }
            return returnValue;
        }
        private IEnumerable<string[]> GetRawFile(FileNameWithUrl fileNameWithUrl)
        {
            Log.Info(string.Format("Getting File  {0} on KVK Server {1}", fileNameWithUrl.FileName, fileNameWithUrl.Url));
            var liststringArray = new List<string[]>();
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(fileNameWithUrl.Url + "/" + fileNameWithUrl.FileName);
            request.Method = WebRequestMethods.Ftp.DownloadFile;
            request.Credentials = new NetworkCredential(fileNameWithUrl.UserName, fileNameWithUrl.Password);
            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(responseStream);
            string line;
            try
            {
                while ((line = reader.ReadLine()) != null)
                {
                    List<string> splitedLines = line.Split('\t').ToList();
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
