using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infoline.Framework.Database
{
    public class TableInfo
    {
        public string TableAlias { get; set; }
        public string TableName { get; set; }
        public string SchemaName { get; set; }
        public List<TableColumn> Columns { get; set; }
        public List<TableIndex> Indexes { get; set; }
        public List<string> PrimaryKey { get; set; }
        public TableColumn this[string name]
        {
            get { return Columns.Where(a => a.ColumnName == name).FirstOrDefault(); }
        }
        public TableInfo()
        {
            Columns = new List<TableColumn>();
            Indexes = new List<TableIndex>();
            PrimaryKey = new List<string>();
            SchemaName = "dbo";
        }
        public override string ToString()
        {
            return string.Format("Name: {0} - ColumnCount: {1} - IndexCount: {2}", TableName, Columns.Count, Indexes.Count);
        }
    }
    public class TableColumn
    {
        public TableColumn()
        {
            NotNull = false;
            IsDomain = false;
            Enums = new List<EnumItem>();
        }
        public string ColumnAlias { get; set; }
        public string ColumnName { get; set; }
        public Type Type { get; set; }
        public int? Length { get; set; }
        public bool NotNull { get; set; }
        public bool IsDomain { get; set; }
        public double? Min { get; set; }
        public double? Max { get; set; }
        public string Pattern { get; set; }
        public ColumnDefaultValue Default { get; set; }
        public AutoIncrement AutoIncrement { get; set; }
        public bool? Unique { get; set; }
        public List<EnumItem> Enums { get; set; }
        public override string ToString()
        {
            return string.Format("Name: {0} - Type: {1}", ColumnName, Type.Name);
        }
    }
    public class TableIndex
    {
        public string Name { get; set; }
        public bool IsUnique { get; set; }
        public string[] Columns { get; set; }
    }
    public class AutoIncrement
    {
        public int Start { get; set; }
        public int Increment { get; set; }

        public AutoIncrement()
        {
            Start = 1;
            Increment = 1;
        }
    }
    public class ColumnDefaultValue
    {
        public string Text { get; set; }
        public SqlFunctions? Function { get; set; }

        public static implicit operator ColumnDefaultValue(string val)
        {
            return new ColumnDefaultValue { Text = val };
        }
        public static implicit operator ColumnDefaultValue(SqlFunctions val)
        {
            return new ColumnDefaultValue { Function = val };
        }
    }
    public class EnumItem
    {
        public string Key { get; set; }
        public int Value { get; set; }
        public Guid? EnumStyleId { get; set; }
        public short? EnumStyleAngle { get; set; }
        public Dictionary<string, string> ColumnDefaults { get; set; }
    }
    public enum SqlFunctions
    {
        NEWID,
        GETDATE,
    }
}
