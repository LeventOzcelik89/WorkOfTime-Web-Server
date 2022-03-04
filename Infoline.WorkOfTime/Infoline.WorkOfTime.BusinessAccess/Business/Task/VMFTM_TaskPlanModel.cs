using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Globalization;
using System.Linq;

namespace Infoline.WorkOfTime.BusinessAccess
{
	public class VMFTM_TaskPlanModel : VWFTM_TaskPlan
	{
		public WorkOfTimeDatabase db { get; set; }
		private DbTransaction trans { get; set; }
		private bool loadedTasks { get; set; } = false;

		public List<string> _Times { get; set; }
		public List<string> _TimesData { get; set; } = new List<string>();

		public List<string> _WeekDays { get; set; }
		public List<string> _WeekDaysData { get; set; } = new List<string>();

		public VWFTM_TaskTemplate TaskTemplate { get; set; }

		public VMFTM_TaskPlanModel Load()
		{

			this.db = this.db ?? new WorkOfTimeDatabase();
			var task = db.GetVWFTM_TaskPlanById(this.id);

			if (task != null)
			{

				this.B_EntityDataCopyForMaterial(task, true);

				if (!String.IsNullOrEmpty(this.weekDays))
				{
					_WeekDays = this.weekDays.Split(',').ToList();
				}

				if (!String.IsNullOrEmpty(this.times))
				{
					_Times = this.times.Split(',').ToList();
				}

				if (this.templateId != null)
				{
					this.TaskTemplate = db.GetVWFTM_TaskTemplateById(this.templateId.Value);
				}

			}

			var timeCounter = new DateTime(2000, 1, 1, 0, 0, 0);
			while (timeCounter < new DateTime(2000, 1, 2, 0, 0, 0))
			{
				this._TimesData.Add(timeCounter.ToString("HH:mm"));
				timeCounter = timeCounter.AddMinutes(30);
			}

			_WeekDaysData.Add("Pazartesi");
			_WeekDaysData.Add("Salı");
			_WeekDaysData.Add("Çarşamba");
			_WeekDaysData.Add("Perşembe");
			_WeekDaysData.Add("Cuma");
			_WeekDaysData.Add("Cumartesi");
			_WeekDaysData.Add("Pazar");



			return this;

		}

