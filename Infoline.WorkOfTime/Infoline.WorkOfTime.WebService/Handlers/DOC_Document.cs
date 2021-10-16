using Infoline.Framework.Database;
using Infoline.Web.SmartHandlers;
using Infoline.WorkOfTime.BusinessAccess;using Infoline.WorkOfTime.BusinessData;using System;
using System.ComponentModel.Composition;
using System.Web;

namespace Infoline.WorkOfTime.WebService
{
    [Export(typeof(ISmartHandler))]
    public partial class DOC_DocumentHandler : BaseSmartHandler
    {
        public DOC_DocumentHandler()
            : base("DOC_Document")
        {

        }

        /// <summary>
        /// DOC_Document tablosundaki kayıtları listelemek için kullanılan Web Servisdir..
        /// </summary>
        /// <param name="context">Opsiyonel olarak [Condition] nesnesi alır. Parametreleri ise;
        /// Filter = [ Field, Operator, Value ] dizi şeklinde alır.
        ///     Field =         Tablodaki kolon adı
        ///     Operator =
        ///         Equal               Eşittir
        ///         NotEqual            Eşit Değildir
        ///         GreaterThan         Büyüktür
        ///         GreaterThanOrEqual  Büyüktür veya Eşittir
        ///         LessThan            Küçüktür
        ///         LessThanOrEqual     Küçüktür veya Eşittir
        ///         In                  İçerir, Dizi formatında
        ///     Sort =          Field ve Type olarak 2 property alır
        ///         Field = Tablodaki kolon adı
        ///         Type = "ASC" ve "DESC" değerlerini alır
        ///     Fields =        Tablodan çekilecek alanlar belirtilir. Dizi şeklinde string olarak giriş yapılır. Örn : ["id","Name"]. Yazılmaz ise tüm kolonlar gelir.
        ///     StartIndex =    Kayıtların alınmaya başlanacağı sırayı belirtir. Örn 100 değeri girilir ise ilk 100 kayıt alınmaz. Kayıtlar 101. kayıttan itibaren gelmeye başlar.
        ///     Count =         Kayıtların toplamda en fazla kaç tane geleceğinin belirtildiği propertydir. Örn : 200 Denir ise en fazla 200 satır kayıt gelir.
        /// </param>
        [HandleFunction("DOC_Document/GetAll")]
        public void DOC_DocumentGetAll(HttpContext context)
        {
            try
            {
                var c = ParseRequest<Condition>(context);
                var cond = c != null ? CondtionToQuery.Convert(c) : new SimpleQuery();
                var db = new WorkOfTimeDatabase();
                var data = db.GetDOC_Document(cond);
                RenderResponse(context, data);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }

        /// <summary>
        /// DOC_Document tablosundan id ye göre tekil kayıt çekmek için kullanılan Web Servisdir..
        /// </summary>
        /// <param name="context">
        /// [id] parametresini alır. Guid.
        /// </param>
        [HandleFunction("DOC_Document/GetById")]
        public void DOC_DocumentGetById(HttpContext context)
        {
            try
            {
                var db = new WorkOfTimeDatabase();
                var id = context.Request["id"];
                var data = db.GetDOC_DocumentById(new Guid((string)id));
                RenderResponse(context, data);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }

        /// <summary>
        /// DOC_Document Tablosuna kayıt ekleme işlemi için kullanılan Web Servisdir..
        /// </summary>
        /// <param name="context">
        /// Parametre olarak DOC_Document nesnesinden alır. ( JSON formatında )
        /// </param>
        [HandleFunction("DOC_Document/Insert")]
        public void DOC_DocumentInsert(HttpContext context)
        {
            try
            {
            var db = new WorkOfTimeDatabase();
            var data = ParseRequest<DOC_Document>(context);
            var dbresult = db.InsertDOC_Document(data);


            RenderResponse(context, dbresult);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }

        /// <summary>
        /// DOC_Document Tablosunda kayıt güncellemek için kullanılan Web Servisdir.
        /// </summary>
        /// <param name="context">
        /// Parametre olarak DOC_Document nesnesinden alır. ( JSON formatında )
        /// </param>
        [HandleFunction("DOC_Document/Update")]
        public void DOC_DocumentUpdate(HttpContext context)
        {
            try
            {

                var db = new WorkOfTimeDatabase();
                var data = ParseRequest<DOC_Document>(context);
                var dbresult = db.UpdateDOC_Document(data);

                RenderResponse(context, dbresult);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }

        /// <summary>
        /// DOC_Document Tablosundan kayıt silmek için kullanılan servistir.
        /// </summary>
        /// <param name="context">
        /// tekil parametre olarak id ( Guid ) alanıda gönderilebilir.
        /// DOC_Document Nesnesi de gönderilebilir.
        /// </param>
        [HandleFunction("DOC_Document/Delete")]
        public void DOC_DocumentDelete(HttpContext context)
        {
            try
            {
                var dbresult = new ResultStatus();
                var db = new WorkOfTimeDatabase();
                var id = context.Request["id"];
                if (id != null)
                {
                    dbresult = db.DeleteDOC_Document(new Guid((string)id));
                }
                else
                {
                    var item = ParseRequest<DOC_Document>(context);
                    dbresult = db.DeleteDOC_Document(item);
                }


                RenderResponse(context, dbresult);

            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }

    }
}
