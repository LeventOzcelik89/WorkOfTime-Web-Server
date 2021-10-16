using Infoline.Framework.Database;
using Infoline.Web.SmartHandlers;
using Infoline.WorkOfTime.BusinessAccess;using Infoline.WorkOfTime.BusinessData;using System;
using System.ComponentModel.Composition;
using System.Web;

namespace Infoline.WorkOfTime.WebService
{
    [Export(typeof(ISmartHandler))]
    public partial class PRJ_ProjectTypeLevelHandler : BaseSmartHandler
    {
        public PRJ_ProjectTypeLevelHandler()
            : base("PRJ_ProjectTypeLevel")
        {

        }

        /// <summary>
        /// PRJ_ProjectTypeLevel tablosundaki kayıtları listelemek için kullanılan Web Servisdir..
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
        [HandleFunction("PRJ_ProjectTypeLevel/GetAll")]
        public void PRJ_ProjectTypeLevelGetAll(HttpContext context)
        {
            try
            {
                var c = ParseRequest<Condition>(context);
                var cond = c != null ? CondtionToQuery.Convert(c) : new SimpleQuery();
                var db = new WorkOfTimeDatabase();
                var data = db.GetPRJ_ProjectTypeLevel(cond);
                RenderResponse(context, data);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }

        /// <summary>
        /// PRJ_ProjectTypeLevel tablosundan id ye göre tekil kayıt çekmek için kullanılan Web Servisdir..
        /// </summary>
        /// <param name="context">
        /// [id] parametresini alır. Guid.
        /// </param>
        [HandleFunction("PRJ_ProjectTypeLevel/GetById")]
        public void PRJ_ProjectTypeLevelGetById(HttpContext context)
        {
            try
            {
                var db = new WorkOfTimeDatabase();
                var id = context.Request["id"];
                var data = db.GetPRJ_ProjectTypeLevelById(new Guid((string)id));
                RenderResponse(context, data);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }

        /// <summary>
        /// PRJ_ProjectTypeLevel Tablosuna kayıt ekleme işlemi için kullanılan Web Servisdir..
        /// </summary>
        /// <param name="context">
        /// Parametre olarak PRJ_ProjectTypeLevel nesnesinden alır. ( JSON formatında )
        /// </param>
        [HandleFunction("PRJ_ProjectTypeLevel/Insert")]
        public void PRJ_ProjectTypeLevelInsert(HttpContext context)
        {
            try
            {
            var db = new WorkOfTimeDatabase();
            var data = ParseRequest<PRJ_ProjectTypeLevel>(context);
            var dbresult = db.InsertPRJ_ProjectTypeLevel(data);


            RenderResponse(context, dbresult);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }

        /// <summary>
        /// PRJ_ProjectTypeLevel Tablosunda kayıt güncellemek için kullanılan Web Servisdir.
        /// </summary>
        /// <param name="context">
        /// Parametre olarak PRJ_ProjectTypeLevel nesnesinden alır. ( JSON formatında )
        /// </param>
        [HandleFunction("PRJ_ProjectTypeLevel/Update")]
        public void PRJ_ProjectTypeLevelUpdate(HttpContext context)
        {
            try
            {

                var db = new WorkOfTimeDatabase();
                var data = ParseRequest<PRJ_ProjectTypeLevel>(context);
                var dbresult = db.UpdatePRJ_ProjectTypeLevel(data);

                RenderResponse(context, dbresult);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }

        /// <summary>
        /// PRJ_ProjectTypeLevel Tablosundan kayıt silmek için kullanılan servistir.
        /// </summary>
        /// <param name="context">
        /// tekil parametre olarak id ( Guid ) alanıda gönderilebilir.
        /// PRJ_ProjectTypeLevel Nesnesi de gönderilebilir.
        /// </param>
        [HandleFunction("PRJ_ProjectTypeLevel/Delete")]
        public void PRJ_ProjectTypeLevelDelete(HttpContext context)
        {
            try
            {
                var dbresult = new ResultStatus();
                var db = new WorkOfTimeDatabase();
                var id = context.Request["id"];
                if (id != null)
                {
                    dbresult = db.DeletePRJ_ProjectTypeLevel(new Guid((string)id));
                }
                else
                {
                    var item = ParseRequest<PRJ_ProjectTypeLevel>(context);
                    dbresult = db.DeletePRJ_ProjectTypeLevel(item);
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
