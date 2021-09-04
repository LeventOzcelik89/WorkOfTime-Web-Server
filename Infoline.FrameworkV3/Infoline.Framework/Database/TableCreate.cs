using Infoline.Framework.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Infoline.Framework.Database
{
    public class TableCreate<T> : ITableCreate<T>
    {
        List<string> _queries;
        List<string> _primaryKeyColumns;
        List<IndexDefinition> _indexes;
        List<DefaultValueDefinition> _defaultValues;
        AutoIncrementDefiniton _autoIncrement;
        string _name;
        Database _database;

        public TableCreate(Database database, string name = null)
        {
            _queries = new List<string>();
            _primaryKeyColumns = new List<string>();
            _indexes = new List<IndexDefinition>();
            _defaultValues = new List<DefaultValueDefinition>();
            _name = name ?? typeof(T).Name;
            _database = database;
        }


        public ITableCreate<T> SetPrimaryKey(Expression<Func<T, object>> columns)
        {
            _primaryKeyColumns = ExpressionHelper.GetPropertyNames<T, object>(columns).ToList();

            var columnsString = string.Join(",", _primaryKeyColumns.Select(a => string.Format("[{0}] ASC", a)).ToArray());
            var query = string.Format("ALTER TABLE [{0}] ADD CONSTRAINT [PK_{0}] PRIMARY KEY ( {1} )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]", _name, columnsString);
            _queries.Add(query);
            return this;
        }

        public ITableCreate<T> SetDefaultValue(Expression<Func<T, object>> column, SqlServerFunctions value)
        {
            var val = string.Format("{0}()", value.ToString());
            return SetDefaultValue(column, val);
        }

        public ITableCreate<T> SetDefaultValue(Expression<Func<T, object>> column, string value)
        {
            var col = ExpressionHelper.GetPropertyNames<T, object>(column).FirstOrDefault();

            _defaultValues.Add(new DefaultValueDefinition { Column = col, Value = value });
            var query = string.Format("ALTER TABLE [dbo].[{0}] ADD  CONSTRAINT [DF_{0}_{1}]  DEFAULT ({2}) FOR {1}", _name, col, value);
            _queries.Add(query);
            return this;
        }

        public ITableCreate<T> SetAsAutoIncermentColumn(Expression<Func<T, int>> column, int start = 1, int increment = 1)
        {
            var col = ExpressionHelper.GetPropertyNames<T, int>(column).FirstOrDefault();

            _autoIncrement = new AutoIncrementDefiniton { Column = col, Start = start, Increment = increment };
            return this;
        }

        public ITableCreate<T> SetAsAutoIncermentColumn(Expression<Func<T, long>> column, int start = 1, int increment = 1)
        {
            var col = ExpressionHelper.GetPropertyNames<T, long>(column).FirstOrDefault();
            _autoIncrement = new AutoIncrementDefiniton { Column = col, Start = start, Increment = increment };
            return this;
        }

        public ITableCreate<T> CreateIndex(Expression<Func<T, object>> columns, bool isUnique = false)
        {
            var cols = ExpressionHelper.GetPropertyNames<T, object>(columns).ToArray();

            IndexDefinition index = new IndexDefinition();
            index.Name = string.Format("IDX_{0}", string.Join("_", cols));
            index.IsUnique = isUnique;
            index.Columns = cols;
            _indexes.Add(index);

            var columText = string.Join(",", index.Columns);
            var query = string.Format("CREATE {3} INDEX {0} ON {1} ({2})", index.Name, _name, columText, isUnique ? "UNIQUE" : "");
            _queries.Add(query);
            return this;
        }

        public ResultStatus Execute()
        {
            var tableExits = _database.ExecuteScaler<bool>("SELECT cast(count(*) as bit) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = {0}", _name);
            if (tableExits) return new ResultStatus { result = false, message = "Tablo zaten var." };

            var t = typeof(T);
            var columns = t.GetProperties().Select(a => string.Format(" [{0}] {1} {2}", a.Name, SQLTypeMapper.GetSQLType(a.PropertyType), a.Name == _autoIncrement.Column ? "IDENTITY(1,1) NOT NULL " : (_primaryKeyColumns.Contains(a.Name) ? "NOT NULL" : "NULL")));
            var pk = string.Join(",", _primaryKeyColumns.Select(a => string.Format("[{0}]", a)));
            string query = string.Format(@"CREATE TABLE [{1}]( {0} ) ON [PRIMARY]", string.Join(",", columns), _name, pk);

            _database.ExecuteNonQuery(query);
            foreach (var que in _queries)
                _database.ExecuteNonQuery(que);

            return new ResultStatus { result = true };
        }


        #region Classes

        class IndexDefinition
        {
            public string Name { get; set; }
            public bool IsUnique { get; set; }
            public string[] Columns { get; set; }
        }
        class DefaultValueDefinition
        {
            public string Column { get; set; }
            public string Value { get; set; }
        }
        class AutoIncrementDefiniton
        {
            public string Column { get; set; }
            public int Start { get; set; }
            public int Increment { get; set; }
        }

        #endregion
    }
}
