namespace Services.Shared.ValidationMessages
{
    public class ValidationMessages
    {
        public const string Success = "operation done successfully";
        public const string Falier = "operation not successfully";

        public static class User
        {
            public const string NameIsRequired = "Name Is Required.";
            public const string PhoneIsRequired = "Phone Is Required.";
            public const string IdIsRequired = "Id Is Required.";
            public const string EmailIsRequired = "Email Is Required.";
            public const string EmailOrPhoneIsRequired = "Email Or Phone Is Required.";
            public const string PasswordIsRequired = "Password Is Required.";
            public const string ConfirmPasswordIsRequired = "ConfirmPassword Is Required.";
            public const string ComparePassword = "ConfirmPassword Not Equal Password.";
            public const string EmailIsExist = "Email Is Exist.";
            public const string EmailIsNotExist = "Email Is Not Exist.";
            public const string ValidMail = "Make Sure This is Valid.";
            public const string UserNotExist = "User Not Exist.";
            public const string ConfirmEmail = "Write this Code Correct that Confirm Email.";
            public const string ValidatePhoneNumber = "Phone Number Invalid.";
            public const string EmailOrPhoneNumberNotExist = "Email Or PhoneNumber Not Exist.";
            public const string IncorrectPassword = "Incorrect Password.";
            public const string LogoutError = "Invalid Token.";
            public const string VerifyCode = "Code Incorrect.";
            public const string ResetPassword = "Reset Password Code.";
            public const string EmailNotConfirmed = "Email Not Confirmed.";
            public const string MakeSureInformation = "Make Sure Information.";
        }

        public static class Database
        {
            public const string Error = "Occur Exception By Transaction.";
        }
    }
}
