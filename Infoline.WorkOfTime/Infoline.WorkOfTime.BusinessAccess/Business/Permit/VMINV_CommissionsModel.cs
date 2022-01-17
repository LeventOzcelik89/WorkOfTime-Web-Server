using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace Infoline.WorkOfTime.BusinessAccess
{
    public class VMINV_CommissionsModel : VWINV_Commissions
    {
        private WorkOfTimeDatabase db { get; set; }
        private DbTransaction trans { get; set; }
        public SummaryHeadersCommissionsNew GetMyCommissionsSummary(Guid userId)
        {
            db = new WorkOfTimeDatabase();
            return db.GetVWINV_CommissionsMyCommissionsCountFilter(userId);
        }

        public SummaryHeadersCommissionsNew GetRequestCommissionsSummary(Guid userId)
        {
            db = new WorkOfTimeDatabase();
            return db.GetVWINV_CommissionsRequestCommissionsCountFilter(userId);
        }
    }

    public class VWINV_CommissionsForm : VWINV_Commissions
    {
        public VWSH_User CurrentUser { get; set; }
        public VWINV_CommissionsPersons[] Users { get; set; }
        public VWINV_CommissionsProjects[] Projects { get; set; }
        public CalculateReturn Calc { get; set; }
        public string Url { get; set; }
        public string Departman { get; set; }
        public string Position { get; set; }
        public string JobStartDate { get; set; }
        public string CommissionTime { get; set; }
        public string LogoPath { get; set; }

    }


    public class VWINV_CommissionsPageReport
    {
        public string BuYilEnCokGorevlendirilenPersonel { get; set; }
        public int BuYilGorevlendirilmeSayisi { get; set; }
        public int BuHaftaGorevlendirilmeSayisi { get; set; }
        public int BuAyGorevlendirilmeSayisi { get; set; }
        public string EnFazlaGorevlendirilmeTipi { get; set; }
        public Guid? idUser { get; set; }
    }

    public class DataCompanyPersonDepartsment
    {
        public string Position_Title { get; set; }
        public string Manager1_Title { get; set; }
        public string LogoPath { get; set; }
    }

    public class SummaryHeadersCommissionsNew
    {
        public HeadersCommissions headerFilters { get; set; }
    }

    public class HeadersCommissions
    {
        public string title { get; set; }
        public List<HeadersCommissionsItem> Filters { get; set; }
    }

    public class HeadersCommissionsItem
    {
        public string title { get; set; }
        public int count { get; set; }
        public string filter { get; set; }
        public bool isActive { get; set; }
    }
}
