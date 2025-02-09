namespace KatieComedy.App;

public static class Constants
{
    public const string UrlPattern = @"^https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b([-a-zA-Z0-9()@:%_\+.~#?&//=]*)$";

    public static class Roles
    {
        public const string Admin = nameof(Admin);
        public const string Owner = nameof(Owner);
    }

    public static class AuthPolicy
    {
        public const string Admin = nameof(Admin);
        public const string Owner = nameof(Owner);
    }
}