		public ResultStatus Save(Guid userid, DbTransaction _trans = null)
		{

			this.db = this.db ?? new WorkOfTimeDatabase();
			this.trans = _trans;

			if (this._Times != null)
			{
				this.times = String.Join(",", this._Times);
			}

			if (this._WeekDays != null)
			{
				this.weekDays = String.Join(",", this._WeekDays);
			}

			var dbRes = new ResultStatus { result = true };
			var plan = db.GetFTM_TaskPlanById(this.id);

			if (this.frequencyStartDate == this.frequencyEndDate)
			{
				this.frequencyEndDate = this.frequencyEndDate.Value.AddDays(1).AddSeconds(-1);
			}

			if (plan == null)
			{

				this.created = DateTime.Now;
				this.createdby = userid;

				dbRes = db.InsertFTM_TaskPlan(this.B_ConvertType<FTM_TaskPlan>(), this.trans);

				if (dbRes.result)
				{
					dbRes.message = "Plan başarıyla oluşturuldu.";
				}
				else
				{
					dbRes.message = "Plan oluşturulamadı !";
					return dbRes;
				}

			}
			else
			{

				this.changed = DateTime.Now;
				this.changedby = userid;

				var tasks = db.GetVWFTM_TaskByTaskPlanId(this.id);
				var canRemovedTasks = new int[] {
					(int)EnumFTM_TaskOperationStatus.GorevOlusturuldu,
					(int)EnumFTM_TaskOperationStatus.GorevOlusturulduMusteri,
					(int)EnumFTM_TaskOperationStatus.GorevOlusturulduSistem,
					(int)EnumFTM_TaskOperationStatus.PersonelAtamaYapildi,
					(int)EnumFTM_TaskOperationStatus.DogrulamaKoduGonderildi,
					(int)EnumFTM_TaskOperationStatus.GorevUstlenildi
				};

				foreach (var task in tasks.Where(a => a.lastOperationStatus.HasValue && canRemovedTasks.Contains(a.lastOperationStatus.Value)))
				{
					var _task = task.B_ConvertType<VMFTM_TaskModel>();
					dbRes &= _task.Delete((Guid?)null);
				}

				dbRes &= db.UpdateFTM_TaskPlan(this.B_ConvertType<FTM_TaskPlan>(), false, this.trans);

				if (dbRes.result)
				{
					dbRes.message = "Plan başarıyla güncellendi. İşlem yapılmamış görevler silindi.";
				}
				else
				{
					dbRes.message = "Plan güncellenemedi !";
					return dbRes;
				}

			}

			if (taskCreationTime == (int)EnumFTM_TaskPlansTaskCreationTime.Hemen)
			{

				var schedulerModel = new TaskSchedulerModel();
				schedulerModel.Load(this.id);

				if (schedulerModel.TemplateModel.taskTemplateInventories.Count() == 0)
				{

					var tasks = this.GetTasks();

					foreach (var task in tasks)
					{

						var _task = task
							.B_ConvertType<FTM_Task>()
							.B_ConvertType<VMFTM_TaskModel>()
							.AppendObjectToOther(schedulerModel.TemplateModel);

						_task.id = Guid.NewGuid();
						_task.code = BusinessExtensions.B_GetIdCode();
						_task.FTM_TaskSubjectTypeIds = schedulerModel.TemplateModel.FTM_TaskTemplateSubjectTypeIds;

						dbRes &= _task.InsertAll(Guid.Empty);

					}
					if (dbRes.result)
					{
						dbRes.message = "Plan başarıyla güncellendi. İşlem yapılmamış görevler silindi. Görev/ler başarıyla oluşturuldu.";
					}
					else
					{
						dbRes.message = "Görev oluşturma işlemi başarısız oldu.";
						return dbRes;
					}
				}
				else
				{

					foreach (var inventoryId in schedulerModel.TemplateModel.taskTemplateInventories)
					{

						var tasks = this.GetTasks();

						foreach (var task in tasks)
						{

							var _task = task
								.B_ConvertType<FTM_Task>()
								.B_ConvertType<VMFTM_TaskModel>()
								.AppendObjectToOther(schedulerModel.TemplateModel);

							_task.id = Guid.NewGuid();
							_task.code = BusinessExtensions.B_GetIdCode();
							_task.fixtureId = inventoryId.inventoryId;
							_task.FTM_TaskSubjectTypeIds = schedulerModel.TemplateModel.FTM_TaskTemplateSubjectTypeIds;

							dbRes &= _task.InsertAll(Guid.Empty);

						}
						if (dbRes.result)
						{
							dbRes.message = "Plan başarıyla güncellendi. İşlem yapılmamış görevler silindi. Görev/ler başarıyla oluşturuldu.";
						}
						else
						{
							dbRes.message = "Görev oluşturma işlemi başarısız oldu.";
							return dbRes;
						}
					}

				}

			}

			return dbRes;

		}

		public ResultStatus Delete()
		{

			this.db = this.db ?? new WorkOfTimeDatabase();

			var tasks = db.GetVWFTM_TaskByTaskPlanId(this.id);
			var rs = db.DeleteFTM_TaskPlan(this.id);

			if (!rs.result)
			{
				return rs;
			}

			foreach (var task in tasks)
			{
				var _task = task.B_ConvertType<VMFTM_TaskModel>();

				if (!_task.lastOperationStatus.HasValue)
				{
					continue;
				}

				var canRemovedTasks = new int[] {
					(int)EnumFTM_TaskOperationStatus.GorevOlusturuldu,
					(int)EnumFTM_TaskOperationStatus.GorevOlusturulduMusteri,
					(int)EnumFTM_TaskOperationStatus.GorevOlusturulduSistem,
					(int)EnumFTM_TaskOperationStatus.PersonelAtamaYapildi,
					(int)EnumFTM_TaskOperationStatus.DogrulamaKoduGonderildi,
					(int)EnumFTM_TaskOperationStatus.GorevUstlenildi
				};

				if (canRemovedTasks.Contains(_task.lastOperationStatus.Value))
				{
					rs &= _task.Delete((Guid?)null);
				}

			}

			if (rs.result)
			{
				rs.message = "Planlanmış Görev başarıyla silindi.";
			}
			else
			{
				rs.message = "Planlanmış Görev silinemedi.";
			}

			return rs;

		}

