using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using Infoline.WorkOfTime.WebProject;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace System.Web.Mvc
{
    public class FileUploads : FactoryBase
    {
        public IDictionary<string, object> HtmlAttributes { get; set; }
        private int fileUploadCount;
        public bool _PreviewMode { get; set; } = false;
        public bool _Validate { get; set; } = false;
        public string _DataKey { get; set; }
        public Guid? _DataId { get; set; }
        public string _DataTable { get; set; }
        public VWSYS_Files[] _Values { get; set; }
        public bool _AutoSend { get; set; } = false;
        public VWSYS_Files[] SYS_Files()
        {
            var locaFileProvider = new LocalFileProvider(this._DataTable);
            return locaFileProvider.GETSYS_FilesByDataIdAndFileGroup(this._DataId.Value, this._DataKey);
        }
        private string _FileExtension()
        {
            var c = LocalFileProvider._dataTableFileGroup.Where(x => x.Key == this._DataTable).Select(x => x.Value).FirstOrDefault();
            if (c != null)
            {
                var r = c.Where(x => x.fileGroup == this._DataKey).FirstOrDefault();
                if (r != null)
                {
                    fileUploadCount = r.count;
                    return r.fileExtensions;
                }
            }
            return String.Concat((object)string.Empty);
        }
        public string Render()
        {
            var sb = new StringBuilder();
            var fext = this._FileExtension().Replace(" *", "").Replace("*", "");
            var files = _Values ?? this.SYS_Files();
            var userStatus = (PageSecurity)HttpContext.Current.Session["userStatus"];
            var hasPreview = userStatus.FilesRole.Count(c => c.dataTable == this._DataTable && c.fileGroup == this._DataKey && c.preview == true) > 0;
            var hasDelete = userStatus.FilesRole.Count(c => c.dataTable == this._DataTable && c.fileGroup == this._DataKey && c.delete == true) > 0 && this._PreviewMode == false;
            var hasInsert = userStatus.FilesRole.Count(c => c.dataTable == this._DataTable && c.fileGroup == this._DataKey && c.insert == true) > 0 && this._PreviewMode == false;

            sb.AppendLine($"<div class=\"fileupload-container\" data-required=\"{_Validate}\" data-extension=\"{fext}\" data-count=\"{fileUploadCount}\" data-insert=\"{hasInsert}\"  data-autosend=\"{this._AutoSend}\" data-id=\"{this._DataId}\" data-table=\"{this._DataTable}\" data-group=\"{this._DataKey}\" data-user=\"{userStatus.user.FullName}\">");
            if (hasInsert)
            {
                sb.AppendLine($"<div class=\"drop-container\" style=\"display:none;\"> SÜRÜKLE BIRAK ILE DOSYA YÜKLEME YAPABILIRSINIZ.</div>");
                sb.AppendLine($"<input type=\"text\" class=\"form-control hide\" {(_Validate ? "required" : "")} {(files.Count() > 0 ? "value='dosya var'" : "value=''")} />");
                sb.AppendLine($"<button class=\"btn btn-info btn-block\" data-toggle=\"tooltip\" title=\"Yükleme Limitiniz {fileUploadCount} Adettir.Kabul edilen uzantılar ({fext})\"  data-task=\"upload\" type=\"button\"><i class=\"fa fa-upload\"></i> {this._DataKey} </button>");
            }

            sb.AppendLine($"<ul class=\"fileupload-content\">");
            if (!hasPreview)
            {
                sb.AppendLine($"<li class=\"file-info\"><strong>{this._DataKey}</strong> dosya(lar)ını görüntüleme yetkiniz bulunmamaktadır.</li>");
            }
            else
            {
                var display = files.Count() > 0 ? "none" : "block";
                sb.AppendLine($"<li class=\"file-info col-md-12\" style=\"display:{display}\" ><strong>{this._DataKey}</strong> dosya(lar)ı bulunmamaktadır.{(hasInsert ? "<br/> Dosya yüklemek için yükleme butonunu kullanabilir veya sürükle bırak ile yükleme yapabilirsiz." : "")}</li>");

                foreach (var item in files.OrderBy(a => a.created))
                {
                    try
                    {
                        var fileInfo = new FileInfo(HttpContext.Current.Server.MapPath(item.FilePath));
                        var ext = fileInfo.Extension.Split('.').LastOrDefault().ToLowerInvariant();

                        sb.AppendLine($"<li class=\"file-item col-md-12\" data-url=\"{item.FilePath}\" data-id=\"{item.id}\">");
                        sb.AppendLine($"<div class=\"clearfix file-container\">");
                        sb.AppendLine($"<div class=\"file-image\"><img src=\"/Content/SYS_Files/img/new_icons/{ext}.png\"></div>");
                        sb.AppendLine($"<div class=\"file-desc\">");
                        sb.AppendLine($"<div class=\"col-md-9\" title=\"{fileInfo.Name}\">{fileInfo.Name}</div>");
                        sb.AppendLine($"<div class=\"col-md-3\">");
                        sb.AppendLine($"<div class=\"file-created\">{string.Format("{0:dd.MM.yyyy HH:MM}", item.created)}</div>");
                        sb.AppendLine($"<div class=\"file-owner\">{item.createdby_Title}</div>");
                        sb.AppendLine($"</div>");
                        sb.AppendLine($"</div>");
                        sb.AppendLine($"<div class=\"file-button\">");
                        if (hasDelete)
                        {
                            sb.AppendLine($"<button type=\"button\" title=\"Dosya sil\" data-task=\"remove\"  class=\"btn btn-xs btn-danger\"><i class=\"fa fa-remove\"></i></button>");
                        }
                        sb.AppendLine($"<button type=\"button\" title=\"Dosya linkini kopyala\" data-task=\"copy\" class=\"btn btn-xs btn-info\"><i class=\"fa fa-copy\"></i></button>");
                        sb.AppendLine($"<button type=\"button\" title=\"Dosya önizleme\" data-task=\"download\" class=\"btn btn-xs btn-success\"><i class=\"icon-zoom-in\"></i></button>");
                        sb.AppendLine($"<button type=\"button\" title=\"Dosya paylaşım\" data-task=\"mail\" class=\"btn btn-xs btn-warning\"><i class=\"icon-mail-3\"></i></button>");
                        sb.AppendLine($"</div>");
                        sb.AppendLine($"</div>");
                        sb.AppendLine($"</li>");
                    }
                    catch
                    {
                    }

                }
            }

            sb.AppendLine($"</ul>");
            sb.AppendLine($"</div>");
            return sb.ToString();
        }
        public FileUploads(ViewContext viewContext, ViewDataDictionary viewData = null) : base(viewContext, viewData)
        {
            this.ViewData = ViewData;
            this.ViewContext = ViewContext;
            this._DataId = default(Guid?);
            this._DataTable = "";
            this.HtmlAttributes = new Dictionary<string, object>();

        }
        protected override void WriteHtml(HtmlTextWriter writer)
        {

        }

    }
  
}