using Infoline.WorkOfTime.BusinessAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Infoline.WorkOfTime.BusinessData;
using Kendo.Mvc.Extensions;


namespace Infoline.WorkOfTime.WebProject.Areas.INV.Models
{

    public class INV_CommissionsDashboardModel
    {


        public object GetResults()
        {
            var db = new WorkOfTimeDatabase();
            var commissionsAll = db.GetVWINV_Commissions();
            var person = db.GetVWINV_CommissionsPersons();
            var project = db.GetVWINV_CommissionsProjects();

            return new
            {
                informationType = commissionsAll != null ? allcommissionsInformation(commissionsAll) : new KeyValue[] { },
                commissionsMostPersons = commissionsAll != null ? allcommissionsPerson(person) : new KeyValue[] { },
                commissionsTravelCounts = commissionsAll != null ? allcommissionsTravel(commissionsAll) : new KeyValue[] { },
                projectCommission = commissionsAll != null ? allcommissionProject(commissionsAll) : new KeyValue[] { },
                lastoneyearCommissionsPerson = commissionsAll != null ? allCommisionPersonLast(commissionsAll) : new KeyValue[] { },
                CommissionsPerson = person != null ? personGorev(person) : new KeyValue[] { },
                CommissionsProject = project != null ? projeGorev(project) : new KeyValue[] { }
            };
        }


        private KeyValue[] projeGorev(VWINV_CommissionsProjects[] project)
        {
            try
            {
                var gorev = new List<KeyValue>();

                for (int i = 1; i <= 12; i++)
                {
                    gorev.Add(new KeyValue
                    {

                        Key = (DateTime.Now.Month >= i
                            ? DateTime.Now.Year
                            : (DateTime.Now.Year - 1)) + " " + System.Globalization.DateTimeFormatInfo.CurrentInfo.GetMonthName(i),
                        Value = project.Where(b => (b.ApproveStatus == (Int32)EnumINV_CommissionsApproveStatus.Onaylandi || b.ApproveStatus == (Int32)EnumINV_CommissionsApproveStatus.IslakImzaYuklendi) && b.StartDate.Value.Month == i && b.StartDate.Value.Year == (DateTime.Now.Month >= i ? DateTime.Now.Year : (DateTime.Now.Year - 1))).Count(),
                        Tooltip = string.Join("</br>", project.Where(b => (b.ApproveStatus == (Int32)EnumINV_CommissionsApproveStatus.Onaylandi || b.ApproveStatus == (Int32)EnumINV_CommissionsApproveStatus.IslakImzaYuklendi) && b.StartDate.Value.Month == i && b.StartDate.Value.Year == (DateTime.Now.Month >= i ? DateTime.Now.Year : (DateTime.Now.Year - 1))).Select(c => c.Project_Title).ToArray())

                    });
                }
                var gecmisYillar = gorev.Skip(DateTime.Now.Month).Take(12).ToList();
                var mevcutYillar = gorev.Take(DateTime.Now.Month).ToList();
                gorev = gecmisYillar;
                gorev.AddRange(mevcutYillar);

                return gorev.ToArray();
            }
            catch (Exception)
            {
                return new KeyValue[] { };
            }

        }

        private KeyValue[] personGorev(VWINV_CommissionsPersons[] person)
        {
            try
            {
                var gorev = new List<KeyValue>();

                for (int i = 1; i <= 12; i++)
                {
                    gorev.Add(new KeyValue
                    {

                        Key = (DateTime.Now.Month >= i
                            ? DateTime.Now.Year
                            : (DateTime.Now.Year - 1)) + " " + System.Globalization.DateTimeFormatInfo.CurrentInfo.GetMonthName(i),
                        Value = person.Where(b => (b.ApproveStatus == (Int32)EnumINV_CommissionsApproveStatus.Onaylandi || b.ApproveStatus == (Int32)EnumINV_CommissionsApproveStatus.IslakImzaYuklendi) && b.StartDate.Value.Month == i && b.StartDate.Value.Year == (DateTime.Now.Month >= i ? DateTime.Now.Year : (DateTime.Now.Year - 1))).Count(),
                        Tooltip = string.Join("</br>", person.Where(b => (b.ApproveStatus == (Int32)EnumINV_CommissionsApproveStatus.Onaylandi || b.ApproveStatus == (Int32)EnumINV_CommissionsApproveStatus.IslakImzaYuklendi) && b.StartDate.Value.Month == i && b.StartDate.Value.Year == (DateTime.Now.Month >= i ? DateTime.Now.Year : (DateTime.Now.Year - 1))).Select(c => c.Person_Title).ToArray())

                    });
                }
                var gecmisYillar = gorev.Skip(DateTime.Now.Month).Take(12).ToList();
                var mevcutYillar = gorev.Take(DateTime.Now.Month).ToList();
                gorev = gecmisYillar;
                gorev.AddRange(mevcutYillar);

                return gorev.ToArray();
            }
            catch (Exception)
            {
                return new KeyValue[] { };
            }

        }


