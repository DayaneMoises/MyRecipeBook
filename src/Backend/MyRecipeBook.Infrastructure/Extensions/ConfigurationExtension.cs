using Microsoft.Extensions.Configuration;

namespace MyRecipeBook.Infrastructure.Extensions;

public static class ConfigurationExtension
{
    public static bool IsUnitTestEnvironment(this IConfiguration configuration)
    {
        return configuration.GetValue<bool>("InMemoryTest");
    }
    public static string ConnectionString(this IConfiguration configuration) //this pq é uma extenção
    {
        return configuration.GetConnectionString("Connection")!;
    }

}
