using Infoline.Framework.Helper;
using System.Collections.Generic;

namespace Infoline.Framework.DatabaseE
{
    public class TableAlter : ITableAlter
    {
        string _name;
        Database _database;
        List<string> _queries;

        public TableAlter(Database database, string name)
        {
            _database = database;
            _name = name;
            _queries = new List<string>();
        }


        public ITableAlter ChangeName(string newTableName)
        {
            _queries.Add(string.Format("exec sp_Rename '{0}', '{1}'", _name, newTableName));
            _name = newTableName;
            return this;
        }

        public ITableAlter ChangeColumnName(string columnName, string newColumnName)
        {
            _queries.Add(string.Format("ALTER TABLE [{0}] ADD [{1}] {2} {3}", _name, columnName));
            return this;
        }

        public ITableAlter AddColumn<T>(string columnName, bool isNullable = true)
        {
            var type = SQLTypeMapper.GetSQLType(typeof(T));
            var nul = isNullable ? "NULL" : "NOT NULL";
            _queries.Add(string.Format("ALTER TABLE [{0}] ADD [{1}] {2} {3}", _name, columnName, type, nul));
            return this;
        }

        public ITableAlter DropColumn(string columnName)
        {

            _queries.Add(string.Format("ALTER TABLE [{0}] DROP COLUMN [{1}]", _name, columnName));
            return this;
        }

        public ResultStatus Execute()
        {
            foreach (var que in _queries)
                _database.ExecuteNonQuery(que);

            return new ResultStatus { result = true };
        }
    }
}
