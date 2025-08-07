using FluentMigrator;
using FluentMigrator.Builders.Create.Table;

namespace TechSyence.Infrastructure.Migrations.Versions;

public abstract class VersionBase : ForwardOnlyMigration
{
    protected ICreateTableColumnOptionOrWithColumnSyntax CreateTable(string table)
    {
        return Create.Table(table)
            .WithColumn("Id").AsInt64().PrimaryKey().Identity()
            .WithColumn("Active").AsBoolean().NotNullable().WithDefaultValue(true)
            .WithColumn("CreatedOn").AsDateTime().NotNullable().WithDefaultValue(DateTime.UtcNow);
    }
}
