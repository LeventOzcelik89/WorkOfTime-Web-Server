using Infoline.Framework.Database;
using Infoline.OmixEntegrationApp.FtpEntegrations.Model;
using Infoline.OmixEntegrationApp.FtpEntegrations.Utils;
using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml;
namespace Infoline.OmixEntegrationApp.FtpEntegrations.Business
{
    public class FtpGenpa : IFtpDistributorEntegration
    {
        public FtpConfiguration ftpConfiguration { get; private set; }
        private string Token { get; set; }
        public string DistributorName { get { return "Genpa"; } }
        public Guid DistributorId { get { return new Guid("da14f7f9-2a41-48b9-acd0-fd62602c8bcf"); } }
        private string DirUrl { get; set; }
        private string LoginUrl { get; set; }
        public FtpGenpa()
        {
            Log.Warning("Start Process Ftp Genpa");

            SetFtpConfiguration();
            Login();
            GetFilesInFtp(DateTime.Now);
        }
        public WorkOfTimeDatabase GetDbConnection()
        {
            var tenantCode = ConfigurationManager.AppSettings["DefaultTenant"].ToString();
            var tenant = TenantConfig.GetTenants().Where(a => a.TenantCode == Convert.ToInt32(tenantCode)).FirstOrDefault();
            return tenant.GetDatabase();
        }
        public ResultStatus ExportFilesToDatabase()
        {
            var processDate = DateTime.Now.AddDays(-30);
            var entegrationFileList = GetFilesInFtp(processDate);

            var result = new ResultStatus();
            foreach (var entegrationFile in entegrationFileList)
            {
                Log.Warning("Start Process File: {0} - {1} - {2}", this.ftpConfiguration.Url, this.DistributorName, entegrationFile.FileName);

                var db = GetDbConnection();
                result = db.InsertPRD_EntegrationFiles(entegrationFile);
                if (!result.result)
                {
                    Log.Error("There was a problem while data recording...: ", result.message);
                    continue;
                }

                if (entegrationFile.FileTypeName == "SELLTHR")
                {
                    var sellThr = GetSellInFilesInFtp(entegrationFile.FileName, entegrationFile.id);
                    if (sellThr != null && sellThr.Count() > 0)
                    {
                        var bultInsertResult = db.BulkInsertPRD_EntegrationAction(sellThr);
                        if (!bultInsertResult.result)
                            Log.Info("SellIn Bulk Insert Problem... {1} : {0} : Message: {2}", this.ftpConfiguration.Url, this.DistributorName, bultInsertResult.message);
                    }
                }
                Log.Success("Finish Process File : {0} - {1} - {2}", this.ftpConfiguration.Url, this.DistributorName, entegrationFile.FileName);
            }
            return result;
        }
        public string FileTypeName(string fileName)
        {
            if (fileName.Contains("SELLIN"))
                return "SELLIN";
            else if (fileName.Contains("SELLTHR"))
                return "SELLTHR";
            else if (fileName.Contains("STOK"))
                return "SELLSTK";
            else
                return null;
        }
        public PRD_EntegrationFiles[] GetFilesInFtp(DateTime processDate)
        {
            Log.Info("Getting All File Names On Genpa Wing Ftp Server");
            var directoryItems = new List<DirectoryItem>();
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
                    var dates = document.DocumentElement.GetElementsByTagName("date");
                    for (int i = 0; i < tagName.Count; i++)
                    {
                        var fileName = tagName[i].InnerText;
                        var date = DateTime.Parse(dates[i].InnerText);
                        if ((fileName.Contains("SELLIN") || fileName.Contains("SELLTHR")))
                        {
                            directoryItems.Add(new DirectoryItem { Name = fileName, DateFileCreated = date });
                        }
                    }
                }
                Log.Info("Files Count:"+ directoryItems.Count);

            }
            catch (Exception e)
            {
                Log.Error("GENPA : " + e.ToString());
            };
            var db = GetDbConnection();
            var entegrationFilesInDb = db.GetPRD_EntegrationFilesByCreatedDate(processDate, DistributorName);
            var entegrationFileList = new List<PRD_EntegrationFiles>();
            foreach (var file in directoryItems)
            {
                if (entegrationFilesInDb.Any(x => x.FileName.Contains(file.Name)))
                    continue;
                entegrationFileList.Add(new PRD_EntegrationFiles
                {
                    id = Guid.NewGuid(),
                    created = DateTime.Now,
                    createdby = Guid.Empty,
                    CreateDateInFtp = file.DateFileCreated,
                    DistributorName = DistributorName,
                    DistributorId = DistributorId,
                    FileName = file.Name,
                    FileNameDate = Tools.GetDateFromFileNameForGenpa(file.Name, "yyyyMMdd"),
                    ProcessTime = DateTime.Now,
                    FileTypeName = FileTypeName(file.Name)
                });
            }
            return entegrationFileList.ToArray();
        }
        public PRD_EntegrationAction[] GetSellInFilesInFtp(string fileName, Guid entegrationFilesId)
        {
            var sellThrs = new List<PRD_EntegrationAction>();
            var datetimeNow = DateTime.Now;
            try
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
                    try
                    {
                        var item = new PRD_EntegrationAction();
                        for (int i = 0; i < rawFile.Length; i++)
                        {
                            try
                            {
                                var indexName = Index.Where(x => x.Index == i).Select(x => x.Name).FirstOrDefault();
                                if (indexName != null)
                                {
                                    var rawFileCheckedData = rawFile[i].Replace("\\", "").Replace("\"", "");
                                    if (!string.IsNullOrEmpty(rawFileCheckedData))
                                    {
                                        if (indexName == "InvoiceNumber")
                                            item.InvoiceNumber = rawFileCheckedData;
                                        if (indexName == "Dist")
                                            item.DistributorName = rawFileCheckedData;
                                        if (indexName == "CustomerOperatorCode")
                                            item.CustomerOperatorCode = rawFileCheckedData;
                                        if (indexName == "CustomerGenpaCode" || indexName == "CustomerKVKCode" || indexName == "CustomerMobitelCode")
                                            item.CustomerOperatorCode = rawFileCheckedData; //TODO: Check
                                        if (indexName == "CustomerName")
                                            item.CustomerOperatorName = rawFileCheckedData;
                                        if (indexName == "BranchCode")
                                            item.BranchCode = rawFileCheckedData;
                                        if (indexName == "BranchName")
                                            item.BranchName = rawFileCheckedData;
                                        if (indexName == "TaxNumber")
                                            item.TaxNumber = rawFileCheckedData;
                                        if (indexName == "ConsolidationCode")
                                            item.ConsolidationCode = rawFileCheckedData;
                                        if (indexName == "ConsolidationName")
                                            item.ConsolidationName = rawFileCheckedData;
                                        if (indexName == "Imei")
                                            item.Imei = rawFileCheckedData;
                                        if (indexName == "SeriNo")
                                            item.SerialNo = rawFileCheckedData;
                                        if (indexName == "Quantity")
                                            item.Quantity = Convert.ToInt32(rawFileCheckedData);
                                        if (indexName == "City")
                                            item.CustomerOperatorStorageCity = rawFileCheckedData;
                                        if (indexName == "Town")
                                            item.CustomerOperatorStorageTown = rawFileCheckedData;
                                    }
                                }
                                else
                                {
                                    Log.Error("There is a null ColumnName...");
                                }
                            }
                            catch (Exception e)
                            {
                                Log.Error("There is Problem When Object Set Value : {0}", e.ToString());
                            }
                        }
                        item.ProductId = Finder.FindInventory(item.SerialNo, item.Imei)?.id;
                        item.InventoryId = Finder.FindInventory(item.SerialNo, item.Imei)?.productId;
                        item.DistributorId = DistributorId;
                        item.DistributorName = this.DistributorName;
                        item.EntegrationFileId = entegrationFilesId;
                        sellThrs.Add(item);
                    }
                    catch (Exception e)
                    {
                        Log.Error("There is a Problem When Reading Raw File {0}", e.ToString());
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
            }
            return sellThrs.ToArray();
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
