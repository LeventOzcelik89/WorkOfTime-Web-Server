using System;

namespace Infoline.Framework.Database
{
    public class QueryExecuteException : Exception
    {
        public QueryExecuteException(ExceptionTypes type)
            : base(GetMessage(type))
        {

        }

        public QueryExecuteException(ExceptionTypes type, string message)
            : base(GetMessage(type) + "\r\n\r\n" + message)
        {

        }

        private static string GetMessage(ExceptionTypes type)
        {
            switch (type)
            {
                case ExceptionTypes.TableAlreadyExists: return "Tablo zaten var.";
                default: return "";
            }
        }

        public enum ExceptionTypes
        {
            TableAlreadyExists = 1,
            TableCreateException = 2,
        }
    }
}
