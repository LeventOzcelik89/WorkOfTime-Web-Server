using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Linq;
using System.Linq.Expressions;
using System.Reflection;

using System.Data.SqlClient;
namespace Infoline.Data
{

    class SQL2008Provider : IDBProvider
    {
        public string Name
        {
            get { return "SQL2008"; }
        }

        public IDbCommand CreateCommand(Expression expression)
        {

            var visitor = new QueryGenerator();
            List<object> parameters = new List<object>();
            return CreateCommand(visitor.Visit(expression, parameters), parameters.ToArray());
        }
        public IDbCommand CreateCommand(string query, params object[] parameters)
        {
            var cmd = new SqlCommand();

            query = string.Format(query, parameters.Select((a, i) => "@p" + i.ToString()).ToArray());
            cmd.CommandText = query;
            for (int i = 0; i < parameters.Length; i++)
            {
                var p = cmd.CreateParameter();
                p.ParameterName = "@p" + i.ToString();
                p.Value = parameters[i]??DBNull.Value;
                cmd.Parameters.Add(p);
            }
            return cmd;

        }
        public System.Data.IDbConnection CreateConnection(string connection)
        {
            return new SqlConnection(connection);
        }

        public string GetQueryText(Expression expression)
        {
            return null;
        }

        class QueryGenerator : ExpressionVisitor
        {
            static Dictionary<ExpressionType, string> _comparisions = new Dictionary<ExpressionType, string>();
            static QueryGenerator()
            {
                _comparisions[ExpressionType.Equal] = "=";
                _comparisions[ExpressionType.NotEqual] = "<>";
                _comparisions[ExpressionType.NotEqual] = "<>";

            }

            List<MemberExpression> parameters;
            StringBuilder sb;
            public string Visit(Expression expression, List<object> parameter)
            {
                sb = new StringBuilder();
                parameters = new List<MemberExpression>();
                Visit(expression);
                foreach (var p in parameters)
                {
                    parameter.Add(EvalExpression(p));

                }
                return sb.ToString();
            }
            object EvalExpression(MemberExpression ex)
            {
                if (ex.Expression.NodeType == ExpressionType.Constant)
                {
                    return GetValue(((ConstantExpression)ex.Expression).Value, ex.Member);
                }
                else
                    if (ex.Expression.NodeType == ExpressionType.MemberAccess)
                    {
                        return GetValue(EvalExpression(ex.Expression as MemberExpression), ex.Member);
                    }
                throw new Exception("Invalid member expression node!! " + ex.Expression.NodeType);
            }
            object GetValue(object value, MemberInfo mi)
            {
                var v = value;
                var fi = mi as FieldInfo;
                if (fi != null)
                    return fi.GetValue(v);
                var pi = mi as PropertyInfo;
                if (pi != null)
                    return pi.GetValue(v, new object[0]);
                throw new Exception("Invalid member call!! " + mi.Name);
            }
            public string ToString(Expression expression)
            {
                sb = new StringBuilder();
                parameters = new List<MemberExpression>();
                Visit(expression);
                return sb.ToString();
            }

