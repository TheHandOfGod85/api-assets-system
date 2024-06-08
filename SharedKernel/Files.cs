using Microsoft.AspNetCore.Http;

namespace SharedKernel;

public static class Files
{
    public static async Task<string> SavePhoto(
        string fileName,
        string folderName,
        IFormFile photo,
        CancellationToken cancellationToken)
    {
        try
        {
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folderName);
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            string filePath = Path.Combine(folderPath, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await photo.CopyToAsync(stream, cancellationToken);
            }
            return filePath;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw new FileUploadFailure("Error uploading photo");
        }
    }

    public static void DeleteFile(string filePath)
    {
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }
}
