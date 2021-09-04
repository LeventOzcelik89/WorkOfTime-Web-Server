using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWPRD_ProductionSchemaStage : InfolineTable
    {
        public string code { get; set;}
        public string name { get; set;}
        public int? stageNum { get; set;}
        public Guid? productionSchemaId { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string productionSchemaId_Title { get; set;}
    }
}