            protected override Expression VisitMethodCall(MethodCallExpression node)
            {
                switch (node.Method.Name)
                {
                    case "Where":
                        Visit(node.Arguments[0]);
                        sb.Append(" WHERE ");
                        Visit(node.Arguments[1]);
                        break;
                    case "OrderBy":
                        Visit(node.Arguments[0]);
                        sb.Append("order by ");
                        Visit(node.Arguments[1]);
                        break;
                    case "OrderByDescending":
                        Visit(node.Arguments[0]);
                        sb.Append("order by ");
                        Visit(node.Arguments[1]);
                        sb.Append(" desc ");
                        break;
                    case "ThenBy":
                        Visit(node.Arguments[0]);
                        sb.Append(",");
                        Visit(node.Arguments[1]);
                        sb.Append(" desc ");
                        break;
                    case "ThenByDescending":
                        Visit(node.Arguments[0]);
                        sb.Append(",");
                        Visit(node.Arguments[1]);
                        sb.Append(" desc ");
                        break;
                    case "FirstOrDefault":
                        sb.Append("select top 1 * from (");

                        Visit(node.Arguments[0]);
                        sb.Append(") as a");
                        if (node.Arguments.Count > 1)
                        {
                            sb.Append(" WHERE ");
                            Visit(node.Arguments[1]);
                        }
                        break;
                    default:
                        throw new Exception(string.Format("Method '{0}' is not supported.", node.Method.Name));
                }
                return node;
            }
            protected override Expression VisitBinary(BinaryExpression node)
            {
                sb.Append("(");
                Visit(node.Left);
                switch (node.NodeType)
                {
                    case ExpressionType.Add:
                        sb.Append("+");
                        break;
                    case ExpressionType.AndAlso:
                        sb.Append(" and ");
                        break;
                    case ExpressionType.Divide:
                        sb.Append("/");
                        break;
                    case ExpressionType.Equal:
                        sb.Append("=");
                        break;
                    case ExpressionType.ExclusiveOr:
                        sb.Append("=");
                        break;
                    case ExpressionType.GreaterThan:
                        sb.Append(">");
                        break;
                    case ExpressionType.GreaterThanOrEqual:
                        sb.Append(">=");
                        break;
                    case ExpressionType.LessThan:
                        sb.Append("<");
                        break;
                    case ExpressionType.LessThanOrEqual:
                        sb.Append("<=");
                        break;
                    case ExpressionType.Multiply:
                        sb.Append("*");
                        break;
                    case ExpressionType.NotEqual:
                        sb.Append("*");
                        break;
                    case ExpressionType.OrElse:
                        sb.Append("*");
                        break;
                    case ExpressionType.Subtract:
                        sb.Append("-");
                        break;
                    default:
                        throw new Exception(string.Format("Not supported operator {0}", node.NodeType));
                }
                Visit(node.Right);
                sb.Append(")");
                return node;

            }

            protected override Expression VisitMember(MemberExpression node)
            {

                if (node.Expression.NodeType == ExpressionType.Parameter)
                {
                    sb.Append(((ParameterExpression)node.Expression).Name + "." + node.Member.Name);

                }
                else if (node.Expression.NodeType == ExpressionType.MemberAccess)
                {
                    sb.AppendFormat("{{{0}}}", parameters.Count);
                    parameters.Add(node);
                }
                else if (node.Expression.NodeType == ExpressionType.Constant)
                {
                    //var e = node.Expression as ConstantExpression;

                    //var fi = (node.Member as FieldInfo);

                    //for (int i = 0; i < parameters.Count; i++)
                    //{
                    //    var ex = parameters[i];
                    //    var cs = ex.Expression as ConstantExpression;
                    //    if (node.Member == ex.Member && e.Value == cs.Value)
                    //    {
                    //        sb.AppendFormat("{{{0}}}", i);

                    //        return node;
                    //    }
                    //}

                    sb.AppendFormat("{{{0}}}", parameters.Count);
                    parameters.Add(node);
                    // AppendValue(fi.FieldType, fi.GetValue(e.Value));
                }
                return node;
            }
            void AppendValue(Type type, object value)
            {
                if (type == typeof(string) || type == typeof(Guid))
                    sb.AppendFormat("'{0}'", value);

                else
                    sb.Append(value);
            }
            protected override Expression VisitConstant(ConstantExpression node)
            {

                if (node.Type.Name == "Query`1")
                {
                    var t = node.Type.GetGenericArguments()[0];
                    sb.AppendFormat("select top 2147483647  * from {0} as a ", t.Name);
                }
                else
                    AppendValue(node.Type, node.Value);
                return node;
            }
        }

