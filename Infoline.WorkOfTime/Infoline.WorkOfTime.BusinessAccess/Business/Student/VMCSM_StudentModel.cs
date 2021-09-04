using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;


namespace Infoline.WorkOfTime.BusinessAccess
{
    public class VMCSM_StudentModel : VWCSM_Student
    {
        private WorkOfTimeDatabase db { get; set; }
        private DbTransaction trans { get; set; }
        public VWCSM_School School { get; set; }
        public VWCSM_StudentDepartment[] StudentDepartments { get; set; }
        public Guid[] StudentDepartmentIds { get; set; }
        public CSM_Activity LastActivity { get; set; }
        public bool? isAddContact { get; set; }

        public VMCSM_StudentModel Load()
        {
            this.db = this.db ?? new WorkOfTimeDatabase();
            var student = db.GetVWCSM_StudentById(this.id);

            if (student != null)
            {
                this.B_EntityDataCopyForMaterial(student, true);
                this.StudentDepartments = db.GetVWCSM_StudentDepartmentByStudentId(this.id);
                this.StudentDepartmentIds = db.GetCSM_StudentDepartmentByStudentId(this.id).Where(a => a.departmentId.HasValue).Select(a => a.departmentId.Value).ToArray();
                this.LastActivity = db.GetCSM_ActivityLastActivity(this.id);
            }
            else
            {
                this.LastActivity = new CSM_Activity
                {
                    stageId = new Guid("BB0559C5-0C40-420E-A81F-12485829E52C"),
                    studentId = this.id,
                    date = DateTime.Now,
                    type = (int)EnumCSM_ActivityType.Cagri,
                };
            }

            if (student != null)
            {
                if (!string.IsNullOrEmpty(student.phone) && student.phone.StartsWith("0") == false)
                {
                    student.phone = "0" + student.phone;
                }
            }

            return this;
        }

        public ResultStatus Save(Guid? userId, DbTransaction _trans = null)
        {
            this.db = this.db ?? new WorkOfTimeDatabase();
            this.trans = _trans ?? db.BeginTransaction();
            var student = db.GetVWCSM_StudentById(this.id);
            var res = IsExistStudentProperty();

            if (res.result == false)
            {
                return res;
            }


            if (student == null)
            {
                this.createdby = userId;
                this.created = DateTime.Now;
                res = Insert();
            }
            else
            {
                this.changedby = userId;
                this.changed = DateTime.Now;
                res = Update();
            }

            if (_trans == null)
            {
                if (res.result)
                    this.trans.Commit();
                else
                    this.trans.Rollback();
            }

            return res;
        }

        public ResultStatus IsExistStudentProperty()
        {
            db = db ?? new WorkOfTimeDatabase();
            var rs = new ResultStatus { result = true };

            if (!String.IsNullOrEmpty(this.phone))
            {
                this.phone = this.phone.Replace(" ", "").Trim().TrimStart('+').TrimStart('9');
                if (this.phone.StartsWith("0") == false && this.phone.Length == 10)
                {
                    this.phone = "0" + this.phone;
                }
                var isExistStudentByPhone = db.GetCSM_StudentByPhone(this.phone);

                if (isExistStudentByPhone != null && isExistStudentByPhone.id != this.id)
                {
                    rs.message = "Bu telefon numarası daha önce kaydedilmiştir. Lütfen kontrol ediniz.";
                    rs.result = false;
                    return rs;
                }

         
            }

            if (!String.IsNullOrEmpty(this.email))
            {
                var isExistStudentByEmail = db.GetCSM_StudentByEmail(this.email);

                if (isExistStudentByEmail != null && isExistStudentByEmail.id != this.id)
                {
                    rs.message = "Bu email adresi daha önce kaydedilmiştir. Lütfen kontrol ediniz.";
                    rs.result = false;
                    return rs;
                }
            }

            return rs;
        }

        private ResultStatus Insert()
        {
            var rs = new ResultStatus { result = true };

            this.LastActivity.created = DateTime.Now;
            this.LastActivity.createdby = this.createdby;
            this.LastActivity.studentId = this.id;

            rs &= db.InsertCSM_Student(new CSM_Student().B_EntityDataCopyForMaterial(this, true), this.trans);

            var studentDepartmentsList = GetListStudentDepartment();
            rs &= db.BulkInsertCSM_StudentDepartment(studentDepartmentsList, this.trans);

            rs &= db.InsertCSM_Activity(new CSM_Activity().B_EntityDataCopyForMaterial(this.LastActivity, true), this.trans);

            return rs;
        }