		public VMFTM_TaskPlanCalendarModel[] CalendarDataSource(List<Guid> userIds, PageSecurity userStatus)
		{
			this.db = this.db ?? new WorkOfTimeDatabase();
			var query = "SELECT id,lastOperationDate,created,changed,closingDate,code,taskPlanId,customer_Title,customerStorage_Title,fixture_Title,planStartDate,dueDate,priority_Title,plate,priority,lastOperationStatus,assignUserId,assignableUserIds,isComplete,taskPlanId_Title,type_Title,description,penaltyStartDate,amercementTotal,SLAText,assignableUserTitles,taskSubjectType_Title,planLater FROM VWFTM_Task WITH (NOLOCK) WHERE DATEFROMPARTS(YEAR(lastOperationDate), MONTH(lastOperationDate), DAY(lastOperationDate)) = DATEFROMPARTS(YEAR(GETDATE()), MONTH(GETDATE()), DAY(GETDATE()))";

			if (userIds.Where(a=>a == Guid.Empty).Count() == 0)
			{
				query += " AND (assignUserId IN (";
				var count = 0;
				foreach (var item in userIds)
				{
					count ++;

					if(count == userIds.Count())
					{
						query += String.Format("'{0}'", item);
					}
					else
					{
						query += String.Format("'{0}',", item);
					}
				}

				query += ")";

				count = 0;
				foreach (var userId in userIds)
				{
					count++;

					if (count == userIds.Count())
					{
						query += " OR (assignableUserIds LIKE '%" + userId + "%' ))";
					}
					else
					{
						query += " OR (assignableUserIds LIKE '%" + userId + "%' )";
					}
				}
			}

			var dbtasks = db.GetVWFTM_TaskByQuery(query).B_ConvertType<VMFTM_TaskPlanCalendarModel>();

			if (userStatus.AuthorizedRoles.Contains(new Guid(SHRoles.SahaGorevYonetici)) || userStatus.AuthorizedRoles.Contains(new Guid(SHRoles.SahaGorevOperator)))
			{
				var authoritys = db.GetVWFTM_TaskAuthorityByUserId(userStatus.user.id);
				if (authoritys.Any())
				{
					dbtasks = dbtasks.Where(x => authoritys.Where(f => f.customerId.HasValue).Select(f => f.customerId.Value).ToArray().Contains(x.customerId.Value)).ToArray();
				}
			}

			//var newTasks = new int[] {
			//		(int)EnumFTM_TaskOperationStatus.GorevOlusturuldu,
			//		(int)EnumFTM_TaskOperationStatus.GorevOlusturulduMusteri,
			//		(int)EnumFTM_TaskOperationStatus.GorevOlusturulduSistem,
			//		(int)EnumFTM_TaskOperationStatus.PersonelAtamaYapildi,
			//		(int)EnumFTM_TaskOperationStatus.DogrulamaKoduGonderildi,
			//		(int)EnumFTM_TaskOperationStatus.GorevUstlenildi
			//	};

			//dbtasks.Where(a => a.lastOperationStatus.HasValue && newTasks.Contains(a.lastOperationStatus.Value)).ToList().ForEach(a =>
			//{
			//	a.lastOperationDate = null;
			//});

			var tasks = new List<VMFTM_TaskPlanCalendarModel>();
			tasks.AddRange(dbtasks);
			return tasks.ToArray();

		}

