using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
namespace Infoline.Framework.Helper
{
    public class TableClass
    {
        private List<KeyValuePair<String, Type>> _fieldInfo = new List<KeyValuePair<String, Type>>();
        private string _className = String.Empty;

        private Dictionary<Type, String> dataMapper
        {
            get
            {
                var dataMapper = new Dictionary<Type, string>();
                dataMapper.Add(typeof(int), "[int]");
                dataMapper.Add(typeof(int?), "[int]");

                dataMapper.Add(typeof(string), "[nvarchar](64)");
                dataMapper.Add(typeof(bool), "[bit]");
                dataMapper.Add(typeof(bool?), "[bit]");

                dataMapper.Add(typeof(DateTime), "[datetime]");
                dataMapper.Add(typeof(DateTime?), "[datetime]");

                dataMapper.Add(typeof(float), "[float]");
                dataMapper.Add(typeof(float?), "[float]");

                dataMapper.Add(typeof(decimal), "[float]");
                dataMapper.Add(typeof(decimal?), "[float]");

                dataMapper.Add(typeof(Guid), "UNIQUEIDENTIFIER");
                return dataMapper;
            }
        }
        public List<KeyValuePair<string, Type>> Fields
        {
            get { return this._fieldInfo; }
            set { this._fieldInfo = value; }
        }

        public string ClassName
        {
            get { return this._className; }
            set { this._className = value; }
        }

        public TableClass(Type t)
        {
            this._className = t.Name;

            foreach (PropertyInfo p in t.GetProperties())
            {
                KeyValuePair<string, Type> field = new KeyValuePair<string, Type>(p.Name, p.PropertyType);

                this.Fields.Add(field);
            }
        }

        public string CreateTableScript()
        {
            System.Text.StringBuilder script = new StringBuilder();
            script.AppendLine(string.Format("IF NOT EXISTS (select * from sysobjects where name='{0}' and xtype='U')", this.ClassName));
            script.AppendLine("CREATE TABLE dbo.[" + this.ClassName+"]");
            script.AppendLine("(");
            script.AppendLine("\t [id] [int] IDENTITY(1,1) NOT null");
            script.AppendLine(")");

            for (int i = 0; i < this.Fields.Count; i++)
            {
                KeyValuePair<String, Type> field = this.Fields[i];

                if (field.Key == "id") continue;
                if (dataMapper.ContainsKey(field.Value))
                {
                    script.AppendLine(string.Format("\t if not exists (select column_name from INFORMATION_SCHEMA.columns where table_name = '{0}' and column_name = '{1}')",this.ClassName,field.Key));
                    
                    script.AppendLine(string.Format("\t ALTER TABLE [{0}] ADD [{1}] {2} NULL",this.ClassName,field.Key,dataMapper[field.Value]));
                                        

                    script.Append(Environment.NewLine);
                }
            }

            return script.ToString();
        }
    }
}
