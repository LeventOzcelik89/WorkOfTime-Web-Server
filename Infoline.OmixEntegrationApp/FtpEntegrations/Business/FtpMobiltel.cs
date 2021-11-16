using Infoline.Framework.Database;
using Infoline.OmixEntegrationApp.DistFtpEntegrations.Concrete;
using Infoline.OmixEntegrationApp.DistFtpEntegrations.Model;
using Infoline.OmixEntegrationApp.DistFtpEntegrations.Utils;
using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;

namespace Infoline.OmixEntegrationApp.DistFtpEntegrations.Business
{
    public class FtpMobitel
    {
        public FtpConfiguration ftpConfiguration { get; }
        public string DistributorName = "Mobiltel";
        public Guid DistributorId = new Guid("da14f7f9-2a41-48b9-acd0-fd62602c8bcf");

        public FtpMobitel()
        {
            var tenantCode = ConfigurationManager.AppSettings["DefaultTenant"].ToString();
            var tenant = TenantConfig.GetTenants().Where(a => a.TenantCode == Convert.ToInt32(tenantCode)).FirstOrDefault();

            var url = ConfigurationManager.AppSettings["MobiltelUrl"].ToString();
            var userName = ConfigurationManager.AppSettings["MobiltelUserName"].ToString();
            var password = ConfigurationManager.AppSettings["MobiltelPassword"].ToString();
            var directory = ConfigurationManager.AppSettings["MobiltelDirectory"].ToString();
            var readAllDirectory = ConfigurationManager.AppSettings["MobiltelReadAllDirectory"].ToString();
            var fileExtension = ConfigurationManager.AppSettings["MobiltelFileExtension"].ToString();

            this.ftpConfiguration = new FtpConfiguration
            {
                Directory = directory,
                Password = password,
                SearchAllDirectory = Convert.ToBoolean(readAllDirectory),
                Url = url,
                UserName = userName,
                FileExtension = fileExtension
            };
        }

        public ResultStatus FileSave()
        {
            var db = new WorkOfTimeDatabase();

            var fileNames = GetFiles();
            var entegrationFileList = new List<PRD_EntegrationFiles>();

            foreach (var file in fileNames.Where(x => x.FileName.Contains("SELLIN") || x.FileName.Contains("SELLTHR")))
            {
                entegrationFileList.Add(new PRD_EntegrationFiles
                {
                    id = Guid.NewGuid(),
                    created = DateTime.Now,
                    createdby = Guid.Empty,
                    CreateDateInFtp = file.FileCreatedDate,
                    DistributorName = this.DistributorName,
                    DistributorId = this.DistributorId,
                    FileName = file.DirectoryFileName,
                    FileNameDate = Tools.GetDateFromFileName(file.FileName, "yyyyMMddss"),
                    ProcessTime = DateTime.Now,
                });
            }

            var result = new ResultStatus();
            foreach (var entegrationFile in entegrationFileList)
            {
                result = db.InsertPRD_EntegrationFiles(entegrationFile);
                if (!result.result)
                {
                    Log.Error("There was a problem while data recording...: ", result.message);
                    continue;
                }

                var sellIn = GetSellInObjectForToday(entegrationFile.FileName, entegrationFile.id);
                if (sellIn != null && sellIn.Count() > 0)
                {
                    db.BulkInsertPRD_EntegrationStorage(sellIn);
                }
                var sellThr = GetSellThrObjectForToday(entegrationFile.FileName, entegrationFile.id);
                if (sellThr != null && sellThr.Count() > 0)
                {
                    db.BulkInsertPRD_EntegrationAction(sellThr);
                }
            }
            //TODO : kayıtlar içerisinde problem var ise ne yapılacak...
            return result;
        }

