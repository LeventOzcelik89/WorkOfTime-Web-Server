using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infoline.Framework.Database;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Data.Common;
using Infoline.WorkOfTime.BusinessData;

namespace Infoline.WorkOfTime.BusinessAccess
{
    partial class WorkOfTimeDatabase
    {
        public VWHDM_Suggestion[] GetVWHDM_SuggestionByText(string text, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                var str = "Select * from VWHDM_Suggestion with(nolock) where 1=1 and (";

                var textArray = text.Split(' ').Select(a => a.Trim()).ToArray();

                for (int i = 0; i < textArray.Length; i++)
                {
                    var t = textArray[i];
                    if (!String.IsNullOrEmpty(t))
                    {
                        if (i != 0)
                        {
                            str += " or ";
                        }

                        str += " (title like '%" + t + "%' or content like '%" + t + "%') ";
                    }
                }

                str += ")";

                return db.ExecuteReader<VWHDM_Suggestion>(str).ToArray();
            }
        }

        public SpesificSuggestModal[] GetVWHDM_SuggestionByTextJustTitleAndContent(string text, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                var str = "Select title,content from VWHDM_Suggestion with(nolock) where 1=1 and (";

                var textArray = text.Split(' ').Select(a => a.Trim()).ToArray();

                for (int i = 0; i < textArray.Length; i++)
                {
                    var t = textArray[i];
                    if (!String.IsNullOrEmpty(t))
                    {
                        if (i != 0)
                        {
                            str += " or ";
                        }

                        str += " (title like '%" + t + "%' or content like '%" + t + "%') ";
                    }
                }

                str += ")";

                return db.ExecuteReader<SpesificSuggestModal>(str).ToArray();
            }
        }

    }
}
