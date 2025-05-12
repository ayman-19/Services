namespace Services.Shared.Exceptions
{
    public sealed class NotConfirmEmail : Exception
    {
        public NotConfirmEmail(string message)
            : base(message) { }
    }
}
