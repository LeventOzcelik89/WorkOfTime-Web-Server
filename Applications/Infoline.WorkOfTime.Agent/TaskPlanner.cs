using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infoline.WorkOfTime.Agent
{
    public class TaskPlanner
    {

        public TEN_Tenant _tenant { get; set; }
        public WorkOfTimeDatabase _db { get; set; }
        public DateTime _lastDate
        {
            get
            {
                return DateTime.Now.AddDays(-30);
            }
        }


        private bool enabled
        {
            get
            {
                return ConfigurationSettings.AppSettings.Get("RunTasks") == "1";
            }
        }

        public TaskPlanner(TEN_Tenant tenant)
        {
            this._tenant = tenant;
            this._db = new WorkOfTimeDatabase();
        }

        public void CreateTasks(DateTime dt)
        {

            try
            {

                if (!enabled)
                {
                    Log.Info("Görev Oluşturma Modülü devre dışı !..");
                    return;
                }

                var counts = new int[] { 0, 0 };
                var dbRes = new ResultStatus { result = true };
                var now = new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, 0);

                var creationTasks = new List<TaskSchedulerModel>();
                var plans = _db.GetFTM_TaskPlan().Where(a => a.enabled == true).ToArray();

                foreach (var plan in plans)
                {

                    var schedulerModel = new TaskSchedulerModel();
                    schedulerModel.Load(plan.id);

                    if (!schedulerModel.TaskPlan.taskCreationTime.HasValue)
                    {
                        continue;
                    }

                    schedulerModel.TaskPlan.LoadTasks(_lastDate);

                    switch ((EnumFTM_TaskPlansTaskCreationTime)schedulerModel.TaskPlan.taskCreationTime)
                    {
                        case EnumFTM_TaskPlansTaskCreationTime.Hemen:

                            continue;
                        //  TaskPlan Insert edildiği anda yapılıyor.

                        case EnumFTM_TaskPlansTaskCreationTime.AyOnce:

                            schedulerModel.TaskPlan._TaskList = schedulerModel.TaskPlan._TaskList.Where(a => a.start.RoundSecond() <= now.AddDays(30)).ToArray();
                            if (schedulerModel.TaskPlan._TaskList.Count() > 0)
                            {

                                var createdTasks = _db.GetVWFTM_TaskByTaskPlanIdAndDates(schedulerModel.TaskPlan.id, _lastDate, now.AddDays(30));

                                schedulerModel.TaskPlan._TaskList = schedulerModel.TaskPlan._TaskList
                                    .Where(a => a.planStartDate.HasValue && !createdTasks.Select(b => b.planStartDate).Contains(a.planStartDate.Value)).ToArray();

                                creationTasks.Add(schedulerModel);

                            }

                            break;
                        case EnumFTM_TaskPlansTaskCreationTime.HaftaOnce:

                            schedulerModel.TaskPlan._TaskList = schedulerModel.TaskPlan._TaskList.Where(a => a.start.RoundSecond() <= now.AddDays(7)).ToArray();
                            if (schedulerModel.TaskPlan._TaskList.Count() > 0)
                            {

                                var createdTasks = _db.GetVWFTM_TaskByTaskPlanIdAndDates(schedulerModel.TaskPlan.id, _lastDate, now.AddDays(7));

                                schedulerModel.TaskPlan._TaskList = schedulerModel.TaskPlan._TaskList
                                    .Where(a => a.planStartDate.HasValue && !createdTasks.Select(b => b.planStartDate).Contains(a.planStartDate.Value)).ToArray();

                                creationTasks.Add(schedulerModel);

                            }

                            break;
                        case EnumFTM_TaskPlansTaskCreationTime.GunOnce:
                        case EnumFTM_TaskPlansTaskCreationTime.Gununde:

                            schedulerModel.TaskPlan._TaskList = schedulerModel.TaskPlan._TaskList.Where(a => a.start.RoundSecond() <= now.AddDays(1)).ToArray();
                            if (schedulerModel.TaskPlan._TaskList.Count() > 0)
                            {

                                var createdTasks = _db.GetVWFTM_TaskByTaskPlanIdAndDates(schedulerModel.TaskPlan.id, _lastDate, now.AddDays(1));

                                schedulerModel.TaskPlan._TaskList = schedulerModel.TaskPlan._TaskList
                                    .Where(a => a.planStartDate.HasValue && !createdTasks.Select(b => b.planStartDate).Contains(a.planStartDate.Value)).ToArray();

                                creationTasks.Add(schedulerModel);

                            }

                            break;

                        default:
                            break;
                    }

                }

                foreach (var schedule in creationTasks)
                {

                    if (schedule.TemplateModel.taskTemplateInventories.Count() == 0)
                    {
                        foreach (var task in schedule.TaskPlan._TaskList)
                        {
                            var _task = task
                                .B_ConvertType<FTM_Task>()
                                .B_ConvertType<VMFTM_TaskModel>()
                                .AppendObjectToOther(schedule.TemplateModel);

                            _task.id = Guid.NewGuid();
                            _task.FTM_TaskSubjectTypeIds = schedule.TemplateModel.FTM_TaskTemplateSubjectTypeIds;

                            dbRes &= _task.InsertAll(Guid.Empty);
                            if (dbRes.result)
                            {
                                counts[0]++;
                            }
                            else
                            {
                                counts[1]++;
                            }

                        }
                    }
                    else
                    {
                        foreach (var inventoryId in schedule.TemplateModel.taskTemplateInventories)
                        {
                            foreach (var task in schedule.TaskPlan._TaskList)
                            {
                                var _task = task
                                    .B_ConvertType<FTM_Task>()
                                    .B_ConvertType<VMFTM_TaskModel>()
                                    .AppendObjectToOther(schedule.TemplateModel);

                                _task.id = Guid.NewGuid();
                                _task.fixtureId = inventoryId.inventoryId;
                                _task.FTM_TaskSubjectTypeIds = schedule.TemplateModel.FTM_TaskTemplateSubjectTypeIds;

                                dbRes &= _task.InsertAll(Guid.Empty);
                                if (dbRes.result)
                                {
                                    counts[0]++;
                                }
                                else
                                {
                                    counts[1]++;
                                }
                            }
                        }
                    }
                }

                if (counts[0] == 0 && counts[1] == 0)
                {
                    return;
                }

                if (counts[1] > 0 && counts[0] == 0)
                {
                    Log.Error("{3} Adet Planlanan Görevler oluşturulamadı. Mesaj: {2} ({0} {1})", _tenant.TenantName, _tenant.TenantCode, dbRes.message, counts[1]);
                    return;
                }

                Log.Success("{2} Başarılı / {3} Başarısız Görev Yaratıldı. {0} {1}", _tenant.TenantName, _tenant.TenantCode, counts[0], counts[1]);

            }
            catch (Exception ex)
            {
                Log.Error("Takvim Görev Planlama çalıştırılırken sorun oluştu. Mesaj: {2} ({0} {1})", _tenant.TenantName, _tenant.TenantCode, ex.Message);
            }

        }

    }
}
