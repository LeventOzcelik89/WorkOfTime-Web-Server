using Infoline.WorkOfTime.BusinessAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Infoline.WorkOfTime.BusinessData;
using Kendo.Mvc.Extensions;
using Infoline.Framework.Database;
using Infoline.WorkOfTime.WebProject.Areas.CRM;

namespace Infoline.WorkOfTime.WebProject.Areas.INV.Models
{
    public class INV_CompanyPersonDashboardModel
    {
        public class TwoDateTime
        {
            public DateTime Date1 { get; set; }
            public DateTime Date2 { get; set; }
        }

        public object GetResults()
        {
            var db = new WorkOfTimeDatabase();

            var invCompanyPerson = db.GetVWINV_CompanyPerson();
            var usersall =db.GetVWSH_UserMyPerson();
            var companyPerson = invCompanyPerson.Where(a => a.JobEndDate == null && a.IdUser.HasValue && usersall.Select(b => b.id).Contains(a.IdUser.Value)).ToArray();
            var positions = db.GetVWINV_CompanyPersonDepartments();
            var data = invCompanyPerson.Where(a => a.IdUser.HasValue && usersall.Select(b => b.id).Contains(a.IdUser.Value)).ToArray();
            var information = db.GetVWSH_PersonInformation().Where(a => a.UserId.HasValue && usersall.Select(b => b.id).Contains(a.UserId.Value)).ToArray();
            var school = db.GetVWSH_PersonSchools().Where(a => a.UserId.HasValue && usersall.Select(b => b.id).Contains(a.UserId.Value)).ToArray();

            return new
            {
                positionType =  new KeyValue[] { },
                positionproje = new KeyValue[] { },
                personAgeArea = usersall != null ? allPersonAge(usersall) : new KeyValue[] { },
                personNumber = companyPerson != null ? allPersonNumber(companyPerson) : new KeyValue[] { },
                akillipersonNumber = companyPerson != null ? akilliPersonNumber(companyPerson) : new KeyValue[] { },
                companyPersonCounts = companyPerson != null ? allPersonCounts(companyPerson) : new KeyValue[] { },
                schoolPerson = school != null ? schoolsPerson(school) : new KeyValue[] { },
                genderCounts = information != null ? allGenderCounts(information) : new KeyValue[] { },
                militaryCounts = information != null ? allMilitaryCounts(information) : new KeyValue[] { },
                //bloodCounts = information != null ? allBloodType(information) : new KeyValue[] { },
                maritalCounts = information != null ? allMaritalType(information) : new KeyValue[] { },
                personBy = companyPerson != null ? allpersons(data) : new KeyValue[] { }
            };
        }

        public object GetResultsByCompanyId(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var positionss = db.GetVWINV_CompanyPersonDepartmentsByCompanyId(id);
            var users = db.GetVWSH_UserByCompanyIdEndPersonNotNull(id);
            var companyPersons = db.GetVWINV_CompanyPersonByCompanyIdAndJobEndDateNull(id);
            var informations = db.GetVWSH_PersonInformationByUserIds(users.Select(c => c.id).ToArray());

            return new
            {
                positionType =  new KeyValue[] { },
                personAgeArea = users != null ? allPersonAgeByCompany(users) : new KeyValue[] { },
                personNumber = companyPersons != null ? allPersonNumberByCompany(companyPersons) : new KeyValue[] { },
                informationBy = informations != null ? allInformationByCompany(informations) : new KeyValue[] { },
                genderCountsBy = informations != null ? allGenderCountsByCompany(informations) : new KeyValue[] { },
                personBy = companyPersons != null ? allpersonsByCompany(companyPersons) : new KeyValue[] { }
            };
        }

        public object GetResultsByPersonId(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var workProjects = db.GetVWINV_CompanyPersonDepartmentsByIdUser(id);

            return new
            {
                workByProjects = workProjects != null ? personWorkProject(workProjects) : new KeyValue[] { },
                projectYears = workProjects != null ? projectYear(workProjects) : new KeyValue[] { }
            };
        }

