using Infoline.Framework.Database;
using Infoline.Framework.Helper;
using Infoline.WorkOfTime.BusinessAccess.Models;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.BusinessAccess
{
    public class VMPRD_EntegrationActionModel : VWPRD_EntegrationAction
    {
        private WorkOfTimeDatabase db { get; set; }
        private DbTransaction trans { get; set; }
        public ResultStatus DistDataSourceReport { get; set; }
        public ResultStatus DistAndProductDataSourceReport { get; set; }
        public ResultStatus DistData { get; set; }
        public ResultStatus DealarDetailData { get; set; }
        public ResultStatus DealarAndProductData { get; set; }
        public string DistName { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public VWPRD_EntegrationAction Load()
        {
            db = db ?? new WorkOfTimeDatabase();
            var entegrationAction = db.GetVWPRD_ProductBonusById(this.id);
            if (entegrationAction != null)
            {
                this.B_EntityDataCopyForMaterial(entegrationAction, true);
            }
            this.DistName = db.GetCMP_CompanyById(this.DistributorId.Value).name;
            return this;
        }
        public ResultStatus SellOutReportData(DateTime startDate, DateTime endDate)
        {
            db = db ?? new WorkOfTimeDatabase();
            var getData = db.GetPRD_SellOutReportByDistrubitor(startDate, endDate);

            return new ResultStatus
            {
                result = true,
                objects = getData
            };
        }
        public ResultStatus SellOutProductReportData(DateTime startDate, DateTime endDate)
        {
            db = db ?? new WorkOfTimeDatabase();
            var getData = db.GetPRD_SellOutProductReportDistrubitorQuery(startDate, endDate);

            return new ResultStatus
            {
                result = true,
                objects = getData
            };
        }

        public ResultStatus SellOutProductDetailDistData(DateTime startDate, DateTime endDate,Guid DistrubitorId)
        {
            db = db ?? new WorkOfTimeDatabase();
            var getData = db.GetPRD_SellOutReportByDistrubitorDetail(startDate, endDate, DistrubitorId);

            return new ResultStatus
            {
                result = true,
                objects = getData
            };
        }

        public ResultStatus SellOutProductDealarData(DateTime startDate, DateTime endDate, Guid DistrubitorId)
        {
            db = db ?? new WorkOfTimeDatabase();
            var getData = db.GetPRD_SellOutReportByDealar(startDate, endDate, DistrubitorId);

            return new ResultStatus
            {
                result = true,
                objects = getData
            };
        }

        public ResultStatus SellOutProductDealarProductData(DateTime startDate, DateTime endDate, Guid DistrubitorId)
        {
            db = db ?? new WorkOfTimeDatabase();
            var getData = db.GetPRD_SellOutProductReportDealarQuery(startDate, endDate, DistrubitorId);

            return new ResultStatus
            {
                result = true,
                objects = getData
            };
        }
    }
    public class SellOutReportNewModel
    {
        public Guid DistributorId { get; set; }
        public string DistributorName { get; set; }
        public Guid DealarId { get; set; }
        public string DealarName { get; set; }
        public int DistSalesCount { get; set; }
        public int SalesCount { get; set; }
        public int ActivatedData { get; set; }
        public int AssignmentCount { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public SellOutProductReportModel[] products { get; set; }
    }

    public class SellOutProductReportModel
    {
        public Guid DistributorId { get; set; }
        public string DistributorName { get; set; }
        public int DistSalesCount { get; set; }
        public int SalesCount { get; set; }
        public int ActivatedData { get; set; }
        public int AssignmentCount { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
    }
}
