using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.ComponentModel;
using System.Linq;

namespace Infoline.WorkOfTime.BusinessAccess
{

    public partial class WorkOfTimeDatabase
    {

        public VWHR_Person[] GetData(Guid[] position, Guid[] keywords, int? Education, Guid? City, int? Military, int? Exprience, int? Result)
        {
            using (var db = GetDB())
            {
                var str = "Select * from VWHR_Person with(nolock) where 1=1 ";
                if (position != null)
                {
                    str += " and id in(Select HrPersonId from HR_PersonPosition with(nolock) where HrPositionId in(" + string.Format("'{0}'", string.Join("','", position)) + ")) ";
                }

                if (keywords != null)
                {
                    str += " and id in(SELECT HrPersonId from HR_PersonKeywords with(nolock) where HrKeywordsId in(" + string.Format("'{0}'", string.Join("','", keywords)) + ")) ";
                }

                if (Education != null)
                {
                    str += " and Education >=" + Education;
                }

                if (City != null)
                {
                    str += " and LocationId ='" + City + "'";
                }

                if (Military != null)
                {
                    str += " and MilitaryStatus =" + Military;
                }

                if (Exprience != null)
                {
                    str += " and ExprienceYear >=" + Exprience;
                }

                if (Result != null)
                {
                    if (Result == 4)
                    {
                        str += " and (Result =" + Result + "or Result is null)";
                    }
                    else
                    {
                        str += " and Result =" + Result;
                    }
                }

                return db.ExecuteReader<VWHR_Person>(str).ToArray();
            }
        }

    }
}