        private IEnumerable<FileNameWithUrl> GetFiles()
        {
            Log.Info(string.Format("Getting File Names On {1} : {0}", this.ftpConfiguration.Url, this.DistributorName));
            var directoryItems = new List<DirectoryItem>();
            var ftpUrl = new List<FileNameWithUrl>();

            try
            {

                var request = (FtpWebRequest)WebRequest.Create(this.ftpConfiguration.Url + this.ftpConfiguration.Directory);
                request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                request.Credentials = new NetworkCredential(this.ftpConfiguration.UserName, this.ftpConfiguration.Password);
                string[] list = null;
                using (var response = (FtpWebResponse)request.GetResponse())
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    list = reader.ReadToEnd().Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                }

                foreach (string line in list)
                {
                    var item = new DirectoryItem();

                    item.DateCreated = Tools.GetDate(line);
                    item.BaseUri = this.ftpConfiguration.Url;
                    item.IsDirectory = Tools.IsDir(line);
                    item.Name = Tools.GetItemName(line);
                    directoryItems.Add(item);
                    if (!item.IsDirectory)
                    {
                        ftpUrl.Add(new FileNameWithUrl { FileName = item.Name, FileCreatedDate = item.DateCreated, DirectoryFileName = this.ftpConfiguration.Url + this.ftpConfiguration.Directory + "//" + item.Name });
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error(this.ftpConfiguration.Url + " failed! : " + e.Message);
            }
            return ftpUrl;
        }

        private IEnumerable<PRD_EntegrationStorage> GetSellInObjectForToday(string fileName, Guid entegrationFilesId)
        {
            List<PRD_EntegrationStorage> sellIns = new List<PRD_EntegrationStorage>();

            try
            {
                List<PropertyIndex> Index = new List<PropertyIndex>();
                var getRawFile = GetRawFile(fileName).ToList();
                var getHeaders = getRawFile[0];

                getRawFile.RemoveAt(0);
                for (int i = 0; i < getHeaders.Length; i++)
                {
                    Index.Add(new PropertyIndex { Index = i, Name = getHeaders[i].Replace("\\", "").Replace("\"", "") });
                }

                foreach (var rawFile in getRawFile)
                {
                    try
                    {
                        var item = new PRD_EntegrationStorage();
                        for (int i = 0; i < rawFile.Length; i++)
                        {
                            try
                            {
                                var indexName = Index.Where(x => x.Index == i).Select(x => x.Name).FirstOrDefault();
                                if (indexName != null)
                                {
                                    if (indexName == "Dist")
                                        item.DistributorName = rawFile[i].Replace("\\", "").Replace("\"", "");
                                    if (indexName == "StorageCode")
                                        item.DistStorageCode = rawFile[i].Replace("\\", "").Replace("\"", "");
                                    if (indexName == "StorageName")
                                        item.DistStorageName = rawFile[i].Replace("\\", "").Replace("\"", "");
                                    if (indexName == "City")
                                        item.DistStorageCity = rawFile[i].Replace("\\", "").Replace("\"", "");
                                    if (indexName == "Town")
                                        item.DistStorageTown = rawFile[i].Replace("\\", "").Replace("\"", "");
                                    if (indexName == "ConsolidationCode")
                                        item.ConsolidationCode = rawFile[i].Replace("\\", "").Replace("\"", "");
                                    if (indexName == "ConsolidationName")
                                        item.ConsolidationName = rawFile[i].Replace("\\", "").Replace("\"", "");
                                    if (indexName == "Imei")
                                        item.Imei = rawFile[i].Replace("\\", "").Replace("\"", "");
                                    if (indexName == "SeriNo")
                                        item.SerialNo = rawFile[i].Replace("\\", "").Replace("\"", "");
                                    if (indexName == "Quantity")
                                        item.Quantity = Convert.ToInt32(rawFile[i].Replace("\\", "").Replace("\"", ""));
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
                        item.DistributorId = this.DistributorId;
                        item.DistributorName = this.DistributorName;
                        item.EntegrationFileId = entegrationFilesId;
                        sellIns.Add(item);
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

            return sellIns;
        }

        private IEnumerable<PRD_EntegrationAction> GetSellThrObjectForToday(string fileName, Guid entegrationFilesId)
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
                        item.DistributorId = this.DistributorId;
                        item.DistributorName = this.DistributorName;
                        item.EntegrationFileId = entegrationFilesId;
                        sellThrs.Add(item);
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
            return sellThrs;
        }

        private IEnumerable<string[]> GetRawFile(string fileName)
        {
            Log.Info(string.Format("Getting File  {0} on Mobitel Server {1}", fileName, ftpConfiguration.Url));
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
                    List<string> splitedLines = line.Split(';').ToList();
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