        private KeyValue[] projectYear(VWINV_CompanyPersonDepartments[] workProjects)
        {
            try
            {
                var commis = new List<KeyValue>();

                for (int i = 1; i <= 12; i++)
                {
                    commis.Add(new KeyValue
                    {

                        Key = (DateTime.Now.Month >= i
                            ? DateTime.Now.Year
                            : (DateTime.Now.Year - 1)) + " " + System.Globalization.DateTimeFormatInfo.CurrentInfo.GetMonthName(i),

                        Value = workProjects.Where(b => b.EndDate == null && b.OrganizationType == (Int32)EnumINV_CompanyDepartmentsType.Matrix && b.StartDate.Value.Month == i && b.StartDate.Value.Year == (DateTime.Now.Month >= i ? DateTime.Now.Year : (DateTime.Now.Year - 1))).Count()

                    });
                }
                var gecmisYillar = commis.Skip(DateTime.Now.Month).Take(12).ToList();
                var mevcutYillar = commis.Take(DateTime.Now.Month).ToList();
                commis = gecmisYillar;
                commis.AddRange(mevcutYillar);

                return commis.OrderByDescending(x => x.Key).ToArray();
            }
            catch (Exception)
            {
                return new KeyValue[] { };
            }

        }

        private KeyValue[] personCommissions(VWINV_Commissions[] commissions)
        {
            try
            {
                var commis = new List<KeyValue>();

                for (int i = 1; i <= 12; i++)
                {
                    commis.Add(new KeyValue
                    {
                        Key = (DateTime.Now.Month >= i ? DateTime.Now.Year : (DateTime.Now.Year - 1)) + " " + System.Globalization.DateTimeFormatInfo.CurrentInfo.GetMonthName(i),
                        Value = commissions.Where(b => b.ApproveStatus == (Int32)EnumINV_CommissionsApproveStatus.Onaylandi && b.StartDate.Value.Month == i && b.StartDate.Value.Year == (DateTime.Now.Month >= i ? DateTime.Now.Year : (DateTime.Now.Year - 1))).Sum(a => a.TotalDays).Value
                    });
                }
                var gecmisYillar = commis.Skip(DateTime.Now.Month).Take(12).ToList();
                var mevcutYillar = commis.Take(DateTime.Now.Month).ToList();
                commis = gecmisYillar;
                commis.AddRange(mevcutYillar);

                return commis.ToArray();
            }
            catch (Exception)
            {
                return new KeyValue[] { };
            }

        }

        private KeyValue[] personWorkProject(VWINV_CompanyPersonDepartments[] workProjects)
        {
            try
            {
                foreach (var item in workProjects.Where(a => a.EndDate == null && a.OrganizationType == (Int32)EnumINV_CompanyDepartmentsType.Matrix))
                {
                    item.EndDate = DateTime.Now;
                }

                var dtList = new List<TwoDateTime>();
                for (int i = 1; i <= 12; i++)
                {
                    dtList.Add(new TwoDateTime
                    {
                        Date1 = new DateTime((DateTime.Now.Month >= i ? DateTime.Now.Year : (DateTime.Now.Year - 1)), i, 1),
                        Date2 = new DateTime((DateTime.Now.Month >= i ? DateTime.Now.Year : (DateTime.Now.Year - 1)), i, System.DateTime.DaysInMonth(DateTime.Now.Year, i))
                    });
                }
                var workJustProject = workProjects.Where(x => x.OrganizationType == (Int32)EnumINV_CompanyDepartmentsType.Matrix);
                var tumList = new List<KeyValue>();
                for (int i = 1; i <= 12; i++)
                {
                    var tumProjeler = workJustProject.Where(a =>

                        (dtList[i - 1].Date1 <= a.EndDate && dtList[i - 1].Date2 >= a.EndDate) ||
                        (dtList[i - 1].Date1 <= a.StartDate && dtList[i - 1].Date2 >= a.StartDate) ||
                        (a.StartDate <= dtList[i - 1].Date1 && a.EndDate >= dtList[i - 1].Date2) ||
                        (a.StartDate <= dtList[i - 1].Date1 && a.EndDate >= dtList[i - 1].Date2));

                    tumList.Add(new KeyValue
                    {
                        Key = (DateTime.Now.Month >= i ? DateTime.Now.Year : (DateTime.Now.Year - 1)) + " " + System.Globalization.DateTimeFormatInfo.CurrentInfo.GetMonthName(i),
                        Value = tumProjeler.Count(),
                        Tooltip = string.Join("</br>", tumProjeler.GroupBy(c => c.Project_Title).Select(d => d.Key))
                    });
                }


                var gecmisYillar = tumList.Skip(DateTime.Now.Month).Take(12).ToList();
                var mevcutYillar = tumList.Take(DateTime.Now.Month).ToList();
                tumList = gecmisYillar;
                tumList.AddRange(mevcutYillar);

                return tumList.ToArray();
            }
            catch (Exception)
            {
                return new KeyValue[] { };
            }
        }




