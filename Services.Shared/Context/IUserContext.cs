namespace Services.Shared.Context
{
	public interface IUserContext
	{
		(string Value, bool Exist) UserId {  get; }
	}
}
