using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infoline.WorkOfTime.BusinessAccess.Mobile
{
    public class Order
    {
        public ResultStatus<Splitted<VWCMP_Order>> MyOrder(Guid userId)
        {
            //&& x.direction == (int)EnumCMP_InvoiceDirectionType.Satis
            var db = new WorkOfTimeDatabase();
            var companyIds = db.GetVWCMP_StorageBySupervisorIdOfCompanyIds(userId).ToArray();
            var orders = db.GetVWCMP_Order();
            if (companyIds != null && companyIds.Count() > 0)
            {
                orders = orders.Where(x => x.createdby == userId || companyIds.Contains(x.customerId.Value)).ToArray();
            }
            else
            {
                orders = orders.Where(x => x.createdby == userId).ToArray();
            }
            var res = new List<VWCMP_Order>();



            foreach (var data in orders)
            {
                var item = new VWCMP_Order().B_EntityDataCopyForMaterial(data, true);
                res.Add(item);
            }

            var tumu = res.ToArray();
            var bekleyen = res.Where(a => a.status == (int)EnumCMP_OrderStatus.CevapBekleniyor && a.direction == (int)EnumCMP_InvoiceDirectionType.Satis).ToArray();
            var onaylanan = res.Where(a => a.status == (int)EnumCMP_OrderStatus.Onay && a.direction == (int)EnumCMP_InvoiceDirectionType.Satis).ToArray();
            var faturasiKesilen = res.Where(a => a.status == (int)EnumCMP_OrderStatus.SurecTamamlandi && a.direction == (int)EnumCMP_InvoiceDirectionType.Satis).ToArray();
            var reddedilen = res.Where(a => a.status == (int)EnumCMP_OrderStatus.Red && a.direction == (int)EnumCMP_InvoiceDirectionType.Satis).ToArray();

            return new ResultStatus<Splitted<VWCMP_Order>>
            {
                result = true,
                objects = new Splitted<VWCMP_Order>
                {
                    All = tumu,
                    Approved = onaylanan,
                    Waiting = bekleyen,
                    Invoiced = faturasiKesilen,
                    Declined = reddedilen
                }
            };
        }
    }
}