		public VMFTM_TaskPlanCalendarModel[] CalendarNewDataSource(PageSecurity userStatus)
		{
			this.db = this.db ?? new WorkOfTimeDatabase();
			var query = "SELECT id,code,customer_Title,customerStorage_Title,fixture_Title,taskPlanId_Title,priority_Title,plate,priority,lastOperationStatus,assignUserId,assignableUserIds,isComplete,taskPlanId_Title,type_Title,description,penaltyStartDate,amercementTotal,SLAText,assignableUserTitles,taskSubjectType_Title,planLater FROM VWFTM_Task WITH (NOLOCK) WHERE lastOperationStatus < " + (int)EnumFTM_TaskOperationStatus.GorevBaslandi + "AND assignUserId IS NULL AND assignableUserIds IS NULL AND dueDate >= GETDATE()";
			var dbtasks = db.GetVWFTM_TaskByQuery(query).B_ConvertType<VMFTM_TaskPlanCalendarModel>();

			if (userStatus.AuthorizedRoles.Contains(new Guid(SHRoles.SahaGorevYonetici)) || userStatus.AuthorizedRoles.Contains(new Guid(SHRoles.SahaGorevOperator)))
			{
				var authoritys = db.GetVWFTM_TaskAuthorityByUserId(userStatus.user.id);
				if (authoritys.Any())
				{
					dbtasks = dbtasks.Where(x => authoritys.Where(f => f.customerId.HasValue).Select(f => f.customerId.Value).ToArray().Contains(x.customerId.Value)).ToArray();
				}
			}

			var newTasks = new int[] {
					(int)EnumFTM_TaskOperationStatus.GorevOlusturuldu,
					(int)EnumFTM_TaskOperationStatus.GorevOlusturulduMusteri,
					(int)EnumFTM_TaskOperationStatus.GorevOlusturulduSistem,
					(int)EnumFTM_TaskOperationStatus.PersonelAtamaYapildi,
					(int)EnumFTM_TaskOperationStatus.DogrulamaKoduGonderildi,
					(int)EnumFTM_TaskOperationStatus.GorevUstlenildi
				};

			dbtasks.Where(a => a.lastOperationStatus.HasValue && newTasks.Contains(a.lastOperationStatus.Value)).ToList().ForEach(a =>
			{
				a.lastOperationDate = null;
			});

			var tasks = new List<VMFTM_TaskPlanCalendarModel>();
			tasks.AddRange(dbtasks);
			return tasks.ToArray();
		}


		public VMFTM_TaskPlanCalendarModel[] CalendarDataSourceByYear(PageSecurity userStatus, int year, Guid? customerId, Guid? planId)
		{

			this.db = this.db ?? new WorkOfTimeDatabase();

			var plans = db.GetVWFTM_TaskPlan()
				.Where(a => a.enabled == true && a.frequencyEndDate.HasValue && a.frequencyEndDate.Value.Year == year)
				.ToList();

			if (customerId.HasValue)
			{
				plans = plans.Where(a => a.customerId == customerId.Value).ToList();
			}

			if (planId.HasValue)
			{
				plans = plans.Where(a => a.id == planId.Value).ToList();
			}


			//  hali hazırda açılmış görevler
			var dbtasks = db.GetVWFTM_TaskByDueDateYear(year, customerId, planId).B_ConvertType<VMFTM_TaskPlanCalendarModel>();

			if (userStatus.AuthorizedRoles.Contains(new Guid(SHRoles.SahaGorevYonetici)) || userStatus.AuthorizedRoles.Contains(new Guid(SHRoles.SahaGorevOperator)))
			{
				var authoritys = db.GetVWFTM_TaskAuthorityByUserId(userStatus.user.id);
				if (authoritys.Count() > 0)
				{
					dbtasks = dbtasks.Where(x => authoritys.Where(f => f.customerId.HasValue).Select(f => f.customerId.Value).ToArray().Contains(x.customerId.Value)).ToArray();

					plans = plans.Where(x => dbtasks.Where(c => c.taskTemplateId.HasValue).Select(c => c.taskTemplateId.Value).ToArray().Contains(x.id)).ToList();
				}
			}

			var newTasks = new int[] {
					(int)EnumFTM_TaskOperationStatus.GorevOlusturuldu,
					(int)EnumFTM_TaskOperationStatus.GorevOlusturulduMusteri,
					(int)EnumFTM_TaskOperationStatus.GorevOlusturulduSistem,
					(int)EnumFTM_TaskOperationStatus.PersonelAtamaYapildi,
					(int)EnumFTM_TaskOperationStatus.DogrulamaKoduGonderildi,
					(int)EnumFTM_TaskOperationStatus.GorevUstlenildi
				};

			dbtasks.Where(a => a.lastOperationStatus.HasValue && newTasks.Contains(a.lastOperationStatus.Value)).ToList().ForEach(a =>
			{
				a.lastOperationDate = null;
			});

			var tasks = new List<VMFTM_TaskPlanCalendarModel>();
			foreach (var plan in plans)
			{
				tasks.AddRange(TaskCalendarDataSource(plan, userStatus));
			}

			tasks.AddRange(dbtasks);

			return tasks.ToArray();

		}


