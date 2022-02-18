using Infoline.WorkOfTime.BusinessAccess;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Infoline.WorkOfTime.WebProject.Controllers
{

    [AllowEveryone]
    public class FilesController : Controller
    {
        public ActionResult Index(Guid DataId, string DataTable, bool Filter = true, string FileGroup = "")
        {
            var provider = new LocalFileProvider(DataTable);
            var fileBase = provider.GETFileBaseByDataId(DataId);
            if (FileGroup != "")
            {
                fileBase = fileBase.Where(c => c.fileGroup == FileGroup).ToArray();
            }
            return View(new SYS_FilesFiltre
            {
                DataId = DataId,
                DataTable = DataTable,
                FileBase = fileBase,
                Filter = Filter
            });
        }

        public ActionResult Preview(Guid DataId, string DataTable, bool Filter = true)
        {
            var provider = new LocalFileProvider(DataTable);
            var fileBase = provider.GETFileBaseByDataId(DataId);
            return View(new SYS_FilesFiltre
            {
                DataId = DataId,
                DataTable = DataTable,
                FileBase = fileBase,
                Filter = Filter
            });
        }

        public FileResult PreviewFile(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var file = db.GetSYS_FilesById(id);
            var path = file.FilePath.Replace("/", "\\");
            var basem = System.Configuration.ConfigurationManager.AppSettings.Get("FilesPath");
#if DEBUG
            basem = Server.MapPath("~/Files");
#endif
            var link = basem.TrimEnd("\\Files".ToCharArray());
            var fullPath = link+path;
            var fileExist = System.IO.File.Exists(fullPath);


            byte[] byteFile = System.IO.File.ReadAllBytes(fullPath);
            var name = Path.GetFileName(fullPath);
            var mime = MimeMapping.GetMimeMapping(name);

            return File(byteFile, mime, name);
            
        }

        public ActionResult PreviewTable(string DataTable, Guid[] DataIds, bool Filter = true)
        {

            var provider = new LocalFileProvider(DataTable);
            var fileBase = provider.GETFileBaseInDataId(DataIds);
            return View(new SYS_FilesFiltre
            {
                DataId = DataIds.FirstOrDefault(),
                DataTable = DataTable,
                FileBase = fileBase,
                Filter = Filter
            });
        }

        [HttpPost]
        public ActionResult FileImport(Guid DataId, string DataTable, string FileGroup, HttpPostedFileBase file)
        {
            var locaFileProvider = new LocalFileProvider(DataTable);
            var rs = locaFileProvider.Import(DataId, FileGroup, file);
            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(Guid id, string DataTable)
        {
            var feedback = new FeedBack();
            var dbresult = new LocalFileProvider(DataTable).Delete(id);
            var result = new ResultStatusUI
            {
                Result = dbresult.Result,
                FeedBack = dbresult.Result ? feedback.Success("Dosya silme işlemi başarıyla gerçekleşti..") : feedback.Error("Dosya silme işlemi başarısız.."),
                Object = id
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Send(string url, string email, string subject, string message, string returnUrl, bool hasTemplate = true, bool attach = false)
        {
            var feedback = new FeedBack();
            var userStatus = (PageSecurity)Session["userStatus"];
            var files = new List<string>();
            if (url.IndexOf("CMP/VWCMP_Request/Print") > -1)
            {
                var id = url.Split('=');
                if(id.Count() == 2)
                {
                    var file = new WorkOfTimeDatabase().GetSYS_FilesByDataId(new Guid(id[1]));
                    if(file != null)
                    {
                        var path = System.Configuration.ConfigurationManager.AppSettings["FilesPath"] + file.FilePath.Replace("/Files", "");
                        files.Add(path);
                    }
          
                }
            }

            var text = "<h3>Merhaba;</h3>";
            if (userStatus != null)
            {
                text += "<p>" + userStatus.user.FullName + " isimli kullanıcı sizinle bir adet dosya paylaştı.</p>";
            }
            text += "<p>" + message + "</p>";
            if (attach == false)
            {
                text += "<a href='" + url + "' target=\"_blank\">Paylaşılan dosya'yı indirmek için tıklayınız.</a>";
            }
            text += "<p>Bu e-postada adı geçen kişi siz değilseniz, lütfen aşağıdaki iletişim bilgilerinden bizimle iletişime geçiniz.Lütfen size gelen e-postaları Spam(Junk) mail olarak işaretlemeyiniz.Bu mailde paylaşılan dosya, link vb.içeriklerin tüm hakları saklıdır. İzinsiz şekilde üçüncü kişilerle paylaşımı yasaktır. </p>";
            text += "<p>Bilgilerinize.</p>";

            try
            {
             
                if (attach) { files.Add(url); }

                if (hasTemplate)
                {
                    new Email().Template("Template1", "101.jpg", subject, text).Send((Int16)EmailSendTypes.Dosya, email, subject, false, null,null, files.ToArray(),false);
                }
                else
                {
                    new Email().Send((Int16)EmailSendTypes.Dosya, email, subject, text, true, false, null,null, files.ToArray(),false);
                }

                return Json(new ResultStatusUI
                {
                    FeedBack = feedback.Success("Mail gönderim işlemi başarılı.", false, returnUrl),
                    Result = true
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new ResultStatusUI
                {
                    FeedBack = feedback.Warning("Mail gönderim işlemi başarısız.", false, null),
                    Result = false
                }, JsonRequestBehavior.AllowGet);
            }
        }
        [AllowEveryone]
        public ActionResult AppDownload()
        {
            return View();
        }
    }
}
public class SYS_FilesFiltre
{
    public Guid DataId { get; set; }
    public string DataTable { get; set; }
    public string FileGroup { get; set; }
    public bool Filter { get; set; }
    public FileBase[] FileBase { get; set; }

}
public class SYS_FilesFilter
{
    public Guid[] DataIds { get; set; }
    public string DataTable { get; set; }
}