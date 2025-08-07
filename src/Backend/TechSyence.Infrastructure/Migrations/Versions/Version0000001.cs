using FluentMigrator;

namespace TechSyence.Infrastructure.Migrations.Versions;

[Migration(DatabaseVersions.TABLE_USER, "Criando o primeiro modelo usuario")]
public class Version0000001 : VersionBase
{
    private const string DATABASE_NAME = "Users";

    public override void Up()
    {
        CreateTable(DATABASE_NAME)
            .WithColumn("IsAdmin").AsBoolean().WithDefaultValue(false)
            .WithColumn("UpdatedOn").AsDateTime().NotNullable()
            .WithColumn("UserIndentifier").AsGuid().NotNullable()
            .WithColumn("Email").AsString().NotNullable().Unique()
            .WithColumn("Phone").AsString().NotNullable().Unique()
            .WithColumn("FirstName").AsString().NotNullable()
            .WithColumn("LastName").AsString().Nullable()
            .WithColumn("Password").AsString().NotNullable();
    }
}
