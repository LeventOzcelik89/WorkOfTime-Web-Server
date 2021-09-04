using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Data.Common;
using System.Linq;

namespace Infoline.WorkOfTime.BusinessAccess
{

    public class SummaryHeadersCommission
    {
        public int waiting { get; set; }
        public string waitingFilter { get; set; }
        public int approved { get; set; }
        public string approvedFilter { get; set; }
        public int rejected { get; set; }
        public string rejectedFilter { get; set; }
        public string searchField { get; set; }
    }

    public partial class WorkOfTimeDatabase
    {

        public VWINV_CommissionsPersons GetVWINV_CommissionsPersonsByControlDate(Guid idUser, DateTime? start, DateTime? end)
        {
            using (var db = GetDB())
            {
                return db.Table<VWINV_CommissionsPersons>().Where(a => a.IdUser == idUser && (a.ApproveStatus != (Int32)EnumINV_CommissionsApproveStatus.Reddedildi) &&
                (((a.StartDate <= start && a.EndDate >= end) ||
                (a.StartDate < start && a.EndDate > start && a.EndDate < end) ||
                (a.StartDate > start && a.StartDate < end && a.EndDate > end) ||
                (start <= a.StartDate && end >= a.EndDate)
                )))
                .OrderByDesc(x => x.created).Execute().FirstOrDefault();
            }
        }

        public VWINV_CommissionsPersons[] GetVWINV_CommissionsPersonCommissionIds(Guid commissionsId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWINV_CommissionsPersons>().Where(a => a.id == commissionsId).OrderByDesc(x => x.created).Execute().ToArray();
            }
        }


        public VWINV_CommissionsPersons[] GetVWINV_CommissionsPersonsByYear(DateTime date)
        {
            using (var db = GetDB())
            {
                var firstDate = new DateTime(date.Year, 1, 1);
                var lastDate = new DateTime(date.Year, 12, 31, 23, 59, 59);
                return db.Table<VWINV_CommissionsPersons>().Where(x => x.StartDate >= firstDate && x.StartDate <= lastDate).OrderBy(x => x.created).Execute().ToArray();
            }
        }

        public VWINV_CommissionsPersons[] GetVWINV_CommissionsPersonsByJustUserId(Guid idUser)
        {
            using (var db = GetDB())
            {
                return db.Table<VWINV_CommissionsPersons>().Where(x => x.IdUser == idUser).OrderByDesc(a => a.created).Execute().ToArray();
            }
        }

