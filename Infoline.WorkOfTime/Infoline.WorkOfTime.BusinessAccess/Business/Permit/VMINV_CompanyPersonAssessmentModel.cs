using Infoline.WorkOfTime.BusinessData;
using System;
using System.Data.Common;

namespace Infoline.WorkOfTime.BusinessAccess
{
	public class VMINV_CompanyPersonAssessmentModel : VWINV_CompanyPersonAssessment
    {
		private WorkOfTimeDatabase db { get; set; }
		private DbTransaction trans { get; set; }
	}
    public class VMVWINV_CompanyPersonAssessment : VWINV_CompanyPersonAssessment
    {
        public INV_CompanyPersonAssessmentRating[] Rating { get; set; }
        public VWSH_User Owner { get; set; }
        public string task { get; set; }
        public string department { get; set; }
        public DateTime? jobStartDate { get; set; }
        public string Url { get; set; }
    }

    public class VWINV_CompanyPersonAssessmentPageReport
    {
        public int ToplamDegerlendirilecekPersonelSayisi { get; set; }
        public int IslemimdenGecmisDegerlendirmelerinSayisi { get; set; }
        public int OnayiminBeklendigiDegerlendirmeler { get; set; }
        public int DegerlendirmeleriminSayisi { get; set; }
        public int IslakImzaBeklenenDegerlendirmeler { get; set; }
        public int SureciTamamlananDegerlendirmeler { get; set; }
        public int OnaySurecindekiDegerlendirmeler { get; set; }
        public int ToplamDegerlendirmelerinSayisi { get; set; }
        public Guid? idUser { get; set; }
    }
}
