﻿using Infoline.Framework.Database;
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
    public class FtpMobitel : IFtpDistributorEntegration
    {
        public FtpConfiguration ftpConfiguration { get; set; }
        public string DistributorName
        { get { return "MobilTel"; } }
        public Guid DistributorId
        { get { return new Guid("da14f7f9-2a41-48b9-acd0-fd62602c8bcf"); } }
        public FtpMobitel()
        {
            Log.Warning("Start Process Ftp Mobitel");
            SetFtpConfiguration();
        }
        public WorkOfTimeDatabase GetDbConnection()
        {
            var tenantCode = ConfigurationManager.AppSettings["DefaultTenant"].ToString();
            var tenant = TenantConfig.GetTenants().Where(a => a.TenantCode == Convert.ToInt32(tenantCode)).FirstOrDefault();
            return tenant.GetDatabase();
        }
        public void SetFtpConfiguration()
        {
            var url = ConfigurationManager.AppSettings["MobiltelUrl"].ToString();
            var userName = ConfigurationManager.AppSettings["MobiltelUserName"].ToString();
            var password = ConfigurationManager.AppSettings["MobiltelPassword"].ToString();
            var directory = ConfigurationManager.AppSettings["MobiltelDirectory"].ToString();
            var fileExtension = ConfigurationManager.AppSettings["MobiltelFileExtension"].ToString();
            this.ftpConfiguration = new FtpConfiguration
            {
                Directory = directory,
                Password = password,
                Url = url,
                UserName = userName,
                FileExtension = fileExtension
            };
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
                    var insertMobiltelList = new List<PRD_EntegrationAction>();
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
                                    insertMobiltelList.Add(item);
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
                        if (insertMobiltelList.Count() > 0)
                        {
                            var InsertResult = db.BulkInsertPRD_EntegrationAction(insertMobiltelList);
                            if (!InsertResult.result)
                                Log.Info("SellIn Insert Problem... {1} : {0} : Message: {2}", this.ftpConfiguration.Url, this.DistributorName, InsertResult.message);
                        }
                    }
                }
                Log.Success("Finish Process File : {0} - {1} - {2}", this.ftpConfiguration.Url, this.DistributorName, entegrationFile.FileName);
            }
            Log.Success($"All Files Are Integrated In {DistributorName} FTP");
            return result;
        }
        public PRD_EntegrationFiles[] GetFilesInFtp()
        {
            Log.Info(string.Format("Getting File Names On {1} : {0}", this.ftpConfiguration.Url, this.DistributorName));
            var directoryItems = new List<DirectoryItem>();
            var fileList = new List<FileNameWithUrl>();
            try
            {
                var request = (FtpWebRequest)WebRequest.Create(this.ftpConfiguration.Url + this.ftpConfiguration.Directory);
                request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                request.Credentials = new NetworkCredential(this.ftpConfiguration.UserName, this.ftpConfiguration.Password);
                string[] lineList = null;
                using (var response = (FtpWebResponse)request.GetResponse())
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    lineList = reader.ReadToEnd().Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                }
                foreach (string line in lineList.Where(x => x.Contains("SELLIN") || x.Contains("SELLTHR")))
                {
                    var item = new DirectoryItem();
                    var fileDate = DateTime.ParseExact(line.Substring(0, 17), "MM-dd-yy  hh:mmtt", CultureInfo.InvariantCulture);
                    var fileName = Tools.GetItemName(line);
                    item.BaseUri = this.ftpConfiguration.Url;
                    item.IsDirectory = Tools.IsDir(line);
                    item.Name = fileName;
                    item.DateFileCreated = fileDate;
                    directoryItems.Add(item);
                    if (!item.IsDirectory)
                    {
                        fileList.Add(new FileNameWithUrl { FileName = item.Name, FileCreatedDate = item.DateFileCreated, DirectoryFileName = this.ftpConfiguration.Url + this.ftpConfiguration.Directory + "//" + item.Name });
                    }
                }
                Log.Info("Files Count:" + fileList.Count);
            }
            catch (Exception e)
            {
                Log.Error(this.ftpConfiguration.Url + " failed! : " + e.Message);
                return null;
            }
            var db = GetDbConnection();
            var entegrationFilesInDb = db.GetPRD_EntegrationFilesByCreatedDate(DistributorName);
            var entegrationFileList = new List<PRD_EntegrationFiles>();
            foreach (var file in fileList)
            {
                entegrationFileList.Add(new PRD_EntegrationFiles
                {
                    id = Guid.NewGuid(),
                    created = DateTime.Now,
                    createdby = Guid.Empty,
                    CreateDateInFtp = file.FileCreatedDate,
                    DistributorName = DistributorName,
                    DistributorId = DistributorId,
                    FileName = file.DirectoryFileName,
                    FileNameDate = Tools.GetDateFromFileName(file.FileName, "yyyyMMddss"),
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
                Log.Error(e.ToString());
            }
            return sellThrs.ToArray();
        }
        public List<string[]> GetRawFile(string fileName)
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
    }
}