using System.Linq;
using System.Text.RegularExpressions;

namespace System.Web.Mvc
{
	public struct StudentBackendValidations
	{
		public static int[] sayilar = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
		public static string[] turkceKarakter = new string[] { "a", "b", "c", "ç", "d", "e", "f", "g", "ğ", "h", "i", "ı", "j", "k", "l", "m", "n", "o", "ö", "p", "r", "s", "ş", "t", "u", "ü", "v", "y", "z", "q", "w" };
		public static string[] ozelKarakter = new string[] { "=", "!", "”", "#", "$", "%", "*", "+", ":", ";", "<", ">", "?", "|", "æ" };
		public static StudentBackEndValidation Name(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				return new StudentBackEndValidation
				{
					data_error = "Ad Alanını Excel Başlıklarında Eşleştiriniz.!",
					IsError = true
				};
			}

			foreach (var sayi in sayilar)
			{
				if (name.Contains(sayi.ToString()))
				{
					return new StudentBackEndValidation
					{
						data_error = "Lütfen Ad Alanına Sadece Harf Girişi Yapınız.!",
						IsError = true
					};
				}
			}
			foreach (var karakter in ozelKarakter)
			{
				if (name.Contains(karakter) || name.Contains("@"))
				{
					return new StudentBackEndValidation
					{
						data_error = "Ad Alanına Özel Karakter Girilemez.!",
						IsError = true
					};
				}
			}

			return new StudentBackEndValidation()
			{
				IsError = false
			};
		}

		public static StudentBackEndValidation Surname(string surname)
		{
			foreach (var sayi in sayilar)
			{
				if (surname.Contains(sayi.ToString()))
				{
					return new StudentBackEndValidation
					{
						data_error = "Lütfen Soyad Alanına Sadece Harf Girişi Yapınız.!",
						IsError = true
					};
				}
			}
			foreach (var karakter in ozelKarakter)
			{
				if (surname.Contains(karakter) || karakter.Contains("@"))
				{
					return new StudentBackEndValidation
					{
						data_error = "Soyad Alanına Özel Karakter Girilemez.!",
						IsError = true
					};
				}
			}

			return new StudentBackEndValidation()
			{
				IsError = false
			};
		}

		public static StudentBackEndValidation Email(string email)
		{
			Regex r = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
			if (string.IsNullOrEmpty(email))
			{
				return new StudentBackEndValidation
				{
					data_error = "E-Posta Alanını Excel Başlıklarında Eşleştiriniz.!",
					IsError = true
				};
			}
			if (!r.IsMatch(email))
			{
				return new StudentBackEndValidation
				{
					data_error = "Lütfen E-Posta Adresinizi Kontrol Ediniz!",
					IsError = true
				};
			}
			foreach (var karakter in ozelKarakter)
			{
				if (email.Contains(karakter))
				{
					return new StudentBackEndValidation
					{
						data_error = "E-Posta Alanına Özel Karakter Girilemez.!",
						IsError = true
					};
				}
			}
			return new StudentBackEndValidation()
			{
				IsError = false
			};
		}

		public static StudentBackEndValidation Phone(string phone)
		{
			Regex r = new Regex("([+][0-9])?(0)[0-9]{10}");
			if (string.IsNullOrEmpty(phone))
			{
				return new StudentBackEndValidation
				{
					data_error = "Telefon Alanını Excel Başlıklarında Eşleştiriniz.!",
					IsError = true
				};
			}

			if (!r.IsMatch(phone))
			{
				return new StudentBackEndValidation
				{
					data_error = "Lütfen Telefon Numaranızın Doğruluğunu Kontrol Ediniz!",
					IsError = true
				};
			}
			else
			{
				return new StudentBackEndValidation()
				{
					IsError = false
				};
			}
		}

		public static StudentBackEndValidation DepartmentTarget(string departmentCurrent)
		{
			if (string.IsNullOrEmpty(departmentCurrent))
			{
				return new StudentBackEndValidation
				{
					IsError = false
				};
			}

			departmentCurrent = departmentCurrent.Replace("@", "");
			departmentCurrent = departmentCurrent.Replace("!", "");
			departmentCurrent = departmentCurrent.Replace("#", "");
			departmentCurrent = departmentCurrent.Replace("*", "");
			departmentCurrent = departmentCurrent.Replace("+", "");
			departmentCurrent = departmentCurrent.Replace("<", "");
			departmentCurrent = departmentCurrent.Replace(">", "");
			departmentCurrent = departmentCurrent.Replace("æ", "");

			if (departmentCurrent.EndsWith(",") || departmentCurrent.EndsWith("-"))
			{
				departmentCurrent = departmentCurrent.Substring(0, departmentCurrent.Length - 1);
			}
			if (departmentCurrent.StartsWith(",") || departmentCurrent.StartsWith("-"))
			{
				departmentCurrent = departmentCurrent.Substring(1, departmentCurrent.Length - 1);
			}
			
			foreach (var karakter in ozelKarakter)
			{
				if (departmentCurrent.Contains(karakter))
				{
					return new StudentBackEndValidation
					{
						data_error = "İlgilendiği Bölümler Alanına Özel Karakter Girilemez.!",
						IsError = true
					};
				}
			}
			return new StudentBackEndValidation()
			{
				IsError = false
			};
		}

		public static StudentBackEndValidation City(string city)
		{
			
			if (string.IsNullOrEmpty(city))
			{
				return new StudentBackEndValidation() {IsError = false };
			}
			
			foreach (var sayi in sayilar)
			{
				if (city.Contains(sayi.ToString()))
				{
					return new StudentBackEndValidation
					{
						data_error = "Lütfen İl Alanına Sadece Harf Girişi Yapınız.!",
						IsError = true
					};
				}
			}
			foreach (var karakter in ozelKarakter)
			{
				if (city.Contains(karakter) || city.Contains("@"))
				{
					return new StudentBackEndValidation
					{
						data_error = "İl Alanına Özel Karakter Girilemez.!",
						IsError = true
					};
				}
			}
			return new StudentBackEndValidation()
			{
				IsError = false
			};
		}

		public static StudentBackEndValidation Town(string town)
		{
			if (string.IsNullOrEmpty(town))
			{
				return new StudentBackEndValidation() { IsError = false };
			}
			
			foreach (var sayi in sayilar)
			{
				if (town.Contains(sayi.ToString()))
				{
					return new StudentBackEndValidation
					{
						data_error = "Lütfen İlçe Alanına Sadece Harf Girişi Yapınız.!",
						IsError = true
					};
				}
			}
			foreach (var karakter in ozelKarakter)
			{
				if (town.Contains(karakter) || town.Contains("@"))
				{
					return new StudentBackEndValidation
					{
						data_error = "İlçe Alanına Özel Karakter Girilemez.!",
						IsError = true
					};
				}
			}
			return new StudentBackEndValidation()
			{
				IsError = false
			};
		}

		public static StudentBackEndValidation School(string school)
		{
			if (string.IsNullOrEmpty(school))
			{
				return new StudentBackEndValidation()
				{
					IsError = false
				};
			}

			foreach (var karakter in ozelKarakter)
			{
				if (school.Contains(karakter) || school.Contains("@"))
				{
					return new StudentBackEndValidation
					{
						data_error = "Okul Alanına Özel Karakter Girilemez.!",
						IsError = true
					};
				}
			}
			return new StudentBackEndValidation()
			{
				IsError = false
			};
		}
	}

	public class StudentBackEndValidation
	{
		public string data_error { get; set; }
		public bool IsError { get; set; }
	}

}