        private KeyValue[] akilliPersonNumber(VWINV_CompanyPerson[] companyPerson)
        {
            try
            {
                var positionsFor = companyPerson.Where(v => v.JobEndDate == null && v.Level != null && v.CompanyId == new Guid("9CE4AD4A-8D5C-43A9-82A1-99933E657869")).GroupBy(x => x.Level).Select((IGrouping<int?, VWINV_CompanyPerson> x) => new KeyValue
                {

                    Key = x.Key + ". Kademe",
                    Value = x.Select(c => c.Level).Count(),

                });
                return positionsFor.OrderByDescending(x => x.Value).ToArray().ToOrderByDescColorGreen();
            }
            catch (Exception) { return new KeyValue[] { }; }
        }

        private KeyValue[] allPersonCounts(VWINV_CompanyPerson[] companyPerson)
        {
            try
            {
                var companyPersonCount = companyPerson.Where(c => c.Company_Title != null && c.JobEndDate == null && c.CompanyId != new Guid("18134cdd-5954-437b-9170-4012b57d8478") && c.Person_Title != null).GroupBy(x => x.Company_Title).Select((IGrouping<string, VWINV_CompanyPerson> x) => new KeyValue
                {

                    Key = x.Key,
                    Value = x.Count()
                });
                return companyPersonCount.OrderByDescending(x => x.Value).ToArray().ToOrderByDescColorGreen();
            }
            catch (Exception) { return new KeyValue[] { }; }
        }

        public KeyValue[] schoolsPerson(VWSH_PersonSchools[] school)
        {
            try
            {
                var cc = school.Where(x => x.JobEndDate == null).GroupBy(x => x.User_Title).Select(x => new
                {
                    Key = x.Key,
                    Level_Title = x.OrderByDescending(b => b.Level).Select(s => s.Level_Title).FirstOrDefault()
                }).ToArray();


                var PersonSchool = cc.GroupBy(g => g.Level_Title).Select(x => new KeyValue
                {
                    Key = x.Key,
                    Value = x.Count()
                });

                return PersonSchool.OrderByDescending(x => x.Value).ToArray().ToOrderByDescColorGreen();
            }
            catch (Exception) { return new KeyValue[] { }; }
        }

        private KeyValue[] allGenderCounts(VWSH_PersonInformation[] information)
        {
            try
            {
                var genderCount = information.Where(x => x.Gender_Title != null && x.JobEndDate == null).GroupBy(x => x.Gender_Title).Select(x => new KeyValue
                {

                    Key = x.Key,
                    Value = x.Count()
                });
                return genderCount.OrderByDescending(x => x.Value).ToArray().ToOrderByDescColorGreen();
            }
            catch (Exception) { return new KeyValue[] { }; }
        }

        private KeyValue[] allMaritalType(VWSH_PersonInformation[] information)
        {
            try
            {
                var maritalCount = information.Where(x => x.MaritalStatus_Title != null && x.JobEndDate == null).GroupBy(x => x.MaritalStatus_Title).Select((IGrouping<string, VWSH_PersonInformation> x) => new KeyValue
                {

                    Key = x.Key,
                    Value = x.Count()
                });
                return maritalCount.OrderByDescending(x => x.Value).ToArray().ToOrderByDescColorGreen();
            }
            catch (Exception) { return new KeyValue[] { }; }
        }

