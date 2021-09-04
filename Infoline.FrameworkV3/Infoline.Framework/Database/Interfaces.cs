using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infoline.Framework.Database
{
    public enum SqlServerFunctions
    {
        NEWID,
        GETDATE
    }

    public interface IExecutable
    {
        ResultStatus Execute();
    }

    public interface ITableCreate<T> : IExecutable
    {
        ITableCreate<T> SetPrimaryKey(Expression<Func<T, object>> columns);
        ITableCreate<T> SetDefaultValue(Expression<Func<T, object>> column, SqlServerFunctions value);
        ITableCreate<T> SetDefaultValue(Expression<Func<T, object>> column, string value);
        ITableCreate<T> SetAsAutoIncermentColumn(Expression<Func<T, int>> column, int start = 1, int increment = 1);
        ITableCreate<T> SetAsAutoIncermentColumn(Expression<Func<T, long>> column, int start = 1, int increment = 1);
        ITableCreate<T> CreateIndex(Expression<Func<T, object>> columns, bool isUnique = false);
    }

    public interface ITableAlter : IExecutable
    {
        ITableAlter ChangeName(string newTableName);
        ITableAlter ChangeColumnName(string columnName, string newColumnName);
        ITableAlter AddColumn<T>(string columnName, bool isNullable = true);
        ITableAlter DropColumn(string columnName);
    }

    interface IQueryBuilder
    {
        Query GetInsertQuery<T>(T parametre, Expression<Func<T, object>> exceptexp, string tableName = null);
        Query GetUpdateQuery<T>(T parametre, Expression<Func<T, object>> idexp, Expression<Func<T, object>> exceptexp = null, bool setNull = false, string tableName = null);
        Query GetDeleteQuery<T>(T parametre, Expression<Func<T, object>> idexp, string tableName = null);
        Query ConvertToQuery(string txt, params object[] parameters);
    }



}
