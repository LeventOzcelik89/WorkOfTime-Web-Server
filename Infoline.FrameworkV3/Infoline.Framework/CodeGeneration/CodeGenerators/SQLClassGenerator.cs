using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infoline.Framework.CodeGeneration.CodeGenerators
{
    public class SQLClassGenerator
    {
        SqlTypeConverter typeConverter = new SqlTypeConverter();
        public class TableDescription
        {
            public string Table { get; set; }
            public string Column { get; set; }
            public string Description { get; set; }

        }
        private List<TableDescription> tableDescriptionList(SqlConnection con)
        {
            var list = new List<TableDescription>();

            using (var cmd = new SqlCommand { Connection = con })
            {
                cmd.CommandText = @"select 
        st.name [Table],
        sc.name [Column],
        sep.value [Description]
    from sys.tables st
    inner join sys.columns sc on st.object_id = sc.object_id
    left join sys.extended_properties sep on st.object_id = sep.major_id
                                         and sc.column_id = sep.minor_id
                                         and sep.name = 'MS_Description'
    where sep.value is  not null

uniON 
select 
        st.name [Table],
        '' as [Column],
        sep.value [Description]
    from sys.tables st 
    left join sys.extended_properties sep on  st.object_id = sep.major_id and minor_id=0
                                         and sep.name = 'MS_Description'
    where sep.value is  not null
	 ";
                cmd.Parameters.Clear();
                var dt = new DataTable();
                dt.Load(cmd.ExecuteReader());
                foreach (DataRow row in dt.Rows)
                {
                    list.Add(new TableDescription
                    {
                        Table = (string)row["Table"],
                        Description = (string)row["Description"],
                        Column = (string)row["Column"],
                    }
                    );
                }
            }
            return list;
        }


        public Dictionary<string, string> GenerateMultiFile(string strConn)
        {

            var result = new Dictionary<string, string>();
            using (var con = new SqlConnection(strConn))
            {
                con.Open();
                var _listDesc = tableDescriptionList(con);
                string[] objArrRestrict;
                DataTable schemaTbl;
                schemaTbl = con.GetSchema("Tables", null);
                //var rows = schemaTbl.Rows.OfType<DataRow>().OrderBy(a => a["TABLE_NAME"]);
                foreach (DataRow row in schemaTbl.Rows)
                {
                    StringBuilder sb = new StringBuilder();
                    bool hasname = false;
                    var tablename = (string)row["TABLE_NAME"];
                    var skipColumns = new[] { "id", "created", "changed", "createdby", "changedby", "Intid", "intId" };

                    if (tablename.IndexOf("ZOUT_") > -1) continue;

                    objArrRestrict = new string[] { null, null, tablename, null };
                    var tbl = con.GetSchema("Columns", objArrRestrict);
                    foreach (DataRow colrow in tbl.Rows)
                        hasname |= (string)colrow["column_name"] == "adi";

                    var _description = _listDesc.Where(a => a.Table == tablename && string.IsNullOrEmpty(a.Column)).Select(b => b.Description).FirstOrDefault();
                    #region _description
                    if (!string.IsNullOrEmpty(_description))
                    {

                        var satir = _description.Split(new string[] { "\r", "\n" }, StringSplitOptions.None);

                        sb.AppendFormat("        /// <summary>");
                        sb.AppendLine();
                        foreach (var item in satir)
                        {
                            sb.AppendFormat("        /// {0}", item);
                            sb.AppendLine();

                        }
                        sb.AppendFormat("        /// </summary>");
                        sb.AppendLine();
                    }
                    #endregion

                    sb.AppendFormat("    public partial class {0} : InfolineTable", tablename); sb.AppendLine();
                    sb.AppendLine("    {");
                    foreach (DataRow colrow in tbl.Rows)
                    {
                        var cname = (string)colrow["column_name"];
                        _description = _listDesc.Where(a => a.Table == tablename && a.Column == cname).Select(b => b.Description).FirstOrDefault();
                        if (skipColumns.Contains(cname)) continue;
                        #region _description
                        if (!string.IsNullOrEmpty(_description))
                        {

                            var satir = _description.Split(new string[] { "\r", "\n" }, StringSplitOptions.None);

                            sb.AppendFormat("        /// <summary>");
                            sb.AppendLine();
                            foreach (var item in satir)
                            {
                                sb.AppendFormat("        /// {0}", item);
                                sb.AppendLine();

                            }
                            sb.AppendFormat("        /// </summary>");
                            sb.AppendLine();
                        }
                        #endregion
                        if ((string)colrow["data_type"] != "geography")
                        {
                            Type stype = typeConverter.Convert((string)colrow["data_type"]);
                            string alias = typeConverter.GetAlias(stype);
                            alias += ((string)colrow["is_nullable"]) == "YES" && !stype.IsClass ? "?" : "";



                            sb.AppendFormat("        public {0} {1} {{ get; set;}}", alias, cname);

                            sb.AppendLine();
                        }
                        else
                        {
                            sb.AppendFormat("        public {0} {1} {{ get; set;}}", "IGeometry ", cname);
                            sb.AppendLine();
                        }

                    }
                    sb.AppendLine("    }");
                    result.Add(tablename, sb.ToString());
                }
                result.Add("InfolineTable", GetBaseTable());
            }
            return result;
        }
        public string GetObject(string strConn, string tablename)
        {
            string result = "";
            var con = new SqlConnection(strConn);
            con.Open();
            string[] objArrRestrict;
            DataTable schemaTbl;
            schemaTbl = con.GetSchema("Tables", null);
            foreach (DataRow row in schemaTbl.Rows)
            {
                if ((string)row["TABLE_NAME"] == tablename)
                {
                    StringBuilder sb = new StringBuilder();
                    bool hasname = false;
                    var _listDesc = tableDescriptionList(con);
                    objArrRestrict = new string[] { null, null, tablename, null };
                    var tbl = con.GetSchema("Columns", objArrRestrict);
                    foreach (DataRow colrow in tbl.Rows)
                        hasname |= (string)colrow["column_name"] == "adi";
                    sb.AppendFormat("    public class {0}", tablename); sb.AppendLine();
                    sb.AppendLine("    {");
                    foreach (DataRow colrow in tbl.Rows)
                    {
                        var cname = (string)colrow["column_name"];
                        var _description = _listDesc.Where(a => a.Table == tablename && a.Column == cname).Select(b => b.Description).FirstOrDefault();

                        if (!string.IsNullOrEmpty(_description))
                        {

                            var satir = _description.Split(new string[] { "\r", "\n" }, StringSplitOptions.None);

                            sb.AppendFormat("        /// <summary>");
                            sb.AppendLine();
                            foreach (var item in satir)
                            {
                                sb.AppendFormat("        /// {0}", item);
                                sb.AppendLine();

                            }
                            sb.AppendFormat("        /// </summary>");
                            sb.AppendLine();
                        }


                        if ((string)colrow["data_type"] != "geography")
                        {
                            Type stype = typeConverter.Convert((string)colrow["data_type"]);
                            string alias = typeConverter.GetAlias(stype);
                            alias += ((string)colrow["is_nullable"]) == "YES" && !stype.IsClass ? "?" : "";
                            sb.AppendFormat("        public {0} {1} {{ get; set;}}", alias, cname);
                            sb.AppendLine();
                        }
                        else
                        {
                            sb.AppendFormat("        public {0} {1} {{ get; set;}}", "SqlGeography ", cname);
                            sb.AppendLine();
                        }

                    }
                    sb.AppendLine("    }");
                    result = sb.ToString();
                }
            }
            return result;
        }
        public string GetBaseTable()
        {
            var str = @"
    
	public class InfolineTable
    {
        public InfolineTable()
        {
            id = Guid.NewGuid();
        }

        /// <summary>
        /// Kayıt ID
        /// </summary>
        public Guid id { get; set; }
        /// <summary>
        /// Oluşturma Tarihi
        /// </summary>
        public DateTime? created { get; set; }
        /// <summary>
        /// Değiştirme Tarihi
        /// </summary>
        public DateTime? changed { get; set; }
        /// <summary>
        /// Değiştiren User
        /// </summary>
        public Guid? changedby { get; set; }
        /// <summary>
        /// Oluşturan User
        /// </summary>
        public Guid? createdby { get; set; }
    }
";
            return str;
        }

        public string Generate(string strConn)
        {
            var classes = GenerateMultiFile(strConn);
            return string.Join("\r\n", classes.OrderBy(a => a.Key).Select(a => a.Value));
        }

    }
}
