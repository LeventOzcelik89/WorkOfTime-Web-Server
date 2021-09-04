using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using Infoline.WorkOfTime.BusinessAccess;

namespace System.Web.Mvc
{
	public struct ProductBackendValidations
	{
		public static ProductBackendValidation Name(string name)
		{
			if (String.IsNullOrEmpty(name))
			{
				return new ProductBackendValidation()
				{
					IsError = true,
					data_error = " İsim alanı zorunludur"
				};
			}

			return new ProductBackendValidation()
			{
				IsError = false,
			};
		}

		public static ProductBackendValidation Code(string code)
		{
			if (String.IsNullOrEmpty(code))
			{
				return new ProductBackendValidation()
				{
					IsError = true,
					data_error = " Kod alanı zorunludur"
				};
			}

			return new ProductBackendValidation()
			{
				IsError = false,
			};
		}

		public static ProductBackendValidation Unit(string unit)
		{
			var db = new WorkOfTimeDatabase();
			var units = db.GetUT_Unit();

			if (!string.IsNullOrEmpty(unit))
			{
				foreach (var unt in units)
				{
					if (unt.name.ToUpper() != unit.ToUpper())
					{
						return new ProductBackendValidation()
						{
							IsError = false,
						};
					}
					if (unt.name.ToLower() != unit.ToLower())
                    {
						return new ProductBackendValidation()
						{
							IsError = false,
						};
					}
				}
			}
			else
			{
				return new ProductBackendValidation()
				{
					IsError = false,
				};
			}

			return new ProductBackendValidation()
			{
				IsError = true,
				data_error = " Birim alanına lütfen örnek excel'de verilen şekilde giriş yapınız."
			};
		}
	}

	public class ProductBackendValidation
	{
		public string data_error { get; set; }
		public bool IsError { get; set; }
	}

}
