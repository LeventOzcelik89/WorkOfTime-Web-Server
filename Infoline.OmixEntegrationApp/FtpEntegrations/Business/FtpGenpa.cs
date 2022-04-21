using Infoline.Framework.Database;
using Infoline.OmixEntegrationApp.FtpEntegrations.Model;
using Infoline.OmixEntegrationApp.FtpEntegrations.Utils;
using Infoline.OmixEntegrationApp.Utils;
using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
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
        public Guid DistributorId { get { return new Guid("32f1df56-636d-4d9e-917b-0878a6ee4c51"); } }
        private string DirUrl { get; set; }
        private string LoginUrl { get; set; }
        public FtpGenpa()
        {
            Log.Warning("Start Process Ftp Genpa");
            SetFtpConfiguration();
            Login();

        }
        public WorkOfTimeDatabase GetDbConnection()
        {
            var tenantCode = ConfigurationManager.AppSettings["DefaultTenant"].ToString();
            var tenant = TenantConfig.GetTenants().Where(a => a.TenantCode == Convert.ToInt32(tenantCode)).FirstOrDefault();
            return tenant.GetDatabase();
        }
        public ResultStatus ExportFilesToDatabase()
        {
            var entegrationFileList = GetFilesInFtp();
            var result = new ResultStatus { result = true };
            foreach (var entegrationFile in entegrationFileList)
            {
                Log.Warning("Start Process File: {0} - {1} - {2}", this.ftpConfiguration.Url, this.DistributorName, entegrationFile.FileName);
                var db = GetDbConnection();
                var filesList = db.GetPRD_EntegrationFiles().Where(a => a.FileName == entegrationFile.FileName).FirstOrDefault();
                if (filesList == null)
                {
                    result = db.InsertPRD_EntegrationFiles(entegrationFile);
                }
                if (!result.result)
                {
                    Log.Error("There was a problem while data recording...: " + result.message);
                    continue;
                }
                if (entegrationFile.FileTypeName == "SELLTHR")
                {
                    var sellThr = GetSellInFilesInFtp(entegrationFile.FileName, entegrationFile.id);
                    var insertGenpaList = new List<PRD_EntegrationAction>();
                    if (sellThr != null && sellThr.Count() > 0)
                    {
                        foreach (var item in sellThr)
                        {
                            if (item.Imei != null)
                            {
                                var checkImei = db.GetPRD_EntegrationAction().Where(a => a.Imei == item.Imei && a.Quantity == 1).OrderByDescending(b => b.created).FirstOrDefault();
                                if (checkImei == null)
                                {
                                    item.EntegrationFileId = entegrationFile.id;
                                    insertGenpaList.Add(item);
                                }
                                else
                                {
                                    if (checkImei.EntegrationFileId != null)
                                    {
                                        var file = db.GetPRD_EntegrationFilesById(checkImei.EntegrationFileId.Value);
                                        Log.Info(item.Imei + " Daha önce " + file.FileName + " adlı dosya ile içeri aktarılmıştır");
                                    }
                                    Log.Info(item.Imei + "Sistemde bulunuyor...");
                                }
                            }
                            else
                            {
                                Log.Info("Imei Numarası Boş");
                            }
                        }
                        if (insertGenpaList.Count() > 0)
                        {
                            var InsertResult = db.BulkInsertPRD_EntegrationAction(insertGenpaList);
                            if (!InsertResult.result)
                                Log.Error("SellIn Insert Problem... {1} : {0} : Message: {2}", this.ftpConfiguration.Url, this.DistributorName, InsertResult.message);
                        }
                    }
                }
                Log.Success("Finish Process File : {0} - {1} - {2}", this.ftpConfiguration.Url, this.DistributorName, entegrationFile.FileName);
            }
            Log.Success($"All Files Are Integrated In {DistributorName} FTP");
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
        public PRD_EntegrationFiles[] GetFilesInFtp()
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
                Log.Info("Genpa Files Count:" + directoryItems.Count);
            }
            catch (Exception e)
            {
                Log.Error("GENPA : " + e.ToString());
            };
            var entegrationFileList = new List<PRD_EntegrationFiles>();
            foreach (var file in directoryItems)
            {
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
            var db = GetDbConnection();
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
                                        indexName = indexName.ToLower(new CultureInfo("en-US", false)).Replace(" ", "").Replace("_", "");
                                        if (indexName == "invoicenumber")
                                            item.InvoiceNumber = rawFileCheckedData;
                                        if (indexName == "dist")
                                            item.DistributorName = rawFileCheckedData;
                                        if (indexName == "customeroperatorcode")
                                            item.CustomerOperatorCode = rawFileCheckedData;
                                        if (indexName == "customergenpacode" || indexName == "customerkvkcode" || indexName == "customermobitelcode")
                                            item.CustomerOperatorCode = rawFileCheckedData; //TODO: Check
                                        if (indexName == "customername")
                                            item.CustomerOperatorName = rawFileCheckedData;
                                        if (indexName == "branchcode")
                                            item.BranchCode = rawFileCheckedData;
                                        if (indexName == "branchname")
                                            item.BranchName = rawFileCheckedData;
                                        if (indexName == "taxnumber")
                                            item.TaxNumber = rawFileCheckedData;
                                        if (indexName == "consolidationcode")
                                            item.ConsolidationCode = rawFileCheckedData;
                                        if (indexName == "consolidationname")
                                            item.ConsolidationName = rawFileCheckedData;
                                        if (indexName == "imei")
                                            item.Imei = rawFileCheckedData;
                                        if (indexName == "seriNo")
                                            item.SerialNo = rawFileCheckedData;
                                        if (indexName == "quantity")
                                            item.Quantity = Convert.ToInt32(rawFileCheckedData);
                                        if (indexName == "city")
                                            item.CustomerOperatorStorageCity = rawFileCheckedData;
                                        if (indexName == "town")
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
                        item.DistributorId = DistributorId;
                        item.DistributorName = this.DistributorName;
                        item.EntegrationFileId = entegrationFilesId;
                        var message = "";
                        var inventory = Finder.FindInventory(item.SerialNo, item.Imei);
                        if (inventory == null)
                        {
                            message = "Cihazın Envanterde Karşılı Yoktur.";
                        }
                        var company = Finder.FindCompany(item);
                        if (!company.HasValue)
                        {
                            var cmp = new CMP_Company
                            {
                                created = DateTime.Now,
                                createdby = item.createdby,
                                id = Guid.NewGuid(),
                                code = item.CustomerOperatorCode,
                                taxNumber = item.TaxNumber,
                                isActive = (int)EnumCMP_CompanyIsActive.Aktif,
                                pid = item.DistributorId,
                                name = item.CustomerOperatorName,
                                type = (int)EnumCMP_CompanyType.Diger,
                                taxType = (int)EnumCMP_CompanyTaxType.TuzelKisi,
                            };
                            var result = db.InsertCMP_Company(cmp);
                            if (!result.result)
                            {
                                message = "Yeni Cari Kaydı Yapılamadı";
                            }
                        }
                        item.ProductId = inventory?.productId;
                        item.InventoryId = inventory?.id;
                        item.CustomerOperatorId = company;
                        sellThrs.Add(item);
                        if (!string.IsNullOrEmpty(message))
                        {
                            NotificationLogger.SaveError(DateTime.Now, message, item);
                        }
                    }
                    catch (Exception e)
                    {
                        Log.Error("There is a Problem When Reading Raw File {0}", e.ToString());
                    }
                }
            }
            catch (Exception e)
            {
                if (e.Message.Contains("404"))
                {
                    SetFtpConfiguration();
                    Login();
                }
                Log.Error(e.ToString());
            }
            return sellThrs.ToArray();
        }
        private IEnumerable<string[]> GetRawFile(string FileName)
        {
            var liststringArray = new List<string[]>();
            var downloadUrl = "https://ftp.genpa.com.tr/?download&filename=" + FileName + "&" + Token;
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
