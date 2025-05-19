namespace Services.Shared.ValidationMessages
{
    public class ValidationMessages
    {
        public const string Success = "Operation completed successfully";
        public const string Failure = "Operation was not successful";

        public static class Users
        {
            public const string NameIsRequired = "Name is required";
            public const string UserTypeIsRequired = "User type is required";
            public const string PhoneIsRequired = "Phone number is required";
            public const string IdIsRequired = "ID is required";
            public const string EmailIsRequired = "Email is required";
            public const string EmailOrPhoneIsRequired = "Email or phone number is required";
            public const string PasswordIsRequired = "Password is required";
            public const string ConfirmPasswordIsRequired = "Confirm password is required";
            public const string ComparePassword = "Confirm password does not match password";
            public const string EmailExists = "Email already exists";
            public const string EmailDoesNotExist = "Email does not exist";
            public const string ValidEmail = "Please enter a valid email address";
            public const string PhoneNumberNotValid = "Phone number is not valid";
            public const string UserDoesNotExist = "User does not exist";
            public const string ConfirmEmail = "Please enter the correct email confirmation code";
            public const string ValidatePhoneNumber = "Phone number is invalid";
            public const string EmailOrPhoneNumberDoesNotExist =
                "Email or phone number does not exist";
            public const string IncorrectPassword = "Incorrect password";
            public const string LogoutError = "Invalid token";
            public const string VerifyCode = "Incorrect verification code";
            public const string ResetPassword = "Reset password code";
            public const string EmailNotConfirmed = "Email is not confirmed";
            public const string VerifyInformation = "Please verify your information";
            public const string MinPasswordLength = "Password must be at least 8 characters";
            public const string MaxPasswordLength = "Password cannot exceed 20 characters";
            public const string NotFound = "Email or password is incorrect";
            public const string RoleIdIsRequired = "Role ID is required";
            public const string PermissionIdIsRequired = "Permission ID is required";
            public const string RoleDoesNotExist = "Role does not exist";
            public const string PermissionDoesNotExist = "Permission does not exist";
            public const string PermissionAlreadyAssignedToRole =
                "Permission is already assigned to this role";
        }

        public static class Database
        {
            public const string Error = "Database transaction error occurred";
        }

        public static class Workers
        {
            public const string WorkerDoesNotExist = "Worker does not exist";
        }

        public static class Customers
        {
            public const string CustomerDoesNotExist = "Customer does not exist";
        }

        public static class Services
        {
            public const string NameIsRequired = "Service name is required";
            public const string NameExists = "Service name already exists";
            public const string DescriptionIsRequired = "Description is required";
            public const string ImageIsRequired = "Image is required";
            public const string IdIsRequired = "Service ID is required";
            public const string ServiceDoesNotExist = "Service does not exist";
            public const string RateIsRequired = "Rate is required";
        }

        public static class WorkerServices
        {
            public const string WorkerIdIsRequired = "Worker ID is required";
            public const string BranchIdIsRequired = "Branch ID is required";
            public const string ServiceIdIsRequired = "Service ID is required";
            public const string ServiceDoesNotExist = "Service does not exist";
            public const string WorkerDoesNotExist = "Worker does not exist";
            public const string WorkerAlreadyAssignedToService =
                "Worker is already assigned to this service";
            public const string WorkerNotAssignedToService =
                "Worker is not assigned to this service";
            public const string BranchDoesNotExist = "Branch does not exist";
            public const string NotFound = "No workers found";
        }

        public static class Branchs
        {
            public const string NameIsRequired = "Branch name is required";
            public const string IdIsRequired = "Branch ID is required";
            public const string IdNotFound = "Branch ID not found";
            public const string LongitudeIsRequired = "Longitude is required";
            public const string LatitudeIsRequired = "Latitude is required";
            public const string NameExists = "Branch name already exists";
            public const string DescriptionIsRequired = "Description is required";
            public const string BranchDoesNotExist = "Branch does not exist";
        }

        public static class Categories
        {
            public const string IdIsRequired = "Category ID is required";
            public const string CategoryDoesNotExist = "Category does not exist";
            public const string CategoryExists = "Category already exists";
            public const string CategoryIdIsRequired = "Category ID is required";
            public const string NameIsRequired = "Category name is required";
            public const string DescriptionIsRequired = "Description is required";
        }

        public static class Bookings
        {
            public const string IdIsRequired = "Booking ID is required";
            public const string WorkerIdIsRequired = "Worker ID is required";
            public const string CustomerIdIsRequired = "Customer ID is required";
            public const string LocationIsRequired = "Location is required";
            public const string BookingDoesNotExist = "Booking does not exist";
            public const string UserNotFound = "User not found";
            public const string RateNotProvided =
                "You must rate your previous service before booking a new one";
            public const string UnpaidPreviousBooking =
                "Unpaid previous booking - Please complete payment before submitting a new booking request";
        }

        public static class Discounts
        {
            public const string IdIsRequired = "Discount ID is required";
            public const string PercentageIsRequired = "Percentage is required";
            public const string ExpireDateIsRequired = "Expiration date is required";
            public const string DiscountExists = "Discount already exists";
            public const string DiscountDoesNotExist = "Discount does not exist";
        }
    }
}