        public IUpdateCommands CreateUpdateCommands(Type type, System.Data.IDbConnection connection)
        {
            return new SqlAdapter(type, connection);

        }
        class SqlAdapter : IUpdateCommands
        {
            Type type;
            public SqlAdapter(Type type, System.Data.IDbConnection connection)
            {
                this.type = type;
                insertCommand = new SqlCommand();
                updateCommand = new SqlCommand();
                deleteCommand = new SqlCommand();

                //var cmd = new SqlCommand() { CommandText = string.Format("select top 1 * from [{0}]", type.Name), Connection = ((SqlConnection)connection) };
                var cmd = connection.CreateCommand();
                cmd.CommandText = string.Format("select top 1 * from [{0}]", type.Name);
                if (connection.State != System.Data.ConnectionState.Open)
                    connection.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    var cols = reader.GetSchemaTable();
                    bool vercolumn = false;
                    foreach (DataRow r in cols.Rows)
                    {
                        if ((bool)r["IsRowVersion"])
                        {
                            vercolumn = true;
                            break;
                        }
                    }
                    foreach (DataRow row in cols.Rows)
                    {

                        ////  var cols = new SQlCommand() { co
                        //foreach (var pi in type.GetProperties())
                        //{
                        //    if (!(pi.CanWrite && pi.CanRead)) continue;
                        //    var sm = pi.GetSetMethod();
                        //    if (!(sm != null && sm.IsVirtual)) continue;


                        var name = (string)row["ColumnName"];
                        var sqltype = (SqlDbType)(int)row["ProviderType"];
                        var nulable = (bool)row["AllowDBNull"];
                        var iskey = name == "id";//(bool)row["IsKey"];
                        if (!(bool)row["IsReadOnly"])
                        {
                            insertCommand.Parameters.Add(new SqlParameter
                            {
                                SourceVersion = System.Data.DataRowVersion.Proposed,
                                ParameterName = "@" + name,
                                SourceColumn = name,
                                SqlDbType = sqltype,
                                IsNullable = nulable
                            });
                            if (!iskey)
                                updateCommand.Parameters.Add(new SqlParameter
                                {
                                    SourceVersion = System.Data.DataRowVersion.Proposed,
                                    ParameterName = "@" + name,
                                    SourceColumn = name,
                                    SqlDbType = sqltype,
                                    IsNullable = nulable
                                });
                        }
                        if (sqltype == System.Data.SqlDbType.NText || sqltype == System.Data.SqlDbType.Binary
                            || sqltype == System.Data.SqlDbType.Image || sqltype == System.Data.SqlDbType.Text)
                            continue;

                        if (iskey || !vercolumn || (vercolumn && (bool)row["IsRowVersion"]))
                        {
                            updateCommand.Parameters.Add(new SqlParameter
                            {
                                SourceVersion = System.Data.DataRowVersion.Original,
                                ParameterName = "@O_" + name,
                                SourceColumn = name,
                                SqlDbType = sqltype,
                                IsNullable = nulable

                            });
                            deleteCommand.Parameters.Add(new SqlParameter
                            {
                                SourceVersion = System.Data.DataRowVersion.Original,
                                ParameterName = "@" + name,
                                SourceColumn = name,
                                SqlDbType = sqltype,
                                IsNullable = nulable
                            });
                        }

                    }
                }
                if (updateCommand.Parameters.Count > 0)
                {
                    var sb = new StringBuilder();
                    sb.AppendFormat("UPDATE [{0}] SET ", type.Name);

                    for (int i = 0, ix = 0; i < updateCommand.Parameters.Count; i++)
                    {
                        var p = updateCommand.Parameters[i];
                        p.ParameterName = "@p" + i.ToString();
                        if (p.SourceVersion == DataRowVersion.Proposed)
                            sb.AppendFormat("{0} [{1}] = {2}", ix++ > 0 ? ", " : "", p.SourceColumn, p.ParameterName);
                    }

                    sb.Append(" WHERE ");

                    for (int i = 0, ix = 0; i < updateCommand.Parameters.Count; i++)
                    {
                        var p = updateCommand.Parameters[i];
                        if (p.SourceVersion == DataRowVersion.Original)
                        {
                            sb.AppendFormat("{0} ([{1}] = {2}", ix++ > 0 ? " and " : "", p.SourceColumn, p.ParameterName);
                            if (p.IsNullable)
                                sb.AppendFormat(" or  ([{0}] is null  and  {1} is null))", p.SourceColumn, p.ParameterName);
                            else sb.Append(")");
                        }
                    }
                    updateCommand.CommandText = sb.ToString();
                }





