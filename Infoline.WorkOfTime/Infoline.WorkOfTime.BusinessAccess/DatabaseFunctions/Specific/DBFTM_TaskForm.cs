using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;

namespace Infoline.WorkOfTime.BusinessAccess
{
    [EnumInfo(typeof(FTM_TaskForm), "isActive")]
    public enum EnumFTM_TaskFormIsActive
    {
        [Description("Pasif"),Generic("order","2")]
        Pasif = 0,
        [Description("Aktif"), Generic("order", "1")]
        Aktif = 1,
    }

    [EnumInfo(typeof(FTM_TaskForm), "type")]
    public enum EnumFTM_TaskFormType
    {
        [Description("Anket"), Generic("order", "1")]
        Anket = 100
    }

    partial class WorkOfTimeDatabase
    {


    }
}
