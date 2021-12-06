using System;
using System.Data.Common;
using System.Linq;
using Infoline.WorkOfTime.BusinessData;
using Infoline.Framework.Database;
using System.Collections.Generic;
using static Infoline.WorkOfTime.BusinessAccess.VMFTM_TaskModel;

namespace Infoline.WorkOfTime.BusinessAccess
{
    public class SummaryHeadersTask
    {
        public int AssignedTask { get; set; }
        public string AssignedTaskFilter { get; set; }
        public int AssignableTask { get; set; }
        public string AssignableTaskFilter { get; set; }
        public int CompletedTask { get; set; }
        public string CompletedTaskFilter { get; set; }
        public int SolutionConfirmedTask { get; set; }
        public string SolutionConfirmedTaskFilter { get; set; }
        public int AssignUser { get; set; }
        public string AssignUserFilter { get; set; }
        public string searchField { get; set; }
    }

    public class SummaryHeadersTaskNew
    {
        public int AllTask { get; set; }
        public string AllTaskFilter { get; set; }
        public int WaitingTask { get; set; }
        public string WaitingTaskFilter { get; set; }
        public int CompletedTask { get; set; }
        public string CompletedTaskFilter { get; set; }
        public HeadersTask headerFilters { get; set; }
    }

    public class HeadersTask
    {
        public string title { get; set; }
        public List<HeadersTaskItem> Filters { get; set; }
    }

    public class HeadersTaskItem
    {
        public string title { get; set; }
        public int count { get; set; }
        public string filter { get; set; }
        public bool isActive { get; set; }
    }

    partial class WorkOfTimeDatabase
    {
        public VWFTM_Task[] GetVWFTM_OpenTask()
        {
            using (var db = GetDB())
            {
                return db.Table<VWFTM_Task>().Where(x =>
                 x.lastOperationStatus != (int)EnumFTM_TaskOperationStatus.CozumBildirildi &&
                 x.lastOperationStatus != (int)EnumFTM_TaskOperationStatus.CozumOnaylandi &&
                 x.lastOperationStatus != (int)EnumFTM_TaskOperationStatus.CozumReddedildi
                ).Execute().ToArray();
            }
        }
        public VWFTM_Task[] GetVWFTM_TaskByIds(Guid[] taskIds, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWFTM_Task>().Where(a => a.id.In(taskIds)).Execute().ToArray();
            }
        }

