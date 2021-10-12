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
        public IEnumerable<FileNameWithUrl> GetFileNames(IEnumerable<FtpUrl> ftpUrls)
        {
            var fileNames = new List<FileNameWithUrl>();
            foreach (var url in ftpUrls)
            {
                try
                {

                    FtpWebRequest request = (FtpWebRequest)WebRequest.Create(url.Url);
                    request.Method = WebRequestMethods.Ftp.ListDirectory;
                    request.Credentials = new NetworkCredential(url.UserName, url.Password);
                    FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                    Log.Info(url.Url + " bağlanıldı");
                    Stream responseStream = response.GetResponseStream();
                    StreamReader reader = new StreamReader(responseStream);
                    string filenames = reader.ReadToEnd();
                    reader.Close();
                    response.Close();
                    var file = filenames.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                    var dateTime = DateTime.Now;
                    var convertedDate = dateTime.Year + "" + dateTime.Month + "" + dateTime.Day;
                    fileNames.AddRange(file.Where(x => (x.Contains("SELLIN") || x.Contains("SELLTHR")) && x.Contains(convertedDate)).Select(x => new FileNameWithUrl { FileName = x, Url = url.Url, Password = url.Password, UserName = url.UserName }));
                }
                catch (Exception e)
                {
                    Log.Error(url.Url + " bağlanılamadı! : " + e.Message);
                    continue;
                }
            }
            return fileNames;
        }
        public IEnumerable<string[]> GetRawFile(FileNameWithUrl fileNameWithUrl)
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
            List<FtpUrl> listOfUrls = new List<FtpUrl>() { new FtpUrl { Url = "ftp://10.100.0.104/", UserName = "ftpuser", Password = "aA123456" }, new FtpUrl { Url = "ftp://10.100.0.156/", UserName = "seyit", Password = "aA123456" } };
            var fileNames = GetFileNames(listOfUrls);
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
            return res;
        }
    }
}
