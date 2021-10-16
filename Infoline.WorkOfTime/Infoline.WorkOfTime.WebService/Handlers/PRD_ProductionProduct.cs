using Infoline.Framework.Database;
using Infoline.Web.SmartHandlers;
using Infoline.WorkOfTime.BusinessAccess;using Infoline.WorkOfTime.BusinessData;using System;
using System.ComponentModel.Composition;
using System.Web;

namespace Infoline.WorkOfTime.WebService
{
    [Export(typeof(ISmartHandler))]
    public partial class PRD_ProductionProductHandler : BaseSmartHandler
    {
        public PRD_ProductionProductHandler()
            : base("PRD_ProductionProduct")
        {

        }

        /// <summary>
        /// PRD_ProductionProduct tablosundaki kayıtları listelemek için kullanılan Web Servisdir..
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
        [HandleFunction("PRD_ProductionProduct/GetAll")]
        public void PRD_ProductionProductGetAll(HttpContext context)
        {
            try
            {
                var c = ParseRequest<Condition>(context);
                var cond = c != null ? CondtionToQuery.Convert(c) : new SimpleQuery();
                var db = new WorkOfTimeDatabase();
                var data = db.GetPRD_ProductionProduct(cond);
                RenderResponse(context, data);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }

        /// <summary>
        /// PRD_ProductionProduct tablosundan id ye göre tekil kayıt çekmek için kullanılan Web Servisdir..
        /// </summary>
        /// <param name="context">
        /// [id] parametresini alır. Guid.
        /// </param>
        [HandleFunction("PRD_ProductionProduct/GetById")]
        public void PRD_ProductionProductGetById(HttpContext context)
        {
            try
            {
                var db = new WorkOfTimeDatabase();
                var id = context.Request["id"];
                var data = db.GetPRD_ProductionProductById(new Guid((string)id));
                RenderResponse(context, data);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }

        /// <summary>
        /// PRD_ProductionProduct Tablosuna kayıt ekleme işlemi için kullanılan Web Servisdir..
        /// </summary>
        /// <param name="context">
        /// Parametre olarak PRD_ProductionProduct nesnesinden alır. ( JSON formatında )
        /// </param>
        [HandleFunction("PRD_ProductionProduct/Insert")]
        public void PRD_ProductionProductInsert(HttpContext context)
        {
            try
            {
            var db = new WorkOfTimeDatabase();
            var data = ParseRequest<PRD_ProductionProduct>(context);
            var dbresult = db.InsertPRD_ProductionProduct(data);


            RenderResponse(context, dbresult);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }

        /// <summary>
        /// PRD_ProductionProduct Tablosunda kayıt güncellemek için kullanılan Web Servisdir.
        /// </summary>
        /// <param name="context">
        /// Parametre olarak PRD_ProductionProduct nesnesinden alır. ( JSON formatında )
        /// </param>
        [HandleFunction("PRD_ProductionProduct/Update")]
        public void PRD_ProductionProductUpdate(HttpContext context)
        {
            try
            {

                var db = new WorkOfTimeDatabase();
                var data = ParseRequest<PRD_ProductionProduct>(context);
                var dbresult = db.UpdatePRD_ProductionProduct(data);

                RenderResponse(context, dbresult);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }

        /// <summary>
        /// PRD_ProductionProduct Tablosundan kayıt silmek için kullanılan servistir.
        /// </summary>
        /// <param name="context">
        /// tekil parametre olarak id ( Guid ) alanıda gönderilebilir.
        /// PRD_ProductionProduct Nesnesi de gönderilebilir.
        /// </param>
        [HandleFunction("PRD_ProductionProduct/Delete")]
        public void PRD_ProductionProductDelete(HttpContext context)
        {
            try
            {
                var dbresult = new ResultStatus();
                var db = new WorkOfTimeDatabase();
                var id = context.Request["id"];
                if (id != null)
                {
                    dbresult = db.DeletePRD_ProductionProduct(new Guid((string)id));
                }
                else
                {
                    var item = ParseRequest<PRD_ProductionProduct>(context);
                    dbresult = db.DeletePRD_ProductionProduct(item);
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