        private KeyValue[] allCommisionPersonLast(VWINV_Commissions[] commissionsAll)
        {
            try
            {
                var gorev = new List<KeyValue>();

                for (int i = 1; i <= 12; i++)
                {
                    gorev.Add(new KeyValue
                    {

                        Key = (DateTime.Now.Month >= i
                            ? DateTime.Now.Year
                            : (DateTime.Now.Year - 1)) + " " + System.Globalization.DateTimeFormatInfo.CurrentInfo.GetMonthName(i),
                        Value = commissionsAll.Where(b => (b.ApproveStatus == (Int32)EnumINV_CommissionsApproveStatus.Onaylandi || b.ApproveStatus == (Int32)EnumINV_CommissionsApproveStatus.IslakImzaYuklendi) && b.StartDate.Value.Month == i && b.StartDate.Value.Year == (DateTime.Now.Month >= i ? DateTime.Now.Year : (DateTime.Now.Year - 1))).Count()

                    });
                }
                var gecmisYillar = gorev.Skip(DateTime.Now.Month).Take(12).ToList();
                var mevcutYillar = gorev.Take(DateTime.Now.Month).ToList();
                gorev = gecmisYillar;
                gorev.AddRange(mevcutYillar);

                return gorev.ToArray();
            }
            catch (Exception)
            {
                return new KeyValue[] { };
            }

        }

        private KeyValue[] allcommissionsInformation(VWINV_Commissions[] commissionsAll)
        {
            try
            {
                var ForPie = commissionsAll.Where(x => x.ApproveStatus == (Int32)EnumINV_CommissionsApproveStatus.Onaylandi || x.ApproveStatus == (Int32)EnumINV_CommissionsApproveStatus.IslakImzaYuklendi).GroupBy(x => x.Information_Title).Select(a => new KeyValue
                {
                    Key = a.Key,
                    Value = a.Select(x => x.Information_Title).Count()
                }).OrderByDescending(x => x.Value).ToArray().ToOrderByDescColorGreen();



                return ForPie;
            }
            catch
            {
                return new KeyValue[] { };
            }
        }
        private KeyValue[] allcommissionsPerson(VWINV_CommissionsPersons[] person)
        {
            try
            {
                var commissionsMostPerson = person.Where(x => x.ApproveStatus == (Int32)EnumINV_CommissionsApproveStatus.Onaylandi || x.ApproveStatus == (Int32)EnumINV_CommissionsApproveStatus.IslakImzaYuklendi).GroupBy(x => x.Person_Title).Select(x => new KeyValue
                {

                    Key = x.Key,
                    Value = x.Count()
                }).OrderByDescending(x => x.Value).Take(5).ToArray().ToOrderByDescColorGreen();
                return commissionsMostPerson;
            }
            catch (Exception) { return new KeyValue[] { }; }
        }
        private KeyValue[] allcommissionsTravel(VWINV_Commissions[] commissionsTravelInformation)
        {
            try
            {
                var ForPie = commissionsTravelInformation.Where(x => x.ApproveStatus == (Int32)EnumINV_CommissionsApproveStatus.Onaylandi || x.ApproveStatus == (Int32)EnumINV_CommissionsApproveStatus.IslakImzaYuklendi).GroupBy(x => x.TravelInformation_Title).Select(a => new KeyValue
                {
                    Key = a.Key,
                    Value = a.Select(x => x.TravelInformation_Title).Count()
                }).OrderByDescending(x => x.Value).ToArray().ToOrderByDescColorGreen();
                return ForPie;
            }
            catch (Exception)
            {
                return new KeyValue[] { };
            }
        }
       
        private KeyValue[] allcommissionProject(VWINV_Commissions[] commissionProjects)
        {
            try
            {
                var IDCommissions = commissionProjects.Where(a => a.ApproveStatus == (Int32)EnumINV_CommissionsApproveStatus.Onaylandi || a.ApproveStatus == (Int32)EnumINV_CommissionsApproveStatus.IslakImzaYuklendi).GroupBy(x => x.id).Select(x => x.Key).ToArray();

                var db = new WorkOfTimeDatabase();

                var data = db.GetVWINV_CommissionsProjectsByIdCommissions(IDCommissions);

                return data.GroupBy(g => g.Project_Title).ToDictionary(x => x.Key, y => y.Count()).Select(s => new KeyValue { Key = s.Key, Value = s.Value }).ToArray().ToOrderByDescColorGreen();

            }
            catch (Exception)
            {
                return new KeyValue[] { };
            }
        }


    }
}
