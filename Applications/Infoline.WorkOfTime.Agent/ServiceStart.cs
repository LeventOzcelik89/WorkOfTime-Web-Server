using Infoline.WorkOfTime.BusinessAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace Infoline.WorkOfTime.Agent
{
	public partial class ServiceStart : ServiceBase
	{
		readonly List<TEN_Tenant> Tenants = new List<TEN_Tenant>();
		List<Task> Tasks = new List<Task>();

		public ServiceStart()
		{
			var temp = TenantConfig.GetTenants();

			var exTenants = new string[] {
			   "724FB68E-AD3B-4015-B445-4E4707B4EC1D",
			   "B0FA8D6B-B6C6-4DB8-8844-C5AFF70D60AB",
			   "E8975D9C-BC02-4A2F-B39F-F87E2B94E39A",
			   "BFACC3EB-0A1C-446B-BDCF-8132700EF822" }.Select(a => a.B_ToGuid());


			Tenants = temp.Where(a => !exTenants.Contains(a.id)).ToList();

			//  TODO [LÖ] Geliştirme ortamı için sadece WOT DEVELOPER açıldı.
			//  Tenants = temp.Where(a => a.DBCatalog == "WorkOfTime").ToList();

		}

		public void Process()
		{
			foreach (var tenant in Tenants)
			{
				var t = new Task(() =>
				{
					new TenantEmailSender(tenant);
				});
				Tasks.Add(t);
				t.Start();
			}
		}

		protected override void Dispose(bool disposing)
		{
			Log.Success("Görevler dispose ediliyor");

			if (Tasks != null && Tasks.Count > 0)
			{
				foreach (var task in Tasks)
				{
					task.Dispose();
				}
				Tasks = new List<Task>();
			}

			base.Dispose(disposing);
		}

		protected override void OnStart(string[] args)
		{
			Log.Success("Uygulama başlatıldı.");
			Process();
		}

		protected override void OnStop()
		{
			Log.Success("Uygulama durduruldu.");
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
