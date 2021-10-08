using GeoAPI.Geometries;
using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessAccess.Models;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Data.Common;
using System.Linq;
using System.Web;
namespace Infoline.WorkOfTime.BusinessAccess.Business.Product
{
   
    public class VMPRD_TitanDeviceActivated : VWPRD_TitanDeviceActivated
    {
        private TitanServices TitanServices = new TitanServices();
        private WorkOfTimeDatabase db { get; set; }
        private DbTransaction trans { get; set; }
        public IGeometry Location { get; set; }
        public VMPRD_TitanDeviceActivated Load()
        {
            this.db = this.db ?? new WorkOfTimeDatabase();
            if (this.SerialNumber != null)
            {
                return new VMPRD_TitanDeviceActivated().B_EntityDataCopyForMaterial(db.GetPRD_TitanDeviceActivatedBySerialCode(SerialNumber)??new PRD_TitanDeviceActivated());
            }
            else if (this.InventoryId != null)
            {
                return new VMPRD_TitanDeviceActivated().B_EntityDataCopyForMaterial(db.GetPRD_TitanDeviceActivatedByInventoryId(InventoryId.Value) ?? new PRD_TitanDeviceActivated());
            }
            else if (this.id != null)
            {
                return new VMPRD_TitanDeviceActivated().B_EntityDataCopyForMaterial(db.GetPRD_TitanDeviceActivatedById(id) ?? new PRD_TitanDeviceActivated());
            }
            return this;
        }
        public IndexData GetIndexData()
        {
            this.db = this.db ?? new WorkOfTimeDatabase();
            var getAllCount = db.GetPRD_TitanDeviceActivatedCount();
            var getTodayCount = db.GetPRD_TitanDeviceActivatedTodayCount();
            var getSevenDayCount = db.GetPRD_TitanDeviceActivatedSevenDaysCount();
            var getMonthCount = db.GetPRD_TitanDeviceActivatedThirtyDaysCount();
            return
                new IndexData
                {
                    All = getAllCount,
                    Today = getTodayCount,
                    Seven = getSevenDayCount,
                    Month = getMonthCount
                };
            
        }
        public ResultStatus GetAllDevices()
        {
            return TitanServices.GetAllDevices();
        }
        public ResultStatus GetDeviceById(Guid id)
        {
            return TitanServices.GetDeviceById(id);
        }
        public ResultStatus GetDeviceInformation(Guid id)
        {
            return TitanServices.GetDeviceInformation(id);
        }
        public ResultStatus GetDeviceActivationInformation()
        {
            return TitanServices.GetDeviceActivationInformations();
        }
    }
    public class IndexData
    {
        public int All { get; set; }
        public int Today { get; set; }
        public int Seven { get; set; }
        public int Month { get; set; }
    }
}
