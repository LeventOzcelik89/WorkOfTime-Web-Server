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

            if (taskPlanId.HasValue)
            {
                this.TaskPlan.id = taskPlanId.Value;
                this.TaskPlan.Load();

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

    }

    public class VM_TaskTemplatePlanList
    {
        public VWFTM_TaskPlan[] TaskPlan { get; set; }
        public VWFTM_TaskTemplate[] TaskTemplate { get; set; }
    }

}
