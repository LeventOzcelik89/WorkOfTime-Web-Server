﻿using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class PDS_QuestionForm : InfolineTable
    {
        public double? factor { get; set;}
        public int? questionOrder { get; set;}
        public string groupName { get; set;}
        public int? groupOrder { get; set;}
        public Guid? questionId { get; set;}
        public Guid? evaluateFormId { get; set;}
    }
}