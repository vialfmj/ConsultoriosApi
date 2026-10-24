namespace ConsultoriosApi.Application.Contracts.Authentication
{
    public static class AuthRoles
    {
        public const string Admin = "Admin";
        public const string Dentist = "Dentist";

        public static readonly string[] All = [Admin, Dentist];
    }
}