        private KeyValue[] allMilitaryCounts(VWSH_PersonInformation[] information)
        {
            try
            {
                var militaryrCount = information.Where(x => x.Military_Title != null && x.JobEndDate == null).GroupBy(x => x.Military_Title).Select((IGrouping<string, VWSH_PersonInformation> x) => new KeyValue
                {

                    Key = x.Key,
                    Value = x.Count()
                });
                return militaryrCount.OrderByDescending(x => x.Value).ToArray().ToOrderByDescColorGreen();
            }
            catch (Exception) { return new KeyValue[] { }; }
        }

        private KeyValue[] allPersonAge(VWSH_User[] usersall)
        {
            try
            {
                var list = new List<KeyValue>();
                foreach (var pers in usersall.Where(x => x.IsWorking == true && x.birthday != null))
                {
                    DateTime dogumGunu = pers.birthday.Value;
                    DateTime today = DateTime.Today;
                    int yas = today.Year - dogumGunu.Year;
                    int sayi = 0;

                    if (yas >= 18 && yas < 25)
                    {
                        list.Add(new KeyValue { Value = sayi + 1, Key = "18-24 yaş aralığı", Value2 = 1 });
                    }
                    else if (yas >= 25 && yas < 33)
                    {
                        list.Add(new KeyValue { Value = sayi + 1, Key = "25-32 yaş aralığı", Value2 = 2 });
                    }
                    else if (yas >= 33 && yas < 40)
                    {
                        list.Add(new KeyValue { Value = sayi + 1, Key = "33-39 yaş aralığı", Value2 = 3 });
                    }
                    else if (yas >= 40 && yas <= 45)
                    {
                        list.Add(new KeyValue { Value = sayi + 1, Key = "40-44 yaş aralığı", Value2 = 4 });
                    }
                    else if (yas > 45)
                    {
                        list.Add(new KeyValue { Value = sayi + 1, Key = "45+ yaş üzeri", Value2 = 5 });
                    }
                }

                var sonuc = list.GroupBy(x => x.Key).Select(a => new KeyValue
                {
                    Key = a.Key,
                    Value = a.Select(x => x.Value).Count()
                }).ToArray();

                return sonuc.ToArray().ToOrderByDescColorGreen();

            }
            catch (Exception) { return new KeyValue[] { }; }
        }

        private KeyValue[] allpermit(VWINV_Permit[] permit)
        {
            try
            {
                var permitList = new List<KeyValue>();
                for (int i = 1; i <= 12; i++)
                {
                    permitList.Add(new KeyValue
                    {
                        Key = (DateTime.Now.Month >= i ? DateTime.Now.Year : (DateTime.Now.Year - 1)) + " " + System.Globalization.DateTimeFormatInfo.CurrentInfo.GetMonthName(i),
                        Value = permit.Where(g => g.StartDate.Value.Month >= i && g.EndDate.Value.Month <= i).GroupBy(c => c.Person_Title).Count(),
                        Tooltip = permit.Where(x => x.StartDate.Value.Month >= i && x.EndDate.Value.Month <= i).Select(X => X.TotalDays).Sum()

                    });
                }
                var gecmisYillar = permitList.Skip(DateTime.Now.Month).Take(12).ToList();
                var mevcutYillar = permitList.Take(DateTime.Now.Month).ToList();
                permitList = gecmisYillar;
                permitList.AddRange(mevcutYillar);

                return permitList.ToArray();
            }
            catch (Exception)
            {
                return new KeyValue[] { };
            }

        }

