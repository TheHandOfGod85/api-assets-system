using SharedKernel;

namespace Domain;

public static class AppUserErrors
{
    public static Error UserAlreadyExists => Error.Conflict(
        "AppUser.AlreadyExists", "The user already exists"
    );
    public static Error UserAlreadyConfirmed => Error.Conflict(
        "AppUser.AlreadyConfirmed", "The user already confirmed registration"
    );
    public static Error UserNotFound(string emailAddress) => Error.Unhautorized(
        "AppUser.NotFound", $"The user with email {emailAddress} was not found"
    );
    public static Error InvalidToken => Error.Unhautorized(
        "AppUser.InvalidToken", "The token is invalid or expired"
    );
    public static Error UserRegistrationFailed => Error.Conflict(
        "AppUser.RegistrationFailure", $"Could not register the new user"
    );
}
