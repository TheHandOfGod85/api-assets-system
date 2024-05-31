namespace Domain;

public class AppUser
{
    public Guid Id { get; private set; }
    public string IdentityId { get; private set; } = default!;
    public string FirstName { get; private set; } = default!;
    public string LastName { get; private set; } = default!;
    public string EmailAddress { get; private set; } = default!;
    public Uri? ProfilePhotoUrl { get; private set; }

    public AppUser(
        string identityId,
        string firstName,
        string lastName,
        string emailAddress,
        Guid? id = null

    )
    {
        Id = id ?? Guid.NewGuid();
        FirstName = firstName;
        LastName = lastName;
        EmailAddress = emailAddress;
        IdentityId = identityId;
    }

    private AppUser() { }
    public void AddPhoto(Uri photoUrl)
    {
        ProfilePhotoUrl = photoUrl;
    }
}
