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
        private DbTransaction Transaction  { get;set; }
        private Guid UserId { get; }
        public ConfirmationRefresher(Guid userId)
        {
            this.db = this.db ?? new WorkOfTimeDatabase();
            this.UserId = userId;
        }


        public ResultStatus RefreshAll(DbTransaction trans=null)
        {
            var res = new ResultStatus { result = true };
            //res&=RefreshLeave();
            res&=RefreshAdvance(trans);
            //res&=RefreshAssignment();
            res&=RefreshTransaction(trans);
            return res;
        }
        public ResultStatus RefreshAdvance(DbTransaction trans=null)
        {
            var getAllAdvanceConfirmation = this.db.GetPA_AdvanceByUserIdAndStatusIsNotProved(this.UserId);
            Transaction = trans ?? db.BeginTransaction();
            var result = new ResultStatus {result=true };
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
                var findConfirmation = db.GetVWPA_AdvanceConfirmationByAdvanceId(findAdvance.id);
                if (findConfirmation.Count() == 0)
                {
                    findAdvance.direction = (short)EnumPA_AdvanceDirection.Cikis;
                }
                else
                {
                    findConfirmation.Where(x => x.userId == this.UserId).ToList().ForEach(x => x.status = (short)EnumPA_AdvanceConfirmationStatus.Onay);
                    var isHave= findConfirmation.Where(x => x.status == null);
                    if (isHave.Count()==0)
                    {
                        findAdvance.direction= (short)EnumPA_AdvanceDirection.Cikis;
                    }
                }
                result = db.UpdatePA_Advance(findAdvance,false, Transaction);
                result &= db.BulkUpdatePA_AdvanceConfirmation(findConfirmation.B_ConvertType<PA_AdvanceConfirmation>(),false, Transaction);
            }
            if (trans==null)
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
                var findConfirmation = db.GetVWPA_AdvanceConfirmationByAdvanceId(findTrans.id);
                if (findConfirmation.Count() == 0)
                {
                    findTrans.direction = (short)EnumPA_AdvanceDirection.Cikis;
                }
                else
                {
                    findConfirmation.Where(x => x.userId == this.UserId).ToList().ForEach(x => x.status = (short)EnumPA_AdvanceConfirmationStatus.Onay);
                    var isHave = findConfirmation.Where(x => x.status == null);
                    if (isHave.Count() == 0)
                    {
                        findTrans.direction = (short)EnumPA_AdvanceDirection.Cikis;
                    }
                }
                result = db.UpdatePA_Transaction(findTrans, false, Transaction);
                result &= db.BulkUpdatePA_TransactionConfirmation(findConfirmation.B_ConvertType<PA_TransactionConfirmation>(), false, Transaction);
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
        public void RefreshLeave()
        {

        }
        public void RefreshAssignment()
        {




        }




    }

}