        public SummaryHeadersCommission GetVWINV_CommissionsPersonsByIdUserCounts(Guid userId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return new SummaryHeadersCommission
                {
                    waiting = db.Table<VWINV_CommissionsPersons>().Where(c => c.IdUser == userId && (c.ApproveStatus == (Int32)EnumINV_CommissionsApproveStatus.Bekliyor || c.ApproveStatus== (Int32)EnumINV_CommissionsApproveStatus.Onaylandi)).Count(),
                    waitingFilter = "{'Filter':{'Operand1':{'Operand1':'IdUser','Operator':'Equal','Operand2':'" + userId + "'},'Operand2':{'Operand1':{'Operand1':'ApproveStatus','Operator':'Equal','Operand2':'0'},'Operand2':{'Operand1':'ApproveStatus','Operator':'Equal','Operand2':'1'},'Operator':'Or'},'Operator':'And'}}",
                    approved = db.Table<VWINV_CommissionsPersons>().Where(c => c.IdUser == userId && c.ApproveStatus == (Int32)EnumINV_CommissionsApproveStatus.IslakImzaYuklendi).Count(),
                    approvedFilter = "{'Filter':{'Operand1':{'Operand1':'IdUser','Operator':'Equal','Operand2':'" + userId + "'},'Operand2':{'Operand1':'ApproveStatus','Operator':'Equal','Operand2':'3'},'Operator':'And'}}",
                    rejected = db.Table<VWINV_CommissionsPersons>().Where(c => c.IdUser == userId && c.ApproveStatus == (Int32)EnumINV_CommissionsApproveStatus.Reddedildi).Count(),
                    rejectedFilter = "{'Filter':{'Operand1':{'Operand1':'IdUser','Operator':'Equal','Operand2':'" + userId + "'},'Operand2':{'Operand1':'ApproveStatus','Operator':'Equal','Operand2':'2'},'Operator':'And'}}",

                    searchField = "{'Filter':{'Operand1':{'Operand1':'IdUser','Operator':'Equal','Operand2':'" + userId + "'},'Operand2':{'Operand1':'searchField','Operator':'Like','Operand2':'%#%'},'Operator':'And'}}"
                };
            }
        }

        public SummaryHeadersCommission GetVWINV_CommissionsPersonsAboutByIdUserCounts(Guid userid, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return new SummaryHeadersCommission
                {
                    waiting = db.Table<VWINV_CommissionsPersons>().Where(a => a.Manager1Approval == userid && a.ApproveStatus == (Int32)EnumINV_CommissionsApproveStatus.Bekliyor && a.IsOwner == true).Count(),
                    waitingFilter = "{'Filter':{'Operand1':{'Operand1':{'Operand1':'Manager1Approval','Operator':'Equal','Operand2':'" + userid + "'},'Operand2':{'Operand1':'ApproveStatus','Operator':'Equal','Operand2':'0'},'Operator':'And'},'Operand2':{'Operand1':'IsOwner','Operator':'Equal','Operand2':'True'},'Operator':'And'}}",

                    approved = db.Table<VWINV_CommissionsPersons>().Where(a => a.Manager1Approval == userid && a.IsOwner == true && 
                    (a.ApproveStatus == (Int32)EnumINV_CommissionsApproveStatus.Onaylandi || 
                    a.ApproveStatus == (Int32)EnumINV_CommissionsApproveStatus.IslakImzaYuklendi)).Count(),
                    approvedFilter = "{'Filter':{'Operand1':{'Operand1':" +
                    "{'Operand1':'Manager1Approval','Operator':'Equal','Operand2':'" + userid + "'}," +
                    "'Operand2':{'Operand1':'IsOwner','Operator':'Equal','Operand2':'True'}," +
                    "'Operator':'And'}," +
                    "'Operand2':" +
                    "{'Operand1':{'Operand1':'ApproveStatus','Operator':'Equal','Operand2':'1'}," +
                    "'Operand2':{'Operand1':'ApproveStatus','Operator':'Equal','Operand2':'3'}," +
                    "'Operator':'Or'},'Operator':'And'}}",

                    rejected = db.Table<VWINV_CommissionsPersons>().Where(a => a.Manager1Approval == userid && a.ApproveStatus == (Int32)EnumINV_CommissionsApproveStatus.Reddedildi && a.IsOwner == true).Count(),
                    rejectedFilter = "{'Filter':{'Operand1':{'Operand1':{'Operand1':'Manager1Approval','Operator':'Equal','Operand2':'" + userid + "'},'Operand2':{'Operand1':'ApproveStatus','Operator':'Equal','Operand2':'2'},'Operator':'And'},'Operand2':{'Operand1':'IsOwner','Operator':'Equal','Operand2':'True'},'Operator':'And'}}",

                    searchField = "{'Filter':{'Operand1':{'Operand1':{'Operand1':'Manager1Approval','Operator':'Equal','Operand2':'" + userid + "'},'Operand2':{'Operand1':'IsOwner','Operator':'Equal','Operand2':'True'},'Operator':'And'},'Operand2':{'Operand1':'searchField','Operator':'Like','Operand2':'%#%'},'Operator':'And'}}",
                };
            }
        }
        public VWINV_CommissionsPersons[] GetVWINV_CommissionsPending(Guid IdUser, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWINV_CommissionsPersons>().Where(a => a.Manager1Approval == IdUser && a.ApproveStatus == (Int32)EnumINV_CommissionsApproveStatus.Bekliyor && a.IsOwner == true).Execute().ToArray();
            }
        }

        public VWINV_CommissionsPersons[] GetVWINV_CommissionsSinePending(Guid idUser, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWINV_CommissionsPersons>().Where(a => a.IdUser == idUser && a.ApproveStatus == (Int32)EnumINV_CommissionsApproveStatus.Onaylandi && (a.Files == null || a.Files == "")).Execute().ToArray();
            }
        }

       

        public VWINV_CommissionsPersons[] GetVWINV_CommissionsPersonsByIds(Guid[] ids, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWINV_CommissionsPersons>().Where(a => a.id.In(ids)).Execute().ToArray();
            }
        }
    }
}
