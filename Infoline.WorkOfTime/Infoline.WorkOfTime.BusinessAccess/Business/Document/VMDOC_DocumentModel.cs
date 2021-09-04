using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.BusinessAccess
{
    public class VMDOC_DocumentModel : VWDOC_Document
    {
        private WorkOfTimeDatabase db { get; set; }
        private DbTransaction trans { get; set; }
        //Onay akışı
        public List<VWDOC_DocumentFlow> flowList { get; set; } = new List<VWDOC_DocumentFlow>();
        //Döküman kapsamı
        public List<Guid> documentScopeOrganizationUnitIds { get; set; }
        [AllowHtml]
        public string content { get; set; }
        public DOC_DocumentRevisionRequest revisionRequest { get; set; }
        public DOC_DocumentRevisionRequestConfirmation revisionRequestConfirmation { get; set; }
        public List<Guid> confirmationUserIds { get; set; } = new List<Guid>();
        public bool isRequest { get; set; }
        public DOC_DocumentVersion documentVersion { get; set; }
        public Guid? newVersionId { get; set; }

        public VMDOC_DocumentModel LoadRequest()
        {
            this.db = this.db ?? new WorkOfTimeDatabase();
            var request = db.GetDOC_DocumentRevisionRequestById(this.revisionRequest.id);
            if (request != null)
            {
                this.B_EntityDataCopyForMaterial(db.GetVWDOC_DocumentById(request.documentId.Value), true);
                this.documentVersion = db.GetDOC_DocumentVersionByDocumentIdAndActive(request.documentId.Value);
                this.documentScopeOrganizationUnitIds = db.GetDOC_DocumentScopeByDocumentId(this.id).Where(x => x.organizationUnitId.HasValue).Select(x => x.organizationUnitId.Value).ToList();
                this.revisionRequest = request;
                var doc = db.GetDOC_DocumentVersionByDocumentIdAndActive(this.id);
                if (doc != null)
                {
                    this.content = doc.content;
                }
                var reqConfirmations = db.GetDOC_DocumentRevisionRequestConfirmationByRequestId(request.id).OrderBy(x => x.order).ToArray();
                var flowList = db.GetDOC_DocumentFlowByDocumentId(this.id).OrderBy(x => x.order).ToList();

                if (reqConfirmations.Count() == 0)
                {
                    this.revisionRequestConfirmation = new DOC_DocumentRevisionRequestConfirmation
                    {
                        documentId = this.id,
                        revisionRequestId = request.id,
                        order = 0
                    };
                    if (flowList.Count() > 0)
                    {
                        var flow = flowList.FirstOrDefault();
                        if (flow != null)
                        {
                            if (flow.userId.HasValue)
                            {
                                this.confirmationUserIds.Add(flow.userId.Value);
                            }
                            else if (flow.organizationUnitId.HasValue)
                            {
                                var organization = db.GetINV_CompanyPersonDepartmentsInDepartmentId(new Guid[] { flow.organizationUnitId.Value });
                                this.confirmationUserIds.AddRange(organization.Where(x => x.IdUser.HasValue).Select(x => x.IdUser.Value).ToList());
                            }
                        }
                    }
                }
                else
                {
                    var lastReqConfirmation = reqConfirmations.LastOrDefault();
                    var flowDta = flowList.Where(x => x.order == (lastReqConfirmation.order + 1)).FirstOrDefault();
                    if (flowDta != null && lastReqConfirmation != null && lastReqConfirmation.status != (Int16)EnumDOC_DocumentRevisionRequestConfirmationStatus.Red)
                    {
                        this.revisionRequestConfirmation = new DOC_DocumentRevisionRequestConfirmation
                        {
                            documentId = this.id,
                            revisionRequestId = request.id,
                            order = flowDta.order
                        };

                        if (flowDta.userId.HasValue)
                        {
                            this.confirmationUserIds.Add(flowDta.userId.Value);
                        }
                        else if (flowDta.organizationUnitId.HasValue)
                        {
                            var organization = db.GetINV_CompanyPersonDepartmentsInDepartmentId(new Guid[] { flowDta.organizationUnitId.Value });
                            this.confirmationUserIds.AddRange(organization.Where(x => x.IdUser.HasValue).Select(x => x.IdUser.Value).ToList());
                        }
                    }
                }

                //this.revisionRequestConfirmation = new DOC_DocumentRevisionRequestConfirmation { documentId = this.id, revisionRequestId = request.id, confirmUserId =  };
            }
            this.code = this.code ?? BusinessExtensions.B_GetIdCode();
            return this;
        }

        public VMDOC_DocumentModel Load()
        {
            this.db = this.db ?? new WorkOfTimeDatabase();
            var document = db.GetVWDOC_DocumentById(this.id);
            if (document != null)
            {
                this.B_EntityDataCopyForMaterial(document, true);
                this.documentVersion = db.GetDOC_DocumentVersionByDocumentIdAndActive(this.id);
                var myflowList = db.GetVWDOC_DocumentFlowByDocumentId(this.id).OrderBy(x => x.order).ToList();
                for (short i = 0; i < 6; i++)
                {
                    var elem = myflowList.Where(a => a.order == i).FirstOrDefault();
                    flowList.Add(elem != null ? elem : new VWDOC_DocumentFlow { order = i, type = (Int16)EnumDOC_DocumentFlowType.OrganizationUnit });
                }
                this.documentScopeOrganizationUnitIds = db.GetDOC_DocumentScopeByDocumentId(this.id).Where(x => x.organizationUnitId.HasValue).Select(x => x.organizationUnitId.Value).ToList();

                var doc = db.GetDOC_DocumentVersionByDocumentIdAndActive(this.id);
                if (doc != null)
                {
                    this.content = doc.content;
                }
                if (this.isRequest == true)
                {
                    this.revisionRequest = new DOC_DocumentRevisionRequest { id = Guid.NewGuid(), documentId = this.id };
                }
            }
            else
            {
                this.documentVersion = new DOC_DocumentVersion { id = Guid.NewGuid() };
                for (short i = 0; i < 6; i++)
                {
                    flowList.Add(new VWDOC_DocumentFlow
                    {
                        order = i,
                        type = (Int16)EnumDOC_DocumentFlowType.OrganizationUnit
                    });
                }
            }
            this.code = this.code ?? BusinessExtensions.B_GetIdCode();

            return this;
        }

        public ResultStatus RequestRevision(Guid? userId, DbTransaction _trans = null)
        {
            this.db = this.db ?? new WorkOfTimeDatabase();
            this.trans = _trans ?? db.BeginTransaction();
            var document = db.GetVWDOC_DocumentById(this.id);

            if (document == null)
            {
                return new ResultStatus { result = false, message = "Doküman bulunamadı" };
            }

            var DBResult = new ResultStatus { result = true };
            this.revisionRequest.revisionUserId = userId;
            this.revisionRequest.documentId = this.id;
            this.revisionRequest.created = DateTime.Now;
            this.revisionRequest.createdby = userId;

            DBResult &= db.InsertDOC_DocumentRevisionRequest(this.revisionRequest, this.trans);

            if (DBResult.result)
            {
                if (_trans == null) trans.Commit();
                EmailSenderRequest(EnumVWDOC_DocumentRevisionRequest.talepyapildi);
                return new ResultStatus { result = true, message = "Revizyon talebi başarı ile gerçekleştirildi." };
            }
            else
            {
                if (_trans == null) trans.Rollback();
                return new ResultStatus { result = false, message = "Revizyon talebi gerçekleştirme işlemi başarısız oldu." };
            }
        }

        public ResultStatus RequestRevisionUpdate(Guid? userId, DbTransaction _trans = null)
        {
            this.db = this.db ?? new WorkOfTimeDatabase();
            var dbresult = new ResultStatus { result = true };
            this.revisionRequestConfirmation.created = DateTime.Now;
            this.revisionRequestConfirmation.date = DateTime.Now;
            this.revisionRequestConfirmation.createdby = userId;
            this.revisionRequestConfirmation.confirmUserId = userId;
            dbresult &= db.InsertDOC_DocumentRevisionRequestConfirmation(this.revisionRequestConfirmation);

            var data = db.GetVWDOC_DocumentRevisionRequestById(this.revisionRequestConfirmation.id);
            if (data != null)
            {

            }

            if (dbresult.result)
            {
                dbresult.message = this.revisionRequestConfirmation.status == (Int16)EnumDOC_DocumentRevisionRequestConfirmationStatus.Onay ? "Revizyon talebini başarı ile onayladınız." : "Revizyon talebini başarı ile reddettiniz.";
                if (this.revisionRequestConfirmation.status == (Int16)EnumDOC_DocumentRevisionRequestConfirmationStatus.Onay)
                {
                    EmailSenderRequest(EnumVWDOC_DocumentRevisionRequest.onaylandi);
                }
                if (this.revisionRequestConfirmation.status == (Int16)EnumDOC_DocumentRevisionRequestConfirmationStatus.Red)
                {
                    EmailSenderRequest(EnumVWDOC_DocumentRevisionRequest.talepreddedildi);
                }
            }
            else
            {
                dbresult.message = "Revizyon talebi yanıtlama işlemi başarısız oldu." + dbresult.message;
            }

            return dbresult;
        }

        public ResultStatus Save(Guid? userId, HttpRequestBase requestFiles = null, DbTransaction _trans = null)
        {
            this.db = this.db ?? new WorkOfTimeDatabase();
            this.trans = _trans ?? db.BeginTransaction();

            var document = db.GetDOC_DocumentById(this.id);
            this.code = string.IsNullOrEmpty(this.code) ? BusinessExtensions.B_GetIdCode() : this.code;

            this.flowList = this.flowList.Where(a => a.userId.HasValue || a.organizationUnitId.HasValue).OrderBy(a => a.order).ToList();

            var rs = new ResultStatus { result = true };

            if (this.flowList.Count() == 0)
            {
                rs.result = false;
                rs.message = "Onay akışı girmediniz.";
                return rs;
            }

            short i = 0;
            foreach (var item in this.flowList)
            {
                item.createdby = this.createdby;
                item.created = DateTime.Now;
                item.documentId = this.id;
                item.userId = item.type == (Int16)EnumDOC_DocumentFlowType.OrganizationUnit ? null : item.userId;
                item.organizationUnitId = item.type == (Int16)EnumDOC_DocumentFlowType.User ? null : item.organizationUnitId;
                item.order = i;
                i += 1;
            }

            if (document == null)
            {
                this.created = DateTime.Now;
                this.createdby = userId;
                rs &= this.Insert();
            }
            else
            {
                this.changed = DateTime.Now;
                this.changedby = userId;
                rs &= this.Update();
            }

            if (rs.result)
            {
                if (_trans == null) trans.Commit();
                if (requestFiles != null)
                {
                    if (this.newVersionId.HasValue)
                    {
                        new FileUploadSave(requestFiles, this.newVersionId, "DOC_DocumentVersion").SaveAs();
                    }
                    else
                    {
                        new FileUploadSave(requestFiles, this.documentVersion.id, "DOC_DocumentVersion").SaveAs();
                    }
                }
            }
            else
            {
                if (_trans == null) trans.Rollback();
            }

            return rs;
        }

        private ResultStatus Insert()
        {
            var DBResult = new ResultStatus { result = true };
            var Document = new DOC_Document().B_EntityDataCopyForMaterial(this);
            var DocumentScopeList = new List<DOC_DocumentScope>();

            DocumentScopeList.AddRange(this.documentScopeOrganizationUnitIds.Select(x => new DOC_DocumentScope
            {
                created = DateTime.Now,
                createdby = this.createdby,
                organizationUnitId = x,
                documentId = this.id
            }));

            DBResult &= db.InsertDOC_Document(Document, this.trans);
            DBResult &= db.BulkInsertDOC_DocumentScope(DocumentScopeList, this.trans);
            DBResult &= db.BulkInsertDOC_DocumentFlow(this.flowList.Select(x => new DOC_DocumentFlow().B_EntityDataCopyForMaterial(x)).ToList(), this.trans);

            this.documentVersion.documentId = this.id;
            this.documentVersion.versionNumber = this.versionNumber;
            this.documentVersion.createdby = this.createdby;
            this.documentVersion.created = DateTime.Now;
            this.documentVersion.content = this.content;
            this.documentVersion.isActive = true;

            DBResult &= db.InsertDOC_DocumentVersion(this.documentVersion, this.trans);

            if (DBResult.result)
            {
                DBResult.message = "Doküman kaydetme işlemi başarılı.";
            }
            else
            {
                DBResult.message = "Doküman kaydetme işlemi başarısız oldu.";
            }

            return DBResult;
        }

        private ResultStatus Update()
        {
            var DBResult = new ResultStatus { result = true };
            var data = db.GetDOC_DocumentById(this.id);
            if (data.versionNumber == this.versionNumber)
            {
                var revisionReq = db.GetDOC_DocumentRevisionRequestByDocumentId(this.id);
                if (revisionReq.Count() > 0)
                {
                    DBResult.result = false;
                    DBResult.message = "Dokümana ait revizyon talebi bulunduğundan güncelleme işlemini versiyon değiştirerek yapabilirsiniz.";
                    return DBResult;
                }
            }

            var oldVersions = db.GetDOC_DocumentVersionByDocumentId(this.id);
            if (oldVersions.Count(x => x.versionNumber == this.versionNumber) > 0)
            {
                DBResult.result = false;
                DBResult.message = "Aynı versiyon numarası mevcut.";
                return DBResult;
            }

            data.changed = this.changed;
            data.changedby = this.createdby;
            data.code = this.code;
            data.title = this.title;
            data.subject = this.subject;
            data.responsibleUserId = this.responsibleUserId;
            data.type = this.type;
            data.versionNumber = this.versionNumber;

            var DocumentScopeList = new List<DOC_DocumentScope>();

            DocumentScopeList.AddRange(this.documentScopeOrganizationUnitIds.Select(x => new DOC_DocumentScope
            {
                created = DateTime.Now,
                createdby = this.createdby,
                organizationUnitId = x,
                documentId = this.id
            }));

            if (oldVersions != null)
            {
                foreach (var oldVersion in oldVersions)
                {
                    oldVersion.isActive = false;
                }
            }
            this.newVersionId = Guid.NewGuid();

            DBResult &= db.UpdateDOC_Document(data, true, this.trans);
            DBResult &= db.InsertDOC_DocumentVersion(new DOC_DocumentVersion
            {
                id = this.newVersionId.Value,
                content = this.content,
                versionNumber = this.versionNumber,
                created = DateTime.Now,
                createdby = this.createdby,
                documentId = this.id,
                isActive = true
            }, this.trans);
            DBResult &= db.BulkDeleteDOC_DocumentFlow(db.GetDOC_DocumentFlowByDocumentId(this.id), this.trans);
            DBResult &= db.BulkInsertDOC_DocumentFlow(this.flowList.Select(x => new DOC_DocumentFlow().B_EntityDataCopyForMaterial(x)).ToList(), this.trans);
            DBResult &= db.BulkDeleteDOC_DocumentScope(db.GetDOC_DocumentScopeByDocumentId(this.id), this.trans);
            DBResult &= db.BulkInsertDOC_DocumentScope(DocumentScopeList, this.trans);
            DBResult &= db.BulkUpdateDOC_DocumentVersion(oldVersions, true, this.trans);

            if (!DBResult.result)
            {
                Log.Error(DBResult.message);
                DBResult.message = "Doküman güncelleme işlemi başarısız oldu.";
                return DBResult;
            }
            else
            {
                EmailRelation();
                DBResult.message = "Doküman güncelleme işlemi başarılı şekilde gerçekleştirildi.";
                return DBResult;
            }
        }

        public ResultStatus Delete(DbTransaction _trans = null)
        {
            this.db = this.db ?? new WorkOfTimeDatabase();
            var document = db.GetDOC_DocumentById(this.id);
            if (document != null)
            {
                var rs = new ResultStatus { result = true };

                var relationRelation = db.GetDOC_DocumentRelationByDocumentId(this.id);
                if (relationRelation.Count() > 0)
                {
                    return new ResultStatus { result = false, message = "Dokümanın başka bir doküman ile ilişkisi bulunduğundan silme işlemini gerçekleştiremezsiniz." };
                }

                var revisionRequest = db.GetDOC_DocumentRevisionRequestByDocumentId(this.id);
                if (revisionRequest != null)
                {
                    return new ResultStatus { result = false, message = "Dokümana ait revizyon talebi bulunduğundan silme işlemini gerçekleştiremezsiniz." };
                }

                this.trans = _trans ?? db.BeginTransaction();

                var requestConfirmation = db.GetDOC_DocumentRevisionRequestConfirmationByDocumentId(this.id);
                var revisionRequests = db.GetDOC_DocumentRevisionRequestByDocumentId(this.id);
                var scopes = db.GetDOC_DocumentScopeByDocumentId(this.id);
                var versions = db.GetDOC_DocumentVersionByDocumentId(this.id);
                var flows = db.GetDOC_DocumentFlowByDocumentId(this.id);
                var relations = db.GetDOC_DocumentRelationByDocumentId(this.id);

                rs &= db.BulkDeleteDOC_DocumentRevisionRequestConfirmation(requestConfirmation, trans);
                rs &= db.BulkDeleteDOC_DocumentRevisionRequest(revisionRequests, trans);
                rs &= db.BulkDeleteDOC_DocumentScope(scopes, trans);
                rs &= db.BulkDeleteDOC_DocumentVersion(versions, trans);
                rs &= db.BulkDeleteDOC_DocumentFlow(flows, trans);
                rs &= db.BulkDeleteDOC_DocumentRelation(relations, trans);
                rs &= db.DeleteDOC_Document(document, trans);

                if (rs.result == true)
                {
                    if (_trans == null) trans.Commit();
                    return new ResultStatus { message = "Doküman başarıyla silindi.", result = true, };
                }
                else
                {
                    if (_trans == null) trans.Rollback();
                    return new ResultStatus { message = "Doküman silinirken sorunlar oluştu.", result = false, };
                }

            }
            else
            {
                return new ResultStatus { message = "Doküman bulunamadı.", result = false, };
            }
        }

        public void EmailSenderRequest(EnumVWDOC_DocumentRevisionRequest enumData)
        {
            switch (enumData)
            {
                case EnumVWDOC_DocumentRevisionRequest.talepyapildi:
                    if (this.revisionRequest != null)
                    {
                        if (this.responsibleUserId.HasValue && this.revisionRequest.createdby.HasValue)
                        {
                            var responsibleUser = db.GetSH_UserById(this.responsibleUserId.Value);
                            var requestUser = db.GetSH_UserById(this.revisionRequest.createdby.Value);
                            if (responsibleUser != null && requestUser != null)
                            {
                                var text = "<h3>Merhaba {0},</h3> " +
                                    "<p>Sorumlusu olduğunuz {1} kodlu doküman ile ilgili {2} tarihinde {3} tarafından revizyon talebinde bulunulmuştur.</p>" +
                                    "<p>Revizyon numarası : <strong> {4} </strong> </p> " +
                                    "<p>Revizyon içeriği : <strong> {5} </strong> </p> " +
                                    "<p>Bilgilerinize</p>" +
                                    "<p>İyi Çalışmalar.</p>";

                                var mesaj = string.Format(text, responsibleUser.firstname + " " + responsibleUser.lastname, this.code, String.Format("{0:dd/MM/yyyy HH:mm}", this.revisionRequest.created), requestUser.firstname + " " + requestUser.lastname, this.revisionRequest.revisionNumber, this.revisionRequest.revisionContent);

                                new Email().Template("Template1", "bos.png", "Revizyon Talebi Hakkında", mesaj).Send((Int16)EmailSendTypes.RevizyonTalebi, responsibleUser.email, TenantConfig.Tenant.TenantName + " | WORKOFTIME | Yeni Revizyon Talebi");
                            }
                        }
                    }
                    var revisionReq = db.GetVWDOC_DocumentRevisionRequestById(this.revisionRequest.id);
                    if (revisionReq != null)
                    {
                        if (this.revisionRequest.createdby.HasValue)
                        {
                            var requestUser = db.GetSH_UserById(this.revisionRequest.createdby.Value);
                            var confirmUsers = db.GetSH_UserByIds(revisionReq.confirmationUserIds.Split(',').Select(x => Guid.Parse(x)).ToArray());
                            foreach (var confirmUser in confirmUsers)
                            {
                                var txt = "<h3>Merhaba {0},</h3> " +
                                    "<p>{1} kodlu doküman ile ilgili {2} tarihinde {3} tarafından revizyon talebinde bulunulmuştur.</p>" +
                                    "<p> Talebi onaylamak için <a href='{4}/DOC/VWDOC_DocumentRevisionRequest/MyAboutIndex'> buraya tıklayabilirsiniz.</a></p>" +
                                    "<p>Bilgilerinize</p>" +
                                    "<p>İyi Çalışmalar.</p>";

                                var msj = string.Format(txt, confirmUser.firstname + " " + confirmUser.lastname, this.code, String.Format("{0:dd/MM/yyyy HH:mm}", this.revisionRequest.created), requestUser.firstname + " " + requestUser.lastname, TenantConfig.Tenant.GetWebUrl());

                                new Email().Template("Template1", "bos.png", "Revizyon Talebi Onayı Hakkında", msj).Send((Int16)EmailSendTypes.RevizyonTalebi, confirmUser.email, TenantConfig.Tenant.TenantName + " | WORKOFTIME | Revizyon Talebi");
                            }
                        }
                    }
                    break;
                case EnumVWDOC_DocumentRevisionRequest.talepreddedildi:
                    if (this.revisionRequest != null)
                    {
                        if (this.revisionRequest.createdby.HasValue)
                        {
                            var requestUser = db.GetSH_UserById(this.revisionRequest.createdby.Value);
                            if (requestUser != null)
                            {
                                var text = "<h3>Merhaba {0},</h3> " +
                                    "<p><a href='{1}/DOC/VWDOC_Document/Detail?id={2}'> {3}</a>  kodlu doküman ile ilgili {2} tarihinde yapmış olduğunuz revizyon talebi reddedilmiştir.</p>" +
                                    "<p>Bilgilerinize</p>" +
                                    "<p>İyi Çalışmalar.</p>";

                                var mesaj = string.Format(text, requestUser.firstname + " " + requestUser.lastname, TenantConfig.Tenant.GetWebUrl(), this.id, this.code, String.Format("{0:dd/MM/yyyy HH:mm}", this.revisionRequest.created));

                                new Email().Template("Template1", "bos.png", "Revizyon Talebi Hakkında", mesaj).Send((Int16)EmailSendTypes.RevizyonTalebi, requestUser.email, TenantConfig.Tenant.TenantName + " | WORKOFTIME | Reddedilen Revizyon Talebi");
                            }
                        }
                    }
                    break;
                case EnumVWDOC_DocumentRevisionRequest.onaybekleniyor:
                    break;
                case EnumVWDOC_DocumentRevisionRequest.onaylandi:
                    var Req = db.GetVWDOC_DocumentRevisionRequestById(this.revisionRequest.id);
                    if (Req != null)
                    {
                        if (this.revisionRequest.createdby.HasValue)
                        {
                            var requestUser = db.GetSH_UserById(this.revisionRequest.createdby.Value);
                            var confirmUsers = db.GetSH_UserByIds(Req.confirmationUserIds.Split(',').Select(x => Guid.Parse(x)).ToArray());
                            foreach (var confirmUser in confirmUsers)
                            {
                                var txt = "<h3>Merhaba {0},</h3> " +
                                    "<p>{1} kodlu doküman ile ilgili {2} tarihinde {3} tarafından revizyon talebinde bulunulmuştur.</p>" +
                                    "<p> Talebi onaylamak için <a href='{4}/DOC/VWDOC_DocumentRevisionRequest/MyAboutIndex'> buraya tıklayabilirsiniz.</a></p>" +
                                    "<p>Bilgilerinize</p>" +
                                    "<p>İyi Çalışmalar.</p>";

                                var msj = string.Format(txt, confirmUser.firstname + " " + confirmUser.lastname, this.code, String.Format("{0:dd/MM/yyyy HH:mm}", this.revisionRequest.created), requestUser.firstname + " " + requestUser.lastname, TenantConfig.Tenant.GetWebUrl());

                                new Email().Template("Template1", "bos.png", "Revizyon Talebi Onayı Hakkında", msj).Send((Int16)EmailSendTypes.RevizyonTalebi, confirmUser.email, TenantConfig.Tenant.TenantName + " | WORKOFTIME | Revizyon Talebi");
                            }
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        public void EmailRelation()
        {
            var documentRelations = db.GetDOC_DocumentRelationByDocumentRelationId(this.id);
            foreach (var documentRelation in documentRelations)
            {
                if (documentRelation.documentId.HasValue)
                {
                    var document = db.GetDOC_DocumentById(documentRelation.documentId.Value);
                    if (document != null)
                    {
                        if (document.responsibleUserId.HasValue)
                        {
                            var responsibleUser = db.GetSH_UserById(document.responsibleUserId.Value);
                            if (responsibleUser != null)
                            {
                                var text = "<h3>Merhaba {0},</h3> " +
                                      "<p>Sorumlusu olduğunuz {1} kodlu doküman ile ilgili {2} tarihinde versiyon güncellemesi yapılmıştır.</p>" +
                                        "<p> Doküman detayına gitmek için <a href='{3}/DOC/VWDOC_Document/Detail?id={4}'> buraya tıklayabilirsiniz.</a></p>" +
                                      "<p>Bilgilerinize</p>" +
                                      "<p>İyi Çalışmalar.</p>";

                                var mesaj = string.Format(text, responsibleUser.firstname + " " + responsibleUser.lastname, this.code, String.Format("{0:dd/MM/yyyy HH:mm}", this.changed), TenantConfig.Tenant.GetWebUrl(), this.id);

                                new Email().Template("Template1", "bos.png", "İlişkili Doküman Versiyon Güncellemesi Hakkında", mesaj).Send((Int16)EmailSendTypes.RevizyonTalebi, responsibleUser.email, TenantConfig.Tenant.TenantName + " | WORKOFTIME | İlişkili Doküman Versiyon Güncellemesi");
                            }
                        }
                    }
                }
            }

        }
    }
}
