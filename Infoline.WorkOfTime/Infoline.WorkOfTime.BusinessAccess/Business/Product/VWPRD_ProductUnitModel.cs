using Infoline.WorkOfTime.BusinessData;
using System;
using Infoline.Framework.Database;
using System.Data.Common;
using System.Web;
using System.Linq;

namespace Infoline.WorkOfTime.BusinessAccess
{
    public class VWPRD_ProductUnitModel : VWPRD_ProductUnit
    {
        private WorkOfTimeDatabase db { get; set; }
        private DbTransaction trans { get; set; }
        public VWPRD_ProductUnit productUnit { get; set; }
        public VWPRD_ProductUnitModel Load()
        {
            db = db ?? new WorkOfTimeDatabase();
            var productUnit = db.GetVWPRD_ProductUnitById(this.id);

            if (productUnit != null)
            {
                this.B_EntityDataCopyForMaterial(productUnit, true);
                
            }

            if (this.productId.HasValue)
            {
                var productUnits = db.GetVWPRD_ProductUnitByProductId(this.productId.Value);
                if (productUnits.Any())
                {
                    this.productUnit = productUnits.FirstOrDefault(a => a.isDefault == (int)EnumPRD_ProductUnitIsDefault.Evet);
                }
            }

            this.isDefault = this.isDefault ?? (int)EnumPRD_ProductUnitIsDefault.Hayir;

            return this;
        }
    }
}
