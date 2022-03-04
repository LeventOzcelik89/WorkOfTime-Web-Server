using Infoline.WorkOfTime.BusinessData;
using System;

namespace Infoline.WorkOfTime.BusinessAccess
{

	public class VMFTM_TaskPlanCalendarModel : VWFTM_Task
    {
        public bool IsTemplate { get; set; } = false;
		public VMFTM_TaskPlanCalendarModel()
        {
            if (!this.planStartDate.HasValue)
            {
                this.planStartDate = new DateTime(2000, 1, 1);
            }

            if (!this.dueDate.HasValue && !this.lastOperationDate.HasValue)
            {
                this.dueDate = new DateTime(2000, 1, 1);
            }
        }

        public DateTime start
        {
            get
            {
                return this.planStartDate.HasValue
                    ? this.planStartDate.Value
                    : new DateTime(2000, 1, 1);
            }
        }
        public DateTime end
        {
            get
            {
                return this.lastOperationDate.HasValue
                    ? this.lastOperationDate.Value
                    : this.dueDate.HasValue
                        ? this.dueDate.Value
                        : new DateTime(2000, 1, 1);

            }
        }

        public string title
        {
            get
            {
                return this.customer_Title;
            }
        }

        public Guid _id
        {
            get
            {
                return this.id;
            }
        }

    }

}