        private KeyValue[] allcommissions(VWINV_Commissions[] commissions)
        {
            try
            {
                var commissiontList = new List<KeyValue>();
                for (int i = 1; i <= 12; i++)
                {
                    commissiontList.Add(new KeyValue
                    {
                        Key = (DateTime.Now.Month >= i ? DateTime.Now.Year : (DateTime.Now.Year - 1)) + " " + System.Globalization.DateTimeFormatInfo.CurrentInfo.GetMonthName(i),
                        //     Value = commissions.Where(g => g.StartDate.Value.Month >= i && g.EndDate.Value.Month <= i).GroupBy(c => c.Person_Title).Count(),
                        //    Value2 = commissions.Where(x => x.StartDate.Value.Month >= i && x.EndDate.Value.Month <= i).Select(X => X.).Sum()
                    });
                }
                var gecmisYillar = commissiontList.Skip(DateTime.Now.Month).Take(12).ToList();
                var mevcutYillar = commissiontList.Take(DateTime.Now.Month).ToList();
                commissiontList = gecmisYillar;
                commissiontList.AddRange(mevcutYillar);

                return commissiontList.ToArray();
            }
            catch (Exception)
            {
                return new KeyValue[] { };
            }

        }

        private KeyValue[] allpersons(VWINV_CompanyPerson[] data)
        {
            try
            {
                foreach (var item in data.Where(a => a.JobEndDate == null))
                {
                    item.JobEndDate = DateTime.Now;
                }

                var dtList = new List<TwoDateTime>();

                var now = DateTime.Now;

                for (int i = 1; i <= 12; i++)
                {

                    if (DateTime.Now.Month == i)
                    {
                        dtList.Add(new TwoDateTime
                        {
                            Date1 = new DateTime(DateTime.Now.Year, i, 1),
                            Date2 = new DateTime(DateTime.Now.Year, i, DateTime.Now.Day)
                        });
                    }
                    else
                    {
                        dtList.Add(new TwoDateTime
                        {
                            Date1 = new DateTime((DateTime.Now.Month > i ? DateTime.Now.Year : (DateTime.Now.Year - 1)), i, 1),
                            Date2 = new DateTime((DateTime.Now.Month > i ? DateTime.Now.Year : (DateTime.Now.Year - 1)), i, (i == 2 ? 28 : DateTime.DaysInMonth(DateTime.Now.Year - 1, i)))
                        });
                    }
                }

                var db = new WorkOfTimeDatabase();
                var sahipOldugumSirketler = db.GetVWCMP_CompanyMyCompanies().Where(x => x.id != new Guid("18134CDD-5954-437B-9170-4012B57D8478"));
                var sirketlerdeCalisanPersoneller = db.GetINV_CompanyPersonByCompanyIds(sahipOldugumSirketler.Select(x => x.id).ToArray());

                var datas = sirketlerdeCalisanPersoneller.Where(x => x.CompanyId != null && x.CompanyId != new Guid("18134CDD-5954-437B-9170-4012B57D8478") && x.JobEndDate == null);

                var tumList = new List<KeyValue>();
                for (int i = 1; i <= 12; i++)
                {
                    var tumCalisanlar = datas
                        .Where(a =>
                        (dtList[i - 1].Date1 <= a.JobEndDate && dtList[i - 1].Date2 >= a.JobEndDate) ||
                        (dtList[i - 1].Date1 <= a.JobStartDate && dtList[i - 1].Date2 >= a.JobStartDate) ||
                        (a.JobStartDate <= dtList[i - 1].Date1 && a.JobEndDate >= dtList[i - 1].Date2) ||
                        (a.JobStartDate <= dtList[i - 1].Date1 && a.JobEndDate == null)).Count();


                    tumList.Add(new KeyValue
                    {
                        Key = (DateTime.Now.Month >= i ? DateTime.Now.Year : (DateTime.Now.Year - 1)).ToString(),
                        Tooltip = i,
                        Color = System.Globalization.DateTimeFormatInfo.CurrentInfo.GetMonthName(i),
                        Value = "Tüm Çalışanlar",
                        Value2 = tumCalisanlar,
                    });

                    foreach (var owner in sahipOldugumSirketler)
                    {

                        var akilliCalisanlar = datas.Where(a => a.CompanyId == owner.id)
                       .Where(a =>
                       (dtList[i - 1].Date1 <= a.JobEndDate && dtList[i - 1].Date2 >= a.JobEndDate) ||
                       (dtList[i - 1].Date1 <= a.JobStartDate && dtList[i - 1].Date2 >= a.JobStartDate) ||
                       (a.JobStartDate <= dtList[i - 1].Date1 && a.JobEndDate >= dtList[i - 1].Date2) ||
                       (a.JobStartDate <= dtList[i - 1].Date1 && a.JobEndDate == null)).Count();

                        tumList.Add(new KeyValue
                        {
                            Key = (DateTime.Now.Month >= i ? DateTime.Now.Year : (DateTime.Now.Year - 1)).ToString(),
                            Tooltip = i,
                            Color = System.Globalization.DateTimeFormatInfo.CurrentInfo.GetMonthName(i),
                            Value = owner.fullName,
                            Value2 = akilliCalisanlar,
                        });
                    }
                }

                return tumList.OrderBy(a => a.Key).ThenBy(a => a.Tooltip).ToArray();
            }

            catch(Exception )
            {
                return new KeyValue[] { };
            }
        }

