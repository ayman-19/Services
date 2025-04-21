namespace Services.Application.Abstarction
{
    public interface IFileService
    {
        Task<string> SaveImageAsync(Stream fileStream, string fileName);
        string GetUrlImage(string fileName);
    }
}
