using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infoline.WorkOfTime.BusinessAccess
{
	public class TaskOperationReportModel : VWFTM_Task
	{
		public WorkOfTimeDatabase db { get; set; }
		private DbTransaction trans { get; set; }

		public int? year { get; set; }
		public int? month { get; set; }
		public List<TaskCustomerReportResult> taskCustomerReportList { get; set; } = new List<TaskCustomerReportResult>();
		public List<TaskCustomerReportResult> taskCustomerReportTodayList { get; set; } = new List<TaskCustomerReportResult>();
		public List<ActivityResult> activityResultList { get; set; } = new List<ActivityResult>();
		public List<OperationReportModel> operationReportList { get; set; } = new List<OperationReportModel>();
		public List<OperationChartModel> operationChartDataList { get; set; } = new List<OperationChartModel>();
		public List<OperationChartModel> operationCustomerChartDataList { get; set; } = new List<OperationChartModel>();


		public TaskOperationReportModel Load()
		{
			db = db ?? new WorkOfTimeDatabase();
			var now = DateTime.Now;
			var startDate = new DateTime();
			var endDate = new DateTime();

			if (!year.HasValue)
			{
				year = DateTime.Now.Year;
			}

			if (!month.HasValue)
			{
				month = DateTime.Now.Month;
			}

			startDate = new DateTime(year.Value, month.Value, 1);
			endDate = startDate.AddMonths(1).AddDays(-1);
			Random rnd = new Random();

			this.operationReportList = new List<OperationReportModel>();
			var tasks = db.GetVWFTM_TaskBetweenPlanDates(startDate, endDate);
			var whileStartDate = new DateTime(startDate.Ticks);
			var taskNo = 0;

			if (tasks.Count() > 0)
			{
				var ftmTaskOperation = db.GetVWFTM_TaskOperationStatusApprovedByTaskIds(tasks.Select(x => x.id).ToArray());
				var userIds = tasks.Where(x => x.assignUserId.HasValue).Select(x => x.assignUserId.Value).ToArray();

				this.operationChartDataList.AddRange(tasks.GroupBy(a => a.type_Title).Select(b => new OperationChartModel
				{
					category = b.Key,
					value = b.Count(),
					color = "#" + Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256)).Name.Substring(0, 6)
				}));


				var taskStaffReportList = new List<VWFTM_Task>();
				this.taskCustomerReportList = new List<TaskCustomerReportResult>();
				this.taskCustomerReportTodayList = new List<TaskCustomerReportResult>();
				if (userIds.Length > 0)
				{
					var user = db.GetVWSH_UserByIds(userIds);
					foreach (var item in user)
					{

						var taskModel = tasks.Where(x => x.assignUserId != null && x.assignUserId == item.id).ToList();
						foreach (var taskItem in taskModel)
						{
							var staffReportModel = new VWFTM_Task
							{
								id = taskItem.id,
								assignUser_Title = item.FullName,
								type_Title = taskItem.type_Title,
								type = taskItem.type,
								planStartDate = taskItem.planStartDate.HasValue ? taskItem.planStartDate.Value : new DateTime(),
								dueDate = taskItem.dueDate.HasValue ? taskItem.dueDate.Value : new DateTime(),
								customer_Title = taskItem.customer_Title,
								customerStorage_Title = taskItem.customerStorage_Title,
								lastOperationStatus_Title = taskItem.lastOperationStatus_Title,
								lastOperationDate = taskItem.lastOperationDate,
								workingHour = taskItem.workingHour
							};
							taskStaffReportList.Add(staffReportModel);
						}
						this.taskCustomerReportList = tasks.GroupBy(x => x.customer_Title).Select(x => new TaskCustomerReportResult
						{
							FullName = x.Key,
							allTaskCount = tasks.Where(c => c.customer_Title == x.Key).Count(),
							finishedTask = tasks.Where(a => (a.lastOperationStatus == (Int32)EnumFTM_TaskOperationStatus.CozumBildirildi || a.lastOperationStatus == (Int32)EnumFTM_TaskOperationStatus.CozumOnaylandi || a.lastOperationStatus == (Int32)EnumFTM_TaskOperationStatus.MemnuniyetAnketiYuklendi) && a.customer_Title == x.Key).Count(),
							finishedTaskIds = tasks.Where(a => (a.lastOperationStatus == (Int32)EnumFTM_TaskOperationStatus.CozumBildirildi || a.lastOperationStatus == (Int32)EnumFTM_TaskOperationStatus.CozumOnaylandi || a.lastOperationStatus == (Int32)EnumFTM_TaskOperationStatus.MemnuniyetAnketiYuklendi) && a.customer_Title == x.Key).Select(l => l.id).ToArray(),
							allStaffCount = x.Where(a => a.assignUserId.HasValue).Select(b => b.assignUserId.Value).Count(),
							allHelperStaffCount = x.Where(a => a.helperUserIds != null).Select(b => b.helperUserIds.Split(',')).Count()
						}).ToList();
					}
				}



				var taskOperationReportDays = new List<TaskOperationDay>();

				var taskGroups = tasks.GroupBy(a => a.customerId).ToArray();
				foreach (var task in taskGroups)
				{
					var customerTasks = tasks.Where(a => a.customerId == task.Key).ToArray();

					while (whileStartDate <= endDate)
					{
						var date = new DateTime(whileStartDate.Ticks);

						var title = "";

						if (date.DayOfWeek == DayOfWeek.Monday)
						{
							title = "Pzt";
						}
						else if (date.DayOfWeek == DayOfWeek.Tuesday)
						{
							title = "Sal";
						}
						else if (date.DayOfWeek == DayOfWeek.Wednesday)
						{
							title = "Çrş";
						}
						else if (date.DayOfWeek == DayOfWeek.Thursday)
						{
							title = "Prş";
						}
						else if (date.DayOfWeek == DayOfWeek.Friday)
						{
							title = "Cum";
						}
						else if (date.DayOfWeek == DayOfWeek.Saturday)
						{
							title = "Cmt";
						}
						else
						{
							title = "Paz";
						}

						var taskOperationDay = new TaskOperationDay
						{
							date = date,
							title = date.Day.ToString() + " " + title,
							text = ""
						};

						if (customerTasks.Where(a => a.created.HasValue && new DateTime(a.created.Value.Year, a.created.Value.Month, a.created.Value.Day) == date).Count() > 0)
						{
							var selectedTasks = customerTasks.Where(a => a.created.HasValue && new DateTime(a.created.Value.Year, a.created.Value.Month, a.created.Value.Day) == date).ToArray();
							if (selectedTasks.Count() > 0)
							{
								taskOperationDay.taskPlanId = task.Key;
								taskOperationDay.taskIds.AddRange(selectedTasks.Select(a => a.id).ToArray());
								taskOperationDay.totalTaskCount = selectedTasks.Count();
								taskOperationDay.isComplateTaskCount = selectedTasks.Where(a => a.lastOperationStatus == (int)EnumFTM_TaskOperationStatus.CozumBildirildi || a.lastOperationStatus == (int)EnumFTM_TaskOperationStatus.CozumOnaylandi).Count();
								taskOperationDay.taskType_Titles = selectedTasks.Select(a => a.type_Title).ToList();
								taskOperationDay.statusList.AddRange(selectedTasks.Select(a => a.lastOperationStatus.Value).ToList());
								taskOperationDay.staffCount = selectedTasks.Where(a => a.assignUserId.HasValue).Select(b => b.assignUserId.Value).Count();
								taskOperationDay.helperStaffCount = selectedTasks.Where(a => a.helperUserIds != null).Select(b => b.helperUserIds.Split(',')).Count();
								taskOperationDay.taskNo = ++taskNo;

								var girdi = selectedTasks.Where(a => a.totalworkingHour.HasValue).Sum(a => a.totalworkingHour.Value);

								int yil = 0;
								int ay = 0;
								int gun = 0;
								int saat = 0;
								int dakika = 0;
								yil = girdi / 31104000;
								girdi = girdi - yil * 3110400;
								ay = girdi / 2592000;
								girdi = girdi - ay * 2592000;
								gun = girdi / 86400;
								girdi = girdi - gun * 86400;
								saat = girdi / 3600;
								girdi = girdi - saat * 3600;
								dakika = girdi / 60;
								girdi = girdi - dakika * 60;
								taskOperationDay.workingHours = (yil > 0 ? yil + " yıl " : "") +
											   (ay > 0 ? ay + " ay " : "") +
											   (gun > 0 ? gun + " gün " : "") +
											   (dakika > 0 ? dakika + " saat " : "") +
											   (girdi > 0 ? girdi + " dakika " : "");
							}

							if (taskOperationDay.totalTaskCount > 0 && taskOperationDay.isComplateTaskCount > 0 && taskOperationDay.totalTaskCount != taskOperationDay.isComplateTaskCount)
							{
								taskOperationDay.color = "rgb(255 187 0 / 85%)";
								taskOperationDay.className = "badge badge-warning";
								taskOperationDay.text = taskOperationDay.totalTaskCount + " / " + taskOperationDay.isComplateTaskCount;
							}

							if (taskOperationDay.totalTaskCount > 0 && taskOperationDay.isComplateTaskCount > 0 && taskOperationDay.totalTaskCount == taskOperationDay.isComplateTaskCount)
							{
								taskOperationDay.color = "#1ab394";
								taskOperationDay.className = "badge badge-primary";
								taskOperationDay.text = taskOperationDay.totalTaskCount + " / " + taskOperationDay.isComplateTaskCount;
							}

							if (taskOperationDay.totalTaskCount > 0 && taskOperationDay.isComplateTaskCount == 0)
							{
								taskOperationDay.color = "rgb(255 0 0 / 54%)";
								taskOperationDay.className = "badge badge-danger";
								taskOperationDay.text = taskOperationDay.totalTaskCount + " / " + taskOperationDay.isComplateTaskCount;
							}

							if (taskOperationDay.totalTaskCount == 0 && taskOperationDay.isComplateTaskCount == 0 && taskOperationDay.taskPlanId == null)
							{
								taskOperationDay.color = "white";
								taskOperationDay.className = "badge badge-white";
								taskOperationDay.text = "";
							}
						}

						taskOperationReportDays.Add(taskOperationDay);
						whileStartDate = whileStartDate.AddDays(1);
					}

					operationReportList.Add(new OperationReportModel
					{
						days = taskOperationReportDays,
						taskPlanName = customerTasks.Where(a => a.customerId == task.Key).Select(b => b.customer_Title).FirstOrDefault(),
						taskType_Title = customerTasks.Where(a => a.customerId == task.Key).Select(b => b.type_Title).FirstOrDefault(),
						taskSubjectId_Title = customerTasks.Where(a => a.customerId == task.Key).Select(b => b.taskSubjectType_Title).FirstOrDefault()
					});

					taskOperationReportDays = new List<TaskOperationDay>();
					whileStartDate = new DateTime(startDate.Ticks);
					taskNo = 0;
				}

				this.operationCustomerChartDataList.AddRange(operationReportList.Select(a => new OperationChartModel
				{
					category = a.taskPlanName,
					value = a.days.Where(b => b.taskPlanId.HasValue).Sum(c => c.totalTaskCount),
					color = "#" + Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256)).Name.Substring(0, 6)
				}));
			}
			return this;
		}

		public class TaskOperationDay
		{
			public string title { get; set; }
			public List<Guid> taskIds { get; set; } = new List<Guid>();
			public DateTime date { get; set; }
			public bool isExistPlan { get; set; }
			public int isComplateTaskCount { get; set; }
			public int totalTaskCount { get; set; }
			public Guid? taskPlanId { get; set; }
			public List<string> taskType_Titles { get; set; } = new List<string>();
			public List<int> statusList { get; set; } = new List<int>();
			public int taskNo { get; set; }
			public short? type { get; set; }
			public string color { get; set; }
			public string className { get; set; }
			public string text { get; set; }
			public int staffCount { get; set; }
			public int helperStaffCount { get; set; }
			public string workingHours { get; set; }

		}

		public class OperationReportModel
		{
			public List<TaskOperationDay> days { get; set; }
			public string taskPlanName { get; set; }
			public string taskType_Title { get; set; }
			public string taskSubjectId_Title { get; set; }
		}

		public class TaskCustomerReportResult
		{
			public Guid Id { get; set; }
			public string FullName { get; set; }
			public int allTaskCount { get; set; }
			public int finishedTask { get; set; }
			public int allStaffCount { get; set; }
			public int allHelperStaffCount { get; set; }
			public Guid[] finishedTaskIds { get; set; }
		}

		public class ActivityResult
		{
			public Guid Id { get; set; }
			public string FullName { get; set; }
			public string Photo { get; set; }
			public int CompleteCount { get; set; }
			public int WorkingNow { get; set; }
			public int taskCount { get; set; }
			public string totalWorkingHours { get; set; }
		}

		public class OperatorActivityResult
		{
			public Guid Id { get; set; }
			public string FullName { get; set; }
			public int OpenedTask { get; set; }
			public int ApprovedTask { get; set; }
			public int MyAppointmentTask { get; set; }
		}

		public class OperationChartModel
		{
			public string category { get; set; }
			public int value { get; set; }
			public string color { get; set; }
		}
	}
}
