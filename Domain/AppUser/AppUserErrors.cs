using SharedKernel;

namespace Domain;

public static class AppUserErrors
{
    public static Error UserAlreadyExists => Error.Conflict(
        "AppUser.AlreadyExists", "The user already exists"
    );
    public static Error UserRegistrationFailed => Error.Conflict(
        "AppUser.RegistrationFailure", $"Could not register the new user"
    );
}
