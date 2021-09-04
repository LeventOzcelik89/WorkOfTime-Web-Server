using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Infoline.WorkOfTime.WebProject.Models
{
	public class SignInResult
	{
		public bool RequiredCaptcha { get; set; }
		public bool Required2FA { get; set; }
	}
}