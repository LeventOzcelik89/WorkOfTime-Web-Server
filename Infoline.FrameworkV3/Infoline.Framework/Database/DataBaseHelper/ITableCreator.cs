namespace Infoline.Framework.Database
{
    public interface ITableCreator
    {
        ResultStatus Create(TableInfo tableInfo);
        ResultStatus Alter(TableInfo tableInfo);

        TableInfo GetSchema(string tableName, bool onlyColums = false);
    }
}
