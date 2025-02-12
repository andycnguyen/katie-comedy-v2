﻿namespace KatieComedy.App;

public static class Constants
{
    public const string UrlPattern = @"^((https?):\/\/)?(www.)?([a-z0-9-]+\.)?([a-z0-9-]+\.)[a-z]+(\/[a-zA-Z0-9#-?]+)*\/?$";

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
