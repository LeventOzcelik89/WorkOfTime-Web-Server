using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessData;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infoline.WorkOfTime.BusinessAccess
{
	public class TaskSchedulerModel
	{

		[JsonIgnore]
		public WorkOfTimeDatabase db { get; set; }

		public void Load(Guid? taskPlanId)
		{

			this.db = this.db ?? new WorkOfTimeDatabase();
			this.TaskPlan.db = this.db;
			this.TemplateModel.db = this.db;

			if (taskPlanId.HasValue)
			{
				this.TaskPlan = new VMFTM_TaskPlanModel { id = taskPlanId.Value, db = this.db }.Load();

				if (this.TaskPlan.templateId.HasValue)
				{
					this.TemplateModel.id = this.TaskPlan.templateId.Value;
				}

				this.TemplateModel.Load();

			}
			else
			{

				this.TaskPlan.Load();
				this.TemplateModel.Load();

			}

		}

		public VMFTM_TaskPlanModel TaskPlan { get; set; } = new VMFTM_TaskPlanModel();
		public VMTaskTemplateModel TemplateModel { get; set; } = new VMTaskTemplateModel();

		public VM_TaskTemplatePlanList GetTaskTemplatePlanList()
		{

			this.db = this.db ?? new WorkOfTimeDatabase();

			var res = new VM_TaskTemplatePlanList
			{
				TaskPlan = db.GetVWFTM_TaskPlan().OrderBy(a => a.frequency).ToArray(),
				TaskTemplate = db.GetVWFTM_TaskTemplate()
			};

			return res;

		}

		public VM_TaskTemplatePlanList GetTaskTemplatePlanByYearList(int? year, Guid? customerId, Guid? planId)
		{
			this.db = this.db ?? new WorkOfTimeDatabase();
			var res = new VM_TaskTemplatePlanList();
			res.TaskPlan = db.GetVWFTM_TaskPlan().Where(a => a.frequencyStartDate.HasValue && a.frequencyEndDate.HasValue && a.frequencyEndDate.Value.Year == year).OrderBy(b => b.frequency).ToArray();

			if (customerId.HasValue)
			{
				res.TaskPlan = res.TaskPlan.Where(a => a.customerId == customerId).ToArray();
			}

			if (planId.HasValue)
			{
				res.TaskPlan = res.TaskPlan.Where(a => a.id == planId.Value).ToArray();
			}

			if (res.TaskPlan.Count() > 0)
			{
				var templatetIds = res.TaskPlan.Where(a => a.templateId.HasValue).Select(b => b.templateId).ToArray();
				res.TaskTemplate = db.GetVWFTM_TaskTemplate().Where(a => a.id.In(templatetIds)).ToArray();
			}

			return res;
		}

	}

	public class VM_TaskTemplatePlanList
	{
		public VWFTM_TaskPlan[] TaskPlan { get; set; }
		public VWFTM_TaskTemplate[] TaskTemplate { get; set; }
		public List<VM_TaskTemplateYearList> YearDatas { get; set; } = new List<VM_TaskTemplateYearList>();
	}

	public class VM_TaskTemplateYearList
	{
		public int Year { get; set; }
		public List<VMFTM_TaskPlanCalendarModel> Data { get; set; } = new List<VMFTM_TaskPlanCalendarModel>();
	}
}
