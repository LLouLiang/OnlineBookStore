using OnlineBookStore.Interfaces;
using Microsoft.Extensions.Configuration;

namespace OnlineBookStore.Services
{
    public class ConfigurationService : IConfigurationService
    {
        public ConfigurationService(IConfiguration configuration)
        {
            // Fetch the connection string
            DefaultConnection = configuration.GetConnectionString("DefaultConnection");
        }
        public string DefaultConnection { get; }
    }
}
