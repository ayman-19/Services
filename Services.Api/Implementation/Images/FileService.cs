using Services.Application.Abstarction;

namespace Services.Api.Implementation.Images
{
    public class FileService(IHttpContextAccessor _contextAccessor, IWebHostEnvironment _env)
        : IFileService
    {
        public async Task<string> SaveImageAsync(Stream fileStream, string fileName)
        {
            var uploadsPath = Path.Combine(_env.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsPath))
            {
                Directory.CreateDirectory(uploadsPath);
            }

            var filePath = Path.Combine(uploadsPath, fileName);

            using (var fileStreamOutput = new FileStream(filePath, FileMode.Create))
            {
                await fileStream.CopyToAsync(fileStreamOutput);
            }

            return $"/uploads/{fileName}";
        }

        public string GetUrlImage(string fileName) =>
            $"{_contextAccessor.HttpContext!.Request.Scheme}://{_contextAccessor.HttpContext!.Request.Host}{_contextAccessor.HttpContext!.Request.PathBase}"
            + fileName;
    }
}
