using GeoAPI.Geometries;
using Infoline.Framework.Database;
using Infoline.Web.SmartHandlers;
using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;
using static Infoline.WorkOfTime.BusinessAccess.VMFTM_TaskModel;

namespace Infoline.WorkOfTime.WebService.Handler
{
    [Export(typeof(ISmartHandler))]
    public partial class FTM_TaskHandler : BaseSmartHandler
    {
        public FTM_TaskHandler()
            : base("FTM_TaskHandler")
        {

        }

        [HandleFunction("FTM_Task/InsertFTM_TaskOperation")]
        public void InsertFTM_TaskOperation(HttpContext context)
        {
            try
            {
                if (CallContext.Current == null)
                {
                    RenderResponse(context, new ResultStatus { result = false, message = "Kullanıcı girişi yapmanız gerekmektedir." });
                    return;
                }
                var item = ParseRequest<VMFTM_TaskOperationModel>(context);
                if (item == null)
                {
                    RenderResponse(context, new ResultStatus { result = false, message = "Operasyon nesnesi boş gönderilemez." });
                    return;
                }

                item.Load();

                var res = item.Insert(CallContext.Current.UserId);

                RenderResponse(context, res);
                return;
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus { result = false, message = ex.Message.ToString() });
                return;
            }
        }

