using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infoline.WorkOfTime.WebProject.Areas.PRJ.Models
{
    public class ProjectPersonModel
    {

        public INV_CompanyPersonSalary[] PersonSalaries { get; set; }

        public VWINV_CompanyPersonAvailability[] PersonAvailability { get; set; }


        //Önizleme
        public ProjectPersonModel(VWINV_CompanyPersonAvailability[] PersonAvailability)
        {
            var db = new WorkOfTimeDatabase();
            this.PersonAvailability = PersonAvailability;
            if(this.PersonAvailability != null) { 
            this.PersonSalaries = db.GETINV_CompanyPersonSalaryByIdUsers(this.PersonAvailability.GroupBy(x => x.IdUser).Where(h => h.Key.HasValue).Select(x => x.Key.Value).ToArray());
            }
        }


        public VMVWINV_CompanyPersonAvailability[] Calculate()
        {

            var returnList = new List<VMVWINV_CompanyPersonAvailability>();
            if(this.PersonAvailability != null) { 
            var personIds = this.PersonAvailability.GroupBy(x => x.IdUser).Select(x => x.Key).ToArray();

            foreach (var person in personIds)
            {
                var personAvailabilities = this.PersonAvailability.Where(x => x.IdUser == person).OrderBy(x => x.StartDate).ToArray();

                foreach (var availability in personAvailabilities)
                {
                    var personSalaries = PersonSalaries.Where(x => x.IdUser == person).ToArray();
                    double personSalary = 0;


                    foreach (var salary in personSalaries)
                    {
                        #region Kapsama 
                        if (salary.StartDate <= availability.StartDate && salary.EndDate >= availability.EndDate)
                        {
                            personSalary = ((((availability.EndDate - availability.StartDate).Value.Days + 1) * (salary.Salary != null ? salary.Salary.Value : 1)) / DateTime.DaysInMonth(availability.StartDate.Value.Year, availability.StartDate.Value.Month));
                            break;
                        }
                        #endregion

                        #region Soldan Kesişme
                        else if (salary.StartDate < availability.StartDate && salary.EndDate > availability.StartDate && salary.EndDate < availability.EndDate)
                        {
                            personSalary += ((((salary.EndDate - (availability.StartDate)).Value.Days + 1) * (salary.Salary != null ? salary.Salary.Value : 1)) / DateTime.DaysInMonth(availability.StartDate.Value.Year, availability.StartDate.Value.Month));
                        }
                        #endregion

                        #region Sağdan Kesişme
                        else if (salary.StartDate > availability.StartDate.Value && salary.StartDate < availability.EndDate.Value && salary.EndDate > availability.EndDate)
                        {
                            personSalary += ((((availability.EndDate - (salary.StartDate.Value)).Value.Days + 1) * (salary.Salary != null ? salary.Salary.Value : 1)) / DateTime.DaysInMonth(availability.StartDate.Value.Year, availability.StartDate.Value.Month));
                        }
                        #endregion

                        #region İçinde Kalma
                        else if (availability.StartDate.Value <= salary.StartDate && availability.EndDate.Value >= salary.EndDate)
                        {
                            personSalary += ((((salary.EndDate - (salary.StartDate.Value)).Value.Days + 1) * (salary.Salary != null ? salary.Salary.Value : 1)) / DateTime.DaysInMonth(availability.StartDate.Value.Year, availability.StartDate.Value.Month));

                            //personSalary += ((((availability.EndDate - availability.StartDate).Value.Days + 1) -
                            //    ((salary.StartDate.Value - availability.StartDate).Value.Days) - ((availability.StartDate.Value - salary.EndDate.Value).Days) * salary.Salary.Value) /
                            //    DateTime.DaysInMonth(availability.StartDate.Value.Year, availability.StartDate.Value.Month));
                        }
                        #endregion

                    }

                    #region Create Return Object

                    availability.rate = availability.rate ?? 0;
                    var item = new VMVWINV_CompanyPersonAvailability().B_EntityDataCopyForMaterial(availability);
                    item.Salary = Math.Round((personSalary * availability.rate.Value * 1.8), 2);
                    returnList.Add(item);

                    #endregion


                }



            }
            }

            return returnList.ToArray();

        }
    }


    public class VMVWINV_CompanyPersonAvailability : VWINV_CompanyPersonAvailability
    {
        public double Salary { get; set; }

    }



}