        private KeyValue[] allPersonNumber(VWINV_CompanyPerson[] companyPerson)
        {
            try
            {
                var positionsFor = companyPerson.Where(v => v.JobEndDate == null && v.Level != null && v.Company_Title != null && v.CompanyId != new Guid("18134cdd-5954-437b-9170-4012b57d8478") && v.Person_Title != null).GroupBy(x => x.Level).Select(x => new KeyValue
                {

                    Key = x.Key + ". Kademe",
                    Value = x.Select(c => c.Level).Count(),
                    Tooltip = string.Join("</br>", x.GroupBy(c => c.Company_Title).Select(d => d.Key + " işletmesinde " + d.Count() + " personel ").ToArray()) + "</br>" + "Toplam " + x.Select(f => f.Title).Count() + " personel."

                });
                return positionsFor.OrderByDescending(x => x.Value).ToArray().ToOrderByDescColorGreen();
            }
            catch (Exception) { return new KeyValue[] { }; }
        }

        public KeyValue[] allPersonNumberByCompany(VWINV_CompanyPerson[] companyPersons)
        {
            try
            {
                var positionsFor = companyPersons.Where(v => v.JobEndDate == null && v.Level != null).GroupBy(x => x.Level).Select((IGrouping<int?, VWINV_CompanyPerson> x) => new KeyValue
                {
                    Key = x.Key + ". Kademe",
                    Value = x.Select(c => c.Level).Count(),

                });
                return positionsFor.OrderByDescending(x => x.Value).ToArray().ToOrderByDescColorGreen();
            }
            catch (Exception) { return new KeyValue[] { }; }
        }

     

        public KeyValue[] allPersonAgeByCompany(VWSH_User[] users)
        {
            try
            {
                var list = new List<KeyValue>();
                foreach (var pers in users.Where(x => x.IsWorking == true))
                {
                    DateTime dogumGunu = pers.birthday.Value;
                    DateTime today = DateTime.Today;
                    int yas = today.Year - dogumGunu.Year;
                    int sayi = 0;

                    if (yas >= 18 && yas < 25)
                    {
                        list.Add(new KeyValue { Value = sayi + 1, Key = "18-25 yaş aralığı" });
                    }

                    if (yas >= 25 && yas <= 40)
                    {
                        list.Add(new KeyValue { Value = sayi + 1, Key = "25-40 yaş aralığı" });
                    }

                    if (yas > 40)
                    {
                        list.Add(new KeyValue { Value = sayi + 1, Key = "40+ yaş üzeri" });
                    }
                }

                var sonuc = list.GroupBy(x => x.Key).Select(a => new KeyValue
                {
                    Key = a.Key,
                    Value = a.Select(x => x.Value).Count()
                }).OrderByDescending(x => x.Value).OrderByDescending(x => x.Value).ToArray().ToOrderByDescColorGreen();

                return sonuc;

            }
            catch (Exception) { return new KeyValue[] { }; }
        }

