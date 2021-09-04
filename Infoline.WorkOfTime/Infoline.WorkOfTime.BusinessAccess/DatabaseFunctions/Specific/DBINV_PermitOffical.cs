using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infoline.Framework.Database;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Data.Common;
using Infoline.WorkOfTime.BusinessData;
using System.ComponentModel;

namespace Infoline.WorkOfTime.BusinessAccess
{
    [EnumInfo(typeof(INV_PermitOffical), "Type")]
    public enum EnumINV_PermitOfficalType
    {
        [Description("Yılbaşı")]
        yilbasi = 0,
        [Description("Ulusal Egemenlik ve Çocuk Bayramı")]
        ulusal = 1,
        [Description("Emek ve Dayanışma Günü(İşçi Bayramı)")]
        emek = 2,
        [Description("Ramazan Bayramı Arifesi")]
        ramazanArife = 3,
        [Description("Ramazan Bayramı")]
        ramazan = 4,
        [Description("Atatürk'ü Anma Gençlik ve Spor Bayramı")]
        ataturk = 5,
        [Description("Kurban Bayramı Arifesi")]
        kurbanArife = 6,
        [Description("Kurban Bayramı")]
        kurban = 7,
        [Description("Zafer Bayramı")]
        zafer = 8,
        [Description("Cumhuriyet Bayramı")]
        cumhuriyet = 9,
        [Description("15 Temmuz Demokrasi ve Milli Birlik Günü")]
        temmuz = 10
    }

    partial class WorkOfTimeDatabase
    {
 
    }
}
