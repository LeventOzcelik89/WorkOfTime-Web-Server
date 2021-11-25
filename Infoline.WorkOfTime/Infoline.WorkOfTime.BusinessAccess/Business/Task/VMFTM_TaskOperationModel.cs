using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace Infoline.WorkOfTime.BusinessAccess
{

    public class VMFTM_TaskOperationModel : VWFTM_TaskOperation
    {
        private WorkOfTimeDatabase db { get; set; }
        private DbTransaction trans { get; set; }
        public VWFTM_Task Task { get; set; }
        public VWFTM_TaskForm[] TaskForm { get; set; }
        public Guid?[] userIds { get; set; }
        public string verifyCode { get; set; }
        public string Logo { get; set; }
        public PRD_StockTaskPlan taskPlan { get; set; } = new PRD_StockTaskPlan();
		public VWCMP_Request Request { get; set; }
        public VWCMP_InvoiceItemReport[] CMP_InvoiceItemReports { get; set; }

        public VMFTM_TaskOperationModel Load()
        {
            this.db = new WorkOfTimeDatabase();
            var operation = db.GetVWFTM_TaskOperationById(this.id);
            if (operation != null)
            {
                this.B_EntityDataCopyForMaterial(operation);
            }
            if (this.taskId.HasValue)
            {
                this.Task = db.GetVWFTM_TaskById(this.taskId.Value);

				if (this.Task != null)
				{
                    this.Request = db.GetVWCMP_RequestByTaskId(this.taskId.Value);
                    this.CMP_InvoiceItemReports = db.GetVWCMP_InvoiceItemReportByTaskId(this.taskId.Value);

                }
            }
            if (this.formId == null)
            {
                if (this.fixtureId == null && this.Task != null)
                {
                    this.fixtureId = this.Task.fixtureId;

                }

                if (this.fixtureId.HasValue && (this.status == (int)EnumFTM_TaskOperationStatus.GorevFormuDoldurulduAltUrun || this.status == (int)EnumFTM_TaskOperationStatus.GorevFormuDoldurulduAnaUrun))
                {
                    var rel = db.GetVWFTM_TaskFormRelationByInventoryId(this.fixtureId);
                    this.formId = rel.Select(a => a.formId).FirstOrDefault();
                }

                if (this.Task != null)
                {
                    if (this.Task.customerStorageId.HasValue)
                    {
                        var form = db.GetVWFTM_TaskFormRelationByStorageId(this.Task.customerStorageId.Value);
                        if (form != null)
                        {
                            this.formId = form.Select(x => x.formId).FirstOrDefault();
                        }
                        else
                        {
                            if (this.Task.companyId.HasValue)
                            {
                                form = db.GetVWFTM_TaskFormRelationByCompanyId(this.Task.companyId.Value);
                                if (form != null)
                                {
                                    this.formId = form.Select(x => x.formId).FirstOrDefault();
                                }
                            }
                        }
                    }
                }
            }

            if (this.fixtureId.HasValue)
            {
                this.fixture_Title = db.GetVWPRD_InventoryById(this.fixtureId.Value)?.fullName;
            }

            return this;
        }


        public ResultStatus Insert(Guid? userId)
        {
            if (this.taskId == null) return new ResultStatus { result = false, message = "Görev seçili değil." };
            this.db = this.db ?? new WorkOfTimeDatabase();
            this.trans = this.db.BeginTransaction();
            this.Task = db.GetVWFTM_TaskById(this.taskId.Value);
            this.created = DateTime.Now.AddSeconds(5);
            this.createdby = userId;
            if (this.Task == null) return new ResultStatus { result = false, message = "Görev kaydı silinmiş." };
            if (this.status == null) return new ResultStatus { result = false, message = "Operasyon durumu boş gönderilemez." };

            var taskOperations = db.GetFTM_TaskOperationByTaskId(this.taskId.Value);
            var taskUsers = db.GetFTM_TaskUserByTaskId(this.taskId.Value);
            var rs = new ResultStatus { result = true };
            var status = (EnumFTM_TaskOperationStatus)this.status;
            var message = "İşlem başarıyla gerçekleşti.";
            var notification = new Notification();
            switch (status)
            {
                case EnumFTM_TaskOperationStatus.GorevOlusturuldu:
                case EnumFTM_TaskOperationStatus.GorevOlusturulduMusteri:
                case EnumFTM_TaskOperationStatus.GorevOlusturulduSistem:
                    break;
                case EnumFTM_TaskOperationStatus.PersonelAtamaYapildi:
                    break;
                case EnumFTM_TaskOperationStatus.DogrulamaKoduGonderildi:

                    if (userIds == null) return new ResultStatus { result = false, message = "Doğrulama kodu göndermek için kullanıcı seçmelisiniz." };
                    var users = db.GetVWSH_UserByIds(userIds.Where(a => a.HasValue).Select(a => a.Value).ToArray());
                    if (users.Count() == 0) return new ResultStatus { result = false, message = "Doğrulama kodu göndermek için kullanıcı seçmelisiniz." };
                    foreach (var user in users)
                    {
                        var verifyCode = VMFTM_TaskModel.RandomString(6);
                        var taskUser = taskUsers.Where(a => a.userId == user.id).FirstOrDefault();
                        if (taskUser != null)
                        {
                            taskUser.changed = DateTime.Now;
                            taskUser.changedby = this.createdby;
                            taskUser.verifyCode = verifyCode;
                        }

                        var text = "<h3>Sayın " + user.FullName + "</h3>";
                        text += "<div>Tarafınıza <strong>" + this.Task.code + "</strong> kodlu görev oluşturulmuştur.</div>";
                        text += "<div>Doğrulama Kodu : <strong>" + verifyCode + "</strong></div>";
                        text += "<div>Görev detaylarını görüntülemek için <a href='" + TenantConfig.Tenant.GetWebUrl() + "/FTM/VWFTM_Task/Detail?id=" + this.taskId + "'>tıklayınız.</a> </div>";
                        text += "<div>Bilgilerinize.</div>";
                        var notify = "Sayın " + user.FullName + " tarafınıza "+ this.Task.code+ "kodlu görev oluşturulmuştur.";
                        new Email().Template("Template1", "gorevMailFoto.jpg", TenantConfig.Tenant.TenantName + " | Saha Görev Yönetimi", text).Send((Int16)EmailSendTypes.Operasyon, user.email, "Saha Görevi Doğrulama Kodu ", true);
                        notification.NotificationSend(user.id,"Saha Görev Yönetimi", notify);
                    }
                    this.description = string.Join(",", users.Select(a => a.FullName)) + " kullanıcılara doğrulama kodu gönderildi";
                    rs &= db.BulkUpdateFTM_TaskUser(taskUsers.Select(a => new FTM_TaskUser().B_EntityDataCopyForMaterial(a, true)), true, trans);
                    message = "Doğrulama kodu başarıyla gönderildi.";


                    break;
                case EnumFTM_TaskOperationStatus.GorevUstlenildi:

                    if (this.createdby == null)
                        return new ResultStatus { result = false, message = "Görevi üstlenecek kişi bilinmiyor." };

                    var muser = db.GetVWSH_UserById(this.createdby.Value);
                    if (muser == null)
                    {
                        return new ResultStatus { result = false, message = "Görevli personel bulunamadı." };
                    }

                    if (taskOperations.Where(a => a.status == (int)EnumFTM_TaskOperationStatus.GorevUstlenildi).Count() > 0)
                        return new ResultStatus { result = false, message = "Görev zaten biri tarafından üstlenilmiş durumda." };

                    var taskUserm = taskUsers.Where(a => a.userId == this.createdby).OrderByDescending(a => a.created).FirstOrDefault();

                    if (this.Task.hasVerifyCode.HasValue && this.Task.hasVerifyCode.Value)
                    {
                        if (taskUserm == null)
                        {
                            return new ResultStatus { result = false, message = "Doğrulama kodu bulunamadı." };
                        }

                        var dueDate = DateTime.Now - (taskUserm.changed ?? taskUserm.created);

                        if (dueDate > VMFTM_TaskModel.VerifyCodeDueDate)
                        {
                            return new ResultStatus { result = false, message = "Doğrulama kodu süresi dolmuş lütfen yeni bir doğrulama kodu talebinde bulununuz." };
                        }

                        if (taskUserm.verifyCode != this.verifyCode)
                        {
                            return new ResultStatus { result = false, message = "Doğrulama kodu hatalı lütfen kontrol ediniz." };
                        }
                    }

                    taskUserm.status = true;
                    rs &= db.UpdateFTM_TaskUser(taskUserm, true, trans);
                    this.description = muser.FullName + " görevi üstlendi.";

                    message = "Görev başarıyla üstlenildi.";
                    break;
                case EnumFTM_TaskOperationStatus.GorevBaslandi:
                    message = "Göreve başlandı.";
                    break;
                case EnumFTM_TaskOperationStatus.GorevIslemYapiliyorAnaUrun:
                case EnumFTM_TaskOperationStatus.GorevIslemYapiliyorAltUrun:
                    this.status = (int)(this.Task.fixtureId == this.fixtureId ? EnumFTM_TaskOperationStatus.GorevIslemYapiliyorAnaUrun : EnumFTM_TaskOperationStatus.GorevIslemYapiliyorAltUrun);
                    message = "Envanter işlem bildirimi başarılı.";
                    break;
                case EnumFTM_TaskOperationStatus.GorevFormuDoldurulduAnaUrun:
                case EnumFTM_TaskOperationStatus.GorevFormuDoldurulduAltUrun:
                    this.status = (int)(this.Task.fixtureId == this.fixtureId ? EnumFTM_TaskOperationStatus.GorevFormuDoldurulduAnaUrun : EnumFTM_TaskOperationStatus.GorevFormuDoldurulduAltUrun);

                    if (string.IsNullOrEmpty(this.jsonResult))
                    {
                        return new ResultStatus { result = false, message = "Lütfen formu doldurunuz." };
                    }

                    if (this.formId == null)
                    {
                        return new ResultStatus { result = false, message = "Form seçmelisiniz." };
                    }

                    var formresult = new FTM_TaskFormResult
                    {
                        createdby = this.createdby,
                        created = DateTime.Now,
                        jsonResult = this.jsonResult,
                        formId = this.formId,
                        taskId = this.taskId,
                        taskOperationId = this.id
                    };

                    rs &= db.InsertFTM_TaskFormResult(formresult);

                    var form = db.GetFTM_TaskFormById(formId.Value);
                    if (form != null)
                    {
                        this.description = form.name + " operasyon formu dolduruldu.";
                    }

                    message = "Görev operasyon formu başarıyla dolduruldu.";
                    break;

                case EnumFTM_TaskOperationStatus.UrunDegisimi:
                    if (this.taskPlan != null)
                    {
                        if (this.Task.customerStorageId.HasValue && this.taskPlan.newInventoryId.HasValue && this.fixtureId.HasValue && this.taskId.HasValue)
                        {
                            var storage = db.GetCMP_StorageById(this.Task.customerStorageId.Value);
                            var plan = db.GetVWPRD_StockTaskPlanByTaskId(this.taskId.Value);
                            if (storage != null)
                            {
                                var cmp = db.GetCMP_CompanyByOwner().FirstOrDefault();
                                if (cmp != null & plan != null)
                                {
                                    var brand = "";
                                    var storageInput = db.GetCMP_StorageByCompanyId(cmp.id).FirstOrDefault();
                                    var outputinventory = db.GetVWPRD_InventoryById(this.fixtureId.Value);
                                    var inputinventory = db.GetVWPRD_InventoryById(this.taskPlan.newInventoryId.Value);
                                    if (outputinventory != null && inputinventory != null)
                                    {

                                        var product = db.GetVWPRD_ProductById(outputinventory.productId.Value);

                                        if (product != null)
                                        {
                                            brand = product.brandId_Title;
                                        }

                                        rs &= new VMPRD_TransactionModel
                                        {
                                            outputId = storage.id,
                                            outputCompanyId = storage.companyId,
                                            inputCompanyId = cmp.id,
                                            inputId = storageInput.id,
                                            inputTable = "CMP_Storage",
                                            outputTable = "CMP_Storage",
                                            type = (Int16)EnumPRD_TransactionType.Transfer,
                                            status = (Int16)EnumPRD_TransactionStatus.islendi,
                                            items = new List<VMPRD_TransactionItems>
                                            {
                                                 new VMPRD_TransactionItems
                                                 {
                                                     productId = outputinventory.productId,
                                                     serialCodes = outputinventory.serialcode,
                                                     quantity = 1
                                                 }
                                            }
                                        }.Save(userId, trans);


                                        rs &= new VMPRD_TransactionModel
                                        {
                                            outputId = inputinventory.lastActionDataId,
                                            outputCompanyId = inputinventory.lastActionDataCompanyId,
                                            inputCompanyId = plan.companyId,
                                            inputId = plan.storageId,
                                            inputTable = "CMP_Storage",
                                            outputTable = "CMP_Storage",
                                            type = (Int16)EnumPRD_TransactionType.Transfer,
                                            status = (Int16)EnumPRD_TransactionStatus.islendi,
                                            items = new List<VMPRD_TransactionItems>
                                            {
                                                new VMPRD_TransactionItems
                                                {
                                                    productId = inputinventory.productId,
                                                    serialCodes = inputinventory.serialcode,
                                                    quantity = 1
                                                }
                                            }
                                        }.Save(userId, trans);

                                        plan.changed = DateTime.Now;
                                        plan.changedby = userId;
                                        plan.serialNumber = inputinventory.serialcode;

                                        if (this.Task.lastOperationStatus == (int)EnumFTM_TaskOperationStatus.CozumBildirildi)
                                        {
                                            plan.changed = this.Task.lastOperationDate;
                                        }

                                        plan.inventoryId = this.Task.fixtureId;
                                        plan.inventorySerialNumber = outputinventory.serialcode;
                                        plan.inventoryIndexValue = this.taskPlan.inventoryIndexValue;
                                        plan.inventoryStampYear = this.taskPlan.inventoryStampYear;

                                        plan.newInventoryId = inputinventory.id;
                                        plan.newInventoryBrand = brand;
                                        plan.newInventoryIndexValue = this.taskPlan.newInventoryIndexValue;
                                        plan.newInventoryStampYear = this.taskPlan.newInventoryStampYear;
                                        rs &= db.UpdatePRD_StockTaskPlan(new PRD_StockTaskPlan().B_EntityDataCopyForMaterial(plan), true, trans);
                                    }
                                }
                            }
                        }
                    }
                    break;
                case EnumFTM_TaskOperationStatus.GorevDosyaIslemiAnaUrun:
                case EnumFTM_TaskOperationStatus.GorevDosyaIslemiAltUrun:

                    this.status = (int)(this.Task.fixtureId == this.fixtureId ? EnumFTM_TaskOperationStatus.GorevDosyaIslemiAnaUrun : EnumFTM_TaskOperationStatus.GorevDosyaIslemiAltUrun);
                    message = "Görev envanter resimleri başarıyla yüklendi.";
                    break;
                case EnumFTM_TaskOperationStatus.CozumBildirildi:
                    var managerUserIds = db.GetSH_UserByRoleId(new Guid(SHRoles.SahaGorevYonetici));
                    var managers = new System.Collections.Generic.List<VWSH_User>();
                    if (managerUserIds.Count() > 0)
                    {
                        managers = db.GetVWSH_UserByIds(managerUserIds).ToList();
                    }

                    if (this.taskId.HasValue)
                    {
                        var followUpUsers = db.GetFTM_TaskFollowUpUserByTaskId(this.taskId.Value);
                        if(followUpUsers.Count() > 0)
                        {
                            var fllwusers = db.GetVWSH_UserByIds(followUpUsers.Where(x => x.userId.HasValue).Select(x => x.userId.Value).ToArray());
                            if(fllwusers.Count() > 0)
                            {
                                managers.AddRange(fllwusers);
                            }
                        }
                    }
                    foreach (var usrm in managers)
                    {
                        var notfy = "Sayın Yetkili " + usrm.FullName+ " "+ this.Task.code+ " kodlu görev için çözüm bildirildi.";
                        var text = "<h3>Sayın Yetkili " + usrm.FullName + "</h3>";
                        text += "<div><strong>" + this.Task.code + "</strong> kodlu görev için çözüm bildirildi. </div>";
                        text += "<p>Görev Tipi : <strong>" + this.Task.type_Title + "</strong></p>";
                        text += "<p>Görevi Oluşturan : <strong>" + this.Task.createdby_Title + "</strong></p>";
                        text += "<p>Görevli İşletmem : <strong>" + this.Task.company_Title + "</strong></p>";
                        text += "<p>Müşteri İşletme : <strong>" + this.Task.customer_Title + "</strong></p>";
                        text += "<p>Görevi Üstlenen : <strong>" + this.Task.assignUser_Title + "</strong></p>";
                        text += "<p>Öncelik Durumu : <strong>" + this.Task.priority_Title + "</strong></p>";
                        if (!string.IsNullOrEmpty(this.Task.product_Title))
                        {
                            text += "<p>Envanter : <strong>" + this.Task.product_Title + "</strong></p>";
                        }
                        text += "<div>Görev detaylarını görüntülemek ve çözümü onaylamak için <a href='" + TenantConfig.Tenant.GetWebUrl() + "/FTM/VWFTM_Task/Detail?id=" + this.taskId + "'>tıklayınız.</a> </div>";
                        text += "<div>Bilgilerinize.</div>";
                        new Email().Template("Template1", "gorevMailFoto.jpg", TenantConfig.Tenant.TenantName + " | Saha Görev Yönetimi", text).Send((Int16)EmailSendTypes.Cozum, usrm.email, "Çözüm Bildirimi", true);
                        notification.NotificationSend(usrm.id,"Saha Görev Yönetimi",notfy);
                    }
                    message = "Çözüm başarılı bir şekilde bildirildi.";

                    break;
                case EnumFTM_TaskOperationStatus.CozumOnaylandi:
                case EnumFTM_TaskOperationStatus.CozumReddedildi:

                    var myuser = db.GetVWSH_UserById(this.Task.assignUserId.Value);
                    if (myuser != null)
                    {
                        var notify = "Sayın " + myuser.FullName + " " + this.Task.code + " kodlu görev için bildirdiğiniz çözüm "+ (status == EnumFTM_TaskOperationStatus.CozumOnaylandi ? "onaylandı" : "reddedildi");
                        var text = "<h3>Sayın " + myuser.FullName + "</h3>";
                        text += "<div><strong>" + this.Task.code + "</strong> kodlu görev için bildirdiğiniz çözüm <strong>" + (status == EnumFTM_TaskOperationStatus.CozumOnaylandi ? "onaylandı" : "reddedildi") + "</strong>. </div>";
                        text += "<div>Onay/Red Açıklaması : " + this.description + " </div>";
                        text += "<div>Görev detaylarını görüntülemek için <a href='" + TenantConfig.Tenant.GetWebUrl() + "/FTM/VWFTM_Task/Detail?id=" + this.taskId + "'>tıklayınız.</a> </div>";
                        text += "<div>Bilgilerinize.</div>";
                        new Email().Template("Template1", "gorevMailFoto.jpg", TenantConfig.Tenant.TenantName + " | Saha Görev Yönetimi", text).Send((Int16)EmailSendTypes.Operasyon, myuser.email, status == EnumFTM_TaskOperationStatus.CozumOnaylandi ? "Çözüm Onay Bilgilendirmesi" : "Çözüm Red Bilgilendirmesi ", true);
                        notification.NotificationSend(myuser.id,"Saha Görev Yönetimi",notify);
                    }
                    message = "Çözüm onay/red işlemi başarılı bir şekilde gerçekleşti.";

                    if (Task.createdby.HasValue)
                    {
                        var customerUser = db.GetVWSH_UserById(Task.createdby.Value);
                        if (customerUser != null && customerUser.type == (Int32)EnumSH_UserType.OtherPerson && status == EnumFTM_TaskOperationStatus.CozumOnaylandi)
                        {
                            var notify="Sayın " + customerUser.FullName + "(" + this.Task.code + ") " + this.Task.product_Title + " ürün için talep ettiğiniz arıza/bakım kaydı çözümlenmiştir.";
                            var msj = "<h3>Sayın " + customerUser.FullName + "</h3>";
                            msj += "<div><strong>" + "(" + this.Task.code + ") " + this.Task.product_Title + "</strong> ürün için talep ettiğiniz arıza/bakım kaydı çözümlenmiştir. </div>";
                            msj += "<div>Bilgilerinize.</div>";
                            new Email().Template("Template1", "bos.png", TenantConfig.Tenant.TenantName + " | Saha Görev Yönetimi", msj).Send((Int16)EmailSendTypes.Cozum, customerUser.email, "Çözüm Onay Bilgilendirmesi", true);
                            notification.NotificationSend(customerUser.id,"Saha Görev Yönetimi", notify);
                        }
                    }
                    //Todo Oğuz => Müşteriye mail  Taskı oluşturan diğer işletme personeliyse mail yollanacak

                    if (status == EnumFTM_TaskOperationStatus.CozumOnaylandi)
                    {
                        if (this.taskId.HasValue)
                        {
                            var plan = db.GetVWPRD_StockTaskPlanByTaskId(this.taskId.Value);
                            if (plan != null)
                            {
                                plan.status = (Int16)EnumPRD_StockTaskPlanStatus.Bitti;
                                rs &= db.UpdatePRD_StockTaskPlan(new PRD_StockTaskPlan().B_EntityDataCopyForMaterial(plan), true, trans);
                            }
                        }
                    }
                    break;
                case EnumFTM_TaskOperationStatus.IslakImzaliFormYuklendi:

					break;
				case EnumFTM_TaskOperationStatus.MemnuniyetAnketiYuklendi:

					if (string.IsNullOrEmpty(this.jsonResult))
					{
						return new ResultStatus { result = false, message = "Lütfen formu doldurunuz." };
					}

					if (this.formId == null)
					{
						return new ResultStatus { result = false, message = "Form seçmelisiniz." };
					}

					var anketFormResult = new FTM_TaskFormResult
					{
						createdby = this.createdby,
						created = DateTime.Now,
						jsonResult = this.jsonResult,
						formId = this.formId,
						taskId = this.taskId,
						taskOperationId = this.id
					};

					rs &= db.InsertFTM_TaskFormResult(anketFormResult);

					var anketForm = db.GetFTM_TaskFormById(formId.Value);
					if (anketForm != null)
					{
						this.description = anketForm.name + " formu dolduruldu.";
					}

					message = "Memnuniyet anketi formu başarıyla dolduruldu.";
					break;
				default:
					break;
			}

            rs &= db.InsertFTM_TaskOperation(new FTM_TaskOperation().B_EntityDataCopyForMaterial(this, true));

            if (rs.result)
            {
                rs.message = message;
                trans.Commit();
            }
            else
            {
                rs.message = "İşlem başarısız.";
                trans.Rollback();
            }
            rs.objects = new { id = this.id };
            return rs;
        }


        public ResultStatus Update(Guid? userId)
        {

            this.db = new WorkOfTimeDatabase();
            this.trans = this.db.BeginTransaction();
            this.changedby = userId;
            this.changed = DateTime.Now;
            var rs = db.UpdateFTM_TaskOperation(new FTM_TaskOperation().B_EntityDataCopyForMaterial(this, true), false, trans);

            if (this.formResultId.HasValue && !String.IsNullOrEmpty(jsonResult) && (this.status == (int)EnumFTM_TaskOperationStatus.GorevFormuDoldurulduAltUrun || this.status == (int)EnumFTM_TaskOperationStatus.GorevFormuDoldurulduAnaUrun))
            {
                var formresult = db.GetFTM_TaskFormResultById(this.formResultId.Value);
                if (formresult != null)
                {
                    formresult.jsonResult = jsonResult;
                    rs &= db.UpdateFTM_TaskFormResult(formresult, false, trans);
                }
            }

            if (rs.result)
            {
                rs.message = "Güncelleme işlemi başarılı.";
                trans.Commit();
            }
            else
            {
                rs.message = "Güncelleme işlemi başarısız.";
                trans.Rollback();
            }
            return rs;

        }

    }

}
