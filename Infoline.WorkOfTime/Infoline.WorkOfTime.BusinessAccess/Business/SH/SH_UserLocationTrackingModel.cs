using GeoAPI.Geometries;
using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace Infoline.WorkOfTime.BusinessAccess
{
    public class SH_UserLocationTrackingModel
    {
        private WorkOfTimeDatabase db { get; set; }
        private DbTransaction trans { get; set; }
    }
    public class SH_UserLocationTrackingMap
    {
        public VWUT_LocationTracking[] LocationTrackings { get; set; }
        public VWUT_LocationConfigUser[] LocationTrackingsAll { get; set; }
        public Guid id { get; set; }
        public string FullName { get; set; }
        public string Title { get; set; }
        public string ProfilePhoto { get; set; }
        public string Department_Title { get; set; }
    }

    public class UTLocationUserFilter
    {
        public Guid? userId { get; set; } = null;
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
    }
}
