﻿using Infoline.OmixEntegrationApp.FtpEntegrations.Business;
using Infoline.WorkOfTime.BusinessAccess;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infoline.OmixEntegrationApp.FtpEntegrations
{

    public class ProcessFtpEntegration : IDisposable
    {
        List<Task> Tasks = new List<Task>();
        private static System.Threading.Tasks.Task task { get; set; }
        private static object control = new object();
        public ProcessFtpEntegration()
        {
            Log.Info("ProcessFtpEntegration is Start");
        }
        public void Run()
        {
            if (task == null)
            {
                lock (control)
                {
                    task = System.Threading.Tasks.Task.Run(() =>
                    {
                        do
                        {
                            try
                            {
                                var now = DateTime.Now;
                                if (now.Hour == 1 && now.Minute == 00)
                                {
                                    var taskMobitel = new Task(() =>
                                    {
                                        new FtpMobitel().ExportFilesToDatabase(); ;
                                    });
                                    Tasks.Add(taskMobitel);

                                    var taskGenpa = new Task(() =>
                                    {
                                        new FtpGenpa().ExportFilesToDatabase(); ;
                                    });
                                    Tasks.Add(taskGenpa);

                                    var taskKvk = new Task(() =>
                                    {
                                        new FtpKvk().ExportFilesToDatabase(); ;
                                    });
                                    Tasks.Add(taskKvk);

                                    var taskPort = new Task(() =>
                                    {
                                        new FtpPort().ExportFilesToDatabase(); ;
                                    });
                                    Tasks.Add(taskPort);

                                    foreach (var task in Tasks)
                                    {
                                        task.Start();
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Log.Error("Entegrasyon servisleri çalışırken sorun oluştu. Mesaj : " + ex.Message + "mm: " + ex.InnerException.Message);
                            }
                            System.Threading.Thread.Sleep(new TimeSpan(0, 1, 0));
                        } while (true);
                    });
                }
            }
        }
        public void Dispose()
        {
            Log.Success("Tasklar durduruldu.");
            if (Tasks != null && Tasks.Count > 0)
            {
                foreach (var task in Tasks)
                {
                    task.Dispose();
                }
                Tasks = new List<Task>();
            }
        }
    }
}
