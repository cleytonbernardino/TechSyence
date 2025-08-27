using FluentMigrator;

namespace TechSyence.Infrastructure.Migrations.Versions;

[Migration(DatabaseVersions.TABLE_SUBSCRIPTIONS, "Creating the subscriptions table")]
public class Version0000003 : VersionBase
{
    public override void Up()
    {
        CreateTable(TableNames.TABLE_SUBSCRIPTIONS_PLANS)
            .WithColumn("CompanyId").AsInt64().NotNullable().ForeignKey("fk_company_id", TableNames.TABLE_COMPANIES, "Id")
            .WithColumn("Name").AsString(50).NotNullable()
            .WithColumn("Description").AsString().Nullable()
            .WithColumn("Price").AsDecimal().NotNullable();
    }
}
