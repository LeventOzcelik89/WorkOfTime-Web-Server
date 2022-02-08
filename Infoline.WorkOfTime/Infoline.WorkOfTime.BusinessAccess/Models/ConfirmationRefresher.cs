using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Data.Common;
using System.Linq;

namespace Infoline.WorkOfTime.BusinessAccess.Models
{
    public class ConfirmationRefresher
    {
        private WorkOfTimeDatabase db { get; set; } = new WorkOfTimeDatabase();
        private DbTransaction Transaction { get; set; }
        private Guid UserId { get; set; }
        public ConfirmationRefresher()
        {
            this.db = this.db ?? new WorkOfTimeDatabase();

        }
        public ResultStatus RefreshAll(Guid userId, DbTransaction trans = null)
        {
            var res = new ResultStatus { result = true };
            this.UserId = userId;
            res &= RefreshLeave(trans);
            res &= RefreshAdvance(trans);
            res&=RefreshAssignment(trans);
            res &= RefreshTransaction(trans);
            return res;
        }
        public ResultStatus RefreshAdvance(DbTransaction trans = null)
        {
            var getAllAdvanceConfirmation = this.db.GetPA_AdvanceByUserIdAndStatusIsNotProved(this.UserId);
            Transaction = trans ?? db.BeginTransaction();
            var result = new ResultStatus { result = true };
            foreach (var item in getAllAdvanceConfirmation.GroupBy(x => x.advanceId))
            {
                if (!item.Key.HasValue)
                {
                    continue;
                }
                var findAdvance = db.GetPA_AdvanceById(item.Key.Value);
                if (findAdvance == null)
                {
                    continue;
                }
                var findConfirmation = db.GetVWPA_AdvanceConfirmationByAdvanceId(findAdvance.id).ToList();
                if (findConfirmation.Count() == 0)
                {
                    findAdvance.direction = (short)EnumPA_AdvanceDirection.Cikis;
                }
                else
                {
                    result &= db.BulkDeletePA_AdvanceConfirmation(findConfirmation.Where(x => x.userId == this.UserId&&x.status==null).B_ConvertType<PA_AdvanceConfirmation>(), trans);
                    findConfirmation.RemoveAll(x => x.userId == this.UserId && x.status == null);
                    var isHave = findConfirmation.Where(x => x.status == null);
                    if (isHave.Count() == 0)
                    {
                        findAdvance.direction = (short)EnumPA_AdvanceDirection.Cikis;
                    }
                }
                result = db.UpdatePA_Advance(findAdvance, false, Transaction);
            }
            if (trans == null)
            {
                if (result.result)
                {
                    Transaction.Commit();
                }
                else
                {
                    Transaction.Rollback();
                }
            }
            return result;
        }
        public ResultStatus RefreshTransaction(DbTransaction trans)
        {
            var getAllTransactionConfirmation = db.GetPA_TransactionConfirmationByUserIdAndIsNotApporeved(this.UserId);
            Transaction = trans ?? db.BeginTransaction();
            var result = new ResultStatus { result = true };
            foreach (var item in getAllTransactionConfirmation.GroupBy(x => x.transactionId))
            {
                if (!item.Key.HasValue)
                {
                    continue;
                }
                var findTrans = db.GetPA_TransactionById(item.Key.Value);
                if (findTrans == null)
                {
                    continue;
                }
                var findConfirmation = db.GetPA_TransactionConfirmationByTransactionId(findTrans.id).ToList();
                if (findConfirmation.Count() == 0)
                {
                    findTrans.direction = (short)EnumPA_TransactionDirection.Cikis;
                }
                else
                {
                    result &= db.BulkDeletePA_TransactionConfirmation(findConfirmation.Where(x => x.userId == this.UserId && x.status == null), trans);
                    var remove=findConfirmation.RemoveAll(x => x.userId == this.UserId && x.status == null);
                    var isHave = findConfirmation.Where(x => x.status == null);
                    if (isHave.Count() == 0)
                    {
                        findTrans.direction = (short)EnumPA_TransactionDirection.Cikis;
                    }
                }
                result &= db.UpdatePA_Transaction(findTrans, false, Transaction);
            }
            if (trans == null)
            {
                if (result.result)
                {
                    Transaction.Commit();
                }
                else
                {
                    Transaction.Rollback();
                }
            }
            return result;
        }
        public ResultStatus RefreshLeave(DbTransaction trans)
        {
            Transaction = trans ?? db.BeginTransaction();
            var result = new ResultStatus { result = true };
            var getAllLeave = db.GetINV_PermitApprovedByStatus(this.UserId);
            foreach (var item in getAllLeave)
            {
                if (item.IkApproval == this.UserId)
                {
                    item.IkApproval = null;
                    item.IkApprovalDate = DateTime.Now;
                }
                else if (item.Manager1Approval == this.UserId)
                {
                    item.Manager1Approval = null;
                    item.Manager1ApprovalDate = DateTime.Now;
                }
                else if (item.Manager2Approval == this.UserId)
                {
                    item.Manager2Approval = null;
                    item.Manager2ApprovalDate = DateTime.Now;
                }

                if (item.IkApprovalDate.HasValue && item.Manager1ApprovalDate.HasValue && item.Manager2ApprovalDate.HasValue)
                {
                    item.ApproveStatus = (short)EnumINV_PermitApproveStatus.IslakImzaYuklendi;
                }
                result &= db.UpdateINV_Permit(item, true, trans);
            }
            if (trans == null)
            {
                if (result.result)
                {
                    Transaction.Commit();
                }
                else
                {
                    Transaction.Rollback();
                }
            }
            return result;



        }
        public ResultStatus RefreshAssignment(DbTransaction trans)
        {
            Transaction = trans ?? db.BeginTransaction();
            var result = new ResultStatus { result = true };
            var getAllLeave = db.GetVWINV_CommissionsHasUserId(this.UserId);
            foreach (var item in getAllLeave)
            {
                if (item.Manager1Approval == this.UserId)
                {
                    item.Manager1Approval = null;
                    item.Manager1ApprovalDate = DateTime.Now;
                    item.ApproveStatus = (short)EnumINV_CommissionsApproveStatus.Onaylandi;
                }
                result &= db.UpdateINV_Commissions(item.B_ConvertType<INV_Commissions>(), true, trans);
            }
            if (trans == null)
            {
                if (result.result)
                {
                    Transaction.Commit();
                }
                else
                {
                    Transaction.Rollback();
                }
            }
            return result;



        }




    }

}

