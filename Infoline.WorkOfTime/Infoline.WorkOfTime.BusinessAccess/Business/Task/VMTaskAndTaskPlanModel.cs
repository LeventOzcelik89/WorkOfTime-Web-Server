using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infoline.WorkOfTime.BusinessAccess
{
    public class VMTaskAndTaskPlanModel
    {
        public bool IsUpdate { get; set; }
        public VMFTM_TaskModel Task { get; set; } = new VMFTM_TaskModel();
        public VMFTM_TaskPlanModel TaskPlans { get; set; } = new VMFTM_TaskPlanModel();

        public VMTaskAndTaskPlanModel Load()
        {

            Task.Load();
            TaskPlans.Load();

            return this;

        }

    }
}
