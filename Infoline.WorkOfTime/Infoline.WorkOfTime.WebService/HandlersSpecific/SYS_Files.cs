﻿using Infoline.Framework.Database;
using Infoline.Web.SmartHandlers;
using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Web;

namespace Infoline.WorkOfTime.WebService.Handler
{
    [Export(typeof(ISmartHandler))]
    public partial class SYS_FilesHandler : BaseSmartHandler
    {
        public SYS_FilesHandler()
            : base("SYS_FilesHandler")
        {

        }

        [HandleFunction("SYS_Files/GetFiles")]
        public void SYS_FilesGetFiles(HttpContext context)
        {
            var db = new WorkOfTimeDatabase();
            if (CallContext.Current == null)
            {
                RenderResponse(context, new ResultStatus { result = false, message = "Kullanıcı girişi yapmanız gerekmektedir." });
                return;
            }
            var dataId = context.Request["id"];
            if (dataId == null || dataId == "")
            {
                RenderResponse(context, new ResultStatus { result = false, message = "Id Bulunamadı." });
                return;
            }
            var sysfile = db.GetSYS_FilesByDataIdAll(new Guid(dataId));
            RenderResponse(context, new ResultStatus { result = true, objects = sysfile });
        }

        [HandleFunction("SYS_FilesSpesific/GetFiles")]
        public void SYS_FilesSpesificGetFiles(HttpContext context)
        {
            var db = new WorkOfTimeDatabase();
            if (CallContext.Current == null)
            {
                RenderResponse(context, new ResultStatus { result = false, message = "Kullanıcı girişi yapmanız gerekmektedir." });
                return;
            }

            var dataId = context.Request["id"];
            if (dataId == null || dataId == "")
            {
                RenderResponse(context, new ResultStatus { result = false, message = "Id Bulunamadı." });
                return;
            }

            var contacts = db.GetCRM_ContactByPresentationId(new Guid(dataId));
            var sysfile = db.GetSYS_FilesInDataId(contacts.Select(x => x.id).ToArray()).ToList();
            var sysfile2 = db.GetSYS_FilesByDataIdAll(new Guid(dataId)).ToList();
            sysfile.AddRange(sysfile2);
            RenderResponse(context, new ResultStatus { result = true, objects = sysfile });
        }

        [HandleFunction("SYS_FilesPersonalRegulation/GetFiles")]
        public void SYS_FilesPersonalRegulationGetFiles(HttpContext context)
        {
            var db = new WorkOfTimeDatabase();
            if (CallContext.Current == null)
            {
                RenderResponse(context, new ResultStatus { result = false, message = "Kullanıcı girişi yapmanız gerekmektedir." });
                return;
            }

            var userId = CallContext.Current.UserId;
            var user = db.GetVWSH_UserById(userId);

            if (user == null)
            {
                RenderResponse(context, new ResultStatus { result = false, message = "Kullanıcı Bulunamadı." });
                return;
            }

            var dataId = user.CompanyId.HasValue ? user.CompanyId.Value : new Guid();
            var sysfile = db.GetSYS_FilesByDataIdAndDataTableAll(dataId, "CMP_CompanyMyPersonDocument");
            RenderResponse(context, new ResultStatus { result = true, objects = sysfile });
        }

        [HandleFunction("SYS_Files/MBDelete")]
        public void SYS_FilesMBDelete(HttpContext context)
        {
            var db = new WorkOfTimeDatabase();
            if (CallContext.Current == null)
            {
                RenderResponse(context, new ResultStatus { result = false, message = "Kullanıcı girişi yapmanız gerekmektedir." });
                return;
            }
            if (context.Request["id"] == null || context.Request["id"] == "")
            {
                RenderResponse(context, new ResultStatus { result = false, message = "Id Bulunamadı." });
                return;
            }
            var data = db.GetSYS_FilesById(new Guid(context.Request["id"]));

            if (data == null)
            {
                RenderResponse(context, new ResultStatus { result = false, message = "Dosya Bulunamadı." });
                return;
            }
            var path = HttpContext.Current.Server.MapPath(data.FilePath);
            if (File.Exists(path) == true)
            {
                File.Delete(path);
            }
            var rsFiles = db.DeleteSYS_Files(data);
            RenderResponse(context, new ResultStatus { result = true, message = "Dosya silme işlemi başarılı" });
        }

        [HandleFunction("UploadFile/Insert")]
        public void UploadFileInsert(HttpContext context)
        {
            var db = new WorkOfTimeDatabase();
            if (CallContext.Current == null)
            {
                RenderResponse(context, new ResultStatus { result = false, message = "Kullanıcı girişi yapmanız gerekmektedir." });
            }

            var userId = CallContext.Current.UserId;
            var res = ParseRequest<UploadFileObject>(context);
            var webPath = System.Configuration.ConfigurationManager.AppSettings["FilesPath"];

            var path = string.Format("{0}/{1}/{2}/", webPath, res.fileName, res.id);
            CreateFolder(path);

            var extension = Path.GetExtension(res.tip);
            var fileName = GenerateFileName(path, res.fileName, extension);
            var imagesPath = Path.Combine(path, fileName);

            if (Directory.Exists(path))
            {
                var bytes = Convert.FromBase64String(res.file64);
                File.WriteAllBytes(imagesPath, bytes);
            }

            path = path.Substring(path.IndexOf("Files", StringComparison.Ordinal));
            var sysfiles = new SYS_Files()
            {
                id = Guid.NewGuid(),
                created = DateTime.Now,
                createdby = userId,
                DataId = res.id,
                FilePath = "/" + path + fileName,
                DataTable = res.fileName,
                FileGroup = res.fileGroup,
                FileExtension = res.tip.Replace(".", "")
            };
            var rsFiles = db.InsertSYS_Files(sysfiles);

            if (rsFiles.result == false)
            {
                RenderResponse(context, new ResultStatus { result = false, message = rsFiles.message });
            }

            RenderResponse(context, new ResultStatus { result = true, message = "Dosya yükleme işlemi başarılı." });
        }

        private string GenerateFileName(string path, string fileName, string extension)
        {
            fileName = (fileName.Replace(extension, "")).Replace(" ", "-").Replace("/", "-").Replace(".", "-");
            if (!Directory.Exists(path + fileName))
            {
                var pathFileName = path + fileName + "___" + DateTime.Now.ToString("s").Replace("-", "_").Replace("T", "_").Replace(":", "_") + extension;
                var fl = fileName + "___" + DateTime.Now.ToString("s").Replace("-", "_").Replace("T", "_").Replace(":", "_") + extension;
                if (pathFileName.Length > 260)
                {
                    return DateTime.Now.ToString("s").Replace("-", "_").Replace("T", "_").Replace(":", "_") + "___" + extension;
                }
                return fl;
            }
            return fileName + extension;
        }

        private void CreateFolder(string path)
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }
            catch
            {
            }
        }
    }
}