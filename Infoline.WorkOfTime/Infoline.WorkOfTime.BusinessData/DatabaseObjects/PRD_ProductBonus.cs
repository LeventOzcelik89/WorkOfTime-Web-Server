﻿using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class PRD_ProductBonus : InfolineTable
    {
        public string code { get; set;}
        public string ruleName { get; set;}
        public string query { get; set;}
        public string present { get; set;}
    }
}