using System;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;
using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessData;

namespace Infoline.WorkOfTime.BusinessAccess
{

    public partial class WorkOfTimeDatabase
    {
        public VWSH_PersonCompetencies GetVWSH_PersonCompetenciesUserAndCompetenciesId(Guid CompetenciesId, Guid UserId)
        {
            using (var db = GetDB())
            {
                return db.Table<VWSH_PersonCompetencies>().Where(a => a.CompetenciesId == CompetenciesId && a.UserId == UserId).Execute().FirstOrDefault();
            }
        }
        public VWSH_PersonCompetencies GetVWSH_PersonCompetenciesUserAndCompetenciesIds(Guid id, Guid CompetenciesId, Guid UserId)
        {
            using (var db = GetDB())
            {
                return db.Table<VWSH_PersonCompetencies>().Where(a => a.id != id && a.CompetenciesId == CompetenciesId && a.UserId == UserId).Execute().FirstOrDefault();
            }
        }

        public VWSH_PersonCertificateType GetVWSH_PersonCertificateTypeName(string CertificateName)
        {
            using (var db = GetDB())
            {
                return db.Table<VWSH_PersonCertificateType>().Where(a => a.CertificateName == CertificateName).Execute().FirstOrDefault();
            }
        }

        public VWSH_PersonCompetencies[] GetVWSH_PersonCompetenciesByUserId(Guid UserId)
        {
            using (var db = GetDB())
            {
                return db.Table<VWSH_PersonCompetencies>().Where(a => a.UserId == UserId).Execute().ToArray();
            }
        }



        //TODO:OĞUZ TAŞI SPECİFİC

        public VWSH_PersonSchools[] GetVWSH_PersonSchoolsByUserId(Guid UserId)
        {
            using (var db = GetDB())
            {
                return db.Table<VWSH_PersonSchools>().Where(a => a.UserId == UserId).Execute().ToArray();
            }
        }

        public VWSH_PersonCertificate[] GetVWSH_PersonCertificateByUserId(Guid UserId)
        {
            using (var db = GetDB())
            {
                return db.Table<VWSH_PersonCertificate>().Where(a => a.UserId == UserId).Execute().ToArray();
            }
        }

        public VWSH_PersonCertificate[] GetVWSH_PersonCertificateByUserIds(Guid[] userIds)
        {
            using (var db = GetDB())
            {
                return db.Table<VWSH_PersonCertificate>().Where(a => a.UserId.In(userIds)).Execute().ToArray();
            }
        }


        public VWSH_PersonCertificate[] GetVWSH_PersonCertificateByIds(Guid[] Ids)
        {
            using (var db = GetDB())
            {
                return db.Table<VWSH_PersonCertificate>().Where(a => a.id.In(Ids)).Execute().ToArray();
            }
        }


        public VWSH_PersonCertificate[] GetVWSH_PersonCertificateByCertificateTypeIds(Guid[] Ids)
        {
            using (var db = GetDB())
            {
                return db.Table<VWSH_PersonCertificate>().Where(a => a.CertificateTypeId.In(Ids)).Execute().ToArray();
            }
        }


        public VWSH_PersonLanguages[] GetVWSH_PersonLanguagesByUserId(Guid UserId)
        {
            using (var db = GetDB())
            {
                return db.Table<VWSH_PersonLanguages>().Where(a => a.UserId == UserId).Execute().ToArray();
            }
        }


     
    }
}