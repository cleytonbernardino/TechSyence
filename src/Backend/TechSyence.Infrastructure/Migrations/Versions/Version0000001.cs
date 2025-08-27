using FluentMigrator;

namespace TechSyence.Infrastructure.Migrations.Versions;

[Migration(DatabaseVersions.TABLE_COMPANY_SUBSCRIPTION, "Creating the company subscription table")]
public class Version0000001 : VersionBase
{
    public override void Up()
    {
        CreateTable(TableNames.TABLE_COMPANY_SUBSCRIPTION)
            .WithColumn("SubscriptionId").AsInt16()
            .WithColumn("IsBillingAnnual").AsBoolean().NotNullable().WithDefaultValue(false)
            .WithColumn("PaymentStatus").AsInt16().NotNullable()
            .WithColumn("NextBillingDate").AsDateTime().NotNullable()
            .WithColumn("PaymentMethod").AsInt16().NotNullable();
    }
}
