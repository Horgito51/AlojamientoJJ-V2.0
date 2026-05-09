namespace Servicio.Hotel.API.Authorization
{
    public static class AuthorizationPolicies
    {
        public const string AdminProfile = "AdminProfile";
        public const string BackOffice = "BackOffice";
        public const string ClienteRole = "CLIENTE";

        public static readonly string[] BackOfficeRoles =
        {
            "ADMINISTRADOR",
            "ADMIN",
            "RECEPCIONISTA",
            "OPERATIVO",
            "DESK_SERVICE"
        };
    }
}
