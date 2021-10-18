using Infoline.OmixEntegrationApp.DistFtpEntegration.Abstract;
using Infoline.OmixEntegrationApp.DistFtpEntegration.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
namespace Infoline.OmixEntegrationApp.DistFtpEntegration.Concrete
{
    public class FtpWorker : IFtpWorker
    {
        public List<FileNameWithUrl> FptUrl=new List<FileNameWithUrl>();
        private IEnumerable<DirectoryItem> GetFileNames(IEnumerable<FtpUrl> ftpUrls)
        {
            List<DirectoryItem> returnValue = new List<DirectoryItem>();
            foreach (var url in ftpUrls)
            {
                try
                {
                    FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(url.Url);
                    request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                    request.Credentials = new NetworkCredential(url.UserName, url.Password);
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
                        data = data.Remove(0, 24);
                        string dir = data.Substring(0, 5);
                        bool isDirectory = dir.Equals("<dir>", StringComparison.InvariantCultureIgnoreCase);
                        data = data.Remove(0, 5);
                        data = data.Remove(0, 10);
                        string name = data;
                        item.BaseUri = new Uri(url.Url);
                        item.IsDirectory = isDirectory;
                        item.Name = name;
                        item.Items = item.IsDirectory ? GetFileNames(new List<FtpUrl>() { new FtpUrl { Url = item.AbsolutePath, UserName = url.UserName, Password = url.Password } }).ToList() : null;
                        returnValue.Add(item);
                        if (!isDirectory)
                        {
                            FptUrl.Add(new FileNameWithUrl { Password = url.Password, UserName = url.UserName, FileName = name, Url = item.BaseUri.ToString() });
                        }
                    }
                }
                catch (Exception e)
                {
                    Log.Error(url.Url + " bağlanılamadı! : " + e.Message);
                }
            }
            return returnValue;
        }
        private IEnumerable<string[]> GetRawFile(FileNameWithUrl fileNameWithUrl)
        {
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
        public IEnumerable<SellIn> GetToDayFile()
        {
            List<FtpUrl> listOfUrls = new List<FtpUrl>() { new FtpUrl { Url = "ftp://82.222.178.101", UserName = "omixmobile", Password = "VpyC8g3R*" } };
            //List<FtpUrl> listOfUrls = new List<FtpUrl>() { new FtpUrl { Url = "ftp://127.0.0.1", UserName = "ftpUser", Password = "aA123456" } };
            GetFileNames(listOfUrls);
            var datetimeNow = DateTime.Now;
            var genpaDate = datetimeNow.Day + "" + datetimeNow.Month + "" + datetimeNow.Year;
            var kvkDate = datetimeNow.Year + "" + datetimeNow.Month + "" + datetimeNow.Day;
            var fileNames = FptUrl.Where(x => x.FileName.Contains("SELLIN") || x.FileName.Contains("SELLTHR")).Where(x => x.FileName.Contains(genpaDate) || x.FileName.Contains(kvkDate)).ToList();
            var res = new List<SellIn>();
            foreach (var fileName in fileNames)
            {
                List<PropertyIndex> Index = new List<PropertyIndex>();
                var getRawFile = GetRawFile(fileName).ToList();
                var getHeaders = getRawFile[0];
                getRawFile.RemoveAt(0);
                for (int i = 0; i < getHeaders.Length; i++)
                {
                    Index.Add(new PropertyIndex { Index = i, Name = getHeaders[i] });
                }
                foreach (var rawFile in getRawFile)
                {
                    var item = new SellIn();
                    if (fileName.FileName.Contains("SELLTHR"))
                    {
                        item = new SellThr();
                    }
                    for (int i = 0; i < rawFile.Length; i++)
                    {
                        var getIndexName = Index.Where(x => x.Index == i).Select(x => x.Name).FirstOrDefault();
                        var prop = item.GetType().GetProperty(getIndexName);
                        if (prop.PropertyType.IsAssignableFrom(typeof(int)))
                        {
                            prop.SetValue(item, Convert.ToInt32(rawFile[i]));
                        }
                        else
                        {
                            prop.SetValue(item, rawFile[i]);
                        }
                    }
                    res.Add(item);
                }
            }
            FptUrl = new List<FileNameWithUrl>();
            return res;
        }
    }
}
