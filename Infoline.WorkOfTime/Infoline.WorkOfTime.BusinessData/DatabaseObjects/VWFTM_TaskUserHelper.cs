﻿using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWFTM_TaskUserHelper : InfolineTable
    {
        public Guid? taskId { get; set;}
        public Guid? userId { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string taskId_Title { get; set;}
        public string userId_Title { get; set;}
        public string photo { get; set;}
    }
}