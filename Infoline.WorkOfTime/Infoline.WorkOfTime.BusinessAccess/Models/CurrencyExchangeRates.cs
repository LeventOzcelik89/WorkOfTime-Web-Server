using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Infoline.WorkOfTime.BusinessAccess
{
    public class Currency
    {
        public string Name { get; }
        public string Code { get; }
        public string CrossRateName { get; }
        public double ForexBuying { get; }
        public double ForexSelling { get; }
        public double BanknoteBuying { get; }
        public double BanknoteSelling { get; }

        public Currency(string name, string code, string crossRateName, double forexBuying, double forexSelling, double banknoteBuying, double banknoteSelling)
        {
            Name = name;
            Code = code;
            CrossRateName = crossRateName;
            ForexBuying = forexBuying;
            ForexSelling = forexSelling;
            BanknoteBuying = banknoteBuying;
            BanknoteSelling = banknoteSelling;

        }
    }

    public enum ExchangeType
    {
        ForexBuying, ForexSelling,
        BanknoteBuying, BanknoteSelling
    }

    public enum CurrencyCode
    {
        USD = 0, AUD = 1, DKK = 2,
        EUR = 3, GBP = 4, CHF = 5,
        SEK = 6, CAD = 7, KWD = 8,
        NOK = 9, SAR = 10, JPY = 11,
        BGN = 12, RON = 13, RUB = 14,
        IRR = 15, CNY = 16, PKR = 17,
        TRY = 18
    }

    public static class CurrencyExchangeRates
    {


        public static Dictionary<string, Currency> GetAllCurrenciesTodaysExchangeRates()
        {
            try
            {
                return GetCurrencyRates("https://www.tcmb.gov.tr/kurlar/today.xml");
            }
            catch
            {
                throw new Exception("The date specified may be a weekend or a public holiday!");
            }
        }

        public static DataTable GetDataTableAllCurrenciesTodaysExchangeRates()
        {
            try
            {
                Dictionary<string, Currency> CurrencyRates = GetCurrencyRates("http://www.tcmb.gov.tr/kurlar/today.xml");

                DataTable dt = new DataTable();
                dt.Columns.Add("Name", typeof(string));
                dt.Columns.Add("Code", typeof(string));
                dt.Columns.Add("CrossRateName", typeof(string));
                dt.Columns.Add("ForexBuying", typeof(double));
                dt.Columns.Add("ForexSelling", typeof(double));
                dt.Columns.Add("BanknoteBuying", typeof(double));
                dt.Columns.Add("BanknoteSelling", typeof(double));

                foreach (string item in CurrencyRates.Keys)
                {
                    DataRow dr = dt.NewRow();
                    dr["Name"] = CurrencyRates[item].Name;
                    dr["Code"] = CurrencyRates[item].Code;
                    dr["CrossRateName"] = CurrencyRates[item].CrossRateName;
                    dr["ForexBuying"] = CurrencyRates[item].ForexBuying;
                    dr["ForexSelling"] = CurrencyRates[item].ForexSelling;
                    dr["BanknoteBuying"] = CurrencyRates[item].BanknoteBuying;
                    dr["BanknoteSelling"] = CurrencyRates[item].BanknoteSelling;
                    dt.Rows.Add(dr);
                }

                return dt;
            }
            catch
            {
                throw new Exception("The date specified may be a weekend or a public holiday!");
            }
        }

        public static Dictionary<string, Currency> GetAllCurrenciesHistoricalExchangeRates(int Year, int Month, int Day)
        {
            try
            {
                string SYear = String.Format("{0:0000}", Year);
                string SMonth = String.Format("{0:00}", Month);
                string SDay = String.Format("{0:00}", Day);

                return GetCurrencyRates("http://www.tcmb.gov.tr/kurlar/" + SYear + SMonth + "/" + SDay + SMonth + SYear + ".xml");
            }
            catch
            {
                throw new Exception("The date specified may be a weekend or a public holiday!");
            }
        }

        public static DataTable GetDataTableAllCurrenciesHistoricalExchangeRates(int Year, int Month, int Day)
        {
            try
            {
                string SYear = String.Format("{0:0000}", Year);
                string SMonth = String.Format("{0:00}", Month);
                string SDay = String.Format("{0:00}", Day);

                Dictionary<string, Currency> CurrencyRates = GetCurrencyRates("http://www.tcmb.gov.tr/kurlar/" + SYear + SMonth + "/" + SDay + SMonth + SYear + ".xml");

                DataTable dt = new DataTable();
                dt.Columns.Add("Name", typeof(string));
                dt.Columns.Add("Code", typeof(string));
                dt.Columns.Add("CrossRateName", typeof(string));
                dt.Columns.Add("ForexBuying", typeof(double));
                dt.Columns.Add("ForexSelling", typeof(double));
                dt.Columns.Add("BanknoteBuying", typeof(double));
                dt.Columns.Add("BanknoteSelling", typeof(double));

                foreach (string item in CurrencyRates.Keys)
                {
                    DataRow dr = dt.NewRow();
                    dr["Name"] = CurrencyRates[item].Name;
                    dr["Code"] = CurrencyRates[item].Code;
                    dr["CrossRateName"] = CurrencyRates[item].CrossRateName;
                    dr["ForexBuying"] = CurrencyRates[item].ForexBuying;
                    dr["ForexSelling"] = CurrencyRates[item].ForexSelling;
                    dr["BanknoteBuying"] = CurrencyRates[item].BanknoteBuying;
                    dr["BanknoteSelling"] = CurrencyRates[item].BanknoteSelling;
                    dt.Rows.Add(dr);
                }

                return dt;

            }
            catch
            {
                throw new Exception("The date specified may be a weekend or a public holiday!");
            }
        }

        public static Dictionary<string, Currency> GetAllCurrenciesHistoricalExchangeRates(DateTime date)
        {
            try
            {
                string SYear = String.Format("{0:0000}", date.Year);
                string SMonth = String.Format("{0:00}", date.Month);
                string SDay = String.Format("{0:00}", date.Day);

                return GetCurrencyRates("http://www.tcmb.gov.tr/kurlar/" + SYear + SMonth + "/" + SDay + SMonth + SYear + ".xml");
            }
            catch
            {
                throw new Exception("The date specified may be a weekend or a public holiday!");
            }
        }

        public static DataTable GetDataTableAllCurrenciesHistoricalExchangeRates(DateTime date)
        {
            try
            {
                string SYear = String.Format("{0:0000}", date.Year);
                string SMonth = String.Format("{0:00}", date.Month);
                string SDay = String.Format("{0:00}", date.Day);

                Dictionary<string, Currency> CurrencyRates = GetCurrencyRates("http://www.tcmb.gov.tr/kurlar/" + SYear + SMonth + "/" + SDay + SMonth + SYear + ".xml");

                DataTable dt = new DataTable();
                dt.Columns.Add("Name", typeof(string));
                dt.Columns.Add("Code", typeof(string));
                dt.Columns.Add("CrossRateName", typeof(string));
                dt.Columns.Add("ForexBuying", typeof(double));
                dt.Columns.Add("ForexSelling", typeof(double));
                dt.Columns.Add("BanknoteBuying", typeof(double));
                dt.Columns.Add("BanknoteSelling", typeof(double));

                foreach (string item in CurrencyRates.Keys)
                {
                    DataRow dr = dt.NewRow();
                    dr["Name"] = CurrencyRates[item].Name;
                    dr["Code"] = CurrencyRates[item].Code;
                    dr["CrossRateName"] = CurrencyRates[item].CrossRateName;
                    dr["ForexBuying"] = CurrencyRates[item].ForexBuying;
                    dr["ForexSelling"] = CurrencyRates[item].ForexSelling;
                    dr["BanknoteBuying"] = CurrencyRates[item].BanknoteBuying;
                    dr["BanknoteSelling"] = CurrencyRates[item].BanknoteSelling;
                    dt.Rows.Add(dr);
                }

                return dt;

            }
            catch
            {
                throw new Exception("The date specified may be a weekend or a public holiday!");
            }
        }

        public static Currency GetTodaysExchangeRates(string Currency)
        {
            try
            {
                Dictionary<string, Currency> CurrencyRates = GetCurrencyRates("http://www.tcmb.gov.tr/kurlar/today.xml");

                if (CurrencyRates.Keys.Contains(Currency))
                {
                    return CurrencyRates[Currency];
                }
                else
                {
                    throw new Exception("The specified currency(" + Currency + ") was not found!");
                }
            }
            catch
            {
                throw new Exception("The date specified may be a weekend or a public holiday!");
            }
        }


        public static Currency GetHistoricalExchangeRates(string Currency, int Year, int Month, int Day)
        {
            try
            {
                string SYear = String.Format("{0:0000}", Year);
                string SMonth = String.Format("{0:00}", Month);
                string SDay = String.Format("{0:00}", Day);

                Dictionary<string, Currency> CurrencyRates = GetCurrencyRates("http://www.tcmb.gov.tr/kurlar/" + SYear + SMonth + "/" + SDay + SMonth + SYear + ".xml");

                if (CurrencyRates.Keys.Contains(Currency.ToString()))
                {
                    return CurrencyRates[Currency.ToString()];
                }
                else
                {
                    throw new Exception("The specified currency(" + Currency.ToString() + ") was not found!");
                }
            }
            catch
            {
                throw new Exception("The date specified may be a weekend or a public holiday!");
            }
        }

        public static Currency GetHistoricalExchangeRates(string Currency, DateTime date)
        {
            try
            {
                string SYear = String.Format("{0:0000}", date.Year);
                string SMonth = String.Format("{0:00}", date.Month);
                string SDay = String.Format("{0:00}", date.Day);

                Dictionary<string, Currency> CurrencyRates = GetCurrencyRates("http://www.tcmb.gov.tr/kurlar/" + SYear + SMonth + "/" + SDay + SMonth + SYear + ".xml");

                if (CurrencyRates.Keys.Contains(Currency.ToString()))
                {
                    return CurrencyRates[Currency.ToString()];
                }
                else
                {
                    throw new Exception("The specified currency(" + Currency.ToString() + ") was not found!");
                }
            }
            catch
            {
                return GetHistoricalExchangeRates(Currency, date.AddDays(-1));
            }
        }

        public static Currency GetTodaysCrossRates(string ToCurrencyCode, string FromCurrencyCode)
        {
            try
            {
                Dictionary<string, Currency> CurrencyRates = GetCurrencyRates("http://www.tcmb.gov.tr/kurlar/today.xml");

                if (!CurrencyRates.Keys.Contains(FromCurrencyCode))
                {
                    throw new Exception("The specified currency(" + FromCurrencyCode + ") was not found!");
                }
                else if (!CurrencyRates.Keys.Contains(ToCurrencyCode))
                {
                    throw new Exception("The specified currency(" + ToCurrencyCode + ") was not found!");
                }
                else
                {
                    Currency MainCurrency = CurrencyRates[FromCurrencyCode];
                    Currency OtherCurrency = CurrencyRates[ToCurrencyCode];

                    return new Currency(
                        OtherCurrency.Name,
                        OtherCurrency.Code,
                        OtherCurrency.Code + "/" + MainCurrency.Code,
                        (OtherCurrency.ForexBuying == 0 || MainCurrency.ForexBuying == 0) ? 0 : Math.Round((OtherCurrency.ForexBuying / MainCurrency.ForexBuying), 4),
                        (OtherCurrency.ForexSelling == 0 || MainCurrency.ForexSelling == 0) ? 0 : Math.Round((OtherCurrency.ForexSelling / MainCurrency.ForexSelling), 4),
                        (OtherCurrency.BanknoteBuying == 0 || MainCurrency.BanknoteBuying == 0) ? 0 : Math.Round((OtherCurrency.BanknoteBuying / MainCurrency.BanknoteBuying), 4),
                        (OtherCurrency.BanknoteSelling == 0 || MainCurrency.BanknoteSelling == 0) ? 0 : Math.Round((OtherCurrency.BanknoteSelling / MainCurrency.BanknoteSelling), 4)
                    );
                }
            }
            catch
            {
                throw new Exception("The date specified may be a weekend or a public holiday!");
            }
        }

        public static double GetTodaysCrossRate(string ToCurrencyCode, string FromCurrencyCode)
        {
            try
            {
                Dictionary<string, Currency> CurrencyRates = GetCurrencyRates("http://www.tcmb.gov.tr/kurlar/today.xml");

                if (!CurrencyRates.Keys.Contains(FromCurrencyCode))
                {
                    throw new Exception("The specified currency(" + FromCurrencyCode + ") was not found!");
                }
                else if (!CurrencyRates.Keys.Contains(ToCurrencyCode))
                {
                    throw new Exception("The specified currency(" + ToCurrencyCode + ") was not found!");
                }
                else
                {
                    Currency MainCurrency = CurrencyRates[FromCurrencyCode];
                    Currency OtherCurrency = CurrencyRates[ToCurrencyCode];

                    return (OtherCurrency.ForexBuying == 0 || MainCurrency.ForexBuying == 0) ? 0 : Math.Round((OtherCurrency.ForexBuying / MainCurrency.ForexBuying), 4);
                }
            }
            catch
            {
                throw new Exception("The date specified may be a weekend or a public holiday!");
            }
        }

        public static Currency GetHistoricalCrossRates(string ToCurrencyCode, string FromCurrencyCode, DateTime date, int counter)
        {
            try
            {
                string SYear = String.Format("{0:0000}", date.Year);
                string SMonth = String.Format("{0:00}", date.Month);
                string SDay = String.Format("{0:00}", date.Day);

                Dictionary<string, Currency> CurrencyRates = GetCurrencyRates("http://www.tcmb.gov.tr/kurlar/" + SYear + SMonth + "/" + SDay + SMonth + SYear + ".xml");

                if (!CurrencyRates.Keys.Contains(FromCurrencyCode))
                {
                    throw new Exception("The specified currency(" + FromCurrencyCode + ") was not found!");
                }
                else if (!CurrencyRates.Keys.Contains(ToCurrencyCode))
                {
                    throw new Exception("The specified currency(" + ToCurrencyCode + ") was not found!");
                }
                else
                {
                    Currency MainCurrency = CurrencyRates[FromCurrencyCode];
                    Currency OtherCurrency = CurrencyRates[ToCurrencyCode];

                    return new Currency(
                        OtherCurrency.Name,
                        OtherCurrency.Code,
                        OtherCurrency.Code + "/" + MainCurrency.Code,
                        (OtherCurrency.ForexBuying == 0 || MainCurrency.ForexBuying == 0) ? 0 : Math.Round((OtherCurrency.ForexBuying / MainCurrency.ForexBuying), 4),
                        (OtherCurrency.ForexBuying == 0 || MainCurrency.ForexBuying == 0) ? 0 : Math.Round((OtherCurrency.ForexBuying / MainCurrency.ForexBuying), 4),
                        (OtherCurrency.ForexBuying == 0 || MainCurrency.ForexBuying == 0) ? 0 : Math.Round((OtherCurrency.ForexBuying / MainCurrency.ForexBuying), 4),
                        (OtherCurrency.ForexBuying == 0 || MainCurrency.ForexBuying == 0) ? 0 : Math.Round((OtherCurrency.ForexBuying / MainCurrency.ForexBuying), 4)
                    );
                }
            }
            catch
            {

                int newCount = counter + 1;
                if (newCount < 15)
                {
                    DateTime newDate = date.AddDays(-1);
                    return GetHistoricalCrossRates(ToCurrencyCode, FromCurrencyCode, newDate, newCount);
                }
                else
                {
                    throw new Exception("The date specified may be a weekend or a public holiday!");
                }
            }
        }

        public static double GetHistoricalCrossRate(string ToCurrencyCode, string FromCurrencyCode, DateTime date)
        {
            try
            {
                string SYear = String.Format("{0:0000}", date.Year);
                string SMonth = String.Format("{0:00}", date.Month);
                string SDay = String.Format("{0:00}", date.Day);

                Dictionary<string, Currency> CurrencyRates = GetCurrencyRates("http://www.tcmb.gov.tr/kurlar/" + SYear + SMonth + "/" + SDay + SMonth + SYear + ".xml");

                if (!CurrencyRates.Keys.Contains(FromCurrencyCode))
                {
                    throw new Exception("The specified currency(" + FromCurrencyCode + ") was not found!");
                }
                else if (!CurrencyRates.Keys.Contains(ToCurrencyCode))
                {
                    throw new Exception("The specified currency(" + ToCurrencyCode + ") was not found!");
                }
                else
                {
                    Currency MainCurrency = CurrencyRates[FromCurrencyCode];
                    Currency OtherCurrency = CurrencyRates[ToCurrencyCode];

                    return (OtherCurrency.ForexBuying == 0 || MainCurrency.ForexBuying == 0) ? 0 : Math.Round((OtherCurrency.ForexBuying / MainCurrency.ForexBuying), 4);
                }
            }
            catch
            {
                throw new Exception("The date specified may be a weekend or a public holiday!");
            }
        }

        public static Currency GetHistoricalCrossRates(string ToCurrencyCode, string FromCurrencyCode, int Year, int Month, int Day)
        {
            try
            {
                string SYear = String.Format("{0:0000}", Year);
                string SMonth = String.Format("{0:00}", Month);
                string SDay = String.Format("{0:00}", Day);

                Dictionary<string, Currency> CurrencyRates = GetCurrencyRates("http://www.tcmb.gov.tr/kurlar/" + SYear + SMonth + "/" + SDay + SMonth + SYear + ".xml");

                if (!CurrencyRates.Keys.Contains(FromCurrencyCode))
                {
                    throw new Exception("The specified currency(" + FromCurrencyCode + ") was not found!");
                }
                else if (!CurrencyRates.Keys.Contains(ToCurrencyCode))
                {
                    throw new Exception("The specified currency(" + ToCurrencyCode + ") was not found!");
                }
                else
                {
                    Currency MainCurrency = CurrencyRates[FromCurrencyCode];
                    Currency OtherCurrency = CurrencyRates[ToCurrencyCode];

                    return new Currency(
                        OtherCurrency.Name,
                        OtherCurrency.Code,
                        OtherCurrency.Code + "/" + MainCurrency.Code,
                        (OtherCurrency.ForexBuying == 0 || MainCurrency.ForexBuying == 0) ? 0 : Math.Round((OtherCurrency.ForexBuying / MainCurrency.ForexBuying), 4),
                        (OtherCurrency.ForexBuying == 0 || MainCurrency.ForexBuying == 0) ? 0 : Math.Round((OtherCurrency.ForexBuying / MainCurrency.ForexBuying), 4),
                        (OtherCurrency.ForexBuying == 0 || MainCurrency.ForexBuying == 0) ? 0 : Math.Round((OtherCurrency.ForexBuying / MainCurrency.ForexBuying), 4),
                        (OtherCurrency.ForexBuying == 0 || MainCurrency.ForexBuying == 0) ? 0 : Math.Round((OtherCurrency.ForexBuying / MainCurrency.ForexBuying), 4)
                    );
                }
            }
            catch
            {
                throw new Exception("The date specified may be a weekend or a public holiday!");
            }
        }

        public static double GetHistoricalCrossRate(string ToCurrencyCode, string FromCurrencyCode, int Year, int Month, int Day)
        {
            try
            {
                string SYear = String.Format("{0:0000}", Year);
                string SMonth = String.Format("{0:00}", Month);
                string SDay = String.Format("{0:00}", Day);

                Dictionary<string, Currency> CurrencyRates = GetCurrencyRates("http://www.tcmb.gov.tr/kurlar/" + SYear + SMonth + "/" + SDay + SMonth + SYear + ".xml");

                if (!CurrencyRates.Keys.Contains(FromCurrencyCode))
                {
                    throw new Exception("The specified currency(" + FromCurrencyCode + ") was not found!");
                }
                else if (!CurrencyRates.Keys.Contains(ToCurrencyCode))
                {
                    throw new Exception("The specified currency(" + ToCurrencyCode + ") was not found!");
                }
                else
                {
                    Currency MainCurrency = CurrencyRates[FromCurrencyCode];
                    Currency OtherCurrency = CurrencyRates[ToCurrencyCode];

                    return (OtherCurrency.ForexBuying == 0 || MainCurrency.ForexBuying == 0) ? 0 : Math.Round((OtherCurrency.ForexBuying / MainCurrency.ForexBuying), 4);
                }
            }
            catch
            {
                throw new Exception("The date specified may be a weekend or a public holiday!");
            }
        }

        public static double CalculateTodaysExchange(double Amount, string FromCurrencyCode, string ToCurrencyCode)
        {
            try
            {
                Dictionary<string, Currency> CurrencyRates = GetCurrencyRates("http://www.tcmb.gov.tr/kurlar/today.xml");

                if (!CurrencyRates.Keys.Contains(FromCurrencyCode))
                {
                    throw new Exception("The specified currency(" + FromCurrencyCode + ") was not found!");
                }
                else if (!CurrencyRates.Keys.Contains(ToCurrencyCode))
                {
                    throw new Exception("The specified currency(" + ToCurrencyCode + ") was not found!");
                }
                else
                {
                    Currency MainCurrency = CurrencyRates[FromCurrencyCode];
                    Currency OtherCurrency = CurrencyRates[ToCurrencyCode];

                    return (OtherCurrency.ForexBuying == 0 || MainCurrency.ForexBuying == 0) ? 0 : Math.Round(Amount * (MainCurrency.ForexBuying / OtherCurrency.ForexBuying), 4);
                }
            }
            catch
            {
                throw new Exception("The date specified may be a weekend or a public holiday!");
            }
        }

        public static double CalculateTodaysExchange(double Amount, string FromCurrencyCode, string ToCurrencyCode, ExchangeType exchangeType)
        {
            try
            {
                Dictionary<string, Currency> CurrencyRates = GetCurrencyRates("http://www.tcmb.gov.tr/kurlar/today.xml");

                if (!CurrencyRates.Keys.Contains(FromCurrencyCode))
                {
                    throw new Exception("The specified currency(" + FromCurrencyCode + ") was not found!");
                }
                else if (!CurrencyRates.Keys.Contains(ToCurrencyCode))
                {
                    throw new Exception("The specified currency(" + ToCurrencyCode + ") was not found!");
                }
                else
                {
                    Currency MainCurrency = CurrencyRates[FromCurrencyCode];
                    Currency OtherCurrency = CurrencyRates[ToCurrencyCode];

                    switch (exchangeType)
                    {
                        case ExchangeType.ForexBuying:
                            return (OtherCurrency.ForexBuying == 0 || MainCurrency.ForexBuying == 0) ? 0 : Math.Round(Amount * (MainCurrency.ForexBuying / OtherCurrency.ForexBuying), 4);
                        case ExchangeType.ForexSelling:
                            return (OtherCurrency.ForexSelling == 0 || MainCurrency.ForexSelling == 0) ? 0 : Math.Round(Amount * (MainCurrency.ForexSelling / OtherCurrency.ForexSelling), 4);
                        case ExchangeType.BanknoteBuying:
                            return (OtherCurrency.BanknoteBuying == 0 || MainCurrency.BanknoteBuying == 0) ? 0 : Math.Round(Amount * (MainCurrency.BanknoteBuying / OtherCurrency.BanknoteBuying), 4);
                        case ExchangeType.BanknoteSelling:
                            return (OtherCurrency.BanknoteSelling == 0 || MainCurrency.BanknoteSelling == 0) ? 0 : Math.Round(Amount * (MainCurrency.BanknoteSelling / OtherCurrency.BanknoteSelling), 4);
                        default:
                            return 0;
                    }
                }
            }
            catch
            {
                throw new Exception("The date specified may be a weekend or a public holiday!");
            }
        }

        public static double CalculateHistoricalExchange(double Amount, string FromCurrencyCode, string ToCurrencyCode, DateTime date)
        {
            try
            {
                string SYear = String.Format("{0:0000}", date.Year);
                string SMonth = String.Format("{0:00}", date.Month);
                string SDay = String.Format("{0:00}", date.Day);

                Dictionary<string, Currency> CurrencyRates = GetCurrencyRates("http://www.tcmb.gov.tr/kurlar/" + SYear + SMonth + "/" + SDay + SMonth + SYear + ".xml");

                if (!CurrencyRates.Keys.Contains(FromCurrencyCode))
                {
                    throw new Exception("The specified currency(" + FromCurrencyCode + ") was not found!");
                }
                else if (!CurrencyRates.Keys.Contains(ToCurrencyCode))
                {
                    throw new Exception("The specified currency(" + ToCurrencyCode + ") was not found!");
                }
                else
                {
                    Currency MainCurrency = CurrencyRates[FromCurrencyCode];
                    Currency OtherCurrency = CurrencyRates[ToCurrencyCode];

                    return (OtherCurrency.ForexBuying == 0 || MainCurrency.ForexBuying == 0) ? 0 : Math.Round(Amount * (MainCurrency.ForexBuying / OtherCurrency.ForexBuying), 4);
                }
            }
            catch
            {
                throw new Exception("The date specified may be a weekend or a public holiday!");
            }
        }

        public static double CalculateHistoricalExchange(double Amount, string FromCurrencyCode, string ToCurrencyCode, ExchangeType exchangeType, DateTime date)
        {
            try
            {
                string SYear = String.Format("{0:0000}", date.Year);
                string SMonth = String.Format("{0:00}", date.Month);
                string SDay = String.Format("{0:00}", date.Day);

                Dictionary<string, Currency> CurrencyRates = GetCurrencyRates("http://www.tcmb.gov.tr/kurlar/" + SYear + SMonth + "/" + SDay + SMonth + SYear + ".xml");

                if (!CurrencyRates.Keys.Contains(FromCurrencyCode))
                {
                    throw new Exception("The specified currency(" + FromCurrencyCode + ") was not found!");
                }
                else if (!CurrencyRates.Keys.Contains(ToCurrencyCode))
                {
                    throw new Exception("The specified currency(" + ToCurrencyCode + ") was not found!");
                }
                else
                {
                    Currency MainCurrency = CurrencyRates[FromCurrencyCode];
                    Currency OtherCurrency = CurrencyRates[ToCurrencyCode];

                    switch (exchangeType)
                    {
                        case ExchangeType.ForexBuying:
                            return (OtherCurrency.ForexBuying == 0 || MainCurrency.ForexBuying == 0) ? 0 : Math.Round(Amount * (MainCurrency.ForexBuying / OtherCurrency.ForexBuying), 4);
                        case ExchangeType.ForexSelling:
                            return (OtherCurrency.ForexSelling == 0 || MainCurrency.ForexSelling == 0) ? 0 : Math.Round(Amount * (MainCurrency.ForexSelling / OtherCurrency.ForexSelling), 4);
                        case ExchangeType.BanknoteBuying:
                            return (OtherCurrency.BanknoteBuying == 0 || MainCurrency.BanknoteBuying == 0) ? 0 : Math.Round(Amount * (MainCurrency.BanknoteBuying / OtherCurrency.BanknoteBuying), 4);
                        case ExchangeType.BanknoteSelling:
                            return (OtherCurrency.BanknoteSelling == 0 || MainCurrency.BanknoteSelling == 0) ? 0 : Math.Round(Amount * (MainCurrency.BanknoteSelling / OtherCurrency.BanknoteSelling), 4);
                        default:
                            return 0;
                    }
                }
            }
            catch
            {
                throw new Exception("The date specified may be a weekend or a public holiday!");
            }
        }

        public static double CalculateHistoricalExchange(double Amount, string FromCurrencyCode, string ToCurrencyCode, int Year, int Month, int Day)
        {
            try
            {
                string SYear = String.Format("{0:0000}", Year);
                string SMonth = String.Format("{0:00}", Month);
                string SDay = String.Format("{0:00}", Day);

                Dictionary<string, Currency> CurrencyRates = GetCurrencyRates("http://www.tcmb.gov.tr/kurlar/" + SYear + SMonth + "/" + SDay + SMonth + SYear + ".xml");

                if (!CurrencyRates.Keys.Contains(FromCurrencyCode))
                {
                    throw new Exception("The specified currency(" + FromCurrencyCode + ") was not found!");
                }
                else if (!CurrencyRates.Keys.Contains(ToCurrencyCode))
                {
                    throw new Exception("The specified currency(" + ToCurrencyCode + ") was not found!");
                }
                else
                {
                    Currency MainCurrency = CurrencyRates[FromCurrencyCode];
                    Currency OtherCurrency = CurrencyRates[ToCurrencyCode];

                    return (OtherCurrency.ForexBuying == 0 || MainCurrency.ForexBuying == 0) ? 0 : Math.Round(Amount * (MainCurrency.ForexBuying / OtherCurrency.ForexBuying), 4);
                }
            }
            catch
            {
                throw new Exception("The date specified may be a weekend or a public holiday!");
            }
        }

        public static double CalculateHistoricalExchange(double Amount, string FromCurrencyCode, string ToCurrencyCode, ExchangeType exchangeType, int Year, int Month, int Day)
        {
            try
            {
                string SYear = String.Format("{0:0000}", Year);
                string SMonth = String.Format("{0:00}", Month);
                string SDay = String.Format("{0:00}", Day);

                Dictionary<string, Currency> CurrencyRates = GetCurrencyRates("http://www.tcmb.gov.tr/kurlar/" + SYear + SMonth + "/" + SDay + SMonth + SYear + ".xml");

                if (!CurrencyRates.Keys.Contains(FromCurrencyCode))
                {
                    throw new Exception("The specified currency(" + FromCurrencyCode + ") was not found!");
                }
                else if (!CurrencyRates.Keys.Contains(ToCurrencyCode))
                {
                    throw new Exception("The specified currency(" + ToCurrencyCode + ") was not found!");
                }
                else
                {
                    Currency MainCurrency = CurrencyRates[FromCurrencyCode];
                    Currency OtherCurrency = CurrencyRates[ToCurrencyCode];

                    switch (exchangeType)
                    {
                        case ExchangeType.ForexBuying:
                            return (OtherCurrency.ForexBuying == 0 || MainCurrency.ForexBuying == 0) ? 0 : Math.Round(Amount * (MainCurrency.ForexBuying / OtherCurrency.ForexBuying), 4);
                        case ExchangeType.ForexSelling:
                            return (OtherCurrency.ForexSelling == 0 || MainCurrency.ForexSelling == 0) ? 0 : Math.Round(Amount * (MainCurrency.ForexSelling / OtherCurrency.ForexSelling), 4);
                        case ExchangeType.BanknoteBuying:
                            return (OtherCurrency.BanknoteBuying == 0 || MainCurrency.BanknoteBuying == 0) ? 0 : Math.Round(Amount * (MainCurrency.BanknoteBuying / OtherCurrency.BanknoteBuying), 4);
                        case ExchangeType.BanknoteSelling:
                            return (OtherCurrency.BanknoteSelling == 0 || MainCurrency.BanknoteSelling == 0) ? 0 : Math.Round(Amount * (MainCurrency.BanknoteSelling / OtherCurrency.BanknoteSelling), 4);
                        default:
                            return 0;
                    }
                }
            }
            catch
            {
                throw new Exception("The date specified may be a weekend or a public holiday!");
            }
        }

        private static Dictionary<string, Currency> GetCurrencyRates(string Link)
        {
            try
            {
                XmlTextReader rdr = new XmlTextReader(Link);
                XmlDocument myxml = new XmlDocument();
                myxml.Load(rdr);
                XmlNode tarih = myxml.SelectSingleNode("/Tarih_Date/@Tarih");
                XmlNodeList mylist = myxml.SelectNodes("/Tarih_Date/Currency");
                XmlNodeList adi = myxml.SelectNodes("/Tarih_Date/Currency/Isim");
                XmlNodeList kod = myxml.SelectNodes("/Tarih_Date/Currency/@Kod");
                XmlNodeList doviz_alis = myxml.SelectNodes("/Tarih_Date/Currency/ForexBuying");
                XmlNodeList doviz_satis = myxml.SelectNodes("/Tarih_Date/Currency/ForexSelling");
                XmlNodeList efektif_alis = myxml.SelectNodes("/Tarih_Date/Currency/BanknoteBuying");
                XmlNodeList efektif_satis = myxml.SelectNodes("/Tarih_Date/Currency/BanknoteSelling");

                Dictionary<string, Currency> ExchangeRates = new Dictionary<string, Currency>();

                ExchangeRates.Add("TRY", new Currency("Türk Lirası", "TRY", "TRY/TRY", 1, 1, 1, 1));

                for (int i = 0; i < adi.Count; i++)
                {
                    Currency cur = new Currency(adi.Item(i).InnerText.ToString(),
                        kod.Item(i).InnerText.ToString(),
                        kod.Item(i).InnerText.ToString() + "/TRY",
                        (String.IsNullOrWhiteSpace(doviz_alis.Item(i).InnerText.ToString())) ? 0 : Convert.ToDouble(doviz_alis.Item(i).InnerText.ToString().Replace(".", ",")),
                        (String.IsNullOrWhiteSpace(doviz_satis.Item(i).InnerText.ToString())) ? 0 : Convert.ToDouble(doviz_satis.Item(i).InnerText.ToString().Replace(".", ",")),
                        (String.IsNullOrWhiteSpace(efektif_alis.Item(i).InnerText.ToString())) ? 0 : Convert.ToDouble(efektif_alis.Item(i).InnerText.ToString().Replace(".", ",")),
                        (String.IsNullOrWhiteSpace(efektif_satis.Item(i).InnerText.ToString())) ? 0 : Convert.ToDouble(efektif_satis.Item(i).InnerText.ToString().Replace(".", ","))
                        );

                    ExchangeRates.Add(kod.Item(i).InnerText.ToString(), cur);
                }

                return ExchangeRates;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Currency GetExchangeRatesByDate(string Currency, DateTime date)
        {
            var currency = Currency == "TL" ? "TRY" : Currency;

            if (date.Date == DateTime.Now.Date)
            {
                return GetTodaysExchangeRates(currency);
            }
            else
            {
                return GetHistoricalExchangeRates(currency, date);
            }
        }

        public static Currency GetCrossExchangeRatesByDate(string ToCurrencyCode, string FromCurrencyCode, DateTime date)
        {
            var from = FromCurrencyCode == "TL" ? "TRY" : FromCurrencyCode;
            var to = ToCurrencyCode == "TL" ? "TRY" : ToCurrencyCode;

            if (date.Date == DateTime.Now.Date)
            {
                return GetTodaysCrossRates(to, from);
            }
            else
            {
                return GetHistoricalCrossRates(to, from, date,0);
            }
        }
    }
}
