using System.Linq;
using System.Text.RegularExpressions;

namespace System.Web.Mvc
{
	public struct CustomerBackendValidations
	{
		public static int[] sayilar = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
		public static CustomerBackendValidation Name(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				return new CustomerBackendValidation
				{
					data_error = "İşletme adı alanı zorunludur!",
					IsError = true
				};
			}

			return new CustomerBackendValidation()
			{
				IsError = false
			};
		}

		public static CustomerBackendValidation Phone(string phone)
		{
			Regex r = new Regex("([+][0-9])?(0)[0-9]{10}");

			if (!r.IsMatch(phone))
			{
				return new CustomerBackendValidation
				{
					data_error = "Lütfen Telefon Numaranızın Doğruluğunu Kontrol Ediniz!",
					IsError = true
				};
			}
			else
			{
				return new CustomerBackendValidation()
				{
					IsError = false
				};
			}
		}
	}

	public class CustomerBackendValidation
	{
		public string data_error { get; set; }
		public bool IsError { get; set; }
	}

}
