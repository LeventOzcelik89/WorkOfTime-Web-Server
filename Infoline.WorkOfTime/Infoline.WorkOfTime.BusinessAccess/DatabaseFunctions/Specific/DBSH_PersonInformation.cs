using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infoline.Framework.Database;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Data.Common;
using Infoline.WorkOfTime.BusinessData;

namespace Infoline.WorkOfTime.BusinessAccess
{
    [EnumInfo(typeof(SH_PersonInformation), "Gender")]
    public enum EnumSH_PersonInformationGender
    {
        [Description("Kadın")]
        Kadin = 0,
        [Description("Erkek")]
        Erkek = 1

    }
    [EnumInfo(typeof(SH_PersonInformation), "MaritalStatus")]
    public enum EnumSH_PersonInformationMarital
    {
        [Description("Bekar")]
        Bekar = 0,
        [Description("Evli")]
        Evli = 1
    }

    [EnumInfo(typeof(SH_PersonInformation), "Military")]
    public enum EnumSH_PersonInformationMilitary
    {
        [Description("Yapıldı")]
        Yapildi = 0,
        [Description("Yapılmadı")]
        Yapilmadi = 1,
        [Description("Muaf")]
        Muaf = 2,
        [Description("Yabancı")]
        Yabanci = 3,
        [Description("Tecilli")]
        Tecilli = 4
    }

    [EnumInfo(typeof(SH_PersonInformation), "IDBloodGroup")]
    public enum EnumSH_PersonInformationIDBloodGroup
    {
        [Description("AB Rh+")]
        ABarti = 0,
        [Description("AB Rh-")]
        ABeksi = 1,
        [Description("A Rh+")]
        Aarti = 2,
        [Description("A Rh-")]
        Aeksi = 3,
        [Description("B Rh+")]
        Barti = 4,
        [Description("B Rh-")]
        Beksi = 5,
        [Description("0 Rh+")]
        SifirArti = 6,
        [Description("0 Rh-")]
        SifirEksi = 7
    }
    partial class WorkOfTimeDatabase
    {

        public SH_PersonInformation GetSH_PersonInformationByUserId(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<SH_PersonInformation>().Where(a => a.UserId == id).Execute().FirstOrDefault();
            }
        }

    }

}
