using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;

namespace Infoline.WorkOfTime.WebProject.Areas.HR.Models
{
    public class StaffNeedsSearch
    {
        public int? EEducation { get; set; }
        public Guid[] Keywords2 { get; set; }
        public Guid[] Position2 { get; set; }
        public int? Military { get; set; }
        public int? Marital { get; set; }
        public Guid? City { get; set; }
        public int? Exprience { get; set; }
        public int? Result { get; set; }

    }
    public class VMStaffNeedsAndSearch : VWHR_StaffNeeds
    {
        //public VWHR_StaffNeeds StaffNeed { get; set; }
        public VWHR_StaffNeedsPerson StaffNeedPerson { get; set; }
        public StaffNeedsSearch filter { get; set; }
        public Guid?[] personPosition { get; set; }
        public Guid?[] personKeywords { get; set; }
        public int Totalcv { get; set; }
        public Card cards { get; set; }
        public string requesters { get; set; }

    }
    public class WMHR_Person : VWHR_Person
    {
        public HR_Plan HR_Plan { get; set; }
        public HR_Plan[] HR_Plans { get; set; }
        public Guid[] Position { get; set; }
        public Guid[] Keywords { get; set; }
        public Guid?[] personPosition { get; set; }
        public Guid?[] personKeywords { get; set; }
        public PDS_FormResult item { get; set; }
        public VWPDS_QuestionForm[] questions { get; set; }

        public PDS_QuestionResult[] questionResult { get; set; }

        public Card cards { get; set; }
    }

    public class Card
    {
        public int Olumlu { get; set; }
        public int Olumsuz { get; set; }
        public int Tekrar { get; set; }
        public int Diger { get; set; }
        public int Gorusulmedi { get; set; }
        public int DahaSonra { get; set; }

    }

    public class VMHR_PlanAndPerson : VWHR_PlanPerson
    {
        public VWHR_Plan hrPlan { get; set; }
        public Guid?[] persons { get; set; }
        public PDS_FormResult item { get; set; }
        public VWPDS_QuestionForm[] questions { get; set; }
        public PDS_QuestionResult[] questionResult { get; set; }
        public HR_Person HrPerson { get; set; }
        public Guid?[] personPosition { get; set; }
        public Guid?[] personKeywords { get; set; }
    }
    public class VMDetailPlanPersonAndPds
    {
        public PDS_FormResult PDS_FormResult { get; set; }
        public HR_Person HR_Person { get; set; }

        public VWHR_Plan HR_Plan { get; set; }

        public PDS_QuestionResult[] questionResult { get; set; }
        public VWPDS_QuestionForm[] questions { get; set; }
    }
    public class VMHR_StaffNeedsAndReview
    {
        public VWHR_StaffNeeds VWHR_StaffNeeds { get; set; }
        public string Keywords { get; set; }
        public string Positions { get; set; }
        public string Logo { get; set; }
	}

    public class VMHR_StaffNeedsPersonAndHrPerson : VWHR_StaffNeedsPerson
    {
        public VWHR_Person HRperson { get; set; }
        //Talebi onaylayabilecek personeller
        public List<Guid?> reqCheckPersons { get; set; }
    }

}