using Infoline.Framework.Database;
using Infoline.OmixEntegrationApp.FtpEntegrations.Model;
using Infoline.OmixEntegrationApp.FtpEntegrations.Utils;
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
    public class FtpKvk : IFtpDistributorEntegration
    {
        public FtpConfiguration ftpConfiguration { get; set; }

        public string DistributorName => "KVK";

        public Guid DistributorId => new Guid("6fc15dc2-e1ce-46e2-8b3e-2e23badb1e80");


        public FtpKvk()
        {
            Log.Warning("Start Process Ftp KVK");

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
            return result;
        }

        public WorkOfTimeDatabase GetDbConnection()
        {
            var tenantCode = ConfigurationManager.AppSettings["DefaultTenant"].ToString();
            var tenant = TenantConfig.GetTenants().Where(a => a.TenantCode == Convert.ToInt32(tenantCode)).FirstOrDefault();
            return tenant.GetDatabase();
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
            Log.Info(string.Format("Getting All File Names From Kvk Server {0}", ftpConfiguration.Url));
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
                    var dateString = line.Substring(43, 56 - 44);

                    bool isDirectory = data[0].ToString() == "d";
                    var name = data.Substring(56);
                    item.DateFileCreated = DateTime.ParseExact(line.Substring(43, 56 - 44), "MMM dd HH:mm", CultureInfo.InvariantCulture);
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
            }
            catch (Exception e)
            {
                Log.Error(ftpConfiguration.Url + " failed! : " + e.Message);
            }
            var db = GetDbConnection();
            var entegrationFilesInDb = db.GetPRD_EntegrationFilesByCreatedDate(processDate, DistributorName);
            var entegrationFileList = new List<PRD_EntegrationFiles>();
            foreach (var file in fileList.Where(x => x.FileCreatedDate >= processDate))
            {
                if (entegrationFilesInDb.Any(x => x.FileName.Contains(file.FileName)))
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
                                        if (indexName.ToLower() == "invoicenumber")
                                            item.InvoiceNumber = rawFileCheckedData;
                                        if (indexName.ToLower() == "dist"|| indexName.ToLower() == "dıst")
                                            item.DistributorName = rawFileCheckedData;
                                        if (indexName.ToLower() == "customeroperatorcode")
                                            item.CustomerOperatorCode = rawFileCheckedData;
                                        if (indexName.ToLower() == "customerGenpaCode" || indexName.ToLower() == "customerkvkcode" || indexName == "customermobitelcode")
                                            item.CustomerOperatorCode = rawFileCheckedData; //TODO: Check
                                        if (indexName.ToLower() == "customername"|| indexName.ToLower() == "customer_name")
                                            item.CustomerOperatorName = rawFileCheckedData;
                                        if (indexName.ToLower() == "branchcode")
                                            item.BranchCode = rawFileCheckedData;
                                        if (indexName.ToLower() == "branchname")
                                            item.BranchName = rawFileCheckedData;
                                        if (indexName.ToLower() == "taxnumber"|| indexName.ToLower() == "tax_number")
                                            item.TaxNumber = rawFileCheckedData;
                                        if (indexName.ToLower() == "consolidationcode")
                                            item.ConsolidationCode = rawFileCheckedData;
                                        if (indexName.ToLower() == "consolidationname")
                                            item.ConsolidationName = rawFileCheckedData;
                                        if (indexName.ToLower() == "imei"|| indexName.ToLower() == "ımei")
                                            item.Imei = rawFileCheckedData;
                                        if (indexName.ToLower() == "serino")
                                            item.SerialNo = rawFileCheckedData;
                                        if (indexName.ToLower() == "quantity")
                                            item.Quantity = Convert.ToInt32(rawFileCheckedData);
                                        if (indexName.ToLower() == "city"||indexName.ToLower()=="cıty")
                                            item.CustomerOperatorStorageCity = rawFileCheckedData;
                                        if (indexName.ToLower() == "town")
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

        public void SetFtpConfiguration()
        {
            var kvkUserName = ConfigurationManager.AppSettings["KvkUserName"].ToString();
            var kvkPassword = ConfigurationManager.AppSettings["KvkPassword"].ToString();
            var kvkUrl = ConfigurationManager.AppSettings["KvkHost"].ToString() ?? "";
            ftpConfiguration = new FtpConfiguration
            {
                Url = kvkUrl,
                UserName = kvkUserName,
                Password = kvkPassword
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
                Log.Warning(e.Message);
            }
            return listStringArray;
        }
    }
}