        [HandleFunction("FTM_Task/InsertFTM_Task")]
        public void InsertFTM_Task(HttpContext context)
        {
            try
            {
                var task = ParseRequest<VMFTM_TaskModel>(context);

                if (task == null)
                {
                    RenderResponse(context, new ResultStatus { result = false, message = "Görev nesnesi boş gönderilemez." });
                    return;
                }

                var result = task.InsertMy(CallContext.Current.UserId);

                RenderResponse(context, new ResultStatus { result = true, message = result.message });
                return;
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus { result = false, message = ex.Message.ToString() });
                return;
            }
        }


        [HandleFunction("FTM_Task/InsertFTM_TaskAll")]
        public void InsertFTM_TaskAll(HttpContext context)
        {
            try
            {
                var task = ParseRequest<VMFTM_TaskModel>(context);

                if (task == null)
                {
                    RenderResponse(context, new ResultStatus { result = false, message = "Görev nesnesi boş gönderilemez." });
                    return;
                }

                var result = task.InsertAll(CallContext.Current.UserId);

                RenderResponse(context, new ResultStatus { result = true, message = result.message });
                return;
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus { result = false, message = ex.Message.ToString() });
                return;
            }
        }


        [HandleFunction("FTM_Task/FTM_TaskUpdate")]
        public void FTM_TaskUpdate(HttpContext context)
        {
            try
            {
                var task = ParseRequest<VMFTM_TaskModel>(context);

                if (task == null)
                {
                    RenderResponse(context, new ResultStatus { result = false, message = "Görev nesnesi boş gönderilemez." });
                    return;
                }

                var result = task.Update(CallContext.Current.UserId);

                RenderResponse(context, new ResultStatus { result = true, message = result.message });
                return;
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus { result = false, message = ex.Message.ToString() });
                return;
            }
        }


        [HandleFunction("FTM_Task/FTM_TaskDelete")]
        public void FTM_TaskDelete(HttpContext context)
        {
            try
            {
                var task = ParseRequest<VMFTM_TaskModel>(context);

                if (task == null)
                {
                    RenderResponse(context, new ResultStatus { result = false, message = "Görev nesnesi boş gönderilemez." });
                    return;
                }

                var result = task.Delete(CallContext.Current.UserId);

                RenderResponse(context, new ResultStatus { result = true, message = result.message });
                return;
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus { result = false, message = ex.Message.ToString() });
                return;
            }
        }

        [HandleFunction("FTM_Task/InsertFTM_TaskCustomer")]
        public void InsertFTM_TaskCustomer(HttpContext context)
        {
            try
            {
                var task = ParseRequest<VMFTM_TaskModel>(context);

                if (task == null)
                {
                    RenderResponse(context, new ResultStatus { result = false, message = "Görev nesnesi boş gönderilemez." });
                    return;
                }
                task.planStartDate = DateTime.Now;
                var result = new VMFTM_TaskModel().B_EntityDataCopyForMaterial(task, true).InsertCustomer(CallContext.Current.UserId);

                RenderResponse(context, new ResultStatus { result = true, message = result.message });
                return;
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus { result = false, message = ex.Message.ToString() });
                return;
            }
        }

        [HandleFunction("FTM_Task/CreateTask")]
        public void CreateFTM_Task(HttpContext context)
        {
            try
            {
                var model = ParseRequest<FTM_TaskCustom>(context);

                if (model == null)
                {
                    RenderResponse(context, new ResultStatus { result = false, message = "Görev nesnesi boş gönderilemez." });
                    return;
                }

                if (string.IsNullOrEmpty(model.NameSurName))
                {
                    RenderResponse(context, new ResultStatus { result = false, message = "Lütfen isim soyisim giriniz." });
                    return;
                }

                if (string.IsNullOrEmpty(model.Phone))
                {
                    RenderResponse(context, new ResultStatus { result = false, message = "Lütfen telefon bilgisi giriniz." });
                    return;
                }

                if (string.IsNullOrEmpty(model.Adress))
                {
                    RenderResponse(context, new ResultStatus { result = false, message = "Lütfen adres bilgisi giriniz." });
                    return;
                }

                if (string.IsNullOrEmpty(model.Description))
                {
                    RenderResponse(context, new ResultStatus { result = false, message = "Lütfen açıkalama giriniz." });
                    return;
                }

                var db = new WorkOfTimeDatabase();

                var ftmTaskModel = new VMFTM_TaskServiceModel();
                var company = db.GetCMP_Company().Where(a => a.type == (int)EnumCMP_CompanyType.Benimisletmem && a.pid == null).Select(a => a.id).FirstOrDefault();
                var description = "Talep Eden : " + model.NameSurName + System.Environment.NewLine;
                description += "Telefon : " + model.Phone + System.Environment.NewLine;
                description += "Adres : " + model.Adress + System.Environment.NewLine;
                description += "Açıklama : " + model.Description + System.Environment.NewLine;

                var task = new VMFTM_TaskModel
                {
                    createdby = Guid.Empty,
                    created = DateTime.Now,
                    companyId = company,
                    location = model.Location,
                    lastOperationLocation = model.Location,
                    type = (int)EnumFTM_TaskType.GelAL,
                    description = description,
                    hasVerifyCode = false,
                    priority = (short)EnumFTM_TaskPriority.Orta,
                    files = model.Files
                };

                var result = task.InsertCustomer(Guid.Empty, null);
                if (result.result)
                {
                    RenderResponse(context, new ResultStatus { result = true, message = "Talebiniz başarıyla iletildi.En kısa sürede sizinle iletişime geçilecektir." });
                }
                else
                {
                    RenderResponse(context, new ResultStatus { result = false, message = "Talebiniz alınırken bir sorun oluştu.Lütfen daha sonra tekrar deneyiniz." });
                }
                return;
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus { result = false, message = ex.Message.ToString() });
                return;
            }
        }

        [HandleFunction("FTM_Task/MBMyTasksAbout")]
        public void MBMyTasksAbout(HttpContext context)
        {
            try
            {
                var userid = CallContext.Current.UserId;
                var res = new VMFTM_TaskServiceModel().GetMyTask(userid);
                RenderResponse(context, new ResultStatus { result = true, objects = res });
                return;
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus { result = false, message = ex.Message.ToString() });
                return;
            }
        }

        [HandleFunction("FTM_Task/GetFTM_TaskRead")]
        public void GetFTM_TaskRead(HttpContext context)
        {
            try
            {
                var qrCode = context.Request["QRCode"];
                var userid = CallContext.Current != null ? CallContext.Current.UserId : Guid.NewGuid();
                var res = new VMFTM_TaskServiceModel().GetFTM_TaskRead(qrCode, userid);
                RenderResponse(context, new ResultStatus { result = true, objects = res });
                return;
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus { result = false, message = ex.Message.ToString() });
                return;
            }
        }

        [HandleFunction("FTM_Task/GetFTM_TaskDetail")]
        public void GetFTM_TaskDetail(HttpContext context)
        {
            try
            {
                if (CallContext.Current == null)
                {
                    RenderResponse(context, new ResultStatus { result = false, message = "Kullanıcı girişi yapmanız gerekmektedir." });
                    return;
                }
                var taskId = Guid.Parse(context.Request["taskId"]);
                var res = new VMFTM_TaskServiceModel().Get(taskId);
                RenderResponse(context, new ResultStatus { result = true, objects = res });
                return;
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus { result = false, message = ex.Message.ToString() });
                return;
            }
        }

        [HandleFunction("FTM_Task/GetFTM_TaskProducts")]
        public void GetFTM_TaskProducts(HttpContext context)
        {
            try
            {
                if (CallContext.Current == null)
                {
                    RenderResponse(context, new ResultStatus { result = false, message = "Kullanıcı girişi yapmanız gerekmektedir." });
                    return;
                }
                var taskId = Guid.Parse(context.Request["taskId"]);
                var res = new VMFTM_TaskServiceModel(taskId).GetProducts();
                RenderResponse(context, new ResultStatus { result = true, objects = res });
                return;
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus { result = false, message = ex.Message.ToString() });
                return;
            }
        }

        [HandleFunction("FTM_Task/GetFTM_TaskProductsByQrCode")]
        public void GetFTM_TaskProductsByQrCode(HttpContext context)
        {
            try
            {
                var qrCode = context.Request["QRCode"];
                var userid = CallContext.Current.UserId;
                var db = new WorkOfTimeDatabase();
                var inventory = db.GetVWPRD_InventoryByCode(qrCode);

                if (inventory != null)
                {
                    RenderResponse(context, new ResultStatus
                    {
                        result = true,
                        objects = new INV_Product
                        {
                            id = inventory.id,
                            pid = null,
                            productId_Title = inventory.productId_Title,
                            productId_Image = inventory.productId_Image,
                            name = inventory.fullName,
                            qrCode = inventory.code,
                        }
                    });

                }
                else
                {
                    RenderResponse(context, new ResultStatus { result = false, message = "Qrkod ile eşleşen envanter bulunamadı." });
                }


                return;
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus { result = false, message = ex.Message.ToString() });
                return;
            }
        }

        [HandleFunction("FTM_Task/GetFTM_TaskProductsByCustomerId")]
        public void GetFTM_TaskProductsByCustomerId(HttpContext context)
        {
            try
            {
                var customerId = Guid.Parse(context.Request["customerId"]);
                var res = new VMFTM_TaskServiceModel(Guid.Empty).GetProductByCustomerId(customerId);
                RenderResponse(context, new ResultStatus { result = true, objects = res });
                return;
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus { result = false, message = ex.Message.ToString() });
                return;
            }
        }

        [HandleFunction("FTM_Task/GetFTM_TaskReadCustomer")]
        public void GetFTM_TaskReadCustomer(HttpContext context)
        {
            try
            {
                var qrCode = context.Request["QRCode"];
                var userid = CallContext.Current != null ? CallContext.Current.UserId : Guid.NewGuid();
                var res = new VMFTM_TaskServiceModel().GetFTM_TaskReadCustomer(qrCode, userid);
                RenderResponse(context, new ResultStatus { result = true, objects = res, message = res.Count() == 0 ? "Görev bulunamadı." : "" });
                return;
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus { result = false, message = ex.Message.ToString() });
                return;
            }
        }

        [HandleFunction("VWPRD_Inventory/GetById")]
        public void VWPRD_InventoryGetById(HttpContext context)
        {
            try
            {
                var db = new WorkOfTimeDatabase();
                var id = context.Request["id"];
                var data = db.GetVWPRD_InventoryById(new Guid((string)id));
                RenderResponse(context, new ResultStatus { result = true, objects = data });
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }

        [HandleFunction("VWINV_Fixture/GetAll")]
        public void VWINV_FixtureGetAll(HttpContext context)
        {
            try
            {
                var c = ParseRequest<Condition>(context);
                var cond = c != null ? CondtionToQuery.Convert(c) : new SimpleQuery();
                var db = new WorkOfTimeDatabase();
                var data = db.GetVWPRD_Inventory(cond).Select(a => new VWINV_Fixture
                {
                    id = a.id,
                    SerialNo = a.serialcode,
                    FixtureCode = a.code,
                    QRCode = a.code,
                    Name = a.productId_Title,
                    FullName = a.fullName,
                    Location = a.location
                });
                RenderResponse(context, data);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }

        [HandleFunction("FTM_Task/GetMyTasksSummary")]
        public void FTM_TaskMyCounts(HttpContext context)
        {
            try
            {
                var data = new VMFTM_TaskModel().GetTaskSummary(CallContext.Current.UserId);
                RenderResponse(context, data);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }

        [HandleFunction("Demo/Task")]
        public void InsertDemoTask(HttpContext context)
        {
            try
            {
                var description = context.Request["aciklama"];

                var task = new VMFTM_TaskModel
                {
                    id = Guid.NewGuid(),
                    notificationDate = DateTime.Now,
                    planStartDate = DateTime.Now,
                    dueDate = DateTime.Now.AddMinutes(30),
                    companyId = new Guid("101F88C2-DC52-4794-919B-F3B8207A68FE"),
                    type = 0,
                    fixtureId = new Guid("37DD1EA9-C2F0-475D-B3B1-A2B7CA7E2225"),
                    priority = 0,
                    customerId = new Guid("0E0B290F-432A-4128-8871-B8D54389AFBD"),
                    customerStorageId = new Guid("477F0663-C8B9-43F9-9BE5-ABE8C6F7E806"),
                    description = description,
                    companyCarId = new Guid("C86CB220-B078-4819-84FC-111D1AA416BF"),
                    planLater = 0,
                    assignableUsers = new System.Collections.Generic.List<Guid> { new Guid("CBFA6929-87D7-4B40-A8CB-205B40841A98") },
                    FTM_TaskSubjectTypeIds = new System.Collections.Generic.List<Guid> { new Guid("8914E5B6-D52E-5223-8B6D-5B9C8176DF85") }.ToArray()
                };

                if (task == null)
                {
                    RenderResponse(context, new ResultStatus { result = false, message = "Görev nesnesi boş gönderilemez." });
                    return;
                }

                var result = task.InsertAll(new Guid("00000000-0000-0000-0000-000000000000"));

                RenderResponse(context, new ResultStatus { result = true, message = result.message });
                return;
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus { result = false, message = ex.Message.ToString() });
                return;
            }
        }

    }
}