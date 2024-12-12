using DataTransferLayer.OtherObject;

namespace Service.Helper
{
    public class AppSettings
    {
        public static DtoAppSettings dtoAppSettings;

        public static void Init()
        {
            dtoAppSettings = new DtoAppSettings();

            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
            IConfigurationRoot configuration = builder.Build();

            DtoAppSettings.connetionStringsMySql = configuration["ConnectionStrings:MySQLConnection"];
            dtoAppSettings.originAudience = configuration["Authentication:Jwt:Audience"];
            dtoAppSettings.originIssuer = configuration["Authentication:Jwt:Issuer"];
            dtoAppSettings.accessJwtSecret = configuration["Authentication:Jwt:AccessTokenSecret"];
            dtoAppSettings.refreshJwtSecret = configuration["Authentication:Jwt:RefreshTokenSecret"];
            dtoAppSettings.originRequest = configuration["Cors:originRequest"];
        }

        public static string GetConnectionStringMySql()
        {
            return DtoAppSettings.connetionStringsMySql;
        }

        public static string GetOriginIssuer()
        {
            return dtoAppSettings.originIssuer;
        }
            
        public static string GetOriginAudience()
        {
            return dtoAppSettings.originAudience;
        }

        public static string GetAccessJwtSecret()
        {
            return dtoAppSettings.accessJwtSecret;
        }

        public static string GetRefreshJwtSecret()
        {
            return dtoAppSettings.refreshJwtSecret;
        }
        public static string GetOriginRequest()
        {
            return dtoAppSettings.originRequest;
        }
    }
}
