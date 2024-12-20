﻿using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWUT_RulesUser : InfolineTable
    {
        public Guid? rulesId { get; set;}
        public Guid? userId { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string rulesId_Title { get; set;}
        public string userId_Title { get; set;}
        public short? ruleType { get; set;}
    }
}
