using GeoAPI.Geometries;
using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace Infoline.WorkOfTime.BusinessAccess
{
	public class OrderFormTemplateModels : VWCMP_Order
    {
        public VWCMP_InvoiceItem[] Items { get; set; }
        public VWCMP_Company Supplier { get; set; }
        public VWCMP_Company Customer { get; set; }
        public string LogoPath { get; set; }
        public int? typeJob { get; set; }
    }


    public class VMCMP_OrderModels : VWCMP_Order
    {
        private WorkOfTimeDatabase db { get; set; }
        private DbTransaction trans { get; set; }
        private string _siteURL { get; set; } = TenantConfig.Tenant.GetWebUrl();
        private string _tenantName { get; set; } = TenantConfig.Tenant.TenantName;
        public List<VWCMP_InvoiceAction> InvoiceActions { get; set; }
        public List<VWCMP_InvoiceItem> InvoiceItems { get; set; }
        public VWCMP_InvoiceTransform TransformFrom { get; set; }
        public VWCMP_InvoiceTransform[] TransformTo { get; set; }
        public VWPA_Transaction[] Transactions { get; set; }
        public VWPRD_Transaction[] PRD_Transactions { get; set; }
        public bool IsTransform { get; set; }
        public bool? IsCopy { get; set; }
        public IGeometry location { get; set; }
        public CMP_Invoice oldInvoice { get; set; }
        public static Guid _approvalRoleId { get; set; } = new Guid(SHRoles.SatisOnaylayici);
        public static Guid muhasebeYonetici { get; set; } = new Guid(SHRoles.OnMuhasebe);
        public Guid[] _approvalPersons = new Guid[0];

        public VMCMP_OrderModels Load(bool? isTransform)
        {
            db = db ?? new WorkOfTimeDatabase();
            var invoice = db.GetCMP_InvoiceById(this.id);
            var order = db.GetVWCMP_OrderById(this.id);

            if (invoice != null)
            {
                this.InvoiceItems = db.GetVWCMP_InvoiceItemByInvoiceId(this.id).OrderBy(a => a.itemOrder).ToList();
                this.InvoiceActions = db.GetVWCMP_InvoiceActionByInvoiceId(this.id).ToList();

                if (isTransform == true)
                {
                    this.B_EntityDataCopyForMaterial(invoice, true);
                    this.description = "";
                    this.status = (short)EnumCMP_OrderStatus.CevapBekleniyor;
                    foreach (var item in this.InvoiceItems)
                    {
                        item.id = Guid.NewGuid();
                        item.description = "";
                    }
                }
                else
                {
                    this.B_EntityDataCopyForMaterial(order, true);
                    this.status = this.status.HasValue ? this.status.Value : (short)EnumCMP_OrderStatus.CevapBekleniyor;
                    this.TransformFrom = db.GetVWCMP_InvoiceTransformByIsTransformedTo(this.id).FirstOrDefault();
                    this.TransformTo = db.GetVWCMP_InvoiceTransformByIsTransformedFrom(this.id);
                    this.Transactions = db.GetVWPA_TransactionByInvoiceId(this.id);
                    this.PRD_Transactions = db.GetVWPRD_TransactionByOrderId(this.id);
                }
            }
            else
            {
                if (this.presentationId.HasValue)
                {
                    var presentation = db.GetCRM_PresentationById(this.presentationId.Value);
                    var presentationProducts = db.GetCRM_PresentationProductsByPresentationId(this.presentationId.Value);
                    var ids = presentationProducts.Where(a => a.ProductId.HasValue).Select(a => a.ProductId.Value).ToArray();

                    this.InvoiceItems = db.GetPRD_ProductByIds(ids).Select(s => new VWCMP_InvoiceItem
                    {
                        productId = s.id,
                        invoiceId = this.id,
                        unitId = s.unitId,
                        quantity = presentationProducts.Where(a => a.ProductId == s.id).Select(a => a.Amount).FirstOrDefault(),
                    }).ToList();

                    this.customerId = presentation.CustomerCompanyId;
                }
            }

            this.IsTransform = isTransform.Value ? isTransform.Value : false;
            this.rowNumber = String.IsNullOrEmpty(this.rowNumber) ? BusinessExtensions.B_GetIdCode() : this.rowNumber;
            this.type = (int)EnumCMP_InvoiceType.Siparis;
            this.status = this.status.HasValue ? this.status.Value : (short)EnumCMP_OrderStatus.CevapBekleniyor;
            this.direction = (short)EnumCMP_InvoiceDirectionType.Satis;

            return this;
        }

        public ResultStatus Save(Guid? userId, HttpRequestBase request = null, DbTransaction trans = null)
        {
            db = db ?? new WorkOfTimeDatabase();
            this.trans = trans ?? db.BeginTransaction();
            var rs = new ResultStatus { result = true };
            var order = db.GetVWCMP_OrderById(this.id);

            if (this.rateExchange == null) { return new ResultStatus { result = false, message = "Lütfen güncel kur giriniz." }; }
            if (this.InvoiceItems == null) { return new ResultStatus { result = false, message = "Sipariş oluşturabilmek için en az bir ürün girişi yapmalısınız." }; }
            if (this.InvoiceItems.Count() == 0) { return new ResultStatus { result = false, message = "Hiç ürün eklemeden sipariş oluşturamazsınız. Lütfen ürün ekleyiniz." }; }
            var productControl = InvoiceItems.Where(a => a.productId == null).ToArray();
            if (productControl.Count() > 0) { return new ResultStatus { result = false, message = "Ürün seçmek zorunludur, lütfen ürün seçmeyecek olduğunuz satırı silin ya da bir ürün seçiniz." }; }
            if (this.paymentType == (int)EnumCMP_InvoicePaymentType.Vadeli && this.expiryDate == null) { return new ResultStatus { result = false, message = "Vadeli sipariş için vade tarihi alanı zorunludur. Lütfen kontrol ediniz." }; }
            if (this.paymentType == (int)EnumCMP_InvoicePaymentType.Taksitli && this.installmentCount == null) { return new ResultStatus { result = false, message = "Taksitli sipariş için taksit sayısı seçimi zorunludur. Lütfen kontrol ediniz." }; }

            this.expiryDate = this.paymentType != (int)EnumCMP_InvoicePaymentType.Vadeli ? null : this.expiryDate;
            this.installmentCount = this.paymentType != (int)EnumCMP_InvoicePaymentType.Taksitli ? null : this.installmentCount;

            this.discount = this.discount != null ? this.discount : 0;
            this.stopaj = this.stopaj != null ? this.stopaj : 0;
            this.tevkifat = this.tevkifat != null ? this.tevkifat : 0;

            this._approvalPersons = db.GetSH_UserByRoleId(_approvalRoleId);

            this.oldInvoice = db.GetCMP_InvoiceById(this.id);

            if (order == null || this.IsCopy == true)
            {
                this.created = DateTime.Now;
                this.createdby = userId;

                this.id = Guid.NewGuid();
                //burda eğer null ise id setleniyordu.Kontrol gerek
                //if (this.id == null)
                //{
                //}

                rs = this.Insert(this.trans);
            }
            else
            {
                this.changed = DateTime.Now;
                this.changedby = userId;
                rs = this.Update(this.trans);
            }

            if (rs.result && request != null)
            {
                new FileUploadSave(request, this.id).SaveAs();
            }

            if (trans == null)
            {
                if (rs.result)
                    this.trans.Commit();
                else
                    this.trans.Rollback();
            }

            return rs;
        }

        private ResultStatus Insert(DbTransaction trans = null)
        {
            this.trans = trans ?? db.BeginTransaction();

            var action = new CMP_InvoiceAction
            {
                created = DateTime.Now,
                createdby = this.createdby,
                invoiceId = this.id,
                type = (int)EnumCMP_InvoiceActionType.YeniSiparis,
                description = "Sipariş oluşturuldu."
            };

            foreach (var invoice in this.InvoiceItems)
            {
                invoice.id = Guid.NewGuid();
                invoice.invoiceId = this.id;
                invoice.created = DateTime.Now;
                invoice.createdby = this.createdby;
                invoice.price = invoice.price != null ? invoice.price : 0;
                invoice.quantity = invoice.quantity != null ? invoice.quantity : 0;
                invoice.OTV = invoice.OTV != null ? invoice.OTV : 0;
                invoice.OIV = invoice.OIV != null ? invoice.OIV : 0;
                invoice.KDV = invoice.KDV != null ? invoice.KDV : 0;
                invoice.discount = invoice.discount != null ? invoice.discount : 0;
            }

            var dbresult = db.InsertCMP_Invoice(new CMP_Invoice().B_EntityDataCopyForMaterial(this), this.trans);
            dbresult &= db.InsertCMP_InvoiceAction(action, this.trans);
            dbresult &= db.BulkInsertCMP_InvoiceItem(this.InvoiceItems.Select(a => new CMP_InvoiceItem().B_EntityDataCopyForMaterial(a)), this.trans);

            if (this.oldInvoice != null)
            {
                var transform = new CMP_InvoiceTransform
                {
                    created = DateTime.Now,
                    createdby = this.createdby,
                    invoiceIdFrom = this.oldInvoice.id,
                    invoiceIdTo = this.id
                };

                var newAction = new CMP_InvoiceAction
                {
                    id = Guid.NewGuid(),
                    created = DateTime.Now,
                    createdby = this.createdby,
                    invoiceId = this.oldInvoice.id,
                    transformInvoiceId = this.id,
                };

                newAction.description = "Teklifin siparişi oluşturuldu.";
                newAction.type = (int)EnumCMP_InvoiceActionType.TeklifSiparis;

                dbresult &= db.InsertCMP_InvoiceAction(newAction, this.trans);
                dbresult &= db.InsertCMP_InvoiceTransform(transform, this.trans);

                var tenderView = db.GetVWCMP_TenderById(this.oldInvoice.id, this.trans);

                if (tenderView != null)
                {
                    new VMCMP_TenderModels { id = this.oldInvoice.id }.Load(false, this.direction).UpdateStatus((int)EnumCMP_TenderStatus.TeklifSiparis, this.createdby.Value, false,this.trans);

                    if (tenderView.presentationId.HasValue)
                    {
                        var presInvoice = new CRM_PresentationInvoice
                        {
                            presentationId = tenderView.presentationId.Value,
                            invoiceId = this.id,
                            type = (int)EnumCRM_PresentationInvoiceType.Siparis,
                            created = DateTime.Now,
                            createdby = this.createdby
                        };

                        var presAction = new CRM_PresentationAction
                        {
                            created = DateTime.Now,
                            createdby = this.createdby,
                            description = "Potansiyele yeni sipariş eklendi.",
                            presentationId = tenderView.presentationId.Value,
                            type = (int)EnumCRM_PresentationActionType.YeniSipariş,
                            location = this.location
                        };

                        dbresult &= db.InsertCRM_PresentationInvoice(presInvoice, this.trans);
                        dbresult &= db.InsertCRM_PresentationAction(presAction, this.trans);
                    }
                }
            }

            if (this.direction == (int)EnumCMP_InvoiceDirectionType.Satis)
            {
                var users = new List<VWSH_User>();
                if (_approvalPersons.Count() > 0)
                {
                    users = db.GetVWSH_UserByIds(_approvalPersons).ToList();
                }
                foreach (var user in users)
                {
                    var text = "<h3>Sayın " + user.FullName + "</h3>";
                    text += "<p>" + this.rowNumber + " kodlu sipariş onayınıza sunulmuştur. </p>";
                    text += "<p>Sipariş detayını görüntülemek ve işlem yapmak için <a href='" + _siteURL + "/CMP/VWCMP_Order/DetailSelling?id=" + this.id + "'>tıklayınız.</a> </p>";
                    text += "<p>Bilgilerinize.</p>";
                    new Email().Template("Template1", "satinalma.jpg", _tenantName + " | WorkOfTime | Satış Sipariş Yönetimi", text).Send((Int16)EmailSendTypes.YeniSiparis, user.email, "Sipariş Oluşturuldu", true);
                }

                if (this.presentationId.HasValue)
                {
                    var presInvoice = new CRM_PresentationInvoice
                    {
                        presentationId = this.presentationId.Value,
                        invoiceId = this.id,
                        type = (int)EnumCRM_PresentationInvoiceType.Siparis,
                        created = DateTime.Now,
                        createdby = this.createdby
                    };

                    var presAction = new CRM_PresentationAction
                    {
                        created = DateTime.Now,
                        createdby = this.createdby,
                        description = "Potansiyele yeni sipariş eklendi.",
                        presentationId = this.presentationId.Value,
                        type = (int)EnumCRM_PresentationActionType.YeniSipariş,
                        location = this.location
                    };

                    dbresult &= db.InsertCRM_PresentationInvoice(presInvoice, this.trans);
                    dbresult &= db.InsertCRM_PresentationAction(presAction, this.trans);
                }
            }

            if (trans == null)
            {
                if (dbresult.result)
                    this.trans.Commit();
                else
                    this.trans.Rollback();
            }

            return dbresult;
        }

        private ResultStatus Update(DbTransaction trans = null)
        {
            this.trans = trans ?? db.BeginTransaction();
            var oldItems = db.GetCMP_InvoiceItemByInvoiceId(this.id);

            foreach (var invoice in this.InvoiceItems)
            {
                invoice.id = Guid.NewGuid();
                invoice.invoiceId = this.id;
                invoice.created = DateTime.Now;
                invoice.createdby = this.createdby;
                invoice.price = invoice.price != null ? invoice.price : 0;
                invoice.quantity = invoice.quantity != null ? invoice.quantity : 0;
                invoice.OTV = invoice.OTV != null ? invoice.OTV : 0;
                invoice.OIV = invoice.OIV != null ? invoice.OIV : 0;
                invoice.KDV = invoice.KDV != null ? invoice.KDV : 0;
                invoice.discount = invoice.discount != null ? invoice.discount : 0;
            }

            var dbresult = db.UpdateCMP_Invoice(new CMP_Invoice().B_EntityDataCopyForMaterial(this), false, this.trans);
            dbresult &= db.BulkDeleteCMP_InvoiceItem(oldItems, this.trans);
            dbresult &= db.BulkInsertCMP_InvoiceItem(this.InvoiceItems.Select(a => new CMP_InvoiceItem().B_EntityDataCopyForMaterial(a)), this.trans);

            if (trans == null)
            {
                if (dbresult.result)
                    this.trans.Commit();
                else
                    this.trans.Rollback();
            }

            return dbresult;
        }

        public ResultStatus Delete(DbTransaction trans = null)
        {
            db = db ?? new WorkOfTimeDatabase();
            this.trans = trans ?? db.BeginTransaction();

            var invoice = db.GetCMP_InvoiceById(this.id);

            if (invoice != null && invoice.status == (int)EnumCMP_OrderStatus.CevapBekleniyor)
            {
                var action = db.GetCMP_InvoiceActionByInvoiceId(this.id);
                var transformTo = db.GetCMP_InvoiceTransformByIsTransformedTo(this.id);
                var transformFrom = db.GetCMP_InvoiceTransformByIsTransformedFrom(this.id);
                var items = db.GetCMP_InvoiceItemByInvoiceId(this.id);
                var transactions = db.GetPA_TransactionByInvoiceId(this.id);
                var ledgers = db.GetPA_LedgerByTransactionIds(transactions.Select(a => a.id).ToArray());

                var dbresult = db.BulkDeletePA_Ledger(ledgers, this.trans);
                dbresult &= db.BulkDeletePA_Transaction(transactions, this.trans);
                dbresult &= db.BulkDeleteCMP_InvoiceTransform(transformFrom, this.trans);
                dbresult &= db.BulkDeleteCMP_InvoiceTransform(transformTo, this.trans);
                dbresult &= db.BulkDeleteCMP_InvoiceAction(action, this.trans);
                dbresult &= db.BulkDeleteCMP_InvoiceItem(items, this.trans);
                dbresult &= db.DeleteCMP_Invoice(invoice, this.trans);

                if (trans == null)
                {
                    if (dbresult.result)
                        this.trans.Commit();
                    else
                        this.trans.Rollback();
                }

                return dbresult;
            }

            return new ResultStatus { result = false, message = "Sadece onay bekleyen siparişler silinebilmektedir." };
        }

        public ResultStatus UpdateStatus(int type, Guid userId, DbTransaction trans = null)
        {
            this.trans = trans ?? db.BeginTransaction();

            this.changed = DateTime.Now;
            this.changedby = userId;
            this.status = (short)type;

            var action = new CMP_InvoiceAction
            {
                id = Guid.NewGuid(),
                created = DateTime.Now,
                createdby = this.changedby,
                invoiceId = this.id,
            };

            var user = db.GetVWSH_UserById(this.createdby.Value);

            var listMuhasebe = new List<Guid>();
            listMuhasebe.AddRange(db.GetSH_UserByRoleId(muhasebeYonetici));
            var users = new List<VWSH_User>();
            if (listMuhasebe.Count() > 0)
            {
                users = db.GetVWSH_UserByIds(listMuhasebe.ToArray()).ToList();
            }

            if (type == (int)EnumCMP_OrderStatus.Onay)
            {
                action.type = (int)EnumCMP_InvoiceActionType.SiparisOnay;
                action.description = "Sipariş, satış onaylayıcısı tarafından onaylandı.";

                var text = "<h3>Sayın " + user.FullName + "</h3>";
                text += "<p>" + this.rowNumber + " kodlu sipariş yönetici tarafından onaylanmıştır. </p>";
                text += "<p>Sipariş detayını görüntülemek ve irsaliyelerinin girilmesi için <a href='" + _siteURL + "/CMP/VWCMP_Order/DetailSelling?id=" + this.id + "'>tıklayınız.</a> </p>";
                text += "<p>Bilgilerinize.</p>";
                new Email().Template("Template1", "satinalma.jpg", _tenantName + " | WorkOfTime | Satış Sipariş Yönetimi", text).Send((Int16)EmailSendTypes.SiparisTeklif, user.email, "Sipariş Onay", true);

                foreach (var muhuser in users)
                {
                    var text2 = "<h3>Sayın " + muhuser.FullName + "</h3>";
                    text2 += "<p>" + this.rowNumber + " kodlu sipariş yönetici tarafından onaylanmıştır. </p>";
                    text2 += "<p>Sipariş detayını görüntülemek ve faturasını girmek için <a href='" + _siteURL + "/CMP/VWCMP_Order/DetailSelling?id=" + this.id + "'>tıklayınız.</a> </p>";
                    text2 += "<p>Bilgilerinize.</p>";
                    new Email().Template("Template1", "satinalma.jpg", _tenantName + " | WorkOfTime | Satış Sipariş Yönetimi", text2).Send((Int16)EmailSendTypes.SiparisTeklif, muhuser.email, "Sipariş Onay", true);
                }
            }

            else if (type == (int)EnumCMP_OrderStatus.Red)
            {
                action.type = (int)EnumCMP_InvoiceActionType.SiparisRed;
                action.description = "Sipariş, satış onaylayıcısı tarafından reddedildi.";

                var text = "<h3>Sayın " + user.FullName + "</h3>";
                text += "<p>" + this.rowNumber + " kodlu sipariş yönetici tarafından reddedilmiştir. </p>";
                text += "<p>Sipariş detayını görüntülemek için <a href='" + _siteURL + "/CMP/VWCMP_Order/DetailSelling?id=" + this.id + "'>tıklayınız.</a> </p>";
                text += "<p>Bilgilerinize.</p>";
                new Email().Template("Template1", "satinalma.jpg", _tenantName + " | WorkOfTime | Satış Sipariş Yönetimi", text).Send((Int16)EmailSendTypes.SiparisTeklif, user.email, "Sipariş Red", true);
            }
            else if (type == (int)EnumCMP_OrderStatus.IrsaliyeKesildi)
            {
                action.type = (int)EnumCMP_InvoiceActionType.Siparisİrsaliye;
                action.description = "Siparişe, irsaliye kesildi.";
            }

            else
            {
                action.type = (int)EnumCMP_InvoiceActionType.SiparisFatura;
                action.description = "Sipariş süreci tamamlandı.";
            }

            var dbresult = db.UpdateCMP_Invoice(new CMP_Invoice().B_EntityDataCopyForMaterial(this), false, this.trans);
            dbresult &= db.InsertCMP_InvoiceAction(action, this.trans);

            var rs = new ResultStatus
            {
                result = dbresult.result,
                objects = action,
            };

            if (trans == null)
            {
                if (dbresult.result)
                {
                    this.trans.Commit();
                    rs.message = type == (int)EnumCMP_OrderStatus.Onay ? "Sipariş onaylama işlemi başarılı." : "Sipariş onaylama işlemi başarısız.";
                }
                else
                {
                    this.trans.Rollback();
                    rs.message = type == (int)EnumCMP_OrderStatus.Onay ? "Sipariş reddetme işlemi başarılı." : "Sipariş reddetme işlemi başarısız.";
                }
            }

            return rs;
        }

        public ResultStatus InsertNote(Guid userId, string note)
        {
            var action = new CMP_InvoiceAction
            {
                id = Guid.NewGuid(),
                created = DateTime.Now,
                createdby = userId,
                invoiceId = this.id,
                description = note,
                type = (short)EnumCMP_InvoiceActionType.NotEkle
            };

            var res = db.InsertCMP_InvoiceAction(action);
            res.objects = db.GetVWCMP_InvoiceActionById(action.id);
            return res;
        }

        public OrderFormTemplateModels GetFormTemplate(int? type)
        {
            var invoice = db.GetVWCMP_OrderById(this.id);

            var model = new OrderFormTemplateModels().B_EntityDataCopyForMaterial(invoice);

            if (type.HasValue)
            {
                model.typeJob = type.Value;
            }

            model.Items = db.GetVWCMP_InvoiceItemByInvoiceId(this.id);

            if (this.customerId.HasValue)
            {
                model.Customer = db.GetVWCMP_CompanyById(this.customerId.Value);
            }

            if (this.supplierId.HasValue)
            {
                model.Supplier = db.GetVWCMP_CompanyById(this.supplierId.Value);
                var file = db.GetSYS_FilesByDataId(this.supplierId.Value);

                if (file != null)
                {
                    model.LogoPath = file.FilePath;
                }
            }

            return model;
        }

        public static SimpleQuery UpdateQuery(SimpleQuery query, PageSecurity userStatus)
        {
            BEXP filter = null;
            var authorizedRoles = userStatus.AuthorizedRoles;
            var satisPersonel = new Guid(SHRoles.SatisPersoneli);
            var satisOnaylayici = new Guid(SHRoles.SatisOnaylayici);
            var muhasebeYonetici = new Guid(SHRoles.MuhasebeSatis);

            if (authorizedRoles.Contains(muhasebeYonetici) && !authorizedRoles.Contains(satisPersonel) && !authorizedRoles.Contains(satisOnaylayici))
            {
                filter = new BEXP
                {
                    Operand1 = new BEXP
                    {
                        Operand1 = new BEXP
                        {
                            Operand1 = (COL)"status",
                            Operator = BinaryOperator.Equal,
                            Operand2 = (VAL)(int)EnumCMP_OrderStatus.Onay
                        },
                        Operand2 = new BEXP
                        {
                            Operand1 = (COL)"direction",
                            Operator = BinaryOperator.Equal,
                            Operand2 = (VAL)(int)EnumCMP_InvoiceDirectionType.Satis
                        },
                        Operator = BinaryOperator.And
                    }
                };
            }
            else
            {
                filter |= new BEXP
                {
                    Operand1 = (COL)"direction",
                    Operator = BinaryOperator.Equal,
                    Operand2 = (VAL)(int)EnumCMP_InvoiceDirectionType.Satis
                };
            }

            query.Filter &= filter;

            return query;

        }

        public static SimpleQuery UpdateQueryAllOrder(SimpleQuery query, PageSecurity userStatus)
        {
            var db = new WorkOfTimeDatabase();
            var companyIds = db.GetVWCMP_StorageBySupervisorIdOfCompanyIds(userStatus.user.id).ToList();
            companyIds.Add(Guid.NewGuid());
            BEXP filter = null;


            filter = new BEXP
            {
                Operand1 = new BEXP
                {
                    Operand1 = (COL)"createdby",
                    Operator = BinaryOperator.Equal,
                    Operand2 = (VAL)(userStatus.user.id)
                },

                Operator = BinaryOperator.Or,
                Operand2 = new BEXP
                {
                    Operand1 = (COL)"customerId",
                    Operator = BinaryOperator.In,
                    Operand2 = new ARR { Values = companyIds.Select(a => (VAL)a).ToArray() }
                }
            };

            filter &= new BEXP
            {
                Operand1 = (COL)"direction",
                Operator = BinaryOperator.Equal,
                Operand2 = (VAL)(int)EnumCMP_InvoiceDirectionType.Satis
            };

            query.Filter &= filter;

            return query;

        }


        public static SimpleQuery UpdateQueryCustomerDropDown(SimpleQuery query, PageSecurity userStatus)
        {
            BEXP filter = null;
            var db = new WorkOfTimeDatabase();
            if(TenantConfig.Tenant.TenantCode == 1135 && TenantConfig.Tenant.TenantCode == 1195 && userStatus.user.CompanyId.HasValue)
            {
                return query;
            }

            var companyIds = db.GetVWCMP_StorageBySupervisorIdOfCompanyIds(userStatus.user.id).ToList();
            companyIds.Add(Guid.NewGuid());
            filter |= new BEXP
            {
                Operand1 = (COL)"id",
                Operator = BinaryOperator.In,
                Operand2 = new ARR { Values = companyIds.Select(a => (VAL)a).ToArray() }
            };

            query.Filter &= filter;

            return query;
        }

        public static SimpleQuery UpdateQueryStorageDropDown(SimpleQuery query, PageSecurity userStatus)
        {
            BEXP filter = null;
            if(TenantConfig.Tenant.TenantCode == 1135 && TenantConfig.Tenant.TenantCode == 1195)
            {
                return query;
            }

            filter |= new BEXP
            {
                Operand1 = (COL)"supervisorId",
                Operator = BinaryOperator.Equal,
                Operand2 = (VAL)(userStatus.user.id)
            };

            query.Filter &= filter;

            return query;
        }


        public SummaryHeadersOrder GetOrderSummary()
        {
            this.Load(false);
            return db.GetVWCMP_OrderByCounts();
        }

    }
}
