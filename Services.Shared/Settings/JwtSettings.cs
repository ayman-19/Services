using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Shared.Settings
{
    public sealed class JwtSettings
    {
        public static string Key => "O0f9yhJ89AgqmLk/6kqPoYWPCTfGgcHEFHI9ee9dAt4=";
        public static string Issuer => "Services";
        public static string Audience => "Services For users";
        public static bool ValidateIssuer => true;
        public static bool ValidateAudience => true;
        public static bool ValidateLifeTime => true;
        public static bool ValidateIssuerSigningKey => true;
        public static int AccessTokenExpireDate => 1;
        public static int RefreshTokenExpireDate => 2;
    }
}
