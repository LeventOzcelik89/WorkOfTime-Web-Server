using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Infoline.WorkOfTime.BusinessAccess
{
	public class RequestFormTemplateModels : VWCMP_Request
	{
		public VWCMP_InvoiceItem[] Items { get; set; }
		public VWCMP_Company Customer { get; set; }
		public string LogoPath { get; set; }
		public int? typeJob { get; set; }
		public VWSH_User User { get; set; }
	}

	public class VMCMP_RequestModels : VWCMP_Request
	{
		private WorkOfTimeDatabase db { get; set; }
		private DbTransaction trans { get; set; }
		private string _siteURL { get; set; } = TenantConfig.Tenant.GetWebUrl();
		private string _tenantName { get; set; } = TenantConfig.Tenant.TenantName;
		public List<VWCMP_InvoiceAction> InvoiceActions { get; set; }
		public List<VWCMP_InvoiceItem> InvoiceItems { get; set; } = new List<VWCMP_InvoiceItem>();
		public VWCMP_InvoiceTransform[] TransformTo { get; set; }
		public CMP_Invoice Order { get; set; }
		public bool? IsTransform { get; set; }
		public bool? IsCopy { get; set; }
		public VWFTM_Task Task { get; set; }
		public Guid[] taskIds { get; set; }
		public static Guid _approvalRoleId { get; set; } = new Guid(SHRoles.SatinAlmaOnaylayici);
		public Guid[] _approvalPersons = new Guid[0];
		public Guid[] _managerPersons = new Guid[0];

		public VMCMP_RequestModels Load(bool? isTransform)
		{
			db = db ?? new WorkOfTimeDatabase();
			var invoice = db.GetCMP_InvoiceById(this.id);
			var request = db.GetVWCMP_RequestById(this.id);

			if (invoice != null)
			{
				this.B_EntityDataCopyForMaterial(request, true);
				this.InvoiceItems = db.GetVWCMP_InvoiceItemByInvoiceId(this.id).OrderBy(a => a.itemOrder).ToList();
				this.InvoiceActions = db.GetVWCMP_InvoiceActionByInvoiceId(this.id).ToList();

				if (this.taskId.HasValue)
				{
					Task = db.GetVWFTM_TaskById(this.taskId.Value);
				}

				if (isTransform == true)
				{
					this.rowNumber = null;
					foreach (var item in this.InvoiceItems)
					{
						item.id = Guid.NewGuid();
						item.description = "";
					}
				}
				else
				{
					this.TransformTo = db.GetVWCMP_InvoiceTransformByIsTransformedFrom(this.id).ToArray();
				}
			}

			this.rowNumber = String.IsNullOrEmpty(this.rowNumber) ? BusinessExtensions.B_GetIdCode() : this.rowNumber;
			this.status = this.status.HasValue ? this.status.Value : (short)EnumCMP_RequestStatus.YoneticiOnayiBekleniyor;
			this.type = (int)EnumCMP_InvoiceType.Talep;
			this.direction = (int)EnumCMP_InvoiceDirectionType.Alis;

			if (this.IsCopy == true)
			{
				this.id = Guid.NewGuid();
			}

			return this;
		}

		public VMCMP_RequestModels Load(Guid userId)
		{
			var db = new WorkOfTimeDatabase();
			this.taskIds = db.GetFTM_TaskByCreatedBy(userId);
			return this;
		}


		public ResultStatus Save(Guid? userId, HttpRequestBase req = null, DbTransaction _trans = null)
		{
			db = db ?? new WorkOfTimeDatabase();
			var rs = new ResultStatus { result = true };
			var request = db.GetVWCMP_RequestById(this.id);
			if (InvoiceItems == null && string.IsNullOrEmpty(this.description)) return new ResultStatus { result = false, message = "Satın alma talebi oluşturabilmek için en az bir ürün girmek ya da açıklama yapmak zorunludur." };
			InvoiceItems = InvoiceItems.Where(a => (a.quantity != null && a.quantity != 0 && a.productId != null) || a.description != null).ToList();
			if (InvoiceItems.Count() == 0 && this.description == null) { return new ResultStatus { result = false, message = "Satın alma talebi oluşturabilmek için en az bir ürün girmek ya da açıklama yapmak zorunludur." }; }

			var operators = db.GetSH_UserByRoleId(new Guid(SHRoles.SatinAlmaPersonel)).ToList();
			this.trans = _trans ?? db.BeginTransaction();
			this._managerPersons = operators.ToArray();
			this._approvalPersons = db.GetSH_UserByRoleId(_approvalRoleId);

			if (request == null || this.IsCopy == true)
			{
				this.created = DateTime.Now;
				this.createdby = userId;
				rs = this.Insert(trans);
			}
			else
			{
				this.changed = DateTime.Now;
				this.changedby = userId;
				rs = this.Update(trans);
			}

			if (rs.result)
			{
				new FileUploadSave(req, this.id).SaveAs();
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

		private ResultStatus Insert(DbTransaction trans)
		{
			var dbresult = new ResultStatus { result = true };
			foreach (var item in this.InvoiceItems)
			{
				item.id = Guid.NewGuid();
				item.invoiceId = this.id;
				item.created = DateTime.Now;
				item.createdby = this.createdby;
			}

			var action = new CMP_InvoiceAction
			{
				created = DateTime.Now,
				createdby = this.createdby,
				invoiceId = this.id,
				type = (int)EnumCMP_InvoiceActionType.YeniTalep,
				description = "Satın alma talebi oluşturuldu."
			};

			_approvalPersons = db.GetSH_UserByRoleId(_approvalRoleId);


			if (this.taskId.HasValue)
			{
				dbresult &= InsertConfirmationTask(this.createdby.Value, this.taskId.Value);
			}

			//Onaylayıcı bir kişi talepde bulunduysa otomatik onay yapacağımız için mail atmıyoruz
			if (!_approvalPersons.Contains(this.createdby.Value) && _approvalPersons.Count() > 0)
			{
				var users = new List<VWSH_User>();

				if (_approvalPersons.Count() > 0)
				{
					users = db.GetVWSH_UserByIds(_approvalPersons).ToList();
				}
				var requestUser = db.GetVWSH_UserById(this.createdby.Value);
				foreach (var user in users)
				{
					var text = "<h3>Sayın " + user.FullName + "</h3>";
					text += "<p>" + requestUser.FullName + " adlı personel tarafından " + this.rowNumber + " kodlu satın alma talebi onayınıza sunulmuştur.</p>";
					text += "<p>Talep detayını görüntülemek ve işlem yapmak için <a href='" + _siteURL + "/CMP/VWCMP_Request/Detail?id=" + this.id + "'>tıklayınız.</a> </p>";
					text += "<p>Bilgilerinize.</p>";
					new Email().Template("Template1", "satinalma.jpg", _tenantName + " | WorkOfTime | Satın Alma Talep Yönetimi", text).Send((Int16)EmailSendTypes.SatinAlma, user.email, "Satın Alma Talebi Onaya Sunuldu ", true);
				}
			}

			dbresult &= db.InsertCMP_Invoice(new CMP_Invoice().B_EntityDataCopyForMaterial(this), this.trans);
			dbresult &= db.InsertCMP_InvoiceAction(action, this.trans);
			dbresult &= db.BulkInsertCMP_InvoiceItem(this.InvoiceItems.Select(a => new CMP_InvoiceItem().B_EntityDataCopyForMaterial(a)), this.trans);


			dbresult &= InsertTaskOperation(this.taskId, (int)EnumFTM_TaskOperationStatus.SatinAlmaTalebiYapildi);

			
			if (this.Order != null)
			{
				var listTransform = new List<CMP_InvoiceTransform>();
				var listAction = new List<CMP_InvoiceAction>();

				var transform = new CMP_InvoiceTransform
				{
					created = DateTime.Now,
					createdby = this.createdby,
					id = Guid.NewGuid(),
					invoiceIdFrom = this.Order.id,
					invoiceIdTo = this.id
				};

				var newAction = new CMP_InvoiceAction
				{
					id = Guid.NewGuid(),
					created = DateTime.Now,
					createdby = this.createdby,
					invoiceId = this.Order.id,
					transformInvoiceId = this.id,
					description = "Sipariş için satın alma talebi yapıldı.",
					type = (int)EnumCMP_InvoiceActionType.SiparisSatinAlma
				};

				dbresult &= db.InsertCMP_InvoiceTransform(transform, this.trans);
				dbresult &= db.InsertCMP_InvoiceAction(newAction, this.trans);
			}

			if (projectId.HasValue)
			{
				var projectInvoice = new PRJ_ProjectInvoice
				{
					id = Guid.NewGuid(),
					created = DateTime.Now,
					createdby = this.createdby,
					invoiceId = this.id,
					projectId = projectId.Value
				};

				dbresult &= db.InsertPRJ_ProjectInvoice(projectInvoice, this.trans);
			}


			//Onaylayıcı yoksa veya talep eden zaten onaylayıcıysa otomatik onay süreci
			if (_approvalPersons.Count() == 0 || _approvalPersons.Contains(this.createdby.Value))
			{
				this.UpdateStatus((int)EnumCMP_RequestStatus.TeklifToplanmasiBekleniyor, _approvalPersons.Where(a => a == this.createdby.Value).FirstOrDefault(), this.trans);
			}

			return dbresult;
		}

		private ResultStatus Update(DbTransaction trans)
		{
			var oldItems = db.GetCMP_InvoiceItemByInvoiceId(this.id);

			var _trans = trans ?? db.BeginTransaction();

			foreach (var item in this.InvoiceItems)
			{
				item.invoiceId = this.id;
				item.created = DateTime.Now;
				item.createdby = this.createdby;
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

		public ResultStatus Cancel(Guid userId, DbTransaction _trans = null)
		{
			db = db ?? new WorkOfTimeDatabase();
			this.trans = _trans ?? db.BeginTransaction();

			var invoice = db.GetCMP_InvoiceById(this.id);

			if (invoice.status >= (Int16)EnumCMP_RequestStatus.FaturasiAlindi)
			{
				return new ResultStatus
				{
					result = false,
					message = "Faturası alınan talepler iptal edilemez."
				};
			}

			invoice.status = (int)EnumCMP_RequestStatus.TalepIptalEdildi;


			var action = new CMP_InvoiceAction
			{
				id = Guid.NewGuid(),
				created = DateTime.Now,
				createdby = userId,
				invoiceId = this.id,
				description = "Talep iptal edildi.",
				type = (short)EnumCMP_InvoiceActionType.TalepIptal
			};

			var dbresult = new ResultStatus { result = true };

			dbresult &= InsertTaskOperation(this.taskId, (int)EnumFTM_TaskOperationStatus.SatinAlmaTalebiIptalEdildi);

			

			dbresult &= db.UpdateCMP_Invoice(invoice, true, this.trans);
			dbresult &= db.InsertCMP_InvoiceAction(action, this.trans);

			if (_trans == null)
			{
				if (dbresult.result)
					this.trans.Commit();
				else
					this.trans.Rollback();
			}

			return dbresult;
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

		public ResultStatus UpdateStatus(int type, Guid userId, DbTransaction _trans = null)
		{
			db = db ?? new WorkOfTimeDatabase();
			this.trans = _trans ?? db.BeginTransaction();
			this.changed = DateTime.Now;
			this.changedby = userId;
			this.status = (short)type;

			var action = new CMP_InvoiceAction
			{
				created = DateTime.Now,
				createdby = this.changedby,
				invoiceId = this.id,
			};

			_managerPersons = db.GetSH_UserByRoleId(new Guid(SHRoles.SatinAlmaPersonel)).ToArray();
			_approvalPersons = db.GetSH_UserByRoleId(_approvalRoleId);

			var requestUser = db.GetVWSH_UserById(this.createdby.Value);
			var managers = new List<VWSH_User>();
			if (_managerPersons.Count() > 0)
			{
				managers = db.GetVWSH_UserByIds(_managerPersons).ToList();
			}

			var approvals = new List<VWSH_User>();
			if (_approvalPersons.Count() > 0)
			{
				approvals = db.GetVWSH_UserByIds(_approvalPersons).ToList();
			}
			if (type == (int)EnumCMP_RequestStatus.TeklifToplanmasiBekleniyor)
			{
				action.type = (short)EnumCMP_InvoiceActionType.TalepOnay;
				action.description = "Satın alma talebi onaylandı, tekliflerin toplanması bekleniyor.";

				foreach (var user in managers)
				{
					var text2 = "<h3>Sayın " + user.FullName + "</h3>";
					text2 += "<p>" + this.rowNumber + " kodlu satın alma talebi için teklif toplama süreci başlamıştır. </p>";
					text2 += "<p>Talep detayını görüntülemek ve işlem yapmak için <a href='" + _siteURL + "/CMP/VWCMP_Request/Detail?id=" + this.id + "'>tıklayınız.</a> </p>";
					text2 += "<p>Bilgilerinize.</p>";
					new Email().Template("Template1", "satinalma.jpg", _tenantName + " | WorkOfTime | Satın Alma Talep Yönetimi", text2).Send((Int16)EmailSendTypes.SatinAlmaTeklif, user.email, "Satın Alma Talebi Teklifler Toplanacak ", true);
				}
			}

			else if (type == (int)EnumCMP_RequestStatus.TalepReddedildi)
			{
				action.type = (short)EnumCMP_InvoiceActionType.TalepRed;
				action.description = "Satın alma talebi reddedildi, süreç sona erdi.";

				var text = "<h3>Sayın " + requestUser.FullName + "</h3>";
				text += "<p>" + this.rowNumber + " kodlu satın alma talebiniz reddedilmiştir.</p>";
				text += "<p>Bilgilerinize.</p>";
				new Email().Template("Template1", "satinalma.jpg", _tenantName + " | WorkOfTime | Satın Alma Talep Yönetimi", text).Send((Int16)EmailSendTypes.SatinAlma, requestUser.email, "Satın Alma Talebi Red", true);
			}

			else if (type == (int)EnumCMP_RequestStatus.TeklifToplandiOnayBekleniyor)
			{
				action.type = (short)EnumCMP_InvoiceActionType.TalepTekliflerOnayaSunuldu;
				action.description = "Teklifler toplandı ve onaya sunuldu.";

				foreach (var user in approvals)
				{
					var text2 = "<h3>Sayın " + user.FullName + "</h3>";
					text2 += "<p>" + this.rowNumber + " kodlu satın alma talebinin teklifleri hazırlanmış ve onayınıza sunulmuştur. </p>";
					text2 += "<p>Talep detayını görüntülemek ve işlem yapmak için <a href='" + _siteURL + "/CMP/VWCMP_Request/Detail?id=" + this.id + "'>tıklayınız.</a> </p>";
					text2 += "<p>Bilgilerinize.</p>";
					new Email().Template("Template1", "satinalma.jpg", _tenantName + " | WorkOfTime | Satın Alma Talep Yönetimi", text2).Send((Int16)EmailSendTypes.SatinAlmaTeklif, user.email, "Satın Alma Talebi Teklifler Onaya Sunuldu", true);
				}
			}

			else if (type == (int)EnumCMP_RequestStatus.YeniTeklifToplanmasiBekleniyor)
			{
				action.type = (short)EnumCMP_InvoiceActionType.TeklifTekrar;
				action.description = "Tekliflerin satın alma onaylayıcısı tarafından revize edilmesi istendi.";
				this.status = (int)EnumCMP_RequestStatus.TeklifToplanmasiBekleniyor;

				foreach (var user in managers)
				{
					var text2 = "<h3>Sayın " + user.FullName + "</h3>";
					text2 += "<p>" + this.rowNumber + " kodlu satın alma talebi için tekrar teklif toplanması istenmektedir. </p>";
					text2 += "<p>Talep detayını görüntülemek ve işlem yapmak için <a href='" + _siteURL + "/CMP/VWCMP_Request/Detail?id=" + this.id + "'>tıklayınız.</a> </p>";
					text2 += "<p>Bilgilerinize.</p>";
					new Email().Template("Template1", "satinalma.jpg", _tenantName + " | WorkOfTime | Satın Alma Talep Yönetimi", text2).Send((Int16)EmailSendTypes.SatinAlmaTeklif, user.email, "Satın Alma Talebi Teklifler Toplanacak ", true);
				}
			}

			else if (type == (int)EnumCMP_RequestStatus.TeklifOnaylandi)
			{
				action.type = (int)EnumCMP_InvoiceActionType.TalepTeklifOnay;
				action.description = "Talebin Teklifi Onaylandı";

				var muhasebe = db.GetSH_UserByRoleId(new Guid(SHRoles.OnMuhasebe)).ToArray();
				var users = new List<VWSH_User>();
				if (muhasebe.Count() > 0)
				{
					users = db.GetVWSH_UserByIds(muhasebe).ToList();
				}

				var tenderTransform = db.GetVWCMP_InvoiceTransformByIsTransformedFrom(this.id).Select(a => a.invoiceIdTo).FirstOrDefault();

				foreach (var muhasebeUser in users)
				{
					var text = "<h3>Sayın " + muhasebeUser.FullName + "</h3>";
					text += "<p>" + this.rowNumber + " kodlu teklif onaylanmıştır. </p>";
					text += "<p>Teklif detayını görüntülemek ve işlem yapmak için <a href='" + _siteURL + "/CMP/VWCMP_Tender/DetailBuying?id=" + tenderTransform + "'>tıklayınız.</a> </p>";
					text += "<p>Bilgilerinize.</p>";
					new Email().Template("Template1", "satinalma.jpg", _tenantName + " | WorkOfTime | Satın Alma Yönetimi", text).Send((Int16)EmailSendTypes.SatinAlmaTeklif, muhasebeUser.email, "Teklif Onaylandı", true);
				}
			}

			else
			{
				action.type = (int)EnumCMP_InvoiceActionType.TalepFatura;
				action.description = "Talebin Faturası Kesildi";
			}

			var dbresult = new ResultStatus { result = true };

			if (type == (int)EnumCMP_RequestStatus.TalepReddedildi)
			{
				dbresult &= InsertTaskOperation(this.taskId, (int)EnumFTM_TaskOperationStatus.SatinAlmaTalebiIptalEdildi);
			}

			if (type == (int)EnumCMP_InvoiceActionType.TalepOnay)
			{
				dbresult &= InsertTaskOperation(this.taskId, (int)EnumFTM_TaskOperationStatus.SatinAlmaTalebiOnaylandi);
			}

			dbresult &= db.UpdateCMP_Invoice(new CMP_Invoice().B_EntityDataCopyForMaterial(this), false, this.trans);
			dbresult &= db.InsertCMP_InvoiceAction(action, this.trans);

			if (type == (int)EnumCMP_RequestStatus.YeniTeklifToplanmasiBekleniyor)
			{
				var tenderTransforms = db.GetCMP_InvoiceTransformByIsTransformedFrom(this.id).Where(a => a.invoiceIdTo.HasValue).Select(a => a.invoiceIdTo.Value).ToArray();
				var tenders = new List<CMP_Invoice>();
				if (tenderTransforms.Count() > 0)
				{
					tenders = db.GetCMP_InvoiceByIds(tenderTransforms).ToList();
				}

				foreach (var tender in tenders)
				{
					tender.status = (int)EnumCMP_TenderStatus.CevapBekleniyor;
				}

				dbresult &= db.BulkUpdateCMP_Invoice(tenders, false, this.trans);
			}

			var rs = new ResultStatus
			{
				result = dbresult.result,
				objects = action,
			};

			if (_trans == null)
			{
				if (dbresult.result)
				{
					this.trans.Commit();
					rs.message = type == (int)EnumCMP_RequestStatus.TeklifToplanmasiBekleniyor ? "Satın alma talebi onaylama işlemi başarılı." : "Satın alma talebi onaylama işlemi başarısız.";
				}
				else
				{
					this.trans.Rollback();
					rs.message = type == (int)EnumCMP_RequestStatus.TeklifToplanmasiBekleniyor ? "Satın alma talebi reddetme işlemi başarılı." : "Satın alma talebi reddetme işlemi başarısız.";
				}
			}

			return rs;
		}

		public RequestFormTemplateModels GetFormTemplate(int? type, VWSH_User user)
		{
			var invoice = db.GetVWCMP_RequestById(this.id);
			var model = new RequestFormTemplateModels().B_EntityDataCopyForMaterial(invoice);
			model.typeJob = type.HasValue ? type.Value : type;
			model.User = user;
			model.Items = db.GetVWCMP_InvoiceItemByInvoiceId(this.id);
			model.Customer = this.customerId.HasValue ? db.GetVWCMP_CompanyById(this.customerId.Value) : null;

			return model;
		}


		public ResultStatus InsertConfirmationTask(Guid userId, Guid taskId, DbTransaction trans = null)
		{
			var dbresult = new ResultStatus { result = true };
			var _trans = trans ?? db.BeginTransaction();
			this.db = this.db ?? new WorkOfTimeDatabase();
			var invoiceCofirmations = new List<CMP_InvoiceConfirmation>();

			var rulesUser = db.GetVWUT_RulesUserByUserIdAndType(userId, (Int16)EnumUT_RulesType.Task);
			var task = db.GetFTM_TaskById(taskId);

			if (rulesUser != null && task != null)
			{
				var shuser = db.GetVWSH_UserById(userId);
				if (shuser != null)
				{
					this._managerPersons = new Guid[1] { shuser.id };
					this._approvalPersons = new Guid[1] { task.createdby.Value};

					invoiceCofirmations.Add(new CMP_InvoiceConfirmation
					{
						created = this.created,
						createdby = userId,
						advanceId = this.id,
						ruleType = (int)EnumUT_RulesUserStage.SecimeBagliKullanici,
						ruleOrder = 1,
						status = 0,
						ruleUserId = task.createdby.Value,
						userId = shuser.id
					});

					
					dbresult &= db.BulkInsertCMP_InvoiceConfirmation(invoiceCofirmations, _trans);
				}
			}
			else
			{
				return new ResultStatus
				{
					result = false,
					message = "Hiç bir satın alma kuralı bulunamadı."
				};
			}

			return new ResultStatus
			{
				result = dbresult.result,
				message = dbresult.result ? "Kayıt başarılı bir şekilde gerçekleştirildi." : "Kayıt başarısız oldu."
			};
		}


		public ResultStatus InsertTaskOperation(Guid? taskId, short? status)
		{
			var dbresult = new ResultStatus { result = true };

			if (this.taskId.HasValue)
			{
				if (status == (int)EnumFTM_TaskOperationStatus.SatinAlmaTalebiYapildi)
				{
					var buyRequestOperation = new FTM_TaskOperation
					{
						taskId = taskId,
						created = DateTime.Now,
						status = (int)EnumFTM_TaskOperationStatus.SatinAlmaTalebiYapildi,
						createdby = this.createdby,
						description =  this.description,
					};

					dbresult &= db.InsertFTM_TaskOperation(buyRequestOperation, this.trans);
				}


				if (status == (int)EnumFTM_TaskOperationStatus.SatinAlmaTalebiIptalEdildi)
				{
					var buyRequestOperation = new FTM_TaskOperation()
					{
						taskId = this.taskId,
						created = DateTime.Now,
						status = status,
						createdby = this.createdby,
						description = "Satın Alma Talebi İptal Edildi."
					};

					dbresult &= db.InsertFTM_TaskOperation(buyRequestOperation, this.trans);
				}


				if (status == (int)EnumFTM_TaskOperationStatus.SatinAlmaTalebiOnaylandi)
				{
					var buyRequestOperation = new FTM_TaskOperation()
					{
						taskId = this.taskId,
						created = DateTime.Now,
						status = status,
						createdby = this.createdby,
						description = "Satın Alma Talebi Onaylandı."
					};

					dbresult &= db.InsertFTM_TaskOperation(buyRequestOperation, this.trans);
				}

				if (status == (int)EnumFTM_TaskOperationStatus.SatinAlmaTalebiReddedildi)
				{
					var buyRequestOperation = new FTM_TaskOperation()
					{
						taskId = this.taskId,
						created = DateTime.Now,
						status = status,
						createdby = this.createdby,
						description = "Satın Alma Talebi Reddedildi."
					};

					dbresult &= db.InsertFTM_TaskOperation(buyRequestOperation, this.trans);
				}
			}

			return new ResultStatus
			{
				result = dbresult.result
			};
		}
	}
}
