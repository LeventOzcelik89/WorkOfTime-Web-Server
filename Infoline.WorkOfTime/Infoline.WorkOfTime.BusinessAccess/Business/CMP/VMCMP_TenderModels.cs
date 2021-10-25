using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace Infoline.WorkOfTime.BusinessAccess
{
    public class TenderFormTemplateModels : VWCMP_Tender
    {
        public VWCMP_InvoiceItem[] Items { get; set; }
        public VWCMP_Company Supplier { get; set; }
        public VWCMP_Company Customer { get; set; }
        public string LogoPath { get; set; }
        public int? typeJob { get; set; }
        public CMP_InvoiceDocumentTemplate CMP_InvoiceDocumentTemplate { get; set; }

    }

    public class SpecInvoiceItem : VWCMP_InvoiceItem
    {
        public double? unitPrice { get; set; }
        public string unitPrice_Title { get; set; }
    }

    public class VMCMP_TenderModels : VWCMP_Tender
    {
        private WorkOfTimeDatabase db { get; set; }
        private DbTransaction trans { get; set; }
        public CMP_InvoiceDocumentTemplate invoiceDocumentTemplate { get; set; } = new CMP_InvoiceDocumentTemplate();
        private string _siteURL { get; set; } = TenantConfig.Tenant.GetWebUrl();
        private string _tenantName { get; set; } = TenantConfig.Tenant.TenantName;
        public List<VWCMP_InvoiceAction> InvoiceActions { get; set; }
        public List<SpecInvoiceItem> InvoiceItems { get; set; }
        public VWCMP_InvoiceTransform TransformFrom { get; set; }
        public VWCMP_InvoiceTransform[] TransformTo { get; set; }
        public CMP_Invoice Request { get; set; }
        public VWCMP_Invoice InvoiceRequest { get; set; }
        public bool IsTransform { get; set; }
        public bool? IsCopy { get; set; }
        public static Guid muhasebeYonetici { get; set; } = new Guid(SHRoles.OnMuhasebe);
        public static Guid _approvalRoleId { get; set; } = new Guid(SHRoles.SatisOnaylayici);
        public Guid[] _approvalPersons = new Guid[0];



        public VMCMP_TenderModels Load(bool? isTransform, int? direction)
        {
            db = db ?? new WorkOfTimeDatabase();
            var invoice = db.GetCMP_InvoiceById(this.id);
            var tender = db.GetVWCMP_TenderById(this.id);

            if (direction.HasValue)
            {
                this.direction = (short)direction.Value;
            }

            if (invoice != null)
            {
                this.InvoiceActions = db.GetVWCMP_InvoiceActionByInvoiceId(this.id).ToList();
                this.InvoiceItems = db.GetVWCMP_InvoiceItemByInvoiceId(this.id).OrderBy(a => a.itemOrder).ToList().Select(x => new SpecInvoiceItem().B_EntityDataCopyForMaterial(x)).ToList();
                foreach (var specInvoiceItem in this.InvoiceItems)
                {
                    if (specInvoiceItem.productId.HasValue)
                    {
                        var product = db.GetVWPRD_ProductById(specInvoiceItem.productId.Value);
                        specInvoiceItem.unitPrice = product?.currentSellingPrice;
                        specInvoiceItem.unitPrice_Title = product?.currentSellingCurrencyId_Title;
                    }
                }


                if (invoice.invoiceDocumentTemplateId.HasValue)
                {
                    var invoiceDocumentTemplateData = db.GetCMP_InvoiceDocumentTemplateById(invoice.invoiceDocumentTemplateId.Value);

                    if (invoiceDocumentTemplateData != null)
                    {
                        invoiceDocumentTemplate = invoiceDocumentTemplateData;
                    }

                }

                if (isTransform == true)
                {
                    this.B_EntityDataCopyForMaterial(invoice, true);
                    this.description = "";
                    this.rowNumber = BusinessExtensions.B_GetIdCode();
                    foreach (var item in this.InvoiceItems)
                    {
                        item.id = Guid.NewGuid();
                        item.description = "";
                    }
                }
                else
                {
                    this.B_EntityDataCopyForMaterial(tender, true);
                    this.status = this.status.HasValue ? this.status.Value : (short)EnumCMP_TenderStatus.CevapBekleniyor;
                    this.TransformFrom = db.GetVWCMP_InvoiceTransformByIsTransformedTo(this.id).FirstOrDefault();
                    this.TransformTo = db.GetVWCMP_InvoiceTransformByIsTransformedFrom(this.id);
                }

                this.direction = invoice.direction;
            }

            if (this.presentationId.HasValue)
            {
                this.rowNumber = BusinessExtensions.B_GetIdCode();
                var presentation = db.GetCRM_PresentationById(this.presentationId.Value);
                var presentationProducts = db.GetCRM_PresentationProductsByPresentationId(this.presentationId.Value);
                var ids = presentationProducts.Where(a => a.ProductId.HasValue).Select(a => a.ProductId.Value).ToArray();

                if (ids.Count() > 0)
                {
                    this.InvoiceItems = db.GetPRD_ProductByIds(ids).Select(s => new SpecInvoiceItem
                    {
                        productId = s.id,
                        invoiceId = this.id,
                        unitId = s.unitId,
                        quantity = presentationProducts.Where(a => a.ProductId == s.id).Select(a => a.Amount).FirstOrDefault(),
                    }).ToList();
                }

                this.customerId = presentation.CustomerCompanyId;
            }

            this.IsTransform = isTransform.Value ? isTransform.Value : false;
            this.rowNumber = !String.IsNullOrEmpty(this.rowNumber) ? this.rowNumber : BusinessExtensions.B_GetIdCode();
            this.type = (int)EnumCMP_InvoiceType.Teklif;
            this.status = this.status.HasValue ? this.status.Value : (short)EnumCMP_TenderStatus.CevapBekleniyor;

            return this;
        }

        public ResultStatus Save(Guid? userId, HttpRequestBase request = null, DbTransaction _trans = null)
        {
            db = db ?? new WorkOfTimeDatabase();
            this.trans = _trans ?? db.BeginTransaction();
            var rs = new ResultStatus { result = true };
            var tender = db.GetVWCMP_TenderById(this.id);

            if (this.rateExchange == null) { return new ResultStatus { result = false, message = "Lütfen güncel kur giriniz." }; }
            if (this.InvoiceItems == null) { return new ResultStatus { result = false, message = "Teklif oluşturabilmek için en az bir ürün girişi yapmalısınız." }; }
            if (this.InvoiceItems.Count() == 0) { return new ResultStatus { result = false, message = "Hiç ürün eklemeden teklif oluşturamazsınız. Lütfen ürün ekleyiniz." }; }
            var productControl = InvoiceItems.Where(a => a.productId == null).ToArray();
            if (productControl.Count() > 0) { return new ResultStatus { result = false, message = "Ürün seçmek zorunludur, lütfen ürün seçmeyecek olduğunuz satırı silin ya da bir ürün seçiniz." }; }
            if (this.paymentType == (int)EnumCMP_InvoicePaymentType.Vadeli && this.expiryDate == null) { return new ResultStatus { result = false, message = "Vadeli fatura için vade tarihi alanı zorunludur. Lütfen kontrol ediniz." }; }
            if (this.paymentType == (int)EnumCMP_InvoicePaymentType.Taksitli && this.installmentCount == null) { return new ResultStatus { result = false, message = "Taksitli fatura için taksit sayısı seçimi zorunludur. Lütfen kontrol ediniz." }; }

            this.expiryDate = this.paymentType == (int)EnumCMP_InvoicePaymentType.Vadeli ? this.expiryDate : null;
            this.installmentCount = this.paymentType == (int)EnumCMP_InvoicePaymentType.Taksitli ? this.installmentCount : null;

            this.discount = this.discount != null ? this.discount : 0;
            this.stopaj = this.stopaj != null ? this.stopaj : 0;
            this.tevkifat = this.tevkifat != null ? this.tevkifat : 0;

            this._approvalPersons = db.GetSH_UserByRoleId(_approvalRoleId);

            if (this.IsTransform == true)
            {
                this.InvoiceRequest = db.GetVWCMP_InvoiceById(this.id);
                this.Request = db.GetCMP_InvoiceById(this.id);
            }



            if (tender == null || this.IsCopy == true)
            {
                this.created = DateTime.Now;
                this.createdby = userId;
                this.id = this.id != null ? this.id : Guid.NewGuid();
                this.status = (int)EnumCMP_TenderStatus.CevapBekleniyor;
                rs = this.Insert();
            }
            else
            {
                this.changed = DateTime.Now;
                this.changedby = userId;
                rs = this.Update();
            }

            if (rs.result && request != null)
            {
                new FileUploadSave(request, this.id).SaveAs();
            }

            if (_trans == null)
            {
                if (rs.result)
                    this.trans.Commit();
                else
                    this.trans.Rollback();
            }

            return rs;
        }

        private ResultStatus Insert()
        {
            var action = new CMP_InvoiceAction
            {
                id = Guid.NewGuid(),
                created = DateTime.Now,
                createdby = this.createdby,
                invoiceId = this.id,
                type = (int)EnumCMP_InvoiceActionType.YeniTeklif,
                description = "Teklif oluşturuldu."
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

            var projectInvoice = new PRJ_ProjectInvoice
            {
                invoiceId = this.id,
                id = Guid.NewGuid(),
                projectId = this.InvoiceRequest?.projectId,
                created = DateTime.Now,
                createdby = this.createdby
            };


            var dbresult = db.InsertCMP_Invoice(new CMP_Invoice().B_EntityDataCopyForMaterial(this), this.trans);
            dbresult &= db.InsertCMP_InvoiceAction(action, this.trans);
            dbresult &= db.BulkInsertCMP_InvoiceItem(this.InvoiceItems.Select(a => new CMP_InvoiceItem().B_EntityDataCopyForMaterial(a)), this.trans);
            dbresult &= db.InsertPRJ_ProjectInvoice(projectInvoice, this.trans);

            if (this.direction == (int)EnumCMP_InvoiceDirectionType.Satis)
            {
                var users = new List<VWSH_User>();
                if (this._approvalPersons.Count() > 0)
                {
                    users = db.GetVWSH_UserByIds(this._approvalPersons).ToList();
                }

                foreach (var user in users)
                {
                    var text = "<h3>Sayın " + user.FullName + "</h3>";
                    text += "<p>" + this.rowNumber + " kodlu teklif onayınıza sunulmuştur. </p>";
                    text += "<p>Teklif detayını görüntülemek ve işlem yapmak için <a href='" + _siteURL + "/CMP/VWCMP_Tender/DetailSelling?id=" + this.id + "'>tıklayınız.</a> </p>";
                    text += "<p>Bilgilerinize.</p>";
                    new Email().Template("Template1", "satinalma.jpg", _tenantName + " | WorkOfTime | Satış Sipariş Yönetimi", text).Send((Int16)EmailSendTypes.SiparisTeklif, user.email, "Teklif Oluşturuldu", true);
                }

                if (this.presentationId.HasValue)
                {
                    var presInvoice = new CRM_PresentationInvoice
                    {
                        presentationId = this.presentationId.Value,
                        invoiceId = this.id,
                        type = (int)EnumCRM_PresentationInvoiceType.Teklif,
                        created = DateTime.Now,
                        createdby = this.createdby
                    };

                    var presAction = new CRM_PresentationAction
                    {
                        created = DateTime.Now,
                        createdby = this.createdby,
                        description = "Potansiyele yeni teklif eklendi.",
                        presentationId = this.presentationId.Value,
                        type = (int)EnumCRM_PresentationActionType.YeniTeklif,
                    };

                    dbresult &= db.InsertCRM_PresentationInvoice(presInvoice, this.trans);
                    dbresult &= db.InsertCRM_PresentationAction(presAction, this.trans);
                }

            }

            else
            {
                if (this.Request != null)
                {
                    var transform = new CMP_InvoiceTransform
                    {
                        created = DateTime.Now,
                        createdby = this.createdby,
                        id = Guid.NewGuid(),
                        invoiceIdFrom = this.Request.id,
                        invoiceIdTo = this.id
                    };

                    var invAction = new CMP_InvoiceAction
                    {
                        id = Guid.NewGuid(),
                        created = DateTime.Now,
                        createdby = this.createdby,
                        invoiceId = this.Request.id,
                        transformInvoiceId = this.id,
                        type = (int)EnumCMP_InvoiceActionType.TalepTeklifEklendi,
                        description = "Talebe teklif eklendi"
                    };

                    dbresult &= db.InsertCMP_InvoiceTransform(transform, this.trans);
                    dbresult &= db.InsertCMP_InvoiceAction(invAction, this.trans);
                    //dbresult &= UpdateTenders(this.trans);
                }
            }

            return dbresult;
        }

        private ResultStatus Update()
        {
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

            return dbresult;
        }

        public ResultStatus Delete(DbTransaction _trans = null)
        {
            db = db ?? new WorkOfTimeDatabase();
            this.trans = _trans ?? db.BeginTransaction();

            var invoice = db.GetCMP_InvoiceById(this.id);
            var action = db.GetCMP_InvoiceActionByInvoiceId(this.id);
            var transformTo = db.GetCMP_InvoiceTransformByIsTransformedTo(this.id);
            var transformFrom = db.GetCMP_InvoiceTransformByIsTransformedFrom(this.id);
            var items = db.GetCMP_InvoiceItemByInvoiceId(this.id);

            var dbresult = db.BulkDeleteCMP_InvoiceTransform(transformFrom, this.trans);
            dbresult &= db.BulkDeleteCMP_InvoiceTransform(transformTo, this.trans);
            dbresult &= db.BulkDeleteCMP_InvoiceAction(action, this.trans);
            dbresult &= db.BulkDeleteCMP_InvoiceItem(items, this.trans);
            dbresult &= db.DeleteCMP_Invoice(invoice, this.trans);

            if (_trans == null)
            {
                if (dbresult.result)
                    this.trans.Commit();
                else
                    this.trans.Rollback();
            }

            return dbresult;
        }

        private ResultStatus UpdateOtherTenders(int type, DbTransaction trans = null)
        {
            var listAction = new List<CMP_InvoiceAction>();
            var listTender = new List<CMP_Invoice>();

            var _trans = trans ?? db.BeginTransaction();
            var transform = db.GetCMP_InvoiceTransformByIsTransformedTo(this.id).FirstOrDefault();

            if (transform != null && transform.invoiceIdFrom.HasValue)
            {
                new VMCMP_RequestModels { id = transform.invoiceIdFrom.Value }.Load(false).UpdateStatus((int)EnumCMP_RequestStatus.TeklifOnaylandi, this.changedby.Value);

                var otherTenderIds = db.GetVWCMP_InvoiceTransformByIsTransformedFrom(transform.invoiceIdFrom.Value).Where(a => a.invoiceIdTo != this.id).Select(a => a.invoiceIdTo.Value).ToArray();
                var otherTenders = new List<CMP_Invoice>();
                if (otherTenderIds.Count() > 0)
                {
                    otherTenders = db.GetCMP_InvoiceByIds(otherTenderIds).ToList();
                }

                foreach (var tender in otherTenders)
                {
                    listAction.Add(new CMP_InvoiceAction
                    {
                        created = DateTime.Now,
                        createdby = this.changedby.Value,
                        invoiceId = tender.id,
                        description = type == (int)EnumCMP_TenderStatus.YoneticiOnay ? "Teklif yönetici tarafından reddedildi." : "Teklif tekrar değerlendirilecek.",
                        type = type == (int)EnumCMP_TenderStatus.YoneticiOnay ? (short)EnumCMP_InvoiceActionType.TeklifRed : (short)EnumCMP_InvoiceActionType.YeniTeklif,
                    });

                    tender.changed = DateTime.Now;
                    tender.changedby = this.changedby.Value;
                    tender.status = type == (int)EnumCMP_TenderStatus.YoneticiOnay ? (short)EnumCMP_TenderStatus.Red : (short)EnumCMP_TenderStatus.CevapBekleniyor;

                    listTender.Add(tender);
                }
            }

            var dbresult = db.BulkInsertCMP_InvoiceAction(listAction.ToArray(), _trans);
            dbresult &= db.BulkUpdateCMP_Invoice(listTender.ToArray(), false, _trans);

            if (trans == null)
            {
                if (dbresult.result)
                    _trans.Commit();
                else
                    _trans.Rollback();
            }

            return dbresult;
        }

        public ResultStatus UpdateStatus(int type, Guid userId, DbTransaction trans = null)
        {
            var _trans = trans ?? db.BeginTransaction();

            var status = this.status;

            this.changed = DateTime.Now;
            this.changedby = userId;
            this.status = (short)type;

            var dbresult = db.UpdateCMP_Invoice(new CMP_Invoice().B_EntityDataCopyForMaterial(this), false, _trans);

            var action = new CMP_InvoiceAction
            {
                id = Guid.NewGuid(),
                created = DateTime.Now,
                createdby = this.changedby,
                invoiceId = this.id
            };

            action.type = type == (int)EnumCMP_TenderStatus.YoneticiOnay ? (short)EnumCMP_InvoiceActionType.TeklifYoneticiOnay :
                          type == (int)EnumCMP_TenderStatus.MusteriOnay ? (short)EnumCMP_InvoiceActionType.TeklifMusteriOnay :
                          type == (int)EnumCMP_TenderStatus.TeklifFatura ? (short)EnumCMP_InvoiceActionType.TeklifFatura :
                          type == (int)EnumCMP_TenderStatus.TeklifSiparis ? (short)EnumCMP_InvoiceActionType.TeklifSiparis :
                          type == (int)EnumCMP_TenderStatus.TeklifIrsaliye ? (short)EnumCMP_InvoiceActionType.TeklifIrsaliye :
                          (short)EnumCMP_InvoiceActionType.TeklifRed;

            action.description = type == (int)EnumCMP_TenderStatus.YoneticiOnay ? "Teklif yönetici tarafından onaylandı" :
                                type == (int)EnumCMP_TenderStatus.MusteriOnay ? "Teklif, müşteri tarafından onaylandı." :
                                type == (int)EnumCMP_TenderStatus.TeklifFatura ? "Teklifin faturası kesildi." :
                                type == (int)EnumCMP_TenderStatus.TeklifIrsaliye ? "Teklifin irsaliyesi oluşturuldu." :
                                type == (int)EnumCMP_TenderStatus.TeklifSiparis ? "Teklif siparişe dönüştürüldü." : "Teklif yönetici tarafından reddedildi.";

            if (type == (int)EnumCMP_TenderStatus.Red)
            {
                action.description = status == (int)EnumCMP_TenderStatus.CevapBekleniyor ? "Teklif yönetici tarafından reddedildi." : "Teklif müşteri tarafından reddedildi.";
            }

            if (type != (int)EnumCMP_TenderStatus.TeklifFatura && type != (int)EnumCMP_TenderStatus.TeklifSiparis)
            {
                dbresult &= db.InsertCMP_InvoiceAction(action, _trans);
            }

            if (this.direction == (int)EnumCMP_InvoiceDirectionType.Alis)
            {
                if (type == (int)EnumCMP_TenderStatus.YoneticiOnay)
                {
                    //dbresult &= UpdateOtherTenders(type, _trans);

                    var listMuhasebe = new List<Guid>();
                    listMuhasebe.AddRange(db.GetSH_UserByRoleId(muhasebeYonetici));
                    var muhasebeUsers = new List<VWSH_User>();
                    if (listMuhasebe.Count() > 0)
                    {
                        muhasebeUsers = db.GetVWSH_UserByIds(listMuhasebe.ToArray()).ToList();
                    }

                    foreach (var muhUser in muhasebeUsers)
                    {
                        var text = "<h3>Sayın " + muhUser.FullName + "</h3>";
                        text += "<p>" + this.rowNumber + " kodlu teklif satın alma onaylayıcısı tarafından onaylanmıştır. </p>";
                        text += "<p>Teklif detayını görüntülemek ve faturasını oluşturmak için <a href='" + _siteURL + "/CMP/VWCMP_Tender/DetailBuying?id=" + this.id + "'>tıklayınız.</a> </p>";
                        text += "<p>Bilgilerinize.</p>";
                        new Email().Template("Template1", "satinalma.jpg", _tenantName + " | WorkOfTime | Satış Sipariş Yönetimi", text).Send((Int16)EmailSendTypes.SiparisTeklif, muhUser.email, "Teklif Onay", true);
                    }
                }
            }
            else
            {
                var user = db.GetVWSH_UserById(this.createdby.Value);

                if (type == (int)EnumCMP_TenderStatus.YoneticiOnay)
                {
                    var text = "<h3>Sayın " + user.FullName + "</h3>";
                    text += "<p>" + this.rowNumber + " kodlu teklif satış onaylayıcısı tarafından onaylanmıştır. </p>";
                    text += "<p>Teklif detayını görüntülemek ve müşteriye sunmak için <a href='" + _siteURL + "/CMP/VWCMP_Tender/DetailSelling?id=" + this.id + "'>tıklayınız.</a> </p>";
                    text += "<p>Bilgilerinize.</p>";
                    new Email().Template("Template1", "satinalma.jpg", _tenantName + " | WorkOfTime | Satış Sipariş Yönetimi", text).Send((Int16)EmailSendTypes.SiparisTeklif, user.email, "Teklif Onay", true);
                }

                if (type == (int)EnumCMP_TenderStatus.Red)
                {
                    var text = "<h3>Sayın " + user.FullName + "</h3>";
                    text += "<p>" + this.rowNumber + " kodlu teklif satış onaylayıcısı tarafından reddedilmiştir. </p>";
                    text += "<p>Teklif detayını görüntülemek ve teklifi yenilemek için <a href='" + _siteURL + "/CMP/VWCMP_Tender/DetailSelling?id=" + this.id + "'>tıklayınız.</a> </p>";
                    text += "<p>Bilgilerinize.</p>";
                    new Email().Template("Template1", "satinalma.jpg", _tenantName + " | WorkOfTime | Satış Sipariş Yönetimi", text).Send((Int16)EmailSendTypes.SiparisTeklif, user.email, "Teklif Red", true);
                }
            }

            if (trans == null)
            {
                if (dbresult.result)
                    _trans.Commit();
                else
                    _trans.Rollback();
            }

            var rs = new ResultStatus { result = dbresult.result, objects = action };

            if (dbresult.result)
            {
                rs.message = this.status == (int)EnumCMP_TenderStatus.YoneticiOnay || this.status == (int)EnumCMP_TenderStatus.MusteriOnay ? "Teklif onaylama işlemi başarılı." :
                            this.status == (int)EnumCMP_TenderStatus.TeklifFatura ? "Teklifin faturası kesildi." :
                            this.status == (int)EnumCMP_TenderStatus.TeklifIrsaliye ? "Teklifin irsaliyesi oluşturuldu." :
                            this.status == (int)EnumCMP_TenderStatus.Red ? "Teklif reddedildi." : "Teklif siparişe dönüştürüldü";
            }

            return rs;
        }

        private ResultStatus UpdateTenders(DbTransaction trans = null)
        {
            var listAction = new List<CMP_InvoiceAction>();
            var listTender = new List<CMP_Invoice>();

            var _trans = trans ?? db.BeginTransaction();

            var tenderTransforms = db.GetVWCMP_InvoiceTransformByIsTransformedFrom(this.Request.id);


            var tenders = db.GetCMP_InvoiceByIds(tenderTransforms.Where(a => a.invoiceIdTo.HasValue).Select(a => a.invoiceIdTo.Value).ToArray())
                .Where(a => a.id != this.id && (a.status == (int)EnumCMP_TenderStatus.Red || a.status == (int)EnumCMP_TenderStatus.YoneticiOnay));

            if (tenders.Count() > 0)
            {
                this.Request.status = (int)EnumCMP_RequestStatus.TeklifToplanmasiBekleniyor;
                this.Request.changedby = this.createdby.Value;
                this.Request.changed = DateTime.Now;

                listTender.Add(this.Request);
                listAction.Add(new CMP_InvoiceAction
                {
                    created = DateTime.Now,
                    createdby = this.createdby.Value,
                    invoiceId = this.Request.id,
                    description = "Yeni teklif eklendi, tekliflerin tekrar onaya gönderilmesi bekleniyor.",
                    type = (short)EnumCMP_InvoiceActionType.YeniTeklif,
                });

                foreach (var tender in tenders)
                {
                    listAction.Add(new CMP_InvoiceAction
                    {
                        created = DateTime.Now,
                        createdby = this.Request.changedby.Value,
                        invoiceId = tender.id,
                        description = "Satın alma talebine yeni teklif eklendi, teklif tekrar değerlendirilecek.",
                        type = (short)EnumCMP_InvoiceActionType.YeniTeklif,
                    });

                    tender.changed = DateTime.Now;
                    tender.changedby = this.Request.changedby.Value;
                    tender.status = (short)EnumCMP_TenderStatus.CevapBekleniyor;

                    listTender.Add(tender);
                }
            }

            var dbresult = db.BulkInsertCMP_InvoiceAction(listAction.ToArray(), _trans);
            dbresult &= db.BulkUpdateCMP_Invoice(listTender.ToArray(), false, _trans);

            if (trans == null)
            {
                if (dbresult.result)
                    _trans.Commit();
                else
                    _trans.Rollback();
            }

            return dbresult;
        }

        public TenderFormTemplateModels GetFormTemplate(int? type)
        {

            var tender = db.GetVWCMP_TenderById(this.id);

            var model = new TenderFormTemplateModels();
            if (type.HasValue)
            {
                model.typeJob = type.Value;
            }

            if (tender.invoiceDocumentTemplateId.HasValue)
            {
                var invoiceDocumentTemplate = db.GetCMP_InvoiceDocumentTemplateById(tender.invoiceDocumentTemplateId.Value);

                if (invoiceDocumentTemplate != null)
                {
                    model.CMP_InvoiceDocumentTemplate = invoiceDocumentTemplate;
                }
            }

            model.B_EntityDataCopyForMaterial(tender);

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

        public ResultStatus InsertNote(Guid userId, string note)
        {
            var actionid = Guid.NewGuid();
            var res = db.InsertCMP_InvoiceAction(new CMP_InvoiceAction
            {
                id = actionid,
                created = DateTime.Now,
                createdby = userId,
                invoiceId = this.id,
                description = note,
                type = (short)EnumCMP_InvoiceActionType.NotEkle
            });
            res.objects = db.GetVWCMP_InvoiceActionById(actionid);
            return res;
        }

        public SummaryHeadersTender GetMyTenderPageInfo(Guid userId)
        {
            this.db = this.db ?? new WorkOfTimeDatabase();
            var roles = db.GetVWSH_UserRoleByUserId(userId);
            List<string> tempRoles = new List<string> { };

            foreach (var role in roles)
            {
                tempRoles.Add(role.roleid.ToString());
            }

            if (tempRoles.Contains(SHRoles.BayiPersoneli))
            {
                return db.GetDBVWCMP_GetMyTenderApproveSummary(userId);

            }
            else if (tempRoles.Contains(SHRoles.SatisPersoneli) && !tempRoles.Contains(SHRoles.SatisOnaylayici) && !tempRoles.Contains(SHRoles.MuhasebeSatis))
            {
                return db.GetDBVWCMP_GetMyTenderApproveSummary(userId);

            }
            else if (tempRoles.Contains(SHRoles.MuhasebeSatis) && !tempRoles.Contains(SHRoles.SatisPersoneli) && !tempRoles.Contains(SHRoles.SatisOnaylayici))
            {
                return db.GetDBVWCMP_GetMyTenderAccountingSummary(userId);

            }
            else
            {
                return db.GetDBVWCMP_GetMyTenderSummary(userId);
            }
        }
    }
}
