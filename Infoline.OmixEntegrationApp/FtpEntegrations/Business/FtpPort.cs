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
namespace Infoline.OmixEntegrationApp.FtpEntegrations.Business
{
    public class FtpPort : IFtpDistributorEntegration
    {
        public FtpConfiguration ftpConfiguration { get; set; }
        public string DistributorName => "Port";
        public Guid DistributorId => new Guid("c965ac82-d3e9-476f-8cac-1cdbfa24b9a9");
        public FtpPort()
        {
            Log.Warning("Start Process Ftp Port");
            SetFtpConfiguration();
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
                            Log.Info("SellThr Bulk Insert Problem... {1} : {0} : Message: {2}", this.ftpConfiguration.Url, this.DistributorName, bultInsertResult.message);
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
        public WorkOfTimeDatabase GetDbConnection()
        {
            var tenantCode = ConfigurationManager.AppSettings["DefaultTenant"].ToString();
            var tenant = TenantConfig.GetTenants().Where(a => a.TenantCode == Convert.ToInt32(tenantCode)).FirstOrDefault();
            return tenant.GetDatabase();
        }
        public PRD_EntegrationFiles[] GetFilesInFtp(DateTime processDate)
        {
            Log.Info(string.Format("Getting All File Names From PORT Server {0}", ftpConfiguration.Url));
            var fileList = new List<FileNameWithUrl>();
            try
            {
                FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(ftpConfiguration.Url);
                request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                request.Credentials = new NetworkCredential(ftpConfiguration.UserName, ftpConfiguration.Password);
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
                    var dateString = line.Substring(37, 56 - 44);
                    bool isDirectory = data[0].ToString() == "d";
                    var name = data.Substring(50);
                    item.DateFileCreated = DateTime.ParseExact(dateString, "MMM dd HH:mm", CultureInfo.InvariantCulture);
                    item.Name = name;
                    item.BaseUri = ftpConfiguration.Url;
                    item.IsDirectory = isDirectory;
                    if (name == "." || name == "..")
                    {
                    }
                    else
                    {
                        if (!isDirectory)
                        {
                            fileList.Add(new FileNameWithUrl { FileName = item.Name, FileCreatedDate = item.DateFileCreated, DirectoryFileName = this.ftpConfiguration.Url + this.ftpConfiguration.Directory + "//" + item.Name });
                        }
                    }
                }
                Log.Info("Files Count:" + fileList.Count);
            }
            catch (Exception e)
            {
                Log.Error(ftpConfiguration.Url + " failed! : " + e.Message);
            }
            var db = GetDbConnection();
            var entegrationFilesInDb = db.GetPRD_EntegrationFilesByCreatedDate(DistributorName);
            var entegrationFileList = new List<PRD_EntegrationFiles>();
            foreach (var file in fileList)
            {
                if (entegrationFilesInDb.Any(x => x.FileName == (file.FileName)))
                    continue;
                entegrationFileList.Add(new PRD_EntegrationFiles
                {
                    id = Guid.NewGuid(),
                    created = DateTime.Now,
                    createdby = Guid.Empty,
                    CreateDateInFtp = file.FileCreatedDate,
                    DistributorName = DistributorName,
                    DistributorId = DistributorId,
                    FileName = file.DirectoryFileName,
                    FileNameDate = Tools.GetDateFromFileName(file.FileName, "yyyyMMdd"),
                    ProcessTime = DateTime.Now,
                    FileTypeName = FileTypeName(file.FileName)
                });
            }
            Log.Warning("There are {0} Files to Process...", entegrationFileList.Count());
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
                            message = "\nCarinin Sistemde Karşılığı Yoktur.";
                        }
                        item.ProductId = inventory?.productId;
                        item.InventoryId = inventory?.id;
                        item.CustomerOperatorId = company;
                        if (item.Imei != null)
                        {
                            var existRetitive = db.GetPRD_EntegrationActionByRepetitive(item.Imei);
                            if (existRetitive != null)
                            {
                                message = item.Imei + " Imei Numarası Sistemde Mevcuttur.";
                            }
                            else
                            {
                                sellThrs.Add(item);
                            }

                        }
                        else
                        {
                            sellThrs.Add(item);
                        }
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
                Log.Error(e.ToString());
            }
            return sellThrs.ToArray();
        }
        public void SetFtpConfiguration()
        {
            var portUserName = ConfigurationManager.AppSettings["PortUserName"].ToString();
            var portPassword = ConfigurationManager.AppSettings["PortPassword"].ToString();
            var portUrl = ConfigurationManager.AppSettings["PortHost"].ToString() ?? "";
            ftpConfiguration = new FtpConfiguration
            {
                Url = portUrl,
                UserName = portUserName,
                Password = portPassword
            };
        }
        private IEnumerable<string[]> GetRawFile(string fileName)
        {
            Log.Info(string.Format("Getting File  {0} on KVK Server", fileName));
            var listStringArray = new List<string[]>();
            var request = (FtpWebRequest)WebRequest.Create(fileName);
            request.Method = WebRequestMethods.Ftp.DownloadFile;
            request.Credentials = new NetworkCredential(ftpConfiguration.UserName, ftpConfiguration.Password);
            var response = (FtpWebResponse)request.GetResponse();
            var responseStream = response.GetResponseStream();
            var reader = new StreamReader(responseStream);
            string line;
            try
            {
                while ((line = reader.ReadLine()) != null)
                {
                    List<string> splitedLines = line.Split('\t').ToList();
                    listStringArray.Add(splitedLines.ToArray());
                }
                response.Close();
                reader.Close();
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
            }
            return listStringArray;
        }
    }
}
