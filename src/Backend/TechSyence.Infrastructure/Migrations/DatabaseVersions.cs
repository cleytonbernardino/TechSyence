using TechSyence.Domain.Entities;

namespace TechSyence.Infrastructure.Migrations;

internal abstract class DatabaseVersions
{
    public const int TABLE_COMPANY_SUBSCRIPTION = 1;
    public const int TABLE_COMPANY = 2;
    public const int TABLE_SUBSCRIPTIONS = 3;
    public const int TABLE_USER = 4;
    public const int COMPANY_MANGER_ID = 5;
    public const int CREATE_INITIAL_USER = 6; // Persistence
}
