
namespace Services.Shared.Exceptions
{
	public sealed class DatabaseTransactionException : Exception
	{
		public DatabaseTransactionException(string message):base(message) { }
	}
}
