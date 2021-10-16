using Infoline.Framework.Database;
using Infoline.Web.SmartHandlers;
using Infoline.WorkOfTime.BusinessAccess;using Infoline.WorkOfTime.BusinessData;using System;
using System.ComponentModel.Composition;
using System.Web;

namespace Infoline.WorkOfTime.WebService
{
    [Export(typeof(ISmartHandler))]
    public partial class DOC_DocumentFlowHandler : BaseSmartHandler
    {
        public DOC_DocumentFlowHandler()
            : base("DOC_DocumentFlow")
        {

        }

        /// <summary>
        /// DOC_DocumentFlow tablosundaki kayıtları listelemek için kullanılan Web Servisdir..
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
        [HandleFunction("DOC_DocumentFlow/GetAll")]
        public void DOC_DocumentFlowGetAll(HttpContext context)
        {
            try
            {
                var c = ParseRequest<Condition>(context);
                var cond = c != null ? CondtionToQuery.Convert(c) : new SimpleQuery();
                var db = new WorkOfTimeDatabase();
                var data = db.GetDOC_DocumentFlow(cond);
                RenderResponse(context, data);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }

        /// <summary>
        /// DOC_DocumentFlow tablosundan id ye göre tekil kayıt çekmek için kullanılan Web Servisdir..
        /// </summary>
        /// <param name="context">
        /// [id] parametresini alır. Guid.
        /// </param>
        [HandleFunction("DOC_DocumentFlow/GetById")]
        public void DOC_DocumentFlowGetById(HttpContext context)
        {
            try
            {
                var db = new WorkOfTimeDatabase();
                var id = context.Request["id"];
                var data = db.GetDOC_DocumentFlowById(new Guid((string)id));
                RenderResponse(context, data);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }

        /// <summary>
        /// DOC_DocumentFlow Tablosuna kayıt ekleme işlemi için kullanılan Web Servisdir..
        /// </summary>
        /// <param name="context">
        /// Parametre olarak DOC_DocumentFlow nesnesinden alır. ( JSON formatında )
        /// </param>
        [HandleFunction("DOC_DocumentFlow/Insert")]
        public void DOC_DocumentFlowInsert(HttpContext context)
        {
            try
            {
            var db = new WorkOfTimeDatabase();
            var data = ParseRequest<DOC_DocumentFlow>(context);
            var dbresult = db.InsertDOC_DocumentFlow(data);


            RenderResponse(context, dbresult);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }

        /// <summary>
        /// DOC_DocumentFlow Tablosunda kayıt güncellemek için kullanılan Web Servisdir.
        /// </summary>
        /// <param name="context">
        /// Parametre olarak DOC_DocumentFlow nesnesinden alır. ( JSON formatında )
        /// </param>
        [HandleFunction("DOC_DocumentFlow/Update")]
        public void DOC_DocumentFlowUpdate(HttpContext context)
        {
            try
            {

                var db = new WorkOfTimeDatabase();
                var data = ParseRequest<DOC_DocumentFlow>(context);
                var dbresult = db.UpdateDOC_DocumentFlow(data);

                RenderResponse(context, dbresult);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }

        /// <summary>
        /// DOC_DocumentFlow Tablosundan kayıt silmek için kullanılan servistir.
        /// </summary>
        /// <param name="context">
        /// tekil parametre olarak id ( Guid ) alanıda gönderilebilir.
        /// DOC_DocumentFlow Nesnesi de gönderilebilir.
        /// </param>
        [HandleFunction("DOC_DocumentFlow/Delete")]
        public void DOC_DocumentFlowDelete(HttpContext context)
        {
            try
            {
                var dbresult = new ResultStatus();
                var db = new WorkOfTimeDatabase();
                var id = context.Request["id"];
                if (id != null)
                {
                    dbresult = db.DeleteDOC_DocumentFlow(new Guid((string)id));
                }
                else
                {
                    var item = ParseRequest<DOC_DocumentFlow>(context);
                    dbresult = db.DeleteDOC_DocumentFlow(item);
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
