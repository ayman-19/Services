using PhoneNumbers;

namespace Services.Shared.Extentions
{
    public static class ValidPhoneNumber
    {
        public static bool ValidatePhoneNumber(this string phoneNumber)
        {
            try
            {
                PhoneNumberUtil phoneNumberUtil = PhoneNumberUtil.GetInstance();
                PhoneNumber number = phoneNumberUtil.Parse(phoneNumber, "eg");
                return phoneNumberUtil.IsValidNumber(number);
            }
            catch
            {
                return false;
            }
        }
    }
}
