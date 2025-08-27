using FluentMigrator;
using TechSyence.Domain.Enums;
using TechSyence.Domain.Security.Cryptography;

namespace TechSyence.Infrastructure.Migrations.Versions;

[Migration(DatabaseVersions.CREATE_INITIAL_USER, "Creating initial user")]
public class InitialUser(
    IPasswordEncripter encripter
    ) : ForwardOnlyMigration
{
    private readonly IPasswordEncripter _encripter = encripter;

    public override void Up()
    {
        Insert.IntoTable(TableNames.TABLE_COMPANIES)
            .Row(new
            {
                Id = 1,
                UpdatedOn = DateTime.UtcNow,
                CNPJ = "00000000000000",
                LegalName = "Delete-Me",
                CEP = "00000000",
                AddressNumber = "0000000000",
                PhoneNumber = "00000000000",
            });

        string hashedPassword = _encripter.Encript("admin123456789");

        Insert.IntoTable(TableNames.TABLE_USER)
            .Row(new
            {
                IsAdmin = true,
                UpdatedOn = DateTime.UtcNow,
                LastLogin = DateTime.UtcNow,
                UserIndentifier = Guid.NewGuid(),
                Email = "admin@admin.com",
                Phone = "00000000000",
                FirstName = "delete-me",
                Password = hashedPassword,
                Role = (short)UserRolesEnum.MANAGER,
                CompanyId = 1
            });
    }
}
