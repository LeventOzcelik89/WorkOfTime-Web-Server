using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infoline.WorkOfTime.BusinessAccess.Models
{
    public class INV_CompanyPersonAvailabilityModel
    {

        public EnumINV_CompanyPersonAvailabilityType? type { get; set; }
        public VWPRJ_Project Project { get; set; }
        public VWINV_CompanyPersonDepartments[] Personels { get; set; }
        public List<VWINV_CompanyPersonAvailability> Availability { get; set; }

        public INV_CompanyPersonAvailabilityModel(Guid IdProject, EnumINV_CompanyPersonAvailabilityType type)
        {
            var db = new WorkOfTimeDatabase();
            this.Project = db.GetVWPRJ_ProjectById(IdProject);
            this.Personels = db.GetVWINV_CompanyPersonDepartmentsByIdProjectNotDateControl(IdProject);
            this.Availability = db.GetVWINV_CompanyPersonAvailabilityByProjectId(IdProject, (int)type).OrderBy(x => x.IdUser).ThenBy(x => x.StartDate).ToList();
            this.type = type;
        }


        public INV_CompanyPersonAvailabilityModel(Guid IdUser, VWINV_CompanyPersonAvailability[] PersonAvailabilities)
        {
            var db = new WorkOfTimeDatabase();
            this.Personels = db.GetVWINV_CompanyPersonDepartmentsAllByIdUserAndType(IdUser, EnumINV_CompanyDepartmentsType.Matrix);
            this.Availability = db.GETVWINV_CompanyPersonAvailabilityByIdUser(this.Personels.Select(x => x.IdUser.Value).FirstOrDefault()).ToList();
        }


        public VWINV_CompanyPersonAvailability[] GetSchema()
        {
            var db = new WorkOfTimeDatabase();

            var returnList = new List<VWINV_CompanyPersonAvailability>();

            #region Schema Creator

            var projectDates = new List<DateTime>();

            if (this.type == EnumINV_CompanyPersonAvailabilityType.Proje)
            {
                if (Project.ProjectStartDate.HasValue && Project.ProjectEndDate.HasValue)
                {
                    for (var dt = Project.ProjectStartDate.Value.AddDays(-(Project.ProjectStartDate.Value.Day - 1)); dt <= Project.ProjectEndDate; dt = dt.AddMonths(1))
                    {
                        projectDates.Add(dt);
                    }
                }
            }
            else
            {
                if (Project.WarrantyStartDate.HasValue && Project.WarrantyEndDate.HasValue)
                {
                    for (var dt = Project.WarrantyStartDate.Value.AddDays(-(Project.WarrantyStartDate.Value.Day - 1)); dt <= Project.WarrantyEndDate; dt = dt.AddMonths(1))
                    {
                        projectDates.Add(dt);
                    }
                }

            }



            if (this.Personels.Length > 0 && projectDates.Count() > 0)
            {
                foreach (var person in this.Personels)
                {
                    DateTime? salaryStartDate = null;
                    DateTime? salaryEndDate = null;

                    var salaryPerson = db.GetINV_CompanyPersonSalaryByIdUser(person.IdUser.Value);

                    if (salaryPerson != null)
                    {
                        salaryStartDate = salaryPerson.StartDate;
                        salaryEndDate = salaryPerson.EndDate;
                    }

                    foreach (var date in projectDates)
                    {

                        bool isSalary = false;

                        var startDate = Project.ProjectStartDate.Value.Month == date.Month && Project.ProjectStartDate.Value.Year == date.Year ?
                        date.AddDays(-(date.Day - Project.ProjectStartDate.Value.Day)) : date;

                        var endDate = Project.ProjectEndDate.Value.Month == date.Month && Project.ProjectEndDate.Value.Year == date.Year ?
                        date.AddDays(-(1 - Project.ProjectEndDate.Value.Day)) : date.AddDays(DateTime.DaysInMonth(date.Year, date.Month) - date.Day);


                        if (this.type == EnumINV_CompanyPersonAvailabilityType.Bakim && Project.WarrantyStartDate.HasValue && Project.WarrantyEndDate.HasValue)
                        {
                            startDate = Project.WarrantyStartDate.Value.Month == date.Month && Project.WarrantyStartDate.Value.Year == date.Year ?
                        date.AddDays(-(date.Day - Project.WarrantyStartDate.Value.Day)) : date;

                            endDate = Project.WarrantyEndDate.Value.Month == date.Month && Project.WarrantyEndDate.Value.Year == date.Year ?
                            date.AddDays(-(1 - Project.WarrantyEndDate.Value.Day)) : date.AddDays(DateTime.DaysInMonth(date.Year, date.Month) - date.Day);
                        }




                        //if (
                        //     (person.StartDate <= startDate && (person.EndDate > startDate || person.EndDate == null)) ||
                        //     (person.StartDate >= startDate && (person.EndDate <= endDate || person.EndDate == null)) ||
                        //     (person.StartDate <= startDate && (person.EndDate > endDate || person.EndDate == null)))
                        //{

                        //    if (person.StartDate > startDate)
                        //    {
                        //        if (person.StartDate != null)
                        //        {
                        //            startDate = person.StartDate.Value;
                        //        }
                        //    }
                        //    if (person.EndDate < endDate)
                        //    {
                        //        if (person.EndDate != null)
                        //        {
                        //            endDate = person.EndDate.Value;
                        //        }
                        //    }

                        if (salaryStartDate != null && salaryEndDate != null)
                        {
                            if ((salaryStartDate > startDate && salaryStartDate < endDate) || (salaryEndDate > startDate && salaryEndDate < endDate) || (salaryStartDate < startDate && salaryEndDate > endDate))
                            {
                                isSalary = true;
                            }
                        }

                        returnList.Add(new VWINV_CompanyPersonAvailability
                        {
                            Project_Title = this.Project.ProjectName + " -:- " + this.Project.ProjectCode,
                            IdProject = this.Project.id,
                            rate = 0,
                            Person_Title = person.Person_Title + " (" + person.Department_Title + ")",
                            IdUser = person.IdUser,
                            StartDate = startDate,
                            EndDate = endDate,
                            isSalary = isSalary,
                            type = (short)this.type,

                        });
                        //}
                    }
                }
            }
            else
            {
                return new List<VWINV_CompanyPersonAvailability>().ToArray();
            }

            #endregion


            #region Match up
            foreach (var item in returnList)
            {
                var value = this.Availability.Where(x => x.IdUser == item.IdUser && x.StartDate == item.StartDate).FirstOrDefault(); // start date mevzusuna da bakılacak
                if (value != null)
                {
                    item.rate = value.rate;
                    item.id = value.id;
                }
            }
            #endregion

            return returnList.ToArray();
        }



        public VWINV_CompanyPersonAvailability[] GetDailySchemaByPerson()
        {
            try
            {   //TODO:Şahin  

                var returnList = new List<VWINV_CompanyPersonAvailability>();
                var personAvailabilities = new List<VWINV_CompanyPersonAvailability>();

                var db = new WorkOfTimeDatabase();
                var Projects = db.GETVWPRJ_ProjectByProjectIds(this.Personels.Where(x => x.ProjectId != null).Select(x => x.ProjectId.Value).ToArray());

                foreach (var project in Projects)
                {
                    this.Project = project;
                    personAvailabilities.AddRange(this.Availability.Where(x => x.IdProject == project.id));
                }

                this.Availability = personAvailabilities;

                // bu kısım js kısmına taşınabilir

                var minDatePerson = personAvailabilities.Min(x => x.StartDate.Value);
                var minDate = new DateTime(minDatePerson.Year, minDatePerson.Month, minDatePerson.Day);

                var maxDatePerson = personAvailabilities.Max(x => x.EndDate.Value);
                var maxDate = new DateTime(maxDatePerson.Year, maxDatePerson.Month, maxDatePerson.Day);

                for (var dt = minDate; dt < maxDate; dt = dt.AddDays(1))
                {
                    if (dt.DayOfWeek == DayOfWeek.Sunday || dt.DayOfWeek == DayOfWeek.Saturday)
                    {
                        continue;
                    }

                    var personAvailabilitiesTemp = this.Availability.Where(x => x.StartDate <= dt && dt <= x.EndDate).ToArray();

                    if (!(personAvailabilitiesTemp == null || personAvailabilitiesTemp.Count() == 0))
                    {

                        foreach (var availability in personAvailabilitiesTemp)
                        {
                            returnList.Add(new VWINV_CompanyPersonAvailability
                            {
                                Person_Title = availability.Person_Title,
                                rate = availability.rate,
                                StartDate = dt,
                                Project_Title = availability.Project_Title,
                                IdProject = availability.IdProject,
                            });
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
                return returnList.ToArray();
            }
            catch
            {
                return new VWINV_CompanyPersonAvailability[] { };
            }
        }

    }
}
