using System;

namespace Infoline.Framework.Database
{
    public class QueryBuildException : Exception
    {
        public QueryBuildException(ExceptionTypes type)
            : base(GetMessage(type))
        {

        }

        private static string GetMessage(ExceptionTypes type)
        {
            switch (type)
            {
                case ExceptionTypes.OperatorNotFound: return "Fonksiyon bulunamadı.";
                case ExceptionTypes.OperatorUnsuported: return "Fonksiyon desteklenmiyor.";
                case ExceptionTypes.ParameterCountIsWrong: return "Parametre sayısı hatalı.";
                default: return "";
            }
        }

        public enum ExceptionTypes
        {
            OperatorNotFound,
            OperatorUnsuported,
            ParameterCountIsWrong,
        }
    }
}
