using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessAccess.Models;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Infoline.WorkOfTime.WebProject.Areas.PRJ.Models
{
    public enum ENUMPRJ_CostType
    {
        [Description("Fatura Maliyeti")]
        fatura = 1,
        [Description("Personel Geliştirme Maliyeti")]
        projeMaliyet = 0,
        [Description("Personel Bakım Maliyeti")]
        bakimMaliyet = 2,

    }

    public class VMPRJ_ProjectModel : VWPRJ_Project
    {
        public List<Guid?> taskIds { get; set; }
		public Guid? ProjectSponsorId { get; set; }
        public Guid? ProjectManagerId { get; set; }

    }

    public class PRJ_ProjectCost
    {
        public double Cost { get; set; }
        public int CostType { get; set; }
        public string CostItem { get; set; }
        public string Position_Title { get; set; }
        public DateTime CostDate { get; set; }
    }


    public class PRJ_ProjectCostModel
    {
        public VWCMP_Invoice[] Invoice { get; set; }

        public VWINV_CompanyPersonAvailability[] Project { get; set; }

        public VWINV_CompanyPersonAvailability[] Maintenance { get; set; }

        public PRJ_ProjectCostModel(Guid ProjectId)
        {
            var db = new WorkOfTimeDatabase();
            //TODO:Invoice
            this.Invoice = db.GetVWCMP_InvoiceByProjectId(ProjectId);
            this.Project = new INV_CompanyPersonAvailabilityModel(ProjectId, EnumINV_CompanyPersonAvailabilityType.Proje).GetSchema();
            this.Maintenance = new INV_CompanyPersonAvailabilityModel(ProjectId, EnumINV_CompanyPersonAvailabilityType.Bakim).GetSchema();
        }

        public PRJ_ProjectCost[] Calculate()
        {
            try
            {
                List<PRJ_ProjectCost> returnList = new List<PRJ_ProjectCost>();


                var list = new List<VMVWINV_CompanyPersonAvailability>();

                #region ProjectCost

                var ProjectCosts = new ProjectPersonModel(Project).Calculate();

                if (ProjectCosts.Count() != 0)
                {
                    list.AddRange(ProjectCosts);
                    foreach (var ProjectCost in ProjectCosts)
                    {
                        
                        returnList.Add(new PRJ_ProjectCost
                        {
                            Cost = ProjectCost.Salary,
                            CostDate = new DateTime(ProjectCost.StartDate.Value.Year, ProjectCost.StartDate.Value.Month, 1),
                            CostItem = ProjectCost.Person_Title,
                            CostType = (int)ENUMPRJ_CostType.projeMaliyet
                        });
                    }
                }
                #endregion



                #region MaintenanceCosts

                var MaintenanceCosts = new ProjectPersonModel(Maintenance).Calculate();

                if (MaintenanceCosts.Count() != 0)
                {
                    list.AddRange(MaintenanceCosts);
                    foreach (var MaintenanceCost in MaintenanceCosts)
                    {
                        returnList.Add(new PRJ_ProjectCost
                        {
                            Cost = (double)MaintenanceCost.Salary,
                            CostDate = new DateTime(MaintenanceCost.StartDate.Value.Year, MaintenanceCost.StartDate.Value.Month, 1),
                            CostItem = MaintenanceCost.Person_Title,
                            CostType = (int)ENUMPRJ_CostType.bakimMaliyet
                        });
                    }
                }
                #endregion



                #region Invoice

            
                //TODO INvoice

                foreach (var date in list.GroupBy(x => x.StartDate.Value.ToString("MM.yyyy")).Select(x => x.Key.Split('.')))
                {

                    var monthlyInvoiceCost = this.Invoice.Where(x => x.issueDate.Value.Year == Convert.ToInt32(date[1]) && x.issueDate.Value.Month == Convert.ToInt32(date[0])).Sum(x => x.totalAmount);

                    returnList.Add(new PRJ_ProjectCost
                    {
                        Cost = monthlyInvoiceCost == null ? 0 : (double)monthlyInvoiceCost,
                        CostDate = new DateTime(Convert.ToInt32(date[1]), Convert.ToInt32(date[0]), 1),
                        CostItem = "Fatura",
                        CostType = (int)ENUMPRJ_CostType.fatura,
                        //Position_Title = availability.Position_Title
                    });
                }
                #endregion


                return returnList.ToArray();
            }
            catch
            {
                return new PRJ_ProjectCost[] { };
            }

        }
    }



}


