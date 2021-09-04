using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessData;

namespace Infoline.WorkOfTime.BusinessAccess
{
	public class VWUT_SectorModel : VWUT_Sector
    {
        public static SimpleQuery UpdateQuery(SimpleQuery query, PageSecurity userStatus)
        {
            BEXP filter = null;

            filter |= new BEXP
            {
                Operand1 = new BEXP
                {
                    Operand1 = (COL)"generation",
                    Operator = BinaryOperator.NotEqual,
                    Operand2 = (VAL)0
                },
                Operator = BinaryOperator.And,
                Operand2 = new BEXP
                {
                    Operand1 = (COL)"generation",
                    Operator = BinaryOperator.NotEqual,
                    Operand2 = (VAL)(1)
                }
            };

            query.Filter &= filter;

            return query;

        }
    }
}
