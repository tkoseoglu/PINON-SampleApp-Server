namespace PINON.SampleApp.Common
{
    public static class Constants
    {        
        public const string JwtIssuer = "http://jwtauthzsrv.azurewebsites.net";
        public const string JwtAudience = "099153c2625149bc8ecb3e85e03f0022";
        public const string JwtSecretKey = "856FECBA3B06519C8DDDBC80BB080554";
        public const int JwtExpirationInMinutes = 30;
        
        public static string Debug_Db_ConnetionString =
            @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=PINON_SampleApp;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
    }
}