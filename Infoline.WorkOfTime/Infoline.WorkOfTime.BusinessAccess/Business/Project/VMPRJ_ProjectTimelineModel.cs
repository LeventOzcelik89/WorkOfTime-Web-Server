using Infoline.WorkOfTime.BusinessData;
using Kendo.Mvc.UI;
using System;
using System.Data.Common;

namespace Infoline.WorkOfTime.BusinessAccess
{
	public class VMPRJ_ProjectTimelineModel : VWPRJ_ProjectTimeline
	{
		private WorkOfTimeDatabase db { get; set; }
		private DbTransaction trans { get; set; }
	}
	public class VMPageData
	{
		public int totalProject { get; set; }
		public int endProject { get; set; }
		public int activeProjectCommission { get; set; }
	}

    public class TaskViewModel : IGanttTask
    {
        public Guid TaskID { get; set; }
        public Guid? ParentID { get; set; }
        public string Title { get; set; }
        private DateTime start;
        public DateTime Start
        {
            get { return start; }
            set { start = value.ToUniversalTime(); }
        }
        private DateTime end;
        public DateTime End
        {
            get { return end; }
            set { end = value.ToUniversalTime(); }
        }
        public bool Summary { get; set; }
        public bool Expanded { get; set; }
        public int OrderId { get; set; }
        public decimal PercentComplete { get; set; }
        //public Guid? IdProject { get; set; }
        public int Type { get; set; }
        public short? Status { get; set; }
        public string Status_Title { get; set; }

        public PRJ_ProjectTimelinePersons persons { get; set; }
    }
    public class DependencyViewModel : IGanttDependency
    {
        public int DependencyID { get; set; }
        public Guid? PredecessorID { get; set; }
        public Guid SuccessorID { get; set; }
        public int Type { get; set; }
        DependencyType IGanttDependency.Type { get; set; }
        //   public Guid IdProject { get; set; }
    }

    public class JiraData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CompleteDate { get; set; }
    }

    public class JiraModel
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public JiraData[] Data { get; set; }
    }

}
