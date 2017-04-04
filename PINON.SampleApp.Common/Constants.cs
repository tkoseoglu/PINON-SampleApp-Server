using System.Net.NetworkInformation;

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

        public static string EmailHost { get; set; } = "smtp.gmail.com";
        public static int EmailHostPort { get; set; } = 587;
        public static string EmailUserName { get; set; } = "tolgaonline@gmail.com";
        public static string EmailPassword { get; set; } = "Ktk19766";

        public static string SentEmailFromName { get; set; } = "Pinon Sample App";
        public static string SentEmailFromEmailAddress { get; set; } = "tolga.k@outlook.com";
        public static string ResetPasswordEmailSubject { get; set; } = "Pinon Sample App | Password Reset";
        public static string ConfirmUseRegistrationEmailSubject { get; set; } = "Pinon Sample App | Confirm Registration";

#if (DEBUG)
        public static string ApplicationUrl { get; set; } = "http://localhost:4200";
#else
           public static string ApplicationUrl { get; set; } = "http://localhost:4200";
#endif

    }
}