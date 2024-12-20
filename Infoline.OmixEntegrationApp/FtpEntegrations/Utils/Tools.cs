﻿using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Infoline.OmixEntegrationApp.FtpEntegrations.Utils
{
    public class Tools
    {
        public static bool IsDir(string value)
        {

            return value.Contains("<DIR>");

        }
        public static DateTime GetDate(string value)
        {
            var date = value.Substring(0, 17);
            return DateTime.Parse(date, CultureInfo.InvariantCulture);
        }

        public static string GetItemName(string line)
        {
            return line.Substring(39);
        }
        public static DateTime GetDateFromFileName(string fileName, string dateTimeFormat)
        {
            if (fileName.Contains("SELLIN"))
            {
                fileName = fileName.Substring(7);
                fileName = fileName.Split('.')[0];
                return DateTime.ParseExact(fileName, dateTimeFormat, CultureInfo.InvariantCulture);
            }
            else if(fileName.Contains("SELLTHR"))
            {

                fileName = fileName.Substring(8);
                fileName = fileName.Split('.')[0];
                return DateTime.ParseExact(fileName, dateTimeFormat, CultureInfo.InvariantCulture);
            }
            return new DateTime(1999,1,1);


        }
        public static DateTime GetDateFromFileNameForMobiltel(string fileName, string dateTimeFormat)
        {
            if (fileName.Contains("SELLIN"))
            {
                fileName = fileName.Substring(46);
                fileName = fileName.Split('.')[0];
                return DateTime.ParseExact(fileName, dateTimeFormat, CultureInfo.InvariantCulture);
            }
            else if (fileName.Contains("SELLTHR"))
            {

                fileName = fileName.Substring(8);
                fileName = fileName.Split('.')[0];
                return DateTime.ParseExact(fileName, dateTimeFormat, CultureInfo.InvariantCulture);
            }
            return new DateTime(1999, 1, 1);


        }
        public static DateTime GetDateFromFileNameForGenpa(string fileName, string dateTimeFormat)
        {
            if (fileName.Contains("SELLIN"))
            {
                fileName = fileName.Substring(7);
                fileName = fileName.Substring(0, fileName.Length - 13);
                fileName = fileName.Split('.')[0];
                return DateTime.ParseExact(fileName, dateTimeFormat, CultureInfo.InvariantCulture);
            }
            else
            {
                fileName = fileName.Substring(8);
                fileName = fileName.Substring(0, fileName.Length - 13);
                fileName = fileName.Split('.')[0];

                return DateTime.ParseExact(fileName, dateTimeFormat, CultureInfo.InvariantCulture);
            }
        }
        public static Guid AddStorage(PRD_EntegrationAction item)
        {
            var db = new WorkOfTimeDatabase();
            var id = Guid.NewGuid();
            db.InsertCMP_Company(new CMP_Company
            {
                id = id,
                code = item.CustomerOperatorCode,
                created = DateTime.Now,
                createdby = Guid.Empty,
                name = item.CustomerOperatorName,
                description = "Otomatik Oluşturulmuştur",
                taxNumber = item.TaxNumber,
                pid = item.DistributorId
            });
            return id;
        }
    }
}
