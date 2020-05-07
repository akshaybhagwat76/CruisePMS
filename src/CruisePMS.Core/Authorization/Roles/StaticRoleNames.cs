namespace CruisePMS.Authorization.Roles
{
    public static class StaticRoleNames
    {
        public static class Host
        {
            public const string Admin = "Admin";
            public const string Cruise = "Is Cruise Operator";
            public const string Travel = "Is Travel Operator";
            public const string Ship = "Is Ship Operator";
        }

        public static class Tenants
        {
            public const string Admin = "Admin";

            public const string User = "User";
        }
    }
}