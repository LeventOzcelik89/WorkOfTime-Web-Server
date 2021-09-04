using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class CMP_TenderTransaction : InfolineTable
    {
        public Guid? tenderId { get; set;}
        public Guid? transactionId { get; set;}
    }
}
