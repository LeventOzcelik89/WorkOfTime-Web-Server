using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infoline.WorkOfTime.WebProject.Areas.CRM
{
    public class CRM_Model
    {
        public List<DropDownDataSource> GetSH_UsersForPresentationById(Guid id)
        {
            var result = new List<DropDownDataSource>();

            var db = new WorkOfTimeDatabase();
            var companies = db.GetVWCMP_CompanyMyCompanies();
            var companyIds = companies.Select(a => a.id).ToArray();
            var _presentation = db.GetCRM_PresentationById(id);

            if (companies.Count() > 0)
            {
                result.AddRange(db.GetVWSH_UserByCompanyIds(companyIds).Select(a => new DropDownDataSource
                {
                    Id = a.id,
                    Name = a.FullName,
                    type = (int)EnumCRM_ContactUserUserType.OwnerUser
                }));
            }
            if (_presentation != null)
            {
                if (_presentation.CustomerCompanyId.HasValue)
                {
                    result.AddRange(db.GetVWSH_UserByCompanyId(_presentation.CustomerCompanyId.Value).Select(a => new DropDownDataSource
                    {
                        Id = a.id,
                        Name = a.FullName,
                        type = (int)EnumCRM_ContactUserUserType.CustomerUser
                    }));
                }

                if (_presentation.ChannelCompanyId.HasValue)
                {
                    result.AddRange(db.GetVWSH_UserByCompanyId(_presentation.ChannelCompanyId.Value).Select(a => new DropDownDataSource
                    {
                        Id = a.id,
                        Name = a.FullName,
                        type = (int)EnumCRM_ContactUserUserType.ChannelUser
                    }));
                }
            }
            else
            {
                result.AddRange(db.GetVWSH_UserByCompanyId(id).Select(a => new DropDownDataSource
                {
                    Id = a.id,
                    Name = a.FullName,
                    type = (int)EnumCRM_ContactUserUserType.CustomerUser
                }));
            }


            
            return result.OrderBy(a => a.Name).ToList();

        }

        public Dictionary<string, string> CrmContactDictionary = new Dictionary<string, string>()
        {
            {"PresentationStageId","Potansiyel/Fırsat Aşaması" },
            {"ContactStartDate","Toplantı Başlangıç Tarihi" },
            {"ContactEndDate","Toplantı Bitiş Tarihi" },
            {"ContactStatusId","Durum" },
            {"ContactType","Aktivite/Randevu Tipi" },
            {"ContactStatus","Aktivite/Randevu Durumu" },
            {"Description","Açıklama" }
        };

    }

    public class DropDownDataSource
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int? type { get; set; }
    }

}