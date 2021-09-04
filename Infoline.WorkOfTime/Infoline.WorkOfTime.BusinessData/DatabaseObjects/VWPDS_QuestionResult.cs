using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWPDS_QuestionResult : InfolineTable
    {
        public double? point { get; set;}
        public string comment { get; set;}
        public Guid? questionFormId { get; set;}
        public Guid? formResultId { get; set;}
        public int? score { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public double? factor { get; set;}
        public string question { get; set;}
        public string groupName { get; set;}
        public int? percentScore { get; set;}
    }
}
