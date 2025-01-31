namespace Services.Shared.Exceptions
{
	public sealed class InvalidException : Exception
	{
		public InvalidException(string message) : base(message) { }
	}
}