        private ResultStatus Update()
        {
            var rs = new ResultStatus { result = true };
            rs &= db.UpdateCSM_Student(new CSM_Student().B_EntityDataCopyForMaterial(this, true), false, this.trans);

            var studentDepartment = db.GetCSM_StudentDepartmentByStudentId(this.id, this.trans);
            rs &= db.BulkDeleteCSM_StudentDepartment(studentDepartment, this.trans);

            var newStudentDepartmentsList = GetListStudentDepartment();
            rs &= db.BulkInsertCSM_StudentDepartment(newStudentDepartmentsList, this.trans);

            return rs;
        }

        private List<CSM_StudentDepartment> GetListStudentDepartment()
        {
            var studentDepartmentsList = new List<CSM_StudentDepartment>();
            if (this.StudentDepartmentIds != null && this.StudentDepartmentIds.Count() > 0)
            {
                foreach (var departmentId in this.StudentDepartmentIds)
                {
                    studentDepartmentsList.Add(new CSM_StudentDepartment
                    {
                        id = Guid.NewGuid(),
                        created = DateTime.Now,
                        createdby = this.createdby,
                        departmentId = departmentId,
                        studentId = this.id
                    });
                }
            }

            return studentDepartmentsList;
        }


        public ResultStatus SendForm(CSM_StudentFBUWeb studentData)
        {
            if (studentData == null)
            {
                return new ResultStatus { result = false, message = "Veri okunamadı." };
            }

            if (studentData.phone == null)
            {
                return new ResultStatus { result = false, message = "Telefon numarası bulunamadı." };
            }

            var db = new WorkOfTimeDatabase();
            var rsm = new ResultStatus { result = true };
            var culture = new System.Globalization.CultureInfo("tr-TR", false);

            var studentControl = db.GetCSM_StudentByPhone(studentData.phone);
            if (studentControl != null)
            {
                return new ResultStatus { result = false, message = "Aynı telefon numarası ile kayıt mevcut." };
            }

            var departmentsDB = db.GetCSM_Department().ToList();
            var locations = db.GetUT_LocationCityAndTownInTR();

            var schoolList = new List<CSM_School>();
            var departmentList = new List<CSM_Department>();
            var studentDeparmentList = new List<CSM_StudentDepartment>();

            var newStudent = new CSM_Student
            {
                id = Guid.NewGuid(),
                name = studentData.name + " " + studentData.surname,
                phone = studentData.phone.Replace(" ", ""),
                email = studentData.mail,
                created = studentData.date.HasValue ? studentData.date : DateTime.Now,
                createdby = Guid.Empty,
                isAllowContact = studentData.hasContact.HasValue ? studentData.hasContact : true,
                source = (short)EnumCSM_StudentSource.WebSite,
                sourceDescription = !string.IsNullOrEmpty(studentData.sourceDescription) ? studentData.sourceDescription : "Web Aday Form",
                deparmentCurrent = studentData.departmentCurrent
            };

            if (!String.IsNullOrEmpty(studentData.school))
            {
                var isExistSchool = db.GetCSM_School().Where(a => a.name.Replace(" ", "").ToLower(culture) == studentData.school.Replace(" ", "").ToLower(culture)).FirstOrDefault();

                if (isExistSchool == null)
                {
                    isExistSchool = new CSM_School
                    {
                        name = studentData.school,
                        created = studentData.date.HasValue ? studentData.date : DateTime.Now,
                        createdby = Guid.Empty,
                        id = Guid.NewGuid(),
                    };

                    schoolList.Add(isExistSchool);
                }

                newStudent.schoolId = isExistSchool.id;
            }

            foreach (var department in studentData.departmentTargets)
            {
                if (String.IsNullOrEmpty(department))
                    continue;

                var isExistDepartment = departmentsDB.Where(a => a.name.Replace(" ", "").ToLower(culture) == department.Replace(" ", "").ToLower(culture)).FirstOrDefault();

                if (isExistDepartment == null)
                {
                    isExistDepartment = new CSM_Department
                    {
                        id = Guid.NewGuid(),
                        created = studentData.date.HasValue ? studentData.date : DateTime.Now,
                        createdby = Guid.Empty,
                        name = department
                    };

                    departmentList.Add(isExistDepartment);
                    departmentsDB.Add(isExistDepartment);
                }

                studentDeparmentList.Add(new CSM_StudentDepartment
                {
                    id = Guid.NewGuid(),
                    created = studentData.date.HasValue ? studentData.date : DateTime.Now,
                    createdby = Guid.Empty,
                    departmentId = isExistDepartment.id,
                    studentId = newStudent.id
                });
            }

            if (!String.IsNullOrEmpty(studentData.city))
            {
                var city = locations.Where(x => x.type == (int)EnumUT_LocationType.Sehir && x.name == studentData.city).FirstOrDefault();

                if (city != null)
                {
                    newStudent.locationId = city.id;

                    if (!String.IsNullOrEmpty(studentData.town))
                    {
                        var town = locations.Where(x => x.type == (int)EnumUT_LocationType.İlce && x.name == studentData.town).ToArray();

                        if (town.Count() > 0)
                        {
                            newStudent.locationId = town.Where(a => a.pid == city.id).Select(a => a.id).FirstOrDefault();
                        }
                    }
                }
            }

            var isExistsStage = db.CSM_StageByStatus((short)EnumCSM_StageStatus.FormDoldurdu);
            bool stage = true;
            if (isExistsStage == null)
            {
                isExistsStage = new CSM_Stage
                {
                    id = Guid.NewGuid(),
                    created = studentData.date.HasValue ? studentData.date : DateTime.Now,
                    createdby = Guid.Empty,
                    code = "1",
                    color = "#48d5da",
                    name = "Form Dolduruldu",
                    status = (int)EnumCSM_StageStatus.FormDoldurdu,
                };

                stage = false;
            }

            var activity = new CSM_Activity
            {
                id = Guid.NewGuid(),
                created = studentData.date.HasValue ? studentData.date : DateTime.Now,
                createdby = Guid.Empty,
                date = studentData.date,
                contactDate = studentData.date.HasValue ? studentData.date : DateTime.Now,
                studentId = newStudent.id,
                type = (int)EnumCSM_ActivityType.Diger,
                stageId = isExistsStage.id
            };

            var trans = db.BeginTransaction();
            if (!stage)
            {
                rsm &= db.InsertCSM_Stage(isExistsStage, trans);
            }
            rsm &= db.BulkInsertCSM_School(schoolList, trans);
            rsm &= db.BulkInsertCSM_Department(departmentList, trans);
            rsm &= db.InsertCSM_Student(newStudent, trans);
            rsm &= db.BulkInsertCSM_StudentDepartment(studentDeparmentList, trans);
            rsm &= db.InsertCSM_Activity(activity, trans);

            if (!rsm.result)
            {
                trans.Rollback();
            }
            else
            {
                trans.Commit();
            }

            return rsm;
        }

    }
    public class CSM_StudentFBUWeb
    {
        public string name { get; set; }
        public string surname { get; set; }
        public string mail { get; set; }
        public string phone { get; set; }
        public string school { get; set; }
        public string grade { get; set; }
        public string departmentCurrent { get; set; }
        public string[] departmentTargets { get; set; }
        public string sourceDescription { get; set; }
        public string city { get; set; }
        public string town { get; set; }
        public DateTime? date { get; set; }
        public bool? hasContact { get; set; }
    }

    public class CallCenterListModel
    {
        public string ListId { get; set; }
        public string ListDescription { get; set; }
        public bool Status { get; set; }
    }

    public class CallCenterSendViewModel
    {
        public string filter { get; set; }
        public string ids { get; set; }
        public CallCenterListModel[] callList { get; set; }
    }

    public class VWCSM_ActivityCallCenter
    {
        public string DialId { get; set; }
        public string AgentName { get; set; }
        public int? ResultCode { get; set; }
        public int? SystemResultCode { get; set; }
        public string ResultDescription { get; set; }
        public DateTime? AppointmentDate { get; set; }
        public string AppointmentPhone { get; set; }
    }
}
