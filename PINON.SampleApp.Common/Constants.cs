namespace PINON.SampleApp.Common
{
    public static class Constants
    {
        public const string JwtSecretKey = "MyTotalSecretAppKey";
        public const string JwtIssuer = "PinonSampleAppTokenService";
        public const int JwtExpirationInMinutes = 30;
#if (DEBUG)
        public const string JwtAudience = "http://localhost:5424";
#else
        public const string JwtAudience = "http://inx-web.azurewebsites.net";
#endif

        public static string Debug_Db_ConnetionString =
            @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=PINON_SampleApp;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
    }
}