        public VWFTM_Task[] GetVWFTM_TaskByUserId(Guid userId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWFTM_Task>().Where(a => (a.lastOperationStatus == (int)EnumFTM_TaskOperationStatus.CozumBildirildi) || (a.assignUserId == userId) || (a.assignUserId == null && (a.assignableUserIds.Contains(userId.ToString())))).Execute().ToArray();
            }
        }
        public VWFTM_Task[] GetVWFTM_TaskByFixtureId(Guid fixtureId, Guid userId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWFTM_Task>().Where(a => a.fixtureId == fixtureId && ((a.assignUserId == userId) || (a.assignUserId == null && (a.assignableUserIds.Contains(userId.ToString()))))).Execute().ToArray();
            }
        }

        public VWFTM_Task[] GetVWFTM_TaskByFixtureId(Guid fixtureId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWFTM_Task>().Where(a => a.fixtureId == fixtureId).Execute().ToArray();
            }
        }

        public VWFTM_Task[] GetVWFTM_TaskByCustomerId(Guid customerId, Guid userId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWFTM_Task>().Where(a => a.customerId == customerId && ((a.assignUserId == userId) || (a.assignUserId == null && (a.assignableUserIds.Contains(userId.ToString()))))).Execute().ToArray();
            }
        }


        public VWFTM_Task[] GetVWFTM_TaskByCustomerStorageId(Guid storage, Guid userId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWFTM_Task>().Where(a => a.customerStorageId == storage && ((a.assignUserId == userId) || (a.assignUserId == null && (a.assignableUserIds.Contains(userId.ToString()))))).Execute().ToArray();
            }
        }

        public VWFTM_Task[] GetVWFTM_TaskByCustomerId(Guid customerId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWFTM_Task>().Where(a => a.customerId == customerId).Execute().ToArray();
            }
        }

        public VWFTM_Task[] GetVWFTM_TaskByTaskPlanIdAndDates(Guid taskPlanId, DateTime startDate, DateTime endDate, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWFTM_Task>().Where(a => a.taskPlanId == taskPlanId && a.planStartDate >= startDate && a.planStartDate <= endDate).Execute().ToArray();
            }
        }

        public VWFTM_Task[] GetVWFTM_TaskByTaskPlanId(Guid taskPlanId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWFTM_Task>().Where(a => a.taskPlanId == taskPlanId).Execute().ToArray();
            }
        }

        public int GetVWFTM_TaskByUserIdTaskOnGoing(Guid userid, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                //return db.Table<VWFTM_Task>().Where(a => a.assignableUserIds.Contains(userid.ToString()) && a.lastOperationStatus < (Int16)EnumFTM_TaskOperationStatus.CozumBildirildi).Execute().Count();

                var aa = db.Table<VWFTM_Task>().Where(x => x.assignUserId == userid && x.lastOperationStatus < (Int16)EnumFTM_TaskOperationStatus.CozumBildirildi && x.lastOperationStatus >= (Int16)EnumFTM_TaskOperationStatus.GorevBaslandi).Execute().ToArray();

                return aa.Count();
            }
        }

        public VWFTM_Task[] GetVWFTM_WaitingAcceptTask()
        {
            using (var db = GetDB())
            {
                return db.Table<VWFTM_Task>().Where(x =>
                 x.lastOperationStatus == (int)EnumFTM_TaskOperationStatus.CozumBildirildi
                ).Execute().ToArray();
            }
        }

        public VWFTM_Task[] GetVWFTM_OpenTodayTask(int day)
        {
            using (var db = GetDB())
            {
                return db.Table<VWFTM_Task>().Where(x =>
                 x.created >= DateTime.Now.Date.AddDays(day) && x.created <= DateTime.Now.Date.AddDays(1).AddSeconds(-1)
                ).Execute().ToArray();
            }
        }

        public VWFTM_Task[] GetVWFTM_TaskBetweenDates(DateTime? startDate, DateTime? endDate)
        {
            using (var db = GetDB())
            {
                return db.Table<VWFTM_Task>().Where(x => x.created >= startDate && x.created <= endDate).Execute().ToArray();
            }
        }

        public VWFTM_Task[] GetVWFTM_TaskBetweenMonths(int month1, int month2, int month3)
        {
            using (var db = GetDB())
            {
                return db.Table<VWFTM_Task>().Where(x => x.created.HasValue && (x.created.Value.Month == month1 || x.created.Value.Month == month2 || x.created.Value.Month == month3)).Execute().ToArray();
            }
        }

        public VWFTM_Task[] GetVWFTM_TaskJustCustomerQuery()
        {
            using (var db = GetDB())
            {
                return db.ExecuteReader<VWFTM_Task>("Select customerId,customer_Title from VWFTM_Task with(nolock)").ToArray();
            }
        }

        public MonthTaskReportModel[] GetVWFTM_MonthTaskReport(int year, string months)
        {
            using (var db = GetDB())
            {
                return db.ExecuteReader<MonthTaskReportModel>("SELECT tbl.taskId,task.customerId as customerId, task.customerStorageId as customerStorageId, DATEPART(YEAR, MIN(tbl.created)) AS 'year', DATEPART(MONTH, MIN(tbl.created)) AS 'month', DATEDIFF(MINUTE, MIN(gorevBaslandi.created), MIN(cozumBildirildi.created)) AS 'resolutionTime', DATEDIFF(MINUTE, MIN(gorevAtandi.created), MIN(gorevUstlenildi.created)) AS 'responseTime' FROM FTM_TaskOperation AS tbl WITH(NOLOCK) LEFT JOIN FTM_TaskOperation AS gorevBaslandi WITH(NOLOCK) ON tbl.taskId = gorevBaslandi.taskId LEFT JOIN FTM_TaskOperation AS cozumBildirildi WITH(NOLOCK) ON tbl.taskId = cozumBildirildi.taskId LEFT JOIN FTM_TaskOperation AS gorevAtandi WITH(NOLOCK) ON tbl.taskId = gorevAtandi.taskId LEFT JOIN FTM_TaskOperation AS olustu WITH(NOLOCK) ON tbl.taskId = olustu.taskId INNER JOIN VWFTM_Task AS task WITH(NOLOCK) ON tbl.taskId = task.id LEFT JOIN FTM_TaskOperation AS gorevUstlenildi WITH(NOLOCK) ON tbl.taskId = gorevUstlenildi.taskId WHERE gorevBaslandi.[status] = 13 AND gorevUstlenildi.[status] = 12 AND cozumBildirildi.[status] = 30 AND olustu.[status] IN(10) AND DATEPART(MONTH, gorevAtandi.created) = DATEPART(MONTH, cozumBildirildi.created) GROUP BY tbl.taskId, task.customerId, task.customerStorageId HAVING DATEPART(YEAR, MIN(tbl.created)) = " + year + " AND DATEPART(MONTH, MIN(tbl.created)) IN(" + months + ") ORDER BY DATEPART(YEAR, MIN(tbl.created)), DATEPART(MONTH, MIN(tbl.created))").ToArray();
            }
        }

        public MonthTaskReportModel[] GetVWFTM_MonthPersonnelTaskReport(int year, string months)
        {
            using (var db = GetDB())
            {
                return db.ExecuteReader<MonthTaskReportModel>("SELECT  task.customerId as customerId, task.customerStorageId as customerStorageId,  grupUser.groupId_Title as groupId_Title,assignUserId,personal.firstname+' '+personal.lastname as personnelName,tbl.taskId, DATEPART(YEAR, MIN(tbl.created)) AS 'year', DATEPART(MONTH, MIN(tbl.created)) AS 'month', DATEDIFF(MINUTE, MIN(gorevBaslandi.created), MIN(cozumBildirildi.created)) AS 'resolutionTime', DATEDIFF(MINUTE, MIN(gorevAtandi.created), MIN(gorevUstlenildi.created)) AS 'responseTime' FROM FTM_TaskOperation AS tbl WITH(NOLOCK) LEFT JOIN FTM_TaskOperation AS gorevBaslandi WITH(NOLOCK) ON tbl.taskId = gorevBaslandi.taskId LEFT JOIN FTM_TaskOperation AS cozumBildirildi WITH(NOLOCK) ON tbl.taskId = cozumBildirildi.taskId LEFT JOIN FTM_TaskOperation AS gorevAtandi WITH(NOLOCK) ON tbl.taskId = gorevAtandi.taskId LEFT JOIN FTM_TaskOperation AS gorevUstlenildi WITH(NOLOCK) ON tbl.taskId = gorevUstlenildi.taskId INNER JOIN VWFTM_Task AS task WITH(NOLOCK) ON tbl.taskId = task.id LEFT JOIN SH_User AS personal WITH(NOLOCK) ON task.assignUserId = personal.id LEFT JOIN VWSH_GroupUsers AS grupUser ON grupUser.userId = task.assignUserId WHERE gorevBaslandi.[status] = 13 AND gorevUstlenildi.[status] = 12 AND cozumBildirildi.[status] = 30 AND gorevAtandi.[status] IN(10) AND DATEPART(MONTH, gorevAtandi.created) = DATEPART(MONTH, cozumBildirildi.created) GROUP BY tbl.taskId, grupUser.groupId_Title, assignUserId, personal.firstname, personal.lastname, task.customerId, task.customerStorageId HAVING DATEPART(YEAR, MIN(tbl.created)) = " + year + " AND DATEPART(MONTH, MIN(tbl.created)) IN(" + months + ") ORDER BY DATEPART(YEAR, MIN(tbl.created)), DATEPART(MONTH, MIN(tbl.created))").ToArray();
            }
        }

        public MonthTaskReportModel[] GetVWFTM_MonthTaskSubjectReport(int year, string months)
        {
            using (var db = GetDB())
            {
                return db.ExecuteReader<MonthTaskReportModel>("SELECT tbl.taskId,task.customerId as customerId, task.customerStorageId as customerStorageId,task.type_Title as taskType_Title,SubjectType.subjectId_Title as subjectTitle, DATEPART(YEAR, MIN(tbl.created)) AS 'year', DATEPART(MONTH, MIN(tbl.created)) AS 'month', DATEDIFF(MINUTE, MIN(gorevBaslandi.created), MIN(cozumBildirildi.created)) AS 'resolutionTime', DATEDIFF(MINUTE, MIN(gorevAtandi.created), MIN(gorevUstlenildi.created)) AS 'responseTime' FROM FTM_TaskOperation AS tbl WITH(NOLOCK)LEFT JOIN FTM_TaskOperation AS gorevBaslandi WITH(NOLOCK) ON tbl.taskId = gorevBaslandi.taskId LEFT JOIN FTM_TaskOperation AS cozumBildirildi WITH(NOLOCK) ON tbl.taskId = cozumBildirildi.taskId LEFT JOIN FTM_TaskOperation AS gorevAtandi WITH(NOLOCK) ON tbl.taskId = gorevAtandi.taskId LEFT JOIN FTM_TaskOperation AS gorevUstlenildi WITH(NOLOCK) ON tbl.taskId = gorevUstlenildi.taskId LEFT JOIN VWFTM_Task AS task WITH(NOLOCK) ON tbl.taskId = task.id LEFT JOIN VWFTM_TaskSubjectType as SubjectType WITH(NOLOCK) on SubjectType.taskId = task.id WHERE gorevBaslandi.[status] = 13 AND gorevUstlenildi.[status] = 12 AND cozumBildirildi.[status] = 30 AND gorevAtandi.[status] IN(10) AND DATEPART(MONTH, gorevAtandi.created) =  DATEPART(MONTH, cozumBildirildi.created) AND SubjectType.subjectId_Title IS NOT NULL GROUP BY tbl.taskId, task.type_Title, task.customerId, task.customerStorageId, task.id, SubjectType.subjectId_Title HAVING DATEPART(YEAR, MIN(tbl.created)) = " + year + " AND DATEPART(MONTH, MIN(tbl.created)) IN(" + months + ") ORDER BY DATEPART(YEAR, MIN(tbl.created)), DATEPART(MONTH, MIN(tbl.created))").ToArray();
            }
        }

        public VWFTM_Task[] GetVWFTM_TaskTodayTask(DateTime? startDate = null, DateTime? endDate = null)
        {
            using (var db = GetDB())
            {
                if (!startDate.HasValue)
                {
                    startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0, 0);
                }
                if (!endDate.HasValue)
                {
                    endDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);
                }



                return db.Table<VWFTM_Task>().Where(x => (x.created >= startDate && x.created <= endDate) || (x.lastOperationDate >= startDate && x.lastOperationDate <= endDate)).Execute().ToArray();
            }
        }

        public VWFTM_Task[] GetVWFTM_TaskByUserIdsAndDate(Guid[] userIds, DateTime startDate, DateTime endDate)
        {
            using (var db = GetDB())
            {
                var enddate = new DateTime(endDate.Year, endDate.Month, endDate.Day, 23, 59, 59);

                if (userIds != null && userIds.Count() > 0)
                {
                    return db.Table<VWFTM_Task>().Where(x => (x.created >= startDate && x.created <= enddate || x.lastOperationDate >= startDate && x.lastOperationDate <= enddate) && x.assignUserId.In(userIds)).Execute().ToArray();
                }

                return db.Table<VWFTM_Task>().Where(x => (x.created >= startDate && x.created <= enddate) || x.lastOperationDate >= startDate && x.lastOperationDate <= enddate).Execute().ToArray();
            }
        }
        public VWFTM_Task[] GetVWFTM_TaskByUserCreatedDate(Guid[] userIds, DateTime startDate, DateTime endDate)
        {
            using (var db = GetDB())
            {
                var enddate = new DateTime(endDate.Year, endDate.Month, endDate.Day, 23, 59, 59);

                if (userIds != null && userIds.Count() > 0)
                {
                    return db.Table<VWFTM_Task>().Where(x => (x.created >= startDate && x.created <= enddate) && x.assignUserId.In(userIds)).Execute().ToArray();
                }

                return db.Table<VWFTM_Task>().Where(x => (x.created >= startDate && x.created <= enddate)).Execute().ToArray();
            }
        }
        public VWFTM_Task[] GetVWFTM_TaskByAssignUserIdNotNullAndAssignableUsers(DateTime date, List<Guid?> userIds, DbTransaction tran = null)
        {
            var startdate = new DateTime();

            var enddate = new DateTime();

            if (date.Month == 1)
            {
                startdate = new DateTime(date.Year, 12, date.Day, 00, 00, 00);
                enddate = new DateTime(date.Year, date.Month + 1, date.Day, 23, 59, 59);
            }
            else if (date.Month == 12)
            {
                startdate = new DateTime(date.Year, date.Month - 1, date.Day, 00, 00, 00);
                enddate = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);
            }
            else
            {
                if (date.Day == 31)
                {
                    startdate = new DateTime(date.Year, date.Month, date.Day, 00, 00, 00);
                    enddate = new DateTime(date.Year, date.Month + 1, 25, 23, 59, 59);
                }
                else
                {
                    startdate = new DateTime(date.Year, date.Month - 1, date.Day, 00, 00, 00);
                    enddate = new DateTime(date.Year, date.Month + 1, date.Day, 23, 59, 59);
                }
            }
            using (var db = GetDB(tran))
            {
                if (userIds != null)
                {
                    return db.ExecuteReader<VWFTM_Task>("SELECT  * FROM VWFTM_Task WHERE assignableUserIds IN (" + string.Format("'{0}'", string.Join("','", userIds)) + ") or  assignUserId IN (" + string.Format("'{0}'", string.Join("','", userIds)) + ") and planStartDate >= '" + string.Format("{0:yyyy-MM-dd HH:mm}", startdate) + "' and dueDate <= '" + string.Format("{0:yyyy-MM-dd HH:mm}", enddate) + "'").ToArray();
                }

                return db.ExecuteReader<VWFTM_Task>("SELECT  * FROM VWFTM_Task WHERE ((LEN(assignableUserIds) = 36 and assignUserId is null) or (assignUserId is not null)) and planStartDate >= '" + string.Format("{0:yyyy-MM-dd HH:mm}", startdate) + "' and dueDate <= '" + string.Format("{0:yyyy-MM-dd HH:mm}", enddate) + "'").ToArray();
            }
        }


        public SummaryHeadersTask GetVWFTM_TaskByUserIdCounts(Guid userId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {

                var AssignableTaskFilter = "{'Filter':{'Operand1':{'Operand1':" +
                "{'Operand1':'assignUserId','Operator':'IsNull','Operand2':null}," +
                "'Operand2':{'Operand1':'isComplete','Operator':'Equal','Operand2':'False'}," +
                "'Operator':'And'}," +
                "'Operand2':" +
                "{'Operand1':{'Operand1':'assignableUserIds','Operator':'IsNotNull','Operand2':null}," +
                "'Operand2':{'Operand1':'assignableUserIds','Operator':'Like','Operand2':'%" + userId.ToString() + "%'}," +
                "'Operator':'And'},'Operator':'And'}}";

                var res = "{'Filter':{'Operand1':{'Operand1':'assignUserId','Operator':'Equal','Operand2':'" + userId + "'},'Operand2':{'Operand1':{'Operand1':'isComplete','Operator':'Equal','Operand2':'false'},'Operand2':{'Operand1':'lastOperationStatus','Operator':'NotEqual','Operand2':'40'},'Operator':'And'},'Operator':'And'}}";
                return new SummaryHeadersTask
                {
                    AssignedTask = db.Table<VWFTM_Task>().Where(a => a.assignUserId == userId && a.isComplete == false && a.lastOperationStatus != (int)EnumFTM_TaskOperationStatus.IslakImzaliFormYuklendi).Count(),
                    AssignedTaskFilter = res,

                    CompletedTask = db.Table<VWFTM_Task>().Where(a => a.assignUserId == userId && a.isComplete == true).Count(),
                    CompletedTaskFilter = "{'Filter':{'Operand1':{'Operand1':'assignUserId','Operator':'Equal','Operand2':'" + userId + "'},'Operand2':{'Operand1':'isComplete','Operator':'Equal','Operand2':'true'},'Operator':'And'}}",

                    SolutionConfirmedTask = db.Table<VWFTM_Task>().Where(a => a.isComplete == false && a.lastOperationStatus == (int)EnumFTM_TaskOperationStatus.CozumBildirildi).Count(),
                    SolutionConfirmedTaskFilter = "{'Filter':{'Operand1':{'Operand1':'lastOperationStatus','Operator':'Equal','Operand2':'30'},'Operand2':{'Operand1':'isComplete','Operator':'Equal','Operand2':'false'},'Operator':'And'}}",

                    AssignUser = db.Table<VWFTM_Task>().Where(c => c.isComplete == false && c.assignUserId == userId && c.lastOperationStatus != (int)EnumFTM_TaskOperationStatus.CozumBildirildi).Count(),
                    AssignUserFilter = "{'Filter':{'Operand1':{'Operand1':'assignUserId','Operator':'Equal','Operand2':'" + userId + "'},'Operand2':{'Operand1':{'Operand1':'isComplete','Operator':'Equal','Operand2':'false'},'Operand2':{'Operand1':'lastOperationStatus','Operator':'NotEqual','Operand2':'30'},'Operator':'And'},'Operator':'And'}}",

                    AssignableTask = db.Table<VWFTM_Task>().Where(a => a.assignUserId == null && a.isComplete == false && ((a.assignableUserIds != null && a.assignableUserIds.Contains(userId.ToString().ToUpper())))).Count(),
                    AssignableTaskFilter = AssignableTaskFilter,

                    searchField = "{'Filter':{'Operand1':{'Operand1':{'Operand1':{'Operand1':'assignUserId','Operator':'IsNull','Operand2':''},'Operand2':{'Operand1':'assignableUserIds','Operator':'Like','Operand2':'%" + userId.ToString() + "%'},'Operator':'And'},'Operand2':{'Operand1':'assignUserId','Operator':'Equal','Operand2':'" + userId.ToString() + "'},'Operator':'Or'},'Operand2':{'Operand1':'searchField','Operator':'Like','Operand2':'%#%'},'Operator':'And'}}",
                };
            }
        }


        public SummaryHeadersTask GetVWFTM_TaskByCounts(Guid userId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {

                var AssignableTaskFilter = "{'Filter':{'Operand1':'assignUserId','Operator':'IsNull','Operand2': null}}";

                var ress = "{'Filter':{'Operand1':{'Operand1':'assignUserId','Operator':'IsNotNull','Operand2':'null'},'Operand2':{'Operand1':{'Operand1':'lastOperationStatus','Operator':'NotEqual','Operand2':'31'},'Operand2':{'Operand1':'lastOperationStatus','Operator':'NotEqual','Operand2':'40'},'Operator':'And'},'Operator':'And'}}";
                return new SummaryHeadersTask
                {
                    AssignedTask = db.Table<VWFTM_Task>().Where(a => a.assignUserId != null && a.lastOperationStatus != (Int16)EnumFTM_TaskOperationStatus.CozumOnaylandi && a.lastOperationStatus != (Int16)EnumFTM_TaskOperationStatus.IslakImzaliFormYuklendi).Count(),
                    AssignedTaskFilter = ress,

                    CompletedTask = db.Table<VWFTM_Task>().Where(a => a.isComplete == true || a.lastOperationStatus == (Int16)EnumFTM_TaskOperationStatus.CozumBildirildi).Count(),
                    CompletedTaskFilter = "{'Filter':{'Operand1':{'Operand1':'isComplete','Operator':'Equal','Operand2':'1'},'Operand2':{'Operand1':'lastOperationStatus','Operator':'Equal','Operand2':'30'},'Operator':'Or'}}",

                    SolutionConfirmedTask = db.Table<VWFTM_Task>().Where(a => a.isComplete == false && a.lastOperationStatus == (int)EnumFTM_TaskOperationStatus.CozumBildirildi).Count(),
                    SolutionConfirmedTaskFilter = "{'Filter':{'Operand1':{'Operand1':'lastOperationStatus','Operator':'Equal','Operand2':'30'},'Operand2':{'Operand1':'isComplete','Operator':'Equal','Operand2':'false'},'Operator':'And'}}",

                    AssignUser = db.Table<VWFTM_Task>().Where(c => c.isComplete == false && c.lastOperationStatus != (int)EnumFTM_TaskOperationStatus.CozumBildirildi).Count(),
                    AssignUserFilter = "{'Filter':{'Operand1':{'Operand1':'lastOperationStatus','Operator':'NotEqual','Operand2':'30'},'Operand2':{'Operand1':'isComplete','Operator':'Equal','Operand2':'false'},'Operator':'And'}}",

                    AssignableTask = db.Table<VWFTM_Task>().Where(a => (a.assignUserId == null)).Count(),
                    AssignableTaskFilter = AssignableTaskFilter,

                    searchField = "{'Filter':{'Operand1':{'Operand1':'searchField','Operator':'Like','Operand2':'%#%'},'Operand2':{'Operand1':'lastOperationStatus','Operator':'NotEqual','Operand2':'26'},'Operator':'And'}}"
                };
            }
        }

        public SummaryHeadersTaskNew GetVWPA_TaskMyRequestsCountFilter(Guid userId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                var headers = new SummaryHeadersTaskNew
                {
                    AllTask = db.Table<VWFTM_Task>().Where(a => a.assignUserId == userId && a.lastOperationStatus != (int)EnumFTM_TaskOperationStatus.CozumBildirildi && a.isComplete == false).Count(),
                    AllTaskFilter = "{'Filter':{'Operand1':{'Operand1':{'Operand1':'assignUserId','Operator':'Equal','Operand2':'" + userId.ToString() + "'},'Operand2':{'Operand1':'lastOperationStatus','Operator':'NotEqual','Operand2':'" + (int)EnumFTM_TaskOperationStatus.CozumBildirildi + "'},'Operator':'And'},'Operand2':{'Operand1':'isComplete','Operator':'Equal','Operand2':'0'},'Operator':'And'}}",

                    WaitingTask = db.Table<VWFTM_Task>().Where(a => a.assignUserId == null && a.assignableUserIds.Contains(userId.ToString().ToUpper()) && a.isComplete == false).Count(),
                    WaitingTaskFilter = "{'Filter':{'Operand1':{'Operand1':{'Operand1':'assignUserId','Operator':'IsNull','Operand2':null},'Operand2':{'Operand1':'assignableUserIds','Operator':'Like','Operand2':'%" + userId.ToString() + "%'},'Operator':'And'},'Operand2':{'Operand1':'isComplete','Operator':'Equal','Operand2':'0'},'Operator':'And'}}",

                    CompletedTask = db.Table<VWFTM_Task>().Where(a => a.assignUserId == userId && a.isComplete == true).Count(),
                    CompletedTaskFilter = "{'Filter':{'Operand1':{'Operand1':'assignUserId','Operator':'Equal','Operand2':'" + userId.ToString() + "'},'Operand2':{'Operand1':'isComplete','Operator':'Equal','Operand2':'1'},'Operator':'And'}}",
                };

                headers.headerFilters = new HeadersTask();
                headers.headerFilters.title = "Durumuna Göre";
                headers.headerFilters.Filters = new List<HeadersTaskItem>();

                headers.headerFilters.Filters.Add(new HeadersTaskItem
                {
                    title = "Üstlenebileceğim Görevler",
                    filter = "{'Filter':{'Operand1':{'Operand1':{'Operand1':'assignUserId','Operator':'IsNull','Operand2':'null'},'Operand2':{'Operand1':'assignableUserIds','Operator':'Like','Operand2':'%" + userId.ToString() + "%'},'Operator':'And'},'Operand2':{'Operand1':'isComplete','Operator':'Equal','Operand2':'0'},'Operator':'And'}}",
                    count = db.Table<VWFTM_Task>().Where(a => a.assignUserId == null && a.assignableUserIds.Contains(userId.ToString().ToUpper()) && a.isComplete == false).Count(),
                    isActive = true
                });

                headers.headerFilters.Filters.Add(new HeadersTaskItem
                {
                    title = "Üstlendiğim Görevler",
                    filter = "{'Filter':{'Operand1':{'Operand1':{'Operand1':'assignUserId','Operator':'Equal','Operand2':'" + userId.ToString() + "'},'Operand2':{'Operand1':'lastOperationStatus','Operator':'NotEqual','Operand2':'" + (int)EnumFTM_TaskOperationStatus.CozumBildirildi + "'},'Operator':'And'},'Operand2':{'Operand1':'isComplete','Operator':'Equal','Operand2':'0'},'Operator':'And'}}",
                    count = db.Table<VWFTM_Task>().Where(a => a.assignUserId == userId && a.lastOperationStatus != (int)EnumFTM_TaskOperationStatus.CozumBildirildi && a.isComplete == false).Count(),
                    isActive = true
                });

                headers.headerFilters.Filters.Add(new HeadersTaskItem
                {
                    title = "Durdurulan Görevler",
                    filter = "{'Filter':{'Operand1':{'Operand1':{'Operand1':'assignUserId','Operator':'Equal','Operand2':'" + userId.ToString() + "'},'Operand2':{'Operand1':'lastOperationStatus','Operator':'Equal','Operand2':'" + (int)EnumFTM_TaskOperationStatus.GorevDurduruldu + "'},'Operator':'And'},'Operand2':{'Operand1':'isComplete','Operator':'Equal','Operand2':'0'},'Operator':'And'}}",
                    count = db.Table<VWFTM_Task>().Where(a => a.assignUserId == userId && a.lastOperationStatus == (int)EnumFTM_TaskOperationStatus.GorevDurduruldu && a.isComplete == false).Count(),
                    isActive = false
                });

                headers.headerFilters.Filters.Add(new HeadersTaskItem
                {
                    title = "Çözüm Bildirilen Görevler",
                    filter = "{'Filter':{'Operand1':{'Operand1':{'Operand1':'assignUserId','Operator':'Equal','Operand2':'" + userId.ToString() + "'},'Operand2':{'Operand1':'lastOperationStatus','Operator':'Equal','Operand2':'" + (int)EnumFTM_TaskOperationStatus.CozumBildirildi + "'},'Operator':'And'},'Operand2':{'Operand1':'isComplete','Operator':'Equal','Operand2':'0'},'Operator':'And'}}",
                    count = db.Table<VWFTM_Task>().Where(a => a.assignUserId == userId && a.lastOperationStatus == (int)EnumFTM_TaskOperationStatus.CozumBildirildi && a.isComplete == false).Count(),
                    isActive = false
                });

                headers.headerFilters.Filters.Add(new HeadersTaskItem
                {
                    title = "Tamamladığım Görevler",
                    filter = "{'Filter':{'Operand1':{'Operand1':'assignUserId','Operator':'Equal','Operand2':'" + userId.ToString() + "'},'Operand2':{'Operand1':'isComplete','Operator':'Equal','Operand2':'1'},'Operator':'And'}}",
                    count = db.Table<VWFTM_Task>().Where(a => a.assignUserId == userId && a.isComplete == true).Count(),
                    isActive = true
                });

                return headers;
            }
        }

        public SummaryHeadersTaskNew GetVWPA_TaskAllRequestsCountFilter(Guid userId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                var headers = new SummaryHeadersTaskNew
                {
                    AllTask = db.Table<VWFTM_Task>().Where(a => a.assignUserId == null && a.assignableUserIds == null && a.isComplete == false).Count(),
                    AllTaskFilter = "{'Filter':{'Operand1':{'Operand1':{'Operand1':'assignUserId','Operator':'IsNull','Operand2':'null'},'Operand2':{'Operand1':'assignableUserIds','Operator':'IsNull','Operand2':'null'},'Operator':'And'},'Operand2':{'Operand1':'isComplete','Operator':'Equal','Operand2':'0'},'Operator':'And'}}",

                    WaitingTask = db.Table<VWFTM_Task>().Where(a => a.assignUserId != null && a.lastOperationStatus != (int)EnumFTM_TaskOperationStatus.CozumBildirildi && a.lastOperationStatus != (int)EnumFTM_TaskOperationStatus.GorevDurduruldu && a.isComplete == false).Count(),
                    WaitingTaskFilter = "{'Filter':{'Operand1':{{'Operand1':{'Operand1':{'Operand1':'assignUserId','Operator':'IsNotNull','Operand2':'null'},'Operand2':{'Operand1':'lastOperationStatus','Operator':'NotEqual','Operand2':'30'},'Operator':'And'},'Operand2':{'Operand1':'lastOperationStatus','Operator':'NotEqual','Operand2':'26'},'Operator':'And'}},'Operand2':{'Operand1':'isComplete','Operator':'Equal','Operand2':'0'},'Operator':'And'}}",

                    CompletedTask = db.Table<VWFTM_Task>().Where(a => a.isComplete == true).Count(),
                    CompletedTaskFilter = "{'Filter':{'Operand1':'isComplete','Operator':'Equal','Operand2':'1'}}",
                };

                headers.headerFilters = new HeadersTask();
                headers.headerFilters.title = "Durumuna Göre";
                headers.headerFilters.Filters = new List<HeadersTaskItem>();

                headers.headerFilters.Filters.Add(new HeadersTaskItem
                {
                    title = "Personel Ataması Bekleyenler",
                    filter = "{'Filter':{'Operand1':{'Operand1':{'Operand1':'assignUserId','Operator':'IsNull','Operand2':'null'},'Operand2':{'Operand1':'assignableUserIds','Operator':'IsNull','Operand2':'null'},'Operator':'And'},'Operand2':{'Operand1':'isComplete','Operator':'Equal','Operand2':'0'},'Operator':'And'}}",
                    count = db.Table<VWFTM_Task>().Where(a => a.assignUserId == null && a.assignableUserIds == null && a.isComplete == false).Count(),
                    isActive = false
                });

                headers.headerFilters.Filters.Add(new HeadersTaskItem
                {
                    title = "Üstlenilmeyi Bekleyenler",
                    filter = "{'Filter':{'Operand1':{'Operand1':{'Operand1':'assignUserId','Operator':'IsNull','Operand2':'null'},'Operand2':{'Operand1':'assignableUserIds','Operator':'IsNotNull','Operand2':'null'},'Operator':'And'},'Operand2':{'Operand1':'isComplete','Operator':'Equal','Operand2':'0'},'Operator':'And'}}",
                    count = db.Table<VWFTM_Task>().Where(a => a.assignUserId == null && a.assignableUserIds != null && a.isComplete == false).Count(),
                    isActive = true
                });

                headers.headerFilters.Filters.Add(new HeadersTaskItem
                {
                    title = "Devam Edenler",
                    filter = "{'Filter':{'Operand1':{'Operand1':{'Operand1':{'Operand1':'assignUserId','Operator':'IsNotNull','Operand2':'null'},'Operand2':{'Operand1':'lastOperationStatus','Operator':'NotEqual','Operand2':'" + (int)EnumFTM_TaskOperationStatus.CozumBildirildi + "'},'Operator':'And'},'Operand2':{'Operand1':'lastOperationStatus','Operator':'NotEqual','Operand2':'" + (int)EnumFTM_TaskOperationStatus.GorevDurduruldu + "'},'Operator':'And'},'Operand2':{'Operand1':'isComplete','Operator':'Equal','Operand2':'0'},'Operator':'And'}}",
                    count = db.Table<VWFTM_Task>().Where(a => a.assignUserId != null && a.lastOperationStatus != (int)EnumFTM_TaskOperationStatus.CozumBildirildi && a.lastOperationStatus != (int)EnumFTM_TaskOperationStatus.GorevDurduruldu && a.isComplete == false).Count(),
                    isActive = true
                });

                headers.headerFilters.Filters.Add(new HeadersTaskItem
                {
                    title = "Planlanmayı Bekleyenler",
                    filter = "{'Filter':{'Operand1':{'Operand1':'planLater','Operator':'Equal','Operand2':'" + (int)EnumFTM_TaskPlanLater.Evet + "'}}}",
                    count = db.Table<VWFTM_Task>().Where(a => a.planLater == (int)EnumFTM_TaskPlanLater.Evet).Count(),
                    isActive = false
                });

                headers.headerFilters.Filters.Add(new HeadersTaskItem
                {
                    title = "Durdurulanlar",
                    filter = "{'Filter':{'Operand1':{'Operand1':{'Operand1':'assignUserId','Operator':'IsNotNull','Operand2':'null'},'Operand2':{'Operand1':'lastOperationStatus','Operator':'Equal','Operand2':'" + (int)EnumFTM_TaskOperationStatus.GorevDurduruldu + "'},'Operator':'And'},'Operand2':{'Operand1':'isComplete','Operator':'Equal','Operand2':'0'},'Operator':'And'}}",
                    count = db.Table<VWFTM_Task>().Where(a => a.assignUserId != null && a.lastOperationStatus == (int)EnumFTM_TaskOperationStatus.GorevDurduruldu && a.isComplete == false).Count(),
                    isActive = false
                });

                headers.headerFilters.Filters.Add(new HeadersTaskItem
                {
                    title = "Çözüm Onayı Bekleyenler",
                    filter = "{'Filter':{'Operand1':{'Operand1':'lastOperationStatus','Operator':'Equal','Operand2':'" + (int)EnumFTM_TaskOperationStatus.CozumBildirildi + "'},'Operand2':{'Operand1':'isComplete','Operator':'Equal','Operand2':'0'},'Operator':'And'}}",
                    count = db.Table<VWFTM_Task>().Where(a => a.lastOperationStatus == (int)EnumFTM_TaskOperationStatus.CozumBildirildi && a.isComplete == false).Count(),
                    isActive = false
                });

                headers.headerFilters.Filters.Add(new HeadersTaskItem
                {
                    title = "Çözümlenmiş Görevler",
                    filter = "{'Filter':{'Operand1':'isComplete','Operator':'Equal','Operand2':'1'}}",
                    count = db.Table<VWFTM_Task>().Where(a => a.isComplete == true).Count(),
                    isActive = true
                });

                headers.headerFilters.Filters.Add(new HeadersTaskItem
                {
                    title = "Benim Oluşturduklarım",
                    filter = "{'Filter':{'Operand1':'createdby','Operator':'Equal','Operand2':'" + userId.ToString() + "'}}",
                    count = db.Table<VWFTM_Task>().Where(a => a.createdby == userId).Count(),
                    isActive = false
                });

                return headers;
            }
        }

        public SummaryHeadersTaskNew GetVWPA_TaskCustomerRequestsCountFilter(Guid companyId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                var headers = new SummaryHeadersTaskNew
                {
                    AllTask = db.Table<VWFTM_Task>().Where(a => a.customerId == companyId).Count(),
                    AllTaskFilter = "{'Filter':{'Operand1':'customerId','Operator':'Equal','Operand2':'" + companyId.ToString() + "'}}",

                    WaitingTask = db.Table<VWFTM_Task>().Where(a => a.customerId == companyId && a.isComplete == false).Count(),
                    WaitingTaskFilter = "{'Filter':{'Operand1':{'Operand1':'customerId','Operator':'Equal','Operand2':'" + companyId.ToString() + "'},'Operand2':{'Operand1':'isComplete','Operator':'Equal','Operand2':'0'},'Operator':'And'}}",

                    CompletedTask = db.Table<VWFTM_Task>().Where(a => a.customerId == companyId && a.isComplete == true).Count(),
                    CompletedTaskFilter = "{'Filter':{'Operand1':{'Operand1':'customerId','Operator':'Equal','Operand2':'" + companyId.ToString() + "'},'Operand2':{'Operand1':'isComplete','Operator':'Equal','Operand2':'1'},'Operator':'And'}}",
                };

                headers.headerFilters = new HeadersTask();
                headers.headerFilters.title = "Durumuna Göre";
                headers.headerFilters.Filters = new List<HeadersTaskItem>();

                headers.headerFilters.Filters.Add(new HeadersTaskItem
                {
                    title = "Çözülmesi Beklenen Kayıtlar",
                    filter = "{'Filter':{'Operand1':{'Operand1':'customerId','Operator':'Equal','Operand2':'" + companyId.ToString() + "'},'Operand2':{'Operand1':'isComplete','Operator':'Equal','Operand2':'0'},'Operator':'And'}}",
                    count = db.Table<VWFTM_Task>().Where(a => a.customerId == companyId && a.isComplete == false).Count(),
                    isActive = true
                });

                headers.headerFilters.Filters.Add(new HeadersTaskItem
                {
                    title = "Planlanlanması Bekleniyor",
                    filter = "{'Filter':{'Operand1':'planLater','Operator':'Equal','Operand2':'" + (int)EnumFTM_TaskPlanLater.Evet + "'}}",
                    count = db.Table<VWFTM_Task>().Where(a => a.planLater == (int)EnumFTM_TaskPlanLater.Evet).Count(),
                    isActive = true
                });

                headers.headerFilters.Filters.Add(new HeadersTaskItem
                {
                    title = "Çözümlenmiş Kayıtlar",
                    filter = "{'Filter':{'Operand1':{'Operand1':'customerId','Operator':'Equal','Operand2':'" + companyId.ToString() + "'},'Operand2':{'Operand1':'isComplete','Operator':'Equal','Operand2':'1'},'Operator':'And'}}",
                    count = db.Table<VWFTM_Task>().Where(a => a.customerId == companyId && a.isComplete == true).Count(),
                    isActive = true
                });

                return headers;
            }
        }

        public SummaryHeadersTaskNew GetVWPA_TaskYukleniciRequestsCountFilter(Guid userId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                var headers = new SummaryHeadersTaskNew
                {
                    //AllTask = db.Table<VWFTM_Task>().Where(a => a.customerId == companyId).Count(),
                    //AllTaskFilter = "{'Filter':{'Operand1':'customerId','Operator':'Equal','Operand2':'" + companyId.ToString() + "'}}",

                    WaitingTask = db.Table<VWFTM_Task>().Where(a => a.assignUserId == userId && a.lastOperationStatus == (int)EnumFTM_TaskOperationStatus.CozumBildirildi && a.isComplete == false).Count(),
                    WaitingTaskFilter = "{'Filter':{'Operand1':{'Operand1':{'Operand1':'assignUserId','Operator':'Equal','Operand2':'" + userId.ToString() + "'},'Operand2':{'Operand1':'lastOperationStatus','Operator':'Equal','Operand2':'30'},'Operator':'And'},'Operand2':{'Operand1':'isComplete','Operator':'Equal','Operand2':'0'},'Operator':'And'}}",

                    CompletedTask = db.Table<VWFTM_Task>().Where(a => a.assignUserId == userId && a.isComplete == true).Count(),
                    CompletedTaskFilter = "{'Filter':{'Operand1':{'Operand1':'assignUserId','Operator':'Equal','Operand2':'" + userId.ToString() + "'},'Operand2':{'Operand1':'isComplete','Operator':'Equal','Operand2':'1'},'Operator':'And'}}",
                };

                headers.headerFilters = new HeadersTask();
                headers.headerFilters.title = "Durumuna Göre";
                headers.headerFilters.Filters = new List<HeadersTaskItem>();

                headers.headerFilters.Filters.Add(new HeadersTaskItem
                {
                    title = "Çözüm Bildirdiğim Görevler",
                    filter = "{'Filter':{'Operand1':{'Operand1':{'Operand1':'assignUserId','Operator':'Equal','Operand2':'" + userId.ToString() + "'},'Operand2':{'Operand1':'lastOperationStatus','Operator':'Equal','Operand2':'30'},'Operator':'And'},'Operand2':{'Operand1':'isComplete','Operator':'Equal','Operand2':'0'},'Operator':'And'}}",
                    count = db.Table<VWFTM_Task>().Where(a => a.assignUserId == userId && a.lastOperationStatus == (int)EnumFTM_TaskOperationStatus.CozumBildirildi && a.isComplete == false).Count(),
                    isActive = true
                });

                headers.headerFilters.Filters.Add(new HeadersTaskItem
                {
                    title = "Tamamladığım Görevler",
                    filter = "{'Filter':{'Operand1':{'Operand1':'assignUserId','Operator':'Equal','Operand2':'" + userId.ToString() + "'},'Operand2':{'Operand1':'isComplete','Operator':'Equal','Operand2':'1'},'Operator':'And'}}",
                    count = db.Table<VWFTM_Task>().Where(a => a.assignUserId == userId && a.isComplete == true).Count(),
                    isActive = true
                });

                return headers;
            }
        }
    }
}
