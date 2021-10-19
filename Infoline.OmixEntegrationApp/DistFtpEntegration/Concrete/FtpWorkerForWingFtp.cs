using Infoline.OmixEntegrationApp.DistFtpEntegration.Abstract;
using Infoline.OmixEntegrationApp.DistFtpEntegration.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml;
namespace Infoline.OmixEntegrationApp.DistFtpEntegration.Concrete
{
    public class FtpWorkerForWingFtp : IFtpWorker
    {
        private List<string> FileList = new List<string>();
        private string DirUrl => "https://ftp.genpa.com.tr/dir.html";
        private string LoginUrl => "https://ftp.genpa.com.tr/loginok.html";
        private string SetCookie { get; set; }
        private string Token { get; set; }
        private void Login()
        {
            Log.Info("Logging On Genpa Wing Ftp Server");
            try
            {
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                var httpRequest = (HttpWebRequest)WebRequest.Create(LoginUrl);
                httpRequest.Method = "POST";
                httpRequest.ContentType = "multipart/form-data";
                var formData = "username=omix&password=9zcHxC4@&username_val=omix&password_val=9zcHxC4@&submit_btn=Oturum%20A%C3%A7";
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
        private void GetTodayFileNames()
        {
            Log.Info("Getting All File Names On Genpa Wing Ftp Server");
            try
            {
                var webRequest = WebRequest.Create(DirUrl);
                webRequest.Headers.Add("Cookie", "client_lang=turkish; client_login_name=omix; viewmode=0; " + Token + " ");
                webRequest.Method = "POST";
                XmlDocument document = new XmlDocument();
                using (var response = webRequest.GetResponse())
                using (var content = response.GetResponseStream())
                using (XmlReader reader = XmlReader.Create(content))
                {
                    document.Load(reader);
                    var tagName = document.DocumentElement.GetElementsByTagName("name");
                    var dateTimeNow = DateTime.Now.Date;
                    var date = dateTimeNow.Day.ToString() + dateTimeNow.Month.ToString() + dateTimeNow.Year.ToString();
                    for (int i = 0; i < tagName.Count; i++)
                    {
                        var fileName = tagName[i].InnerText;
                        if ((fileName.Contains("SELLIN") || fileName.Contains("SELLTHR")) && fileName.Contains(date))
                        {
                            FileList.Add(fileName);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error("GENPA : " + e.ToString());
            };
        }
        private IEnumerable<string[]> GetRawFile(string FileName)
        {
            var liststringArray = new List<string[]>();
            var downloadUrl = "https://ftp.genpa.com.tr/?download&filename=" + FileName + "&" + Token + "&r=0.9193858193742144";
            var webRequest = WebRequest.Create(downloadUrl);
            using (var response = webRequest.GetResponse())
            using (var content = response.GetResponseStream())
            using (var reader = new StreamReader(content))
            {
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
                    Log.Error(e.Message);
                }
            }
            return liststringArray;
        }
        public IEnumerable<SellIn> GetToDayFile()
        {
            Login();
            GetTodayFileNames();
            Log.Info("Getting Today Files On Genpa Wing Ftp Server");
            var res = new List<SellIn>();
            Log.Info(string.Format("{0} File Found On Genpa Wing Ftp Server", FileList.Count));
            if (FileList.Count >= 0)
            {
                foreach (var fileName in FileList)
                {
                    try
                    {
                        List<PropertyIndex> Index = new List<PropertyIndex>();
                        var getRawFile = GetRawFile(fileName).ToList();
                        var headers = getRawFile[0];
                        getRawFile.RemoveAt(0);
                        for (int i = 0; i < headers.Length; i++)
                        {
                            Index.Add(new PropertyIndex { Index = i, Name = headers[i] });
                        }
                        foreach (var rawFile in getRawFile)
                        {
                            try
                            {
                                var item = new SellIn();
                                if (fileName.Contains("SELLTHR"))
                                {
                                    item = new SellThr();
                                }
                                for (int i = 0; i < headers.Length; i++)
                                {
                                    var getIndexName = Index.Where(x => x.Index == i).Select(x => x.Name).FirstOrDefault();
                                    var prop = item.GetType().GetProperty(getIndexName.Replace("GENPA","").Replace(" ", ""));
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
                            catch (Exception e)
                            {
                                Log.Error(e.ToString());
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Log.Error(e.ToString());
                    }
                }
            }
            return res;
        }
    }
}