        public KeyValue[] allInformationByCompany(VWSH_PersonInformation[] informations)
        {
            try
            {
                var militaryrCount = informations.Where(x => x.Military_Title != null && x.JobEndDate == null).GroupBy(x => x.Military_Title).Select((IGrouping<string, VWSH_PersonInformation> x) => new KeyValue
                {

                    Key = x.Key,
                    Value = x.Count()
                });
                return militaryrCount.OrderByDescending(x => x.Value).ToArray().ToOrderByDescColorGreen();
            }
            catch (Exception) { return new KeyValue[] { }; }
        }

        public KeyValue[] allGenderCountsByCompany(VWSH_PersonInformation[] informations)
        {
            try
            {
                var genderCount = informations.Where(x => x.Gender_Title != null && x.JobEndDate == null).GroupBy(x => x.Gender_Title).Select((IGrouping<string, VWSH_PersonInformation> x) => new KeyValue
                {

                    Key = x.Key,
                    Value = x.Count()
                });
                return genderCount.OrderByDescending(x => x.Value).ToArray().ToOrderByDescColorGreen();
            }
            catch (Exception) { return new KeyValue[] { }; }
        }

        private KeyValue[] allBloodTypeByCompany(VWSH_PersonInformation[] informations)
        {
            try
            {
                var bloodCount = informations.Where(x => x.IDBloodGroup_Title != null && x.JobEndDate == null).GroupBy(x => x.IDBloodGroup_Title).Select((IGrouping<string, VWSH_PersonInformation> x) => new KeyValue
                {

                    Key = x.Key,
                    Value = x.Count()
                });
                return bloodCount.OrderByDescending(x => x.Value).ToArray().ToOrderByDescColorGreen();
            }
            catch (Exception) { return new KeyValue[] { }; }
        }

        private KeyValue[] allpersonsByCompany(VWINV_CompanyPerson[] companyPersons)
        {
            try
            {
                foreach (var item in companyPersons.Where(a => a.JobEndDate == null))
                {
                    item.JobEndDate = DateTime.Now;
                }

                var dtList = new List<TwoDateTime>();
                for (int i = 1; i <= 12; i++)
                {
                    dtList.Add(new TwoDateTime
                    {
                        Date1 = new DateTime((DateTime.Now.Month >= i ? DateTime.Now.Year : (DateTime.Now.Year - 1)), i, 1),
                        Date2 = new DateTime((DateTime.Now.Month >= i ? DateTime.Now.Year : (DateTime.Now.Year - 1)), i, System.DateTime.DaysInMonth(DateTime.Now.Year, i))
                    });
                }

                var tumList = new List<KeyValue>();
                for (int i = 1; i <= 12; i++)
                {
                    var tumCalisanlar = companyPersons
                        .Where(a =>
                        (dtList[i - 1].Date1 <= a.JobEndDate && dtList[i - 1].Date2 >= a.JobEndDate) ||
                        (dtList[i - 1].Date1 <= a.JobStartDate && dtList[i - 1].Date2 >= a.JobStartDate) ||
                        (a.JobStartDate <= dtList[i - 1].Date1 && a.JobEndDate >= dtList[i - 1].Date2) ||
                        (a.JobStartDate <= dtList[i - 1].Date1 && a.JobEndDate >= dtList[i - 1].Date2)).Count();

                    tumList.Add(new KeyValue
                    {
                        Key = (DateTime.Now.Month >= i ? DateTime.Now.Year : (DateTime.Now.Year - 1)) + " " + System.Globalization.DateTimeFormatInfo.CurrentInfo.GetMonthName(i),
                        Value = tumCalisanlar
                    });
                }

                var gecmisYillar = tumList.Skip(DateTime.Now.Month).Take(12).ToList();
                var mevcutYillar = tumList.Take(DateTime.Now.Month).ToList();
                tumList = gecmisYillar;
                tumList.AddRange(mevcutYillar);

                return tumList.ToArray();
            }
            catch (Exception)
            {
                return new KeyValue[] { };
            }
        }
    }
}