		public VMFTM_TaskPlanCalendarModel[] TaskCalendarDataSource(VWFTM_TaskPlan plan, PageSecurity userStatus)
		{

			this.db = this.db ?? new WorkOfTimeDatabase();

			var tasks = new List<VMFTM_TaskPlanCalendarModel>();

			//  hali hazırda açılmış görevler
			var dbtasks = db.GetVWFTM_TaskByTaskPlanId(plan.id).B_ConvertType<VMFTM_TaskPlanCalendarModel>();

			if (userStatus.AuthorizedRoles.Contains(new Guid(SHRoles.SahaGorevYonetici)) || userStatus.AuthorizedRoles.Contains(new Guid(SHRoles.SahaGorevOperator)))
			{
				var authoritys = db.GetVWFTM_TaskAuthorityByUserId(userStatus.user.id);
				if (authoritys.Count() > 0)
					dbtasks = dbtasks.Where(x => authoritys.Where(f => f.customerId.HasValue).Select(f => f.customerId.Value).ToArray().Contains(x.customerId.Value)).ToArray();
			}

			var newTasks = new int[] {
					(int)EnumFTM_TaskOperationStatus.GorevOlusturuldu,
					(int)EnumFTM_TaskOperationStatus.GorevOlusturulduMusteri,
					(int)EnumFTM_TaskOperationStatus.GorevOlusturulduSistem,
					(int)EnumFTM_TaskOperationStatus.PersonelAtamaYapildi,
					(int)EnumFTM_TaskOperationStatus.DogrulamaKoduGonderildi,
					(int)EnumFTM_TaskOperationStatus.GorevUstlenildi
				};

			dbtasks.Where(a => a.lastOperationStatus.HasValue && newTasks.Contains(a.lastOperationStatus.Value)).ToList().ForEach(a =>
			{
				a.lastOperationDate = null;
			});

			tasks.AddRange(dbtasks);

			if (!plan.taskCreationTime.HasValue)
			{
				return tasks.ToArray();
			}

			switch ((EnumFTM_TaskPlansTaskCreationTime)plan.taskCreationTime)
			{

				case EnumFTM_TaskPlansTaskCreationTime.Hemen:

					//  Hemen dediklerimiz zaten fiziki olarak açıldı.
					return tasks.ToArray();

				case EnumFTM_TaskPlansTaskCreationTime.AyOnce:

					var tasks_1 = plan.B_ConvertType<VMFTM_TaskPlanModel>().GetTasks()
						.Where(a => a.planStartDate.HasValue && a.planStartDate.Value > DateTime.Now.AddDays(30)).ToArray();

					tasks.AddRange(tasks_1);

					break;
				case EnumFTM_TaskPlansTaskCreationTime.HaftaOnce:

					var tasks_2 = plan.B_ConvertType<VMFTM_TaskPlanModel>().GetTasks()
						.Where(a => a.planStartDate.HasValue && a.planStartDate.Value > DateTime.Now.AddDays(7)).ToArray();

					tasks.AddRange(tasks_2);

					break;
				case EnumFTM_TaskPlansTaskCreationTime.GunOnce:
				case EnumFTM_TaskPlansTaskCreationTime.Gununde:

					var tasks_3 = plan.B_ConvertType<VMFTM_TaskPlanModel>().GetTasks()
						.Where(a => a.planStartDate.HasValue && a.planStartDate.Value > DateTime.Now.AddDays(1)).ToArray();

					tasks.AddRange(tasks_3);

					break;

				default:

					break;

			}

			return tasks.ToArray();

		}


		public VMFTM_TaskPlanCalendarModel[] _TaskList { get; set; } = new List<VMFTM_TaskPlanCalendarModel>().ToArray();


