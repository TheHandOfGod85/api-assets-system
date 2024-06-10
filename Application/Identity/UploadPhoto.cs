using System.ComponentModel.DataAnnotations;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SharedKernel;

namespace Application;

public class UploadPhoto : IRequest<Result<string>>
{
    [Required]
    [PhotoValidator]
    public IFormFile Photo { get; set; } = default!;
}

public class UploadPhotoHandler : IRequestHandler<UploadPhoto, Result<string>>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly CurrentAppUser _currentAppUser;
    private IWebHostEnvironment _webHostEnvironment;

    public UploadPhotoHandler(
        UserManager<AppUser> userManager,
        IHttpContextAccessor httpContextAccessor,
        CurrentAppUser currentAppUser,
        IWebHostEnvironment webHostEnvironment)
    {
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
        _currentAppUser = currentAppUser;
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task<Result<string>> Handle(UploadPhoto request, CancellationToken cancellationToken)
    {
        try
        {
            var appUserId = _currentAppUser.AppUserId;
            var appUser = await _userManager.FindByIdAsync(appUserId.ToString());
            var requestUrl = _httpContextAccessor?.HttpContext?.Request;
            var baseUri = $"{requestUrl?.Scheme}://{requestUrl?.Host}{requestUrl?.PathBase}";
            var existingPhotoUri = appUser?.ProfilePhotoUrl;
            if (existingPhotoUri is not null)
            {
                var localFilePath = $"{_webHostEnvironment.WebRootPath}/{existingPhotoUri.LocalPath}";
                Files.DeleteFile(localFilePath);
            }
            var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
            var extension = Path.GetExtension(request.Photo.FileName);
            var newFileName = $"{Path.GetFileNameWithoutExtension(request.Photo.FileName).Replace(" ", "_")}-{timestamp}{extension}";
            var savedFilePath = await Files.SavePhoto(newFileName, "AppUserPhotos", request.Photo, cancellationToken);
            var photoUri = new Uri(new Uri(baseUri), $"AppUserPhotos/{newFileName}");
            appUser?.AddPhoto(photoUri);
            var result = await _userManager.UpdateAsync(appUser!);
            return Result.Success(photoUri.ToString());
        }
        catch (FileUploadFailure)
        {
            return Result.Failure<string>(Error.Conflict("File.Uploadfailure", "Error uploading photo"));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }
}
