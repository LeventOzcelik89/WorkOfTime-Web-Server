using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infoline.WorkOfTime.BusinessAccess
{
	public class LoginModel
	{
		[Required(ErrorMessage = "Kullanıcı Adı alanı boş geçilemez. ")]
		[Display(Name = "Eposta veya Kullanıcı Adı", Prompt = "Eposta veya Kullanıcı Adı")]
		public string loginname { get; set; }

		[Required(ErrorMessage = "Şifre alanı boş geçilemez."), DataType(DataType.Password)]
		[Display(Name = "Şifre", Prompt = "Şifre")]
		public string password { get; set; }

		[Display(Name = "Captcha Doğrulama Kodu", Prompt = "Captcha Doğrulama Kodu giriniz.")]
		public string CapthcaCode { get; set; }
	}
}
