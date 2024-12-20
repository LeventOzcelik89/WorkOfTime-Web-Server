﻿using Infoline.Framework.Database;
using Infoline.Helper;
using Infoline.WorkOfTime.BusinessAccess.RecurringDateModel;
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
		public DateTime? planStartDate { get; set; }
		public DateTime? dueDate { get; set; }
		private DbTransaction trans { get; set; }
		private bool loadedTasks { get; set; } = false;
		public List<string> _Times { get; set; }
		public List<string> _TimesData { get; set; } = new List<string>();
		public List<string> _WeekDays { get; set; }
		public List<string> _WeekDaysData { get; set; } = new List<string>();
		public List<string> _MonthFrequency { get; set; } = new List<string>();
		public List<string> _MonthFrequenciesData { get; set; } = new List<string>();
		public List<string> _DayFrequency { get; set; } = new List<string>();
		public List<string> _DayFrequenciesData { get; set; } = new List<string>();
		public List<VWFTM_TaskUser> taskUsers { get; set; } = new List<VWFTM_TaskUser>();
		public List<VWFTM_TaskUser> taskHelperUsers { get; set; } = new List<VWFTM_TaskUser>();


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


				if (this.dayFrequency.HasValue)
				{
					var dayFrequencyEnum = EnumsProperties.EnumToArrayValues<EnumFTM_TaskPlanDayFrequency>().Where(a => a.Key == this.dayFrequency.Value.ToString()).Select(b => b.Value).FirstOrDefault();
					this._DayFrequency.Add(dayFrequencyEnum);
				}

				if (!string.IsNullOrEmpty(this.assignableUserIds))
				{
					var assignableUserIds = this.assignableUserIds.Split(',').Select(a => new Guid(a)).ToArray();

					var assignableUsers = db.GetVWSH_UserByIds(assignableUserIds.ToArray());

					taskUsers = assignableUsers.Select(a => new VWFTM_TaskUser
					{
						created = (DateTime.Now),
						userId = a.id,
						userId_Title = a.FullName,
						photo = a.ProfilePhoto
					}).ToList();
				}

				if (!string.IsNullOrEmpty(this.helperUserIds))
				{
					var helperUserIds = this.helperUserIds.Split(',').Select(a => new Guid(a)).ToArray();

					var assignableUsers = db.GetVWSH_UserByIds(helperUserIds.ToArray());

					taskHelperUsers = assignableUsers.Select(a => new VWFTM_TaskUser
					{
						created = (DateTime.Now),
						userId = a.id,
						userId_Title = a.FullName,
						photo = a.ProfilePhoto
					}).ToList();
				}

				if (this.monthFrequency.HasValue)
				{
					var monthFrequencyEnum = EnumsProperties.EnumToArrayValues<EnumFTM_TaskPlanMonthFrequency>().Where(a => a.Key == this.monthFrequency.Value.ToString()).Select(b => b.Value).FirstOrDefault();
					this._MonthFrequency.Add(monthFrequencyEnum);
				}
			}

			if (!this.frequencyInterval.HasValue)
			{
				this.frequencyInterval = 1;
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


			_MonthFrequenciesData.Add("İlk");
			_MonthFrequenciesData.Add("İkinci");
			_MonthFrequenciesData.Add("Üçüncü");
			_MonthFrequenciesData.Add("Dördüncü");
			_MonthFrequenciesData.Add("Son");

			_DayFrequenciesData.Add("Gün");
			_DayFrequenciesData.Add("Pazar");
			_DayFrequenciesData.Add("Pazartesi");
			_DayFrequenciesData.Add("Salı");
			_DayFrequenciesData.Add("Çarşamba");
			_DayFrequenciesData.Add("Perşembe");
			_DayFrequenciesData.Add("Cuma");
			_DayFrequenciesData.Add("Cumartesi");

			return this;

		}

		public VMFTM_TaskPlanModel LoadPlan(VWFTM_TaskPlan plan)
		{

			this.db = this.db ?? new WorkOfTimeDatabase();

			if (plan != null)
			{
				this.B_EntityDataCopyForMaterial(plan, true);

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


				if (this.dayFrequency.HasValue)
				{
					var dayFrequencyEnum = EnumsProperties.EnumToArrayValues<EnumFTM_TaskPlanDayFrequency>().Where(a => a.Key == this.dayFrequency.Value.ToString()).Select(b => b.Value).FirstOrDefault();
					this._DayFrequency.Add(dayFrequencyEnum);
				}


				if (this.monthFrequency.HasValue)
				{
					var monthFrequencyEnum = EnumsProperties.EnumToArrayValues<EnumFTM_TaskPlanMonthFrequency>().Where(a => a.Key == this.monthFrequency.Value.ToString()).Select(b => b.Value).FirstOrDefault();
					this._MonthFrequency.Add(monthFrequencyEnum);
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


			_MonthFrequenciesData.Add("İlk");
			_MonthFrequenciesData.Add("İkinci");
			_MonthFrequenciesData.Add("Üçüncü");
			_MonthFrequenciesData.Add("Dördüncü");
			_MonthFrequenciesData.Add("Son");

			_DayFrequenciesData.Add("Gün");
			_DayFrequenciesData.Add("Pazar");
			_DayFrequenciesData.Add("Pazartesi");
			_DayFrequenciesData.Add("Salı");
			_DayFrequenciesData.Add("Çarşamba");
			_DayFrequenciesData.Add("Perşembe");
			_DayFrequenciesData.Add("Cuma");
			_DayFrequenciesData.Add("Cumartesi");

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


			if (this._DayFrequency != null)
			{
				var dayFrequencyKey = EnumsProperties.EnumToArrayValues<EnumFTM_TaskPlanDayFrequency>().Where(a => a.Value == this._DayFrequency.FirstOrDefault()).Select(b => b.Key).FirstOrDefault();

				if (dayFrequencyKey != null)
				{
					this.dayFrequency = Convert.ToInt32(dayFrequencyKey);
				}

			}

			if (this._MonthFrequency != null)
			{
				var monthFrequencyKey = EnumsProperties.EnumToArrayValues<EnumFTM_TaskPlanMonthFrequency>().Where(a => a.Value == this._MonthFrequency.FirstOrDefault()).Select(b => b.Key).FirstOrDefault();

				if (monthFrequencyKey != null)
				{
					this.monthFrequency = Convert.ToInt32(monthFrequencyKey);
				}
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

		public VMFTM_TaskCalendarModel[] CalendarDataSource(List<Guid> userIds, int? type, PageSecurity userStatus)
		{
			//Type 0 : Görev
			//Type 1 : Plan
			this.db = this.db ?? new WorkOfTimeDatabase();
			var tasks = new List<VMFTM_TaskCalendarModel>();
			var dbtasks = new List<VMFTM_TaskCalendarModel>();

			if (!type.HasValue)
			{
				type = 0;
			}


			if (type.Value == 0)
			{
				var query = "SELECT id,lastOperationDate,created,changed,closingDate,code,taskPlanId,customer_Title,customerStorage_Title,fixture_Title,planStartDate,dueDate,priority_Title,plate,priority,lastOperationStatus,assignUserId,assignableUserIds,isComplete,taskPlanId_Title,type_Title,description,penaltyStartDate,amercementTotal,SLAText,assignableUserTitles,taskSubjectType_Title,planLater FROM VWFTM_Task WITH (NOLOCK) ";


				if (userIds.Where(a => a == Guid.Empty).Count() == 0)
				{
					query += "WHERE (assignUserId IN (";
					var count = 0;
					foreach (var item in userIds)
					{
						count++;

						if (count == userIds.Count())
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

				dbtasks = db.GetVWFTM_TaskByQuery(query).B_ConvertType<VMFTM_TaskCalendarModel>().ToList();
				if (userStatus.AuthorizedRoles.Contains(new Guid(SHRoles.SahaGorevYonetici)) || userStatus.AuthorizedRoles.Contains(new Guid(SHRoles.SahaGorevOperator)))
				{
					var authoritys = db.GetVWFTM_TaskAuthorityByUserId(userStatus.user.id);
					if (authoritys.Any())
					{
						dbtasks = dbtasks.Where(x => authoritys.Where(f => f.customerId.HasValue).Select(f => f.customerId.Value).ToArray().Contains(x.customerId.Value)).ToArray().ToList();
					}
				}

				foreach (var task in dbtasks)
				{
					task.isTask = true;
					tasks.Add(task);
				}
			}
			else
			{
				var query = "SELECT * FROM VWFTM_TaskPlan WITH (NOLOCK)";

				if (userIds.Where(a => a == Guid.Empty).Count() == 0)
				{
					query += " WHERE  ";
					var count = 0;

					count = 0;
					foreach (var userId in userIds)
					{
						count++;

						if (count == 1)
						{
							query += " (assignableUserIds LIKE '%" + userId + "%' ) OR ";
							query += " (helperUserIds LIKE '%" + userId + "%' )";
						}

						query += " OR (assignableUserIds LIKE '%" + userId + "%' )";
						query += " OR (helperUserIds LIKE '%" + userId + "%' )";
					}
				}

				var plans = db.GetVWFTM_TaskPlanByQuery(query);



				foreach (var plan in plans)
				{
					var times = GetPlanTaskTimes(plan);

					foreach (var date in times)
					{
						var planStartDate = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
						var planEndDate = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
						planEndDate = planEndDate.AddMinutes(plan.estimatedTaskMinute.HasValue ? plan.estimatedTaskMinute.Value : 30);

						tasks.Add(new VMFTM_TaskCalendarModel
						{
							planStartDate = planStartDate,
							dueDate = planEndDate,
							customerId = plan.customerId,
							customer_Title = plan.customerId_Title,
							lastOperationStatus = 999,
							taskPlanId = plan.id,
							taskPlanId_Title = plan.name,
							isTask = false,
							type = plan.taskType
						});
					}
				}

			}


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
				.Where(a => a.enabled == true)
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
			RecurrenceValues values = null;

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
						start = start.AddMonths(this.frequencyInterval.Value);
						break;

				}
			}


			if (this.frequency == (int)EnumFTM_TaskPlansFrequency.Tekrarlanan)
			{
				MonthlyRecurrenceSettings mo;
				mo = new MonthlyRecurrenceSettings(this.frequencyStartDate.Value, this.frequencyEndDate.Value);
				mo.AdjustmentValue = int.Parse("0");
				values = mo.GetValues((MonthlySpecificDatePartOne)this.monthFrequency, (MonthlySpecificDatePartTwo)this.dayFrequency, this.frequencyInterval.Value);

				var times = this.times.Split(',')
								.Select(a => DateTime.ParseExact(start.ToString("yyyy-MM-dd") + " " + a.Trim(), "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture))
								.ToArray();


				foreach (var value in values.Values)
				{
					var random = new Random();
					var index = random.Next(times.Count());
					taskTimes.Add(new DateTime(value.Year, value.Month, value.Day, times[index].Hour, times[index].Minute, 0));
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
					task.taskPlanId_Title = this.name;
					task.taskTemplateId = this.templateId;

					task.dueDate = a.AddMinutes(TaskTemplate.estimatedTaskMinute.Value);

					return task;

				}).ToArray();

			this._TaskList = tasks;

			return this._TaskList;

		}


		public List<DateTime> GetPlanTaskTimes(VWFTM_TaskPlan data)
		{
			this.loadedTasks = true;
			RecurrenceValues values = null;

			var res = new List<VWFTM_Task>();
			var taskTimes = new List<DateTime>();
			this.db = this.db ?? new WorkOfTimeDatabase();
			this.LoadPlan(data);

			if (!this.frequencyStartDate.HasValue || !this.frequencyEndDate.HasValue || !this.frequencyInterval.HasValue || !this.templateId.HasValue)
			{
				return new List<DateTime>();
			}

			if (this.TaskTemplate == null)
			{
				return new List<DateTime>();
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
						start = start.AddMonths(this.frequencyInterval.Value);
						break;

				}
			}


			if (this.frequency == (int)EnumFTM_TaskPlansFrequency.Tekrarlanan)
			{
				MonthlyRecurrenceSettings mo;
				mo = new MonthlyRecurrenceSettings(this.frequencyStartDate.Value, this.frequencyEndDate.Value);
				mo.AdjustmentValue = int.Parse("0");
				values = mo.GetValues((MonthlySpecificDatePartOne)this.monthFrequency, (MonthlySpecificDatePartTwo)this.dayFrequency, this.frequencyInterval.Value);
				taskTimes.AddRange(values.Values);
			}


			return taskTimes;
		}

	}
}