		//  Görev Plan Listesinde [Takvim Üzerinde Göster] Fonksiyonu eklenebilir. Public kalması ondan sebep.
		public VMFTM_TaskPlanCalendarModel[] GetTasks()
		{

			if (this.loadedTasks == false)
			{
				this.LoadTasks();
			}

			return this._TaskList;

		}

		public VMFTM_TaskPlanCalendarModel[] LoadTasks(DateTime? lastDate = null)
		{

			lastDate = lastDate ?? DateTime.Now;

			this.loadedTasks = true;

			var res = new List<VWFTM_Task>();
			var taskTimes = new List<DateTime>();
			this.db = this.db ?? new WorkOfTimeDatabase();
			this.Load();

			if (!this.frequencyStartDate.HasValue || !this.frequencyEndDate.HasValue || !this.frequencyInterval.HasValue || !this.templateId.HasValue)
			{
				return new VMFTM_TaskPlanCalendarModel[0];
			}

			if (this.TaskTemplate == null)
			{
				return new VMFTM_TaskPlanCalendarModel[0];
			}

			var end = this.frequencyEndDate.Value;
			var start = this.frequencyStartDate.Value;

			while (start <= end)
			{

				switch ((EnumFTM_TaskPlansFrequency)this.frequency)
				{

					case EnumFTM_TaskPlansFrequency.Gunluk:
						{
							var times = this.times.Split(',')
								.Select(a => DateTime.ParseExact(start.ToString("yyyy-MM-dd") + " " + a.Trim(), "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture))
								.ToArray();

							taskTimes.AddRange(times);

							start = start.AddDays(this.frequencyInterval.Value);
							break;
						}

					case EnumFTM_TaskPlansFrequency.Haftalik:
						{
							if (start.DayOfWeek == DayOfWeek.Monday)
							{

								var _times = this.weekDays.Split(',').Select(a =>
								{
									var day = a.Trim();
									var dayOfWeek = (DayOfWeek)new CultureInfo("tr-TR").DateTimeFormat.DayNames.ToList().IndexOf(day);
									return DateTime.ParseExact(start.AddDays((int)dayOfWeek - 1).ToString("yyyy-MM-dd") + " " + this.times.Trim(), "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
								}).ToArray();

								taskTimes.AddRange(_times);

								start = start.AddDays(7 * this.frequencyInterval.Value);

							}
							else
							{
								start = start.AddDays((-1) * ((int)start.DayOfWeek - 1)).AddDays(-7);
							}
							break;
						}

					case EnumFTM_TaskPlansFrequency.Aylik:
						{
							//var dateCorrect = Int32.Parse(this.monthDays);
							//var daysinMonth = DateTime.DaysInMonth(start.Year, start.Month);
							//dateCorrect = daysinMonth <= dateCorrect ? daysinMonth : dateCorrect;

							var __times = this.monthDays.ToString().Split(',').Select(a =>
							{
								var dateCorrect = Int32.Parse(a);
								var daysinMonth = DateTime.DaysInMonth(start.Year, start.Month);
								dateCorrect = daysinMonth < dateCorrect ? daysinMonth : dateCorrect;
								return DateTime.ParseExact(start.ToString("yyyy-MM-") + dateCorrect.ToString().Trim().PadLeft(2, '0') + " " + this.times.Trim(), "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
							});

							taskTimes.AddRange(__times);

							start = start.AddMonths(this.frequencyInterval.Value);

							break;
						}

					default:
						break;

				}

			}

			var tasks = taskTimes
				.Where(a => a > lastDate && a < end)
				.Select(a =>
				{

					var task = TaskTemplate.B_ConvertType<VWFTM_Task>().B_ConvertType<VMFTM_TaskPlanCalendarModel>();

					task.id = Guid.NewGuid();
					task.created = DateTime.Now;
					task.createdby = this.createdby;
					task.changed = null;
					task.changedby = null;
					task.code = BusinessExtensions.B_GetIdCode();

					task.planStartDate = a;
					task.IsTemplate = true;
					task.lastOperationDate = null;
					task.taskPlanId = this.id;
					task.taskTemplateId = this.templateId;

					task.dueDate = a.AddMinutes(TaskTemplate.estimatedTaskMinute.Value);

					return task;

				}).ToArray();

			this._TaskList = tasks;

			return this._TaskList;

		}

	}

}
