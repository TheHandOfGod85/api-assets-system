using Microsoft.AspNetCore.Identity;

namespace Domain;

public class AppUser : IdentityUser
{
    public string FirstName { get; private set; } = default!;
    public string LastName { get; private set; } = default!;
    public Uri? ProfilePhotoUrl { get; private set; }

    public AppUser(
        string firstName,
        string lastName,
        string email,
        string userName
    )
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        UserName = userName;
    }

    private AppUser() { }
    public void AddPhoto(Uri photoUrl)
    {
        ProfilePhotoUrl = photoUrl;
    }
}
