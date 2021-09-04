using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infoline.Framework.Database
{


    #region TableCreate
    public interface ITableCreate
    {
        ResultStatus Execute();
        ITableCreate Column(TableColumn column);
        ITableCreate SetPrimaryKey(params string[] column);
        ITableCreate CreateIndex(string[] columns, bool isUnique = false);
    }

    public class TableCreate : ITableCreate
    {
        private TableInfo _tableInfo;
        private ITableCreator _creator;

        public TableCreate(string tableName, string schemaName, ITableCreator creator)
        {
            _tableInfo = new TableInfo();
            _tableInfo.TableName = tableName;
            _tableInfo.SchemaName = schemaName;
            _creator = creator;
        }
        public ITableCreate Column(TableColumn column)
        {
            _tableInfo.Columns.Add(column);
            return this;
        }
        public ITableCreate SetPrimaryKey(params string[] column)
        {
            _tableInfo.PrimaryKey.AddRange(column);
            return this;
        }
        public ITableCreate CreateIndex(string[] columns, bool isUnique = false)
        {
            _tableInfo.Indexes.Add(new TableIndex { Columns = columns, IsUnique = isUnique });
            return this;
        }
        public ResultStatus Execute()
        {
            return _creator.Create(_tableInfo);
        }
    }

    public class TableAlter : ITableCreate
    {
        private TableInfo _tableInfo;
        private ITableCreator _creator;

        public TableAlter(string tableName, string schemaName, ITableCreator creator)
        {
            _tableInfo = new TableInfo();
            _tableInfo.TableName = tableName;
            _tableInfo.SchemaName = schemaName;
            _creator = creator;
        }
        public ITableCreate Column(TableColumn column)
        {
            _tableInfo.Columns.Add(column);
            return this;
        }
        public ITableCreate SetPrimaryKey(params string[] column)
        {
            _tableInfo.PrimaryKey.AddRange(column);
            return this;
        }
        public ITableCreate CreateIndex(string[] columns, bool isUnique = false)
        {
            _tableInfo.Indexes.Add(new TableIndex { Columns = columns, IsUnique = isUnique });
            return this;
        }
        public ResultStatus Execute()
        {
            return _creator.Alter(_tableInfo);
        }
    }

    public interface ITableCreate<T>
    {
        ResultStatus Execute();
        ITableCreate<T> SetPrimaryKey(Expression<Func<T, object>> columns);
        ITableCreate<T> SetDefaultValue(Expression<Func<T, object>> column, SqlFunctions value);
        ITableCreate<T> SetDefaultValue(Expression<Func<T, object>> column, string value);
        ITableCreate<T> SetAsAutoIncermentColumn(Expression<Func<T, int>> column, int start = 1, int increment = 1);
        ITableCreate<T> SetAsAutoIncermentColumn(Expression<Func<T, long>> column, int start = 1, int increment = 1);
        ITableCreate<T> CreateIndex(Expression<Func<T, object>> columns, bool isUnique = false);
    }

    public class TableCreate<T> : ITableCreate<T>
    {
        TableInfo _tableInfo;
        ITableCreator _creator; 

        public TableCreate(string tableName, string schemaName, ITableCreator creator)
        {
            _tableInfo = new TableInfo();
            _tableInfo.TableName = tableName;
            _tableInfo.SchemaName = schemaName;
            _creator = creator;
            foreach (var property in typeof(T).GetProperties())
            {
                _tableInfo.Columns.Add(new TableColumn 
                { 
                    ColumnName = property.Name, 
                    Type = property.PropertyType 
                });
            }
        }

        public ITableCreate<T> SetPrimaryKey(Expression<Func<T, object>> columns)
        {
            var columnNames = ExpressionHelper.GetPropertyNames<T, object>(columns);
            _tableInfo.PrimaryKey = columnNames.ToList();
            return this;
        }
        public ITableCreate<T> SetDefaultValue(Expression<Func<T, object>> column, SqlFunctions value)
        {
            var columnName = ExpressionHelper.GetPropertyName<T, object>(column);
            var col = _tableInfo.Columns.Where(a => a.ColumnName == columnName).FirstOrDefault();
            col.Default = value;
            return this;
        }
        public ITableCreate<T> SetDefaultValue(Expression<Func<T, object>> column, string value)
        {
            var columnName = ExpressionHelper.GetPropertyName<T, object>(column);
            var col = _tableInfo.Columns.Where(a => a.ColumnName == columnName).FirstOrDefault();
            col.Default = value;
            return this;
        }
        public ITableCreate<T> SetAsAutoIncermentColumn(Expression<Func<T, int>> column, int start = 1, int increment = 1)
        {
            var columnName = ExpressionHelper.GetPropertyName<T, int>(column);
            var col = _tableInfo.Columns.Where(a => a.ColumnName == columnName).FirstOrDefault();
            col.AutoIncrement = new AutoIncrement { Start = start, Increment = increment };
            return this;
        }
        public ITableCreate<T> SetAsAutoIncermentColumn(Expression<Func<T, long>> column, int start = 1, int increment = 1)
        {
            var columnName = ExpressionHelper.GetPropertyName<T, long>(column);
            var col = _tableInfo.Columns.Where(a => a.ColumnName == columnName).FirstOrDefault();
            col.AutoIncrement = new AutoIncrement { Start = start, Increment = increment };
            return this;
        }
        public ITableCreate<T> CreateIndex(Expression<Func<T, object>> columns, bool isUnique = false)
        {
            var columnNames = ExpressionHelper.GetPropertyNames<T, object>(columns);
            _tableInfo.Indexes.Add(new TableIndex { Columns = columnNames.ToArray(), IsUnique = isUnique });
            return this;
        }
        public ResultStatus Execute()
        {
            return _creator.Create(_tableInfo);
        }
    }
    public class TableAlter<T> : ITableCreate<T>
    {
        TableInfo _tableInfo;
        ITableCreator _creator;

        public TableAlter(string tableName, string schemaName, ITableCreator creator)
        {
            _tableInfo = new TableInfo();
            _tableInfo.TableName = tableName;
            _tableInfo.SchemaName = schemaName;
            _creator = creator;
            foreach (var property in typeof(T).GetProperties())
                _tableInfo.Columns.Add(new TableColumn
                {
                    ColumnName = property.Name,
                    Type = property.PropertyType
                });
        }

        public ITableCreate<T> SetPrimaryKey(Expression<Func<T, object>> columns)
        {
            var columnNames = ExpressionHelper.GetPropertyNames<T, object>(columns);
            _tableInfo.PrimaryKey = columnNames.ToList();
            return this;
        }
        public ITableCreate<T> SetDefaultValue(Expression<Func<T, object>> column, SqlFunctions value)
        {
            var columnName = ExpressionHelper.GetPropertyName<T, object>(column);
            var col = _tableInfo.Columns.Where(a => a.ColumnName == columnName).FirstOrDefault();
            col.Default = value;
            return this;
        }
        public ITableCreate<T> SetDefaultValue(Expression<Func<T, object>> column, string value)
        {
            var columnName = ExpressionHelper.GetPropertyName<T, object>(column);
            var col = _tableInfo.Columns.Where(a => a.ColumnName == columnName).FirstOrDefault();
            col.Default = value;
            return this;
        }
        public ITableCreate<T> SetAsAutoIncermentColumn(Expression<Func<T, int>> column, int start = 1, int increment = 1)
        {
            var columnName = ExpressionHelper.GetPropertyName<T, int>(column);
            var col = _tableInfo.Columns.Where(a => a.ColumnName == columnName).FirstOrDefault();
            col.AutoIncrement = new AutoIncrement { Start = start, Increment = increment };
            return this;
        }
        public ITableCreate<T> SetAsAutoIncermentColumn(Expression<Func<T, long>> column, int start = 1, int increment = 1)
        {
            var columnName = ExpressionHelper.GetPropertyName<T, long>(column);
            var col = _tableInfo.Columns.Where(a => a.ColumnName == columnName).FirstOrDefault();
            col.AutoIncrement = new AutoIncrement { Start = start, Increment = increment };
            return this;
        }
        public ITableCreate<T> CreateIndex(Expression<Func<T, object>> columns, bool isUnique = false)
        {
            var columnNames = ExpressionHelper.GetPropertyNames<T, object>(columns);
            _tableInfo.Indexes.Add(new TableIndex { Columns = columnNames.ToArray(), IsUnique = isUnique });
            return this;
        }
        public ResultStatus Execute()
        {
            return _creator.Create(_tableInfo);
        }
    }
    #endregion
}