                if (deleteCommand.Parameters.Count > 0)
                {
                    var sb = new StringBuilder();
                    sb.AppendFormat("delete from [{0}] where", type.Name);
                    for (int i = 0; i < deleteCommand.Parameters.Count; i++)
                    {
                        var p = deleteCommand.Parameters[i];

                        if (p.SourceVersion == DataRowVersion.Original)
                        {
                            sb.AppendFormat("{0} ([{1}] = {2}", i > 0 ? " and " : "", p.SourceColumn, p.ParameterName);
                            if (p.IsNullable)
                                sb.AppendFormat(" and  ([{0}] is null  or  {1} is null))", p.SourceColumn, p.ParameterName);
                            else sb.Append(")");
                        }
                    }
                    deleteCommand.CommandText = sb.ToString();
                }



                if (insertCommand.Parameters.Count > 0)
                {
                    var sb = new StringBuilder();
                    sb.AppendFormat("INSERT INTO [{0}] (", type.Name);
                    for (int i = 0; i < insertCommand.Parameters.Count; i++)
                    {
                        updateCommand.Parameters[i].ParameterName = "@p" + i.ToString();
                        sb.AppendFormat("{0}[{1}]", i > 0 ? "," : "", insertCommand.Parameters[i].SourceColumn);
                    }
                    sb.Append(") VALUES (");
                    for (int i = 0; i < insertCommand.Parameters.Count; i++)
                        sb.AppendFormat("{0}{1}", i > 0 ? "," : "", insertCommand.Parameters[i].ParameterName);

                    sb.Append(")");
                    insertCommand.CommandText = sb.ToString();
                }

            }


            SqlCommand updateCommand, insertCommand, deleteCommand;


            public System.Data.IDbCommand DeleteCommand
            {
                get { return deleteCommand.Clone(); }
            }

            public System.Data.IDbCommand InsertCommand
            {
                get { return insertCommand.Clone(); }
            }

            System.Data.IDbCommand IUpdateCommands.UpdateCommand(IEnumerable<string> changedprops)
            {
                var cmd = updateCommand.Clone();

                if (cmd.Parameters.Count > 0)
                {
                    for (int i = 0; i < cmd.Parameters.Count; i++)
                    {
                        var p = cmd.Parameters[i];
                        if (p.SourceVersion == DataRowVersion.Proposed && !changedprops.Contains(p.SourceColumn))
                        {
                            cmd.Parameters.RemoveAt(i);
                            i--;
                        }
                    }
                    var sb = new StringBuilder();
                    sb.AppendFormat("UPDATE [{0}] SET ", type.Name);

                    for (int i = 0, ix = 0; i < cmd.Parameters.Count; i++)
                    {
                        var p = cmd.Parameters[i];
                        p.ParameterName = "@p" + i.ToString();
                        if (p.SourceVersion == DataRowVersion.Proposed)
                            sb.AppendFormat("{0} [{1}] = {2}", ix++ > 0 ? ", " : "", p.SourceColumn, p.ParameterName);
                    }

                    sb.Append(" WHERE ");

                    for (int i = 0, ix = 0; i < cmd.Parameters.Count; i++)
                    {
                        var p = cmd.Parameters[i];
                        if (p.SourceVersion == DataRowVersion.Original)
                        {
                            sb.AppendFormat("{0} ([{1}] = {2}", ix++ > 0 ? " and " : "", p.SourceColumn, p.ParameterName);
                            if (p.IsNullable)
                                sb.AppendFormat(" or  ([{0}] is null  and  {1} is null))", p.SourceColumn, p.ParameterName);
                            else sb.Append(")");
                        }
                    }
                    cmd.CommandText = sb.ToString();
                }
                //Burda sadece değişenleri update edecek query yapılabilir..
                return cmd;
            }

        }


        public System.Data.IDbCommand DeleteCommand(Expression expression)
        {
            throw new NotImplementedException();
        }
    }



